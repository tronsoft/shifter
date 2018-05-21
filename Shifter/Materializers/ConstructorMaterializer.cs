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

using System.Reflection;
using Shifter.Exceptions;
using Shifter.Injectors;
using Shifter.Selectors;

namespace Shifter.Materializers
{
    internal class ConstructorMaterializer : IMaterializer
    {
        private readonly IShifterContext context;

        public ConstructorMaterializer(IShifterContext context)
        {
            this.context = context;
        }

        public void Engage()
        {
            // Already instantiated
            if (context.Instance != null)
                return;

            // Select a constructor
            var selector = new ConstructorSelector();
            var constructor = selector.Selecter(context);

            // Throw an exception if no constructor is found
            if (constructor == null)
            {
                throw new TypeResolvingFailedException(Strings.TypeHasNoConstructorToInject);
            }

            // resolve the arguments of the constructor
            var constructorInjector = new ConstructorInjector(constructor, context, ResolveArguments(constructor));
            constructorInjector.Inject();
        }

        private object[] ResolveArguments(ConstructorInfo contructor)
        {
            // Now get the parameters of the contructor and register the types
            ParameterInfo[] parameters = contructor.GetParameters();
            var arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                // if the field is not yet registered throw an exception 
                if (!context.Container.IsTypeRegistered(parameter.ParameterType))
                {
                    throw new TypeResolvingFailedException(Strings.TypeNotRegistered);
                }

                // Add arguments 
                arguments[i] = context.Container.Resolve(parameter.ParameterType);
            }

            return arguments;
        }
    }
}
