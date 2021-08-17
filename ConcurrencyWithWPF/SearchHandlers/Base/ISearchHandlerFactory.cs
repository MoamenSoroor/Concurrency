namespace ConcurrencyWithWPF.SearchHandlers.Base
{
    public interface ISearchHandlerFactory
    {
        ISearchHandler Default { get; }

        public T GetSearchHandler<T>() where T : ISearchHandler;
    }

}
