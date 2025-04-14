using System;
using System.Collections.Generic;
using Core;

namespace Infrastructure
{
    public class ProviderFactory
    {
        private readonly Dictionary<string, IProvider> _providers;

        public ProviderFactory(IEnumerable<IProvider> providers)
        {
            _providers = new Dictionary<string, IProvider>();
            foreach (var provider in providers)
            {
                _providers[provider.Name] = provider;
            }
        }

        public IProvider GetProvider(string name)
        {
            if (_providers.TryGetValue(name, out var provider))
            {
                return provider;
            }

            throw new ArgumentException($"Provider with name '{name}' not found.");
        }
    }
}