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
        #region Nested classes

        private class TypeInfo
        {
            public object Instance { get; set; }
            public string Key { get; set; }
        }

        private class TypeWrapper
        {
            public TypeWrapper(Type type)
            {
                Type = type;
            }

            public Type Type { get; set; } 
        }

        #endregion

        /// <summary>
        /// The default container.
        /// </summary>
        private static IShifterContainer DefaultContainer;

        /// <summary>
        /// The options used to configure the container.
        /// </summary>
        private readonly ShifterContainerOptions options = new ShifterContainerOptions();

        /// <summary>
        /// A dictionary that contains the types that are registered without a name.
        /// </summary>
        private readonly Dictionary<TypeWrapper, TypeInfo> registeredTypes = new Dictionary<TypeWrapper, TypeInfo>();

        /// <summary>
        /// The strategies for resolving a injection point.
        /// </summary>
        private readonly IList<IResolutionStrategy> strategies = new List<IResolutionStrategy>
                                                                 {
                                                                     new PropertyResolutionStrategy(),
                                                                     new MethodResolutionStrategy(),
                                                                     new FieldResolutionStrategy()
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
        public static IShifterContainer Default
        {
            get
            {
                return DefaultContainer ?? (DefaultContainer = new ShifterContainer());
            }
        }

        /// <summary>
        /// The strategies for resolving a injection point.
        /// </summary>
        public IList<IResolutionStrategy> Strategies
        {
            get
            {
                return strategies;
            }
        }

        /// <summary>
        /// The options used to configure the container.
        /// </summary>
        public ShifterContainerOptions Options
        {
            get
            {
                return options;
            }
        }

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
                var wrapper = new TypeWrapper(instanceToRegister.GetType());
                registeredTypes.Add(wrapper, new TypeInfo
                                             {
                                                 Instance = instanceToRegister
                                             });
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

                var wrapper = new TypeWrapper(typeToRegister);
                registeredTypes.Add(wrapper, new TypeInfo
                                             {
                                                 Instance = dependedClass
                                             });
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

                var wrapper = new TypeWrapper(typeToRegister);
                registeredTypes.Add(wrapper, new TypeInfo
                                             {
                                                 Instance = dependedType
                                             });
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
                        .Single(x => x.Key.Type == type)
                        .Value
                        .Instance; 
                    Type typeToResolve;
                    if (resolvedObject is Type)
                    {
                        var resolvedType = (Type)resolvedObject;
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

                    var context = new ShifterContext(typeToResolve, this, Strategies);

                    if (!(resolvedObject is Type))
                    {
                        context.Instance = resolvedObject;
                    }

                    return context.Resolve();
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
                if (registeredTypes.Values.All(x => x.Key != name))
                {
                    throw new TypeResolvingFailedException(Strings.NamedTypeCouldNotBeResolved);
                }

                return registeredTypes
                    .Values
                    .Single(x => x.Key == name)
                    .Instance; 
            }
        }

        public object Resolve<T>(string name)
        {
            return (T)Resolve(name);
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            lock (syncLock)
            {
                return registeredTypes
                    .Where(x => x.Key.Type == type)
                    .Select(x => x.Value.Instance)
                    .ToList();
            }
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            lock (syncLock)
            {
                return registeredTypes
                    .Where(x => x.Key.Type == typeof(T))
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
                    foreach (var wrapper in registeredTypes.Where(x => x.Key.Type == type)
                                                           .Select(x => x.Key)
                                                           .ToList())
                    {
                        registeredTypes.Remove(wrapper);
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
            registeredTypes.Clear();
        }

        public bool IsTypeRegistered(Type type)
        {
            Assume.ArgumentNotNull(type, "type");

            lock (syncLock)
            {
                return registeredTypes.Any(x => x.Key.Type == type);
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
                var wrapper = new TypeWrapper(instanceToRegister.GetType());
                registeredTypes.Add(wrapper, new TypeInfo
                                             {
                                                 Instance = instanceToRegister, 
                                                 Key = name
                                             }); 
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
                throw new ActivationException(string.Format("Could not resolve an object with {0} key", key), ex);
            }
            catch (NoInheritanceDependencyException ex)
            {
                throw new ActivationException(string.Format("Could not resolve an object with {0} key", key), ex);
            }

            if (resolved.GetType() != serviceType)
            {
                throw new ActivationException(string.Format("Could not resolve an object with {0} key and type {1}", 
                                                            key, 
                                                            serviceType.FullName));
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