namespace PersonaVault.Api.Validators
{
    public interface IRequestDataValidator
    {
        bool IsRequestDataValid<T>(T request);
        bool IsLongValid(string num);
        bool IsStringValid(string text);
    }
}