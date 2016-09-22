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
using MDHandbookAppService.Common.Models.RequestMessages;
using Prism.Logging;


namespace MDHandbookApp.Forms.Services
{
    public class FullLogger : IMyLogger
    {
        private ILogStoreService _logStoreService;

        public FullLogger(
            ILogStoreService logStoreService)
        {
            _logStoreService = logStoreService;
        }

        public void Log(string message, Category category, Priority priority)
        {
            var dt = DateTimeOffset.UtcNow;
            var item = new AppLogItemMessage {
                LogDateTime = dt.ToString("O"),
                LogName = string.Format($"{category}:{priority}"),
                LogDataJson = string.Format($"{message}")
            };

            _logStoreService.AddItem(item);
        }
    }
}
