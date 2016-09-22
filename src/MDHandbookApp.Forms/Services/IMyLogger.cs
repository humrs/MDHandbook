using Prism.Logging;

namespace MDHandbookApp.Forms.Services
{
    public interface IMyLogger
    {
        void Log(string message, Category category, Priority priority);
    }
}