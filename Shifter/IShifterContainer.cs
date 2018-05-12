using System;
using System.Collections.Generic;
using CommonServiceLocator;

namespace Shifter
{
    public interface IShifterContainer : IServiceLocator
    {
        ShifterContainerOptions Options { get; }

        IShifterContainer AddInstance(object instanceToRegister);

        IShifterContainer AddInstance(Type typeToRegister, object dependedClass);

        IShifterContainer AddInstance<T>(T instanceToRegister);

        IShifterContainer AddType(Type typeToRegister, Type dependedType);

        IShifterContainer AddType<TTypeToRegister, TDependedType>() where TDependedType : TTypeToRegister;

        object Resolve(Type type);

        T Resolve<T>();

        bool IsTypeRegistered(Type type);

        object Resolve(string name);

        object Resolve<T>(string name);

        /// <summary>
        /// Resolves all instances.
        /// </summary>
        /// <param name="type">The type of the instances to resolve.</param>
        /// <returns>The instances.</returns>
        IEnumerable<object> ResolveAll(Type type);

        /// <summary>
        /// Resolves all instances.
        /// </summary>
        /// <typeparam name="T">The type of the instances to resolve.</typeparam>
        /// <returns>The instances.</returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// Unregisters an instance.
        /// </summary>
        /// <param name="type">The type of the instance to unregister.</param>
        void Unregister(Type type);

        /// <summary>
        /// Unregisters an instance.
        /// </summary>
        /// <typeparam name="T">The type of the instance to unregister.</typeparam>
        void Unregister<T>();

        /// <summary>
        /// Resets this instance.
        /// </summary>
        void Reset();

        /// <summary>
        /// This method adds an instance to the container under the name <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name to register the type under.</param>
        /// <param name="instanceToRegister">The instance to register.</param>
        /// <returns>A reference to the container.</returns>
        IShifterContainer AddNamedInstance(string name, object instanceToRegister);
    }
}