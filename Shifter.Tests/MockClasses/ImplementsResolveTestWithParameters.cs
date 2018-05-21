using Shifter.Tests.Interfaces;

namespace Shifter.Tests.MockClasses
{
    public class ImplementsResolveTestWithParameters : IResolveTest
    {
        [Inject]
#pragma warning disable 649
        private string fieldToInject;
#pragma warning restore 649

        public ImplementsResolveTestWithParameters(string param1)
        {
            Param1 = param1;
        }

        public string FieldToInject => fieldToInject;

        public string Param1 { get; }

        public string MethodFieldInjected { get; set; }

        [Inject]
        public string PropertyInjected { get; set; }

        [Inject]
        private void SetMethodField(string value)
        {
            MethodFieldInjected = value;
        }
    }
}