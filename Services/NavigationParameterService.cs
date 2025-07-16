namespace FireTestingApp_net8.Services
{
    public static class NavigationParameterService
    {
        private static readonly Dictionary<string, object?> _parameters = new();

        public static void Set(string key, object? value)
        {
            _parameters[key] = value;
        }

        public static T? Get<T>(string key)
        {
            if (_parameters.TryGetValue(key, out var value) && value is T t)
                return t;

            return default;
        }

        public static void Clear(string key)
        {
            _parameters.Remove(key);
        }
    }
}
