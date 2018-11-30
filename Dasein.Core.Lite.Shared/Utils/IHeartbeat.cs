namespace Dasein.Core.Lite.Shared
{
    public interface IHeartbeat<out T>
    {
        bool IsHeartbeat { get; }
        T Update { get; }
    }
}