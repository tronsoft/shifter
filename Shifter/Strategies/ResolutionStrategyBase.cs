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
using Shifter.Utils;

namespace Shifter.Strategies
{
    internal abstract class ResolutionStrategyBase : IResolutionStrategy
    {
        public void Initialize(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            if (context.Instance == null)
            {
                throw new TypeResolvingFailedException(string.Format(Strings.InstanceIsNull, context.TypeToResolve.Name));
            }

            InitializeImpl(context);
        }

        protected abstract void InitializeImpl(IShifterContext context);
    }
}