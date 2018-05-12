//===============================================================================
// TRONSoft
//
// Shifter
//
//===============================================================================
// Copyright © Ton de Ron.
//
// All rights reserved.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//
//===============================================================================

using Shifter.Exceptions;
using Shifter.Injectors;
using Shifter.Selectors;
using Shifter.Utils;

namespace Shifter.Materializers
{
    public class FieldMaterializer : IMaterializer
    {
        private readonly IShifterContext context;

        public FieldMaterializer(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            this.context = context;
        }

        public void Engage()
        {
            foreach (var field in new FieldSelector().Select(context))
            {
                // if the field type is not yet registered throw an exception 
                if (!context.Container.IsTypeRegistered(field.FieldType))
                {
                    throw new TypeResolvingFailedException(Strings.TypeNotRegistered);
                }

                // Engage the value
                var fieldValue = context.Container.Resolve(field.FieldType);

                // Set the fields value
                var fieldInjector = new FieldInjector(context, field, fieldValue);
                fieldInjector.Inject();
            }
        }
    }
}
