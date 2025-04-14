namespace Core
{
    public interface IProvider
    {
        string Name { get; }
        Task<string> ExecuteAsync(string action, params object[] parameters);
    }
}