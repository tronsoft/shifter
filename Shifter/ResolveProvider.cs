using System.Linq;
using Shifter.Exceptions;
using Shifter.Materializers;

namespace Shifter
{
    internal class ResolveProvider : IResolveProvider
    {
        private readonly IShifterContext context;

        public ResolveProvider(IShifterContext context)
        {
            this.context = context;
        }

        public bool CanCreate => !context.TypeToResolve.IsAbstract && !context.TypeToResolve.IsInterface;

        public object Resolve()
        {
            if (!CanCreate)
            {
                throw new TypeResolvingFailedException(string.Format(Strings.TypeIsAnInterfaceOrAnAbstractClass, context.TypeToResolve.FullName));
            }

            var constructorMaterializer = new ConstructorMaterializer(context);
            constructorMaterializer.Engage();

            foreach (var strategy in context.StrategyFactories.Select(s => s()))
            {
                strategy.Initialize(context);
            }

            return context.Instance;
        }
    }
}