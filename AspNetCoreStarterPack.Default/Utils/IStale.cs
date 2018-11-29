namespace AspNetCoreStarterPack.Default
{
    public interface IStale<out T>
    {
        bool IsStale { get; }
        T Update { get; }
    }
}