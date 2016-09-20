using System.Runtime.CompilerServices;
using Prism.Logging;

namespace MDHandbookApp.Forms.Services
{
    public interface ILogService
    {
        void Log(string message, Category category, Priority priority, [CallerMemberName] string memberName = "");
    }
}