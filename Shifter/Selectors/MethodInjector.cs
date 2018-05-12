using System.Collections.Generic;
using System.Reflection;
using Shifter.Utils;

namespace Shifter.Selectors
{
    public class MethodInjector : MultipleSelector<MethodInfo>
    {
        public override IEnumerable<MethodInfo> Select(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            var methodList = new List<MethodInfo>(context.TypeToResolve.GetMethods(context.Container.Options.BindingFlags));
            foreach (var method in methodList)
            {
                if (method.IsDefined(typeof(InjectAttribute), false))
                {
                    yield return method;
                }
            }
        }
    }
}