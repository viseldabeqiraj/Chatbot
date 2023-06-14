namespace Chatbot.Interfaces
{
    public interface ICacheService
    {
        void AddItem(string key, object value);
        bool TryGetItem<TItem>(string key, out TItem item);
        void ClearCache();
    }
}
