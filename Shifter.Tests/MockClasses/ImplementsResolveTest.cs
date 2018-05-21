using Shifter.Tests.Interfaces;

namespace Shifter.Tests.MockClasses
{
    public class ImplementsResolveTest : IResolveTest
    {
        private string injectedProperty;

        public string InjectedProperty
        {
            get => injectedProperty;
            set => injectedProperty = value;
        }

        [Inject]
        private void SetInjectedProperty(string value)
        {
            InjectedProperty = value;
        }
    }
}