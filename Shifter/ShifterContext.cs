using System;
using System.Collections.Generic;
using Shifter.Exceptions;
using Shifter.Materializers;
using Shifter.Strategies;
using Shifter.Utils;

namespace Shifter
{
    public class ShifterContext : IShifterContext
    {
        #region Private Fields

        private readonly IShifterContainer container;
        private readonly IList<IResolutionStrategy> strategies;
        private readonly Type typeToResolve;
        private object instance;

        #endregion

        #region ctors

        public ShifterContext(Type typeToResolve, IShifterContainer container, IList<IResolutionStrategy> strategies)
        {
            Assume.ArgumentNotNull(typeToResolve, "typeToResolve");
            Assume.ArgumentNotNull(container, "container");
            Assume.ArgumentNotNull(strategies, "strategies");

            this.typeToResolve = typeToResolve;
            this.container = container;
            this.strategies = strategies;
        }

        #endregion

        #region Properties

        public IShifterContainer Container
        {
            get
            {
                return container;
            }
        }

        public Type TypeToResolve
        {
            get
            {
                return typeToResolve;
            }
        }

        public object Instance
        {
            get
            {
                return instance;
            }
            set
            {
                Assume.ArgumentNotNull(value, "value");

                instance = value;
            }
        }

        public IList<IResolutionStrategy> Strategies
        {
            get
            {
                return strategies;
            }
        }

        public bool CanCreate
        {
            get
            {
                return !typeToResolve.IsAbstract && !typeToResolve.IsInterface;
            }
        }

        #endregion

        public object Resolve()
        {
            if (!CanCreate)
            {
                throw new TypeResolvingFailedException(string.Format(Strings.TypeIsAnInterfaceOrAnAbstractClass, typeToResolve.FullName));
            }

            var constructorMaterializer = new ConstructorMaterializer(this);
            constructorMaterializer.Engage();

            foreach (var strategy in strategies)
            {
                strategy.Initialize(this);
            }

            return instance;
        }
    }
}