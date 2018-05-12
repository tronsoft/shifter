namespace Shifter
{
    public class ShifterContainerOptions
    {
        private bool resolvePrivateMembers = true;

        public bool ResolvePrivateMembers
        {
            get
            {
                return resolvePrivateMembers;
            }
            set
            {
                resolvePrivateMembers = value;
            }
        }
    }
}