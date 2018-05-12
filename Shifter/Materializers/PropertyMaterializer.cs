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
    public class PropertyMaterializer : IMaterializer
    {
        private IShifterContext context;

        public PropertyMaterializer(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            this.context = context; 
        }

        public void Engage()
        {
            foreach (var property in new PropertySelector().Select(context))
            {
                // if the property type is not yet registered throw an exception 
                if (!context.Container.IsTypeRegistered(property.PropertyType))
                {
                    throw new TypeResolvingFailedException(Strings.TypeNotRegistered);
                }

                // Engage the value
                var propertyValue = context.Container.Resolve(property.PropertyType);

                // Set the propertys value
                var propertyInjector = new PropertyInjector(property, context, propertyValue);
                propertyInjector.Inject();
            }
        }
    }
}
