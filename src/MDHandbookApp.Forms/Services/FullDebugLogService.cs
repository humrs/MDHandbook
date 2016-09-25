//
//  Copyright 2016  R. Stanley Hum <r.stanley.hum@gmail.com>
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//

using System;
using System.Runtime.CompilerServices;
using Prism.Logging;


namespace MDHandbookApp.Forms.Services
{
    public class FullDebugLogService : ILogService
    {
        private IMyLogger _logger;

        public FullDebugLogService(IMyLogger logger)
        {
            _logger = logger;
        }

        public void Debug(string message, Priority priority = Priority.Low, [CallerMemberName] string memberName = "")
        {
            this.Log(message, Category.Debug, priority, memberName);
        }

        public void Info(string message, Priority priority = Priority.Low, [CallerMemberName] string memberName = "")
        {
            this.Log(message, Category.Info, priority, memberName);
        }

        public void InfoException(string message, Exception ex, Priority priority = Priority.Low, [CallerMemberName] string memberName = "")
        {
            var fullMessage = $"{message} [{ex.ToString()}]";
            this.Log(fullMessage, Category.Exception, priority, memberName);
        }

        private void Log(string message, Category category, Priority priority, string memberName)
        {
            _logger.Log($"({memberName}) {message}", category, priority);
        }
    }
}
