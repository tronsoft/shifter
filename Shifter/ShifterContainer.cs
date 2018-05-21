using System;
using System.Collections.Generic;
using System.Linq;
using CommonServiceLocator;
using Shifter.Exceptions;
using Shifter.Strategies;
using Shifter.Utils;

namespace Shifter
{
    /// <summary>
    /// This class is used to register and resolve types or instances.
    /// </summary>
    public class ShifterContainer : IShifterContainer
    {
        /// <summary>
        /// The default container.
        /// </summary>
        private static IShifterContainer defaultContainer;

        /// <summary>
        /// A dictionary that contains the types that are registered without a name.
        /// </summary>
        private readonly Dictionary<Type, (object Instance, string Name)> registeredTypes = new Dictionary<Type, (object Instance, string Name)>();

        /// <summary>
        /// The strategies for resolving a injection point.
        /// </summary>
        private readonly IEnumerable<Func<IResolutionStrategy>> strategyFactories = new List<Func<IResolutionStrategy>>
        {
            () => new PropertyResolutionStrategy(),
            () => new MethodResolutionStrategy(),
            () => new FieldResolutionStrategy()
        };

        /// <summary>
        /// Synchronization object.
        /// </summary>
        private readonly object syncLock = new object();

        /// <summary>
        /// Gets the default container.
        /// </summary>
        /// <value>
        /// The default container.
        /// </value>
        public static IShifterContainer Default => defaultContainer ?? (defaultContainer = new ShifterContainer());

        /// <summary>
        /// The options used to configure the container.
        /// </summary>
        public ShifterContainerOptions Options { get; } = new ShifterContainerOptions();

        /// <summary>
        /// This method adds an instance to the container.
        /// </summary>
        /// <param name="instanceToRegister">The instance to register.</param>
        /// <returns>A reference to the container.</returns>
        public IShifterContainer AddInstance(object instanceToRegister)
        {
            Assume.ArgumentNotNull(instanceToRegister, "instanceToRegister");

            lock (syncLock)
            {
                registeredTypes.Add(instanceToRegister.GetType(), (instanceToRegister, string.Empty));
            }

            return this;
        }

        /// <summary>
        /// This method adds an instance to the container. The instance is registered 
        /// under the type supplied with the parameter <paramref name="typeToRegister"/>.
        /// </summary>
        /// <remarks>
        /// The registered class must be of the type or derived from <paramref name="typeToRegister"/>.
        /// </remarks>
        /// <param name="typeToRegister">The type to register the instance under.</param>
        /// <param name="dependedClass">The instance to register.</param>
        /// <returns>A reference to the container.</returns>
        public IShifterContainer AddInstance(Type typeToRegister, object dependedClass)
        {
            Assume.ArgumentNotNull(typeToRegister, "typeToRegister");
            Assume.ArgumentNotNull(dependedClass, "dependedClass");

            lock (syncLock)
            {
                if (!typeToRegister.IsInstanceOfType(dependedClass))
                {
                    throw new NoInheritanceDependencyException(string.Format(Strings.TypeIsNotDerivedFromOtherType, dependedClass.GetType().FullName, typeToRegister.FullName));
                }

                registeredTypes.Add(typeToRegister, (dependedClass, string.Empty));
            }

            return this;
        }

        /// <summary>
        /// This method adds an instance to the container. The instance is registered 
        /// under the type supplied with the parameter <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to register the instance under.</typeparam>
        /// <param name="instanceToRegister">The instance to register.</param>
        /// <returns>A reference to the container.</returns>
        public IShifterContainer AddInstance<T>(T instanceToRegister)
        {
            AddInstance(typeof(T), instanceToRegister);

            return this;
        }

        public IShifterContainer AddType(Type typeToRegister, Type dependedType)
        {
            Assume.ArgumentNotNull(typeToRegister, "typeToRegister");
            Assume.ArgumentNotNull(dependedType, "dependedType");

            lock (syncLock)
            {
                if (!typeToRegister.IsAssignableFrom(dependedType))
                {
                    throw new NoInheritanceDependencyException(string.Format(Strings.TypeIsNotDerivedFromOtherType, dependedType.FullName, typeToRegister.FullName));
                }

                registeredTypes.Add(typeToRegister, (dependedType, string.Empty));
            }

            return this;
        }

        public IShifterContainer AddType<TTypeToRegister, TDependedType>() where TDependedType : TTypeToRegister
        {
            return AddType(typeof(TTypeToRegister), typeof(TDependedType));
        }

        public object Resolve(Type type)
        {
            Assume.ArgumentNotNull(type, "type");

            lock (syncLock)
            {
                if (IsTypeRegistered(type))
                {
                    var resolvedObject = registeredTypes
                        .Single(x => x.Key == type)
                        .Value
                        .Instance;
                    Type typeToResolve;
                    if (resolvedObject is Type resolvedType)
                    {
                        if (resolvedType.IsAbstract || resolvedType.IsInterface)
                        {
                            throw new TypeResolvingFailedException(string.Format(Strings.TypeIsAnInterfaceOrAnAbstractClass, resolvedType.FullName));
                        }

                        typeToResolve = resolvedType;
                    }
                    else
                    {
                        typeToResolve = resolvedObject.GetType();
                    }

                    var context = new ShifterContext(typeToResolve, this, strategyFactories);

                    if (!(resolvedObject is Type))
                    {
                        context.Instance = resolvedObject;
                    }

                    return new ResolveProvider(context).Resolve();
                }
            }

            throw new TypeResolvingFailedException(string.Format(Strings.TypeCouldNotBeResolved, type.FullName));
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        public object Resolve(string name)
        {
            Assume.NotNullOrEmpty(name, "name");

            lock (syncLock)
            {
                if (registeredTypes.Values.All(x => x.Name != name))
                {
                    throw new TypeResolvingFailedException(Strings.NamedTypeCouldNotBeResolved);
                }

                return registeredTypes
                    .Values
                    .Single(x => x.Name == name)
                    .Instance;
            }
        }

        public T Resolve<T>(string name)
        {
            return (T)Resolve(name);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            lock (syncLock)
            {
                return registeredTypes
                    .Where(x => x.Key == type)
                    .Select(x => x.Value.Instance)
                    .ToList();
            }
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            lock (syncLock)
            {
                return registeredTypes
                    .Where(x => x.Key == typeof(T))
                    .Select(x => x.Value.Instance)
                    .Cast<T>()
                    .ToList();
            }
        }

        public void Unregister(Type type)
        {
            lock (syncLock)
            {
                if (IsTypeRegistered(type))
                {
                    foreach (var key in registeredTypes.Where(x => x.Key == type)
                                                       .Select(x => x.Key)
                                                       .ToList())
                    {
                        registeredTypes.Remove(key);
                    }
                }
            }
        }

        public void Unregister<T>()
        {
            Unregister(typeof(T));
        }

        public void Reset()
        {
            lock (syncLock)
            {
                registeredTypes.Clear();
            }
        }

        public bool IsTypeRegistered(Type type)
        {
            Assume.ArgumentNotNull(type, "type");

            lock (syncLock)
            {
                return registeredTypes.Any(x => x.Key == type);
            }
        }

        /// <summary>
        /// This method adds an instance to the container under the name <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name to register the type under.</param>
        /// <param name="instanceToRegister">The instance to register.</param>
        /// <returns>A reference to the container.</returns>
        public IShifterContainer AddNamedInstance(string name, object instanceToRegister)
        {
            Assume.NotNullOrEmpty(name, "name");
            Assume.ArgumentNotNull(instanceToRegister, "instanceToRegister");

            lock (syncLock)
            {
                registeredTypes.Add(instanceToRegister.GetType(), (instanceToRegister, name ?? string.Empty));
            }

            return this;
        }

        #region Implementation of IServiceProvider

        /// <summary>
        /// Gets the service object of the specified type.
        /// </summary>
        /// <returns>
        /// A service object of type <paramref name="serviceType"/>.-or- null if there is no service object of type <paramref name="serviceType"/>.
        /// </returns>
        /// <param name="serviceType">An object that specifies the type of service object to get. </param>
        public object GetService(Type serviceType)
        {
            return GetInstance(serviceType);
        }

        #endregion

        #region Implementation of IServiceLocator

        public object GetInstance(Type serviceType)
        {
            return GetInstance(serviceType, null);
        }

        public object GetInstance(Type serviceType, string key)
        {
            object resolved;
            try
            {
                resolved = (key == null)
                               ? Resolve(serviceType)
                               : Resolve(key);
            }
            catch (TypeResolvingFailedException ex)
            {
                throw new ActivationException($"Could not resolve an object with {key} key", ex);
            }
            catch (NoInheritanceDependencyException ex)
            {
                throw new ActivationException($"Could not resolve an object with {key} key", ex);
            }

            if (resolved.GetType() != serviceType)
            {
                throw new ActivationException($"Could not resolve an object with {key} key and type {serviceType.FullName}");
            }

            return resolved;
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return ResolveAll(serviceType);
        }

        public TService GetInstance<TService>()
        {
            return (TService)GetInstance(typeof(TService), null);
        }

        public TService GetInstance<TService>(string key)
        {
            return (TService)GetInstance(typeof(TService), key);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return ResolveAll(typeof(TService)).OfType<TService>();
        }

        #endregion
    }
}