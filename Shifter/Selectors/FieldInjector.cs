using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Shifter.Utils;

namespace Shifter.Selectors
{
    public class FieldInjector : MultipleSelector<FieldInfo>
    {
        public override IEnumerable<FieldInfo> Select(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            var fieldList = new List<FieldInfo>(context.TypeToResolve.GetFields(context.Container.Options.BindingFlags));
            foreach (var field in fieldList)
            {
                if (field.IsDefined(typeof(InjectAttribute), false))
                {
                    yield return field;
                }
            }
        }
    }
}
