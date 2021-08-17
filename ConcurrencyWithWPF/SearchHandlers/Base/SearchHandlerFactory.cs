using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConcurrencyWithWPF.SearchHandlers.Base
{
    public class SearchHandlerFactory : ISearchHandlerFactory
    {
        private readonly IServiceProvider provider;

        public SearchHandlerFactory(IServiceProvider provider)
        {
            this.provider = provider;
        }

        public T GetSearchHandler<T>()
            where T : ISearchHandler
        {
            return provider.GetService<T>();
        }

        public ISearchHandler Default { get => provider.GetService<WhenAllSearchHandler>(); }


    }

}
