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


namespace MDHandbookApp.Forms.States
{
    public class EventsState
    {
        public int  UnauthorizedCount { get; set; }

        public bool IsNetworkDown { get; set; }
        public bool IsNetworkBusy { get; set; }

        public bool? LoginSuccessful { get; set; }
        public bool? LicenceKeySuccessful { get; set; }

        public bool UnauthorizedError { get; set; }

        public bool ReturnToMainPage { get; set; }

        public bool NeedsUpdate { get; set; }

        public DateTimeOffset LastNetworkAttempt { get; set; }


        public EventsState()
        {

        }

        protected EventsState(EventsState old)
        {
            this.UnauthorizedCount = old.UnauthorizedCount;

            this.IsNetworkDown = old.IsNetworkDown;
            this.IsNetworkBusy = old.IsNetworkBusy;

            this.LoginSuccessful = old.LoginSuccessful;
            this.LicenceKeySuccessful = old.LicenceKeySuccessful;

            this.UnauthorizedError = old.UnauthorizedError;

            this.ReturnToMainPage = old.ReturnToMainPage;

            this.LastNetworkAttempt = old.LastNetworkAttempt;

            this.NeedsUpdate = old.NeedsUpdate;
        }


        public EventsState Clone()
        {
            return new EventsState(this);
        }


        public static EventsState CreateEmpty()
        {
            return new EventsState {
                UnauthorizedCount = 0,
                IsNetworkDown = false,
                IsNetworkBusy = false,
                LoginSuccessful = null,
                LicenceKeySuccessful = null,
                UnauthorizedError = false,
                ReturnToMainPage = false,
                NeedsUpdate = false,
                LastNetworkAttempt = new System.DateTimeOffset(1970, 1, 1, 0, 0, 0, new System.TimeSpan(-5, 0, 0)),
            };
        }
    }
}
