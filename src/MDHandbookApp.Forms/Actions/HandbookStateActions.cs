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


namespace MDHandbookApp.Forms.Actions
{
    public class LoginAction : IAction
    {
        public string UserId { get; set; }
        public string AuthToken { get; set; }
    }
    public class LogoutAction : IAction { }

    public class SetLicenceKeyAction : IAction
    {
        public string LicenceKey { get; set; }
    }
    public class ClearLicenceKeyAction : IAction { }

    public class SetLicensedAction : IAction { }
    public class ClearLicensedAction : IAction { }


    public class SetIsDataUpdatedAction : IAction { }
    public class ClearIsDataUpdatedAction : IAction { }

    public class SetLastUpdateTimeAction : IAction
    {
        public DateTimeOffset UpdateTime { get; set; }
    }

    public class SetRefreshTokenAction : IAction
    {
        public string Token { get; set; }
    }

}
