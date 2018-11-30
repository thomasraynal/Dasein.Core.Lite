namespace Dasein.Core.Lite.Shared
{
    public interface IStale<out T>
    {
        bool IsStale { get; }
        T Update { get; }
    }
}