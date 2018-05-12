using System.Collections.Generic;
using System.Reflection;
using Shifter.Utils;

namespace Shifter.Selectors
{
    public class PropertyInjector : MultipleSelector<PropertyInfo>
    {
        public override IEnumerable<PropertyInfo> Select(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            var propertyList = new List<PropertyInfo>(context.TypeToResolve.GetProperties(context.Container.Options.BindingFlags));
            foreach (var property in propertyList)
            {
                if (property.IsDefined(typeof(InjectAttribute), false))
                {
                    yield return property;
                }
            }
        }
    }
}