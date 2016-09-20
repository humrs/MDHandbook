using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging;

namespace MDHandbookApp.Forms.Services
{
    public class FullDebugLogService : ILogService
    {
        private ILoggerFacade _logger;

        public FullDebugLogService(ILoggerFacade logger)
        {
            _logger = logger;
        }

        public void Log(string message, Category category, Priority priority, [CallerMemberName]string memberName = "")
        {
            _logger.Log($"({memberName}) {message}", category, priority);
        }
    }
}
