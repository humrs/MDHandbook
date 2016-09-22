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
using Redux;
using MDHandbookApp.Forms.Actions;
using MDHandbookApp.Forms.Services;
using MDHandbookApp.Forms.States;


namespace MDHandbookApp.Forms.Reducers
{
    public class EventsStateReducers : IEventsStateReducers
    {
        private ILogService _logService;

        public EventsStateReducers(ILogService logService)
        {
            _logService = logService;
        }

        public EventsState EventsStateReducer(EventsState previousState, IAction action)
        {
            if (action is SetIsNetworkDownAction)
                return setIsNetworkDownReducer(previousState, (SetIsNetworkDownAction)action);

            if (action is ClearIsNetworkDownAction)
                return clearIsNetworkDownReducer(previousState, (ClearIsNetworkDownAction)action);

            return previousState;
        }

        private EventsState clearIsNetworkDownReducer(EventsState previousState, ClearIsNetworkDownAction action)
        {
            _logService.Info("ClearIsNetworkDownReducer");
            EventsState newState = previousState.Clone();
            newState.IsNetworkDown = false;
            newState.LastNetworkAttempt = new DateTimeOffset(1970, 1, 1, 0, 0, 0, new TimeSpan(-5, 0, 0));
            return newState;
            
        }

        private EventsState setIsNetworkDownReducer(EventsState previousState, SetIsNetworkDownAction action)
        {
            _logService.Info("SetIsNetworkDownReducer");
            EventsState newState = previousState.Clone();
            newState.IsNetworkDown = true;
            newState.LastNetworkAttempt = action.NetworkDownLastAttemptDateTime;
            return newState;
        }
    }
}
