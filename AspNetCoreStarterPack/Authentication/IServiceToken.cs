namespace AspNetCoreStarter.Authentication
{
    public interface IServiceToken
    {
        string Digest { get; set; }
    }
}