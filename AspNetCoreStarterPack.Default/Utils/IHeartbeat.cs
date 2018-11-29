namespace AspNetCoreStarterPack.Default
{
    public interface IHeartbeat<out T>
    {
        bool IsHeartbeat { get; }
        T Update { get; }
    }
}