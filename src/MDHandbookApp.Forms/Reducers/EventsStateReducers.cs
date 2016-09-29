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
            if (action is ClearUnauthorizedCountAction)
                return clearUnauthorizedCountReducer(previousState, (ClearUnauthorizedCountAction) action);

            if (action is IncrementUnauthorizedCountAction)
                return incrementUnauthorizedCountReducer(previousState, (IncrementUnauthorizedCountAction) action);

            if (action is SetIsNetworkDownAction)
                return setIsNetworkDownReducer(previousState, (SetIsNetworkDownAction)action);

            if (action is ClearIsNetworkDownAction)
                return clearIsNetworkDownReducer(previousState, (ClearIsNetworkDownAction)action);

            if (action is SetIsNetworkBusyAction)
                return setIsNetworkBusyReducer(previousState, (SetIsNetworkBusyAction) action);

            if (action is ClearIsNetworkBusyAction)
                return clearIsNetworkBusyReducer(previousState, (ClearIsNetworkBusyAction)action);
        
            if (action is SetLoginSuccessfulAction)
                return setLoginSuccessfulReducer(previousState, (SetLoginSuccessfulAction)action);

            if (action is SetLoginNotSuccessfulAction)
                return setLoginNotSuccessfulReducer(previousState, (SetLoginNotSuccessfulAction) action);

            if (action is ClearLoginSuccessfullAction)
                return clearLoginSuccessfulReducer(previousState, (ClearLoginSuccessfullAction)action);

            if (action is SetLicenceKeySuccessfulAction)
                return setLicenceKeySuccessfulReducer(previousState, (SetLicenceKeySuccessfulAction)action);

            if (action is SetLicenceKeyNotSuccessfulAction)
                return setLicenceKeyNotSuccessfulReducer(previousState, (SetLicenceKeyNotSuccessfulAction) action);

            if (action is ClearLicenceKeySuccessfulAction)
                return clearLicenceKeySuccessfulReducer(previousState, (ClearLicenceKeySuccessfulAction) action);

            if (action is SetUnauthorizedErrorAction)
                return setUnauthorizedErrorReducer(previousState, (SetUnauthorizedErrorAction) action);

            if (action is ClearUnauthorizedErrorAction)
                return clearUnauthorizedErrorReducer(previousState, (ClearUnauthorizedErrorAction) action);

            if (action is SetReturnToMainPageAction)
                return setReturnToMainPageReducer(previousState, (SetReturnToMainPageAction) action);

            if (action is ClearReturnToMainPageAction)
                return clearReturnToMainPageReducer(previousState, (ClearReturnToMainPageAction) action);

            if (action is SetNeedsUpdateAction)
                return setNeedsUpdateReducer(previousState, (SetNeedsUpdateAction) action);

            if (action is ClearNeedsUpdateAction)
                return clearNeedsUpdateReducer(previousState, (ClearNeedsUpdateAction) action);

            return previousState;
        }

        private EventsState clearNeedsUpdateReducer(EventsState previousState, ClearNeedsUpdateAction action)
        {
            _logService.Info("ClearNeedsUpdateReducer");
            EventsState newState = previousState.Clone();
            newState.NeedsUpdate = false;
            return newState;
        }

        private EventsState setNeedsUpdateReducer(EventsState previousState, SetNeedsUpdateAction action)
        {
            _logService.Info("SetNeedsUpdateReducer");
            EventsState newState = previousState.Clone();
            newState.NeedsUpdate = true;
            return newState;
        }

        private EventsState clearReturnToMainPageReducer(EventsState previousState, ClearReturnToMainPageAction action)
        {
            _logService.Info("ClearReturnToMainPageReducer");
            EventsState newState = previousState.Clone();
            newState.ReturnToMainPage = false;
            return newState;
        }

        private EventsState setReturnToMainPageReducer(EventsState previousState, SetReturnToMainPageAction action)
        {
            _logService.Info("SetReturnToMainPageReducer");
            EventsState newState = previousState.Clone();
            newState.ReturnToMainPage = true;
            return newState;
        }

        private EventsState incrementUnauthorizedCountReducer(EventsState previousState, IncrementUnauthorizedCountAction action)
        {
            _logService.Info("IncrementUnauthorizedCountReducer");
            EventsState newState = previousState.Clone();
            newState.UnauthorizedCount = newState.UnauthorizedCount + 1;
            return newState;
        }

        private EventsState clearUnauthorizedCountReducer(EventsState previousState, ClearUnauthorizedCountAction action)
        {
            _logService.Info("ClearUnauthorizedCountReducer");
            EventsState newState = previousState.Clone();
            newState.UnauthorizedCount = 0;
            return newState;
        }

        private EventsState clearUnauthorizedErrorReducer(EventsState previousState, ClearUnauthorizedErrorAction action)
        {
            _logService.Info("ClearUnauthorizedErrorReducer");
            EventsState newState = previousState.Clone();
            newState.UnauthorizedError = false;
            return newState;
        }

        private EventsState setUnauthorizedErrorReducer(EventsState previousState, SetUnauthorizedErrorAction action)
        {
            _logService.Info("SetUnauthorizedErrorReducer");
            EventsState newState = previousState.Clone();
            newState.UnauthorizedError = true;
            return newState;
        }

        private EventsState clearLicenceKeySuccessfulReducer(EventsState previousState, ClearLicenceKeySuccessfulAction action)
        {
            _logService.Info("ClearLicenceKeySuccessfulReducer");
            EventsState newState = previousState.Clone();
            newState.LicenceKeySuccessful = null;
            return newState;
        }

        private EventsState setLicenceKeyNotSuccessfulReducer(EventsState previousState, SetLicenceKeyNotSuccessfulAction action)
        {
            _logService.Info("SetLicenceKeyNotSuccessfulReducer");
            EventsState newState = previousState.Clone();
            newState.LicenceKeySuccessful = false;
            return newState;
        }

        private EventsState setLicenceKeySuccessfulReducer(EventsState previousState, SetLicenceKeySuccessfulAction action)
        {
            _logService.Info("SetLicenceKeySuccessfulReducer");
            EventsState newState = previousState.Clone();
            newState.LicenceKeySuccessful = true;
            return newState;
        }

        private EventsState clearIsNetworkBusyReducer(EventsState previousState, ClearIsNetworkBusyAction action)
        {
            _logService.Info("ClearIsNetworkBusyReducer");
            EventsState newState = previousState.Clone();
            newState.IsNetworkBusy = false;
            return newState;
        }

        private EventsState setIsNetworkBusyReducer(EventsState previousState, SetIsNetworkBusyAction action)
        {
            _logService.Info("SetIsNetworkBusyReducer");
            EventsState newState = previousState.Clone();
            newState.IsNetworkBusy = true;
            return newState;
        }

        private EventsState setLoginNotSuccessfulReducer(EventsState previousState, SetLoginNotSuccessfulAction action)
        {
            _logService.Info("SetLoginNotSuccessfulReducer");
            EventsState newState = previousState.Clone();
            newState.LoginSuccessful = false;
            return newState;
        }

        private EventsState clearLoginSuccessfulReducer(EventsState previousState, ClearLoginSuccessfullAction action)
        {
            _logService.Info("ClearLoginSuccessfulReducer");
            EventsState newState = previousState.Clone();
            newState.LoginSuccessful = null;
            return newState;
        }

        private EventsState setLoginSuccessfulReducer(EventsState previousState, SetLoginSuccessfulAction action)
        {
            _logService.Info("SetLoginSuccesfulReducer");
            EventsState newState = previousState.Clone();
            newState.LoginSuccessful = true;
            return newState;
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
