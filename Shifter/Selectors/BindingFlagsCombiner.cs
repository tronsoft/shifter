using System.Reflection;

namespace Shifter.Selectors
{
    /// <summary>
    /// This class combines binding flags based on configuration. The default binding flags are 
    /// <see cref="DefaultBindingFlags">BindingFlags.Instance</see> and <see cref="DefaultBindingFlags">BindingFlags.Public</see>.
    /// </summary>
    internal class BindingFlagsCombiner
    {
        private const BindingFlags DefaultBindingFlags = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// Returns the binding flags.
        /// </summary>
        /// <param name="resolvePrivateMembers">Adds <see cref="DefaultBindingFlags">BindingFlags.NonPublic</see> if this parameter is <c>true</c>.</param>
        /// <returns>The combined flags.</returns>
        public BindingFlags Execute(bool resolvePrivateMembers)
        {
            if (resolvePrivateMembers)
            {
                return DefaultBindingFlags | BindingFlags.NonPublic;
            }

            return DefaultBindingFlags;
        }
    }
}