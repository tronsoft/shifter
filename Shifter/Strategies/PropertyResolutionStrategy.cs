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

using Shifter.Materializers;

namespace Shifter.Strategies
{
    public class PropertyResolutionStrategy : ResolutionStrategyBase
    {
        protected override void InitializeImpl(IShifterContext context)
        {
            var propertyMaterializer = new PropertyMaterializer(context);
            propertyMaterializer.Engage();
        }
    }
}