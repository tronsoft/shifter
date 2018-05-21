using System;
using System.Collections.Generic;
using System.Linq;
using Shifter.Exceptions;
using Shifter.Materializers;
using Shifter.Strategies;
using Shifter.Utils;
using static System.Diagnostics.Debug;

namespace Shifter
{
    internal class ShifterContext : IShifterContext
    {
        private object instance;

        public ShifterContext(Type typeToResolve, IShifterContainer container, IEnumerable<Func<IResolutionStrategy>> strategyFactoryFactories)
        {
            Assume.ArgumentNotNull(typeToResolve, "typeToResolve");
            Assume.ArgumentNotNull(container, "container");
            Assume.ArgumentNotNull(strategyFactoryFactories, "strategyFactoryFactories");

            TypeToResolve = typeToResolve;
            Container = container;
            StrategyFactories = strategyFactoryFactories;
        }

        public IShifterContainer Container { get; }

        public Type TypeToResolve { get; private set; }

        public object Instance
        {
            get => instance;
            set
            {
                Assert(value != null);

                instance = value;
            }
        }

        public IEnumerable<Func<IResolutionStrategy>> StrategyFactories { get; }

        public bool CanCreate => !TypeToResolve.IsAbstract && !TypeToResolve.IsInterface;

        public object Resolve()
        {
            if (!CanCreate)
            {
                throw new TypeResolvingFailedException(string.Format(Strings.TypeIsAnInterfaceOrAnAbstractClass, TypeToResolve.FullName));
            }

            var constructorMaterializer = new ConstructorMaterializer(this);
            constructorMaterializer.Engage();

            foreach (var strategy in StrategyFactories.Select(s => s()))
            {
                strategy.Initialize(this);
            }

            return instance;
        }
    }
}