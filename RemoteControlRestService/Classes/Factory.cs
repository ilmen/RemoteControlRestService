namespace RemoteControlRestService.Classes
{
    public interface IFactory<T>
    {
        T Create();
    }
}
