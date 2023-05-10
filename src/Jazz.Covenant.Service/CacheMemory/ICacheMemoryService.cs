namespace Jazz.Covenant.Service.CacheMemory
{
    public interface ICacheMemoryService
    {
        Task<object> Get(object key);
        void Set(object key, object data,int timeExperient);
    }
}
