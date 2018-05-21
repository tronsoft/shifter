namespace Shifter
{
    internal interface IResolveProvider
    {
        bool CanCreate { get; }

        object Resolve();
    }
}