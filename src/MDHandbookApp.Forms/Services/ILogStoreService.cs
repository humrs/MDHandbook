using System.Collections.Immutable;
using System.Threading.Tasks;
using MDHandbookAppService.Common.Models.RequestMessages;

namespace MDHandbookApp.Forms.Services
{
    public interface ILogStoreService
    {
        ImmutableList<AppLogItemMessage> LogStore { get; }

        void Clear();
        Task Save();
        void AddItem(AppLogItemMessage item);
    }
}