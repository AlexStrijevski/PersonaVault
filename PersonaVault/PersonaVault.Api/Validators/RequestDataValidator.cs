using System.Reflection;

namespace PersonaVault.Api.Validators
{
    internal class RequestDataValidator : IRequestDataValidator
    {
        public bool IsRequestDataValid<T>(T request)
        {
            var type = typeof(T);

            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    var value = prop.GetValue(request);
                    if (string.IsNullOrWhiteSpace(value as string)) return false;
                }
            }
            return true;
        }

        public bool IsLongValid(string num)
        {
            if(long.TryParse(num, out _))
            {
                return true;
            }
            return false;
        }

        public bool IsStringValid(string text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }
    }
}
