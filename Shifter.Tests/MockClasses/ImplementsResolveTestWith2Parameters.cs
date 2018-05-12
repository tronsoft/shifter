using Shifter.Tests.Interfaces;

namespace Shifter.Tests.MockClasses
{
    public class ImplementsResolveTestWith2Parameters : IResolveTest
    {
        private readonly string param1;
        private readonly ImplementsResolveTestWithParameters param2;

        public ImplementsResolveTestWith2Parameters(string param1, ImplementsResolveTestWithParameters param2)
        {
            this.param1 = param1;
            this.param2 = param2;
        }

        public string Param1
        {
            get { return param1; }
        }

        public ImplementsResolveTestWithParameters Param2
        {
            get { return param2; }
        }
    }
}