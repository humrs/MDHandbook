using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Logging;

namespace MDHandbookApp.Forms.Services
{
    public class CustomDebugLogger : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            var dt = DateTimeOffset.UtcNow;

            System.Diagnostics.Debug.WriteLine($"{dt:o}:{category}:{priority}::  {message}");
        }
    }
}
