using System.Threading.Tasks;

namespace ConcurrencyWithWPF.SearchHandlers.Base
{
    public interface ISearchHandler
    {
        string Title { get; }

        Task AfterSearchAsync();
        Task BeforeSearchAsync();
        Task HandleSearchErrorAsync();
        Task SearchingAsync();
    }
}