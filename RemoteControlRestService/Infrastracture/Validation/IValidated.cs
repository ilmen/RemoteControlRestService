namespace RemoteControlRestService.Infrastracture.Validation
{
    public interface IValidated
    {
        ValidResult IsValid();

        //TODO: сделать тут GetValidator<T>()
    }
}
