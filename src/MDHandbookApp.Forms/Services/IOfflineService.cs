using System.Collections.Immutable;
using System.Threading.Tasks;
using MDHandbookApp.Forms.States;
using MDHandbookAppService.Common.Models.RequestMessages;

namespace MDHandbookApp.Forms.Services
{
    public interface IOfflineService
    {
        AppState LoadOfflineAppState();
        ImmutableList<AppLogItemMessage> LoadOfflineLogStore();
        Task SaveAppState(AppState state);
        Task SaveLogStore(ImmutableList<AppLogItemMessage> logstores);
    }
}