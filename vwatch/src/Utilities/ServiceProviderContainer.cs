﻿namespace vwatch.Utilities
{
    public static class ServiceProviderContainer
    {
        private static IServiceProvider _serviceProvider;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static T GetService<T>() where T : class
        {
            return _serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
