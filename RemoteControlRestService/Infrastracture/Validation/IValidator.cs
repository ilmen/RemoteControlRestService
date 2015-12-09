namespace RemoteControlRestService.Infrastracture.Validation
{
    public interface IValidator<T>
    {
        ValidResult Validate(T value);
    }
}
