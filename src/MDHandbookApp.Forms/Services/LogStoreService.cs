﻿//
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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDHandbookAppService.Common.Models.RequestMessages;

namespace MDHandbookApp.Forms.Services
{
    public class LogStoreService : ILogStoreService
    {
        private IOfflineService _offlineService;

        public ImmutableList<AppLogItemMessage> LogStore { get; protected set; }


        public LogStoreService(
            IOfflineService offlineService)
        {
            _offlineService = offlineService;
            LogStore = _offlineService.LoadOfflineLogStore();
        }
        
        public void Clear()
        {
            LogStore = ImmutableList<AppLogItemMessage>.Empty;
        }

        public async Task Save()
        {
            await _offlineService.SaveLogStore(LogStore);
        }

        public void AddItem(AppLogItemMessage item)
        {
            LogStore = LogStore.Add(item);
        }

    }
}
