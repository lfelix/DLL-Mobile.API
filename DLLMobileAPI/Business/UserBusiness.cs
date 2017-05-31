using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace DLLMobileAPI.Business
{
    public class UserBusiness
    {
        public ApplicationUser Authtenticate(string userName, string password)
        {
            ApplicationUser user = null;
            using (ApiContext context = new ApiContext())
            {
                user = context.Users.FirstOrDefault(u => u.UserName == userName && u.Password == password);
            }
            
            return user;
        }

        internal bool AuthenticatedInAnotherDevice(string deviceId, int idUser)
        {
            bool isAuthenticatedInAnotherDevice = false;
            int tokenTimeoutMins = int.Parse(ConfigurationManager.AppSettings["tokenTimeoutMins"]);
            using (ApiContext context = new ApiContext())
            {                
                DateTime tokenTimeout = DateTime.UtcNow.AddMinutes(-tokenTimeoutMins);
                isAuthenticatedInAnotherDevice = context.LoginActivities.Any(l => l.IdUser == idUser && l.DeviceId != deviceId && l.LoginDate > tokenTimeout);
            }

            return isAuthenticatedInAnotherDevice;
        }


        internal void SetLoginActivity(string deviceId, int idUser)
        {
            using (ApiContext context = new ApiContext())
            {
                var loginActivity = context.LoginActivities.FirstOrDefault(a => a.DeviceId == deviceId && a.IdUser == idUser);

                if (loginActivity == null)
                {
                    loginActivity = new LoginActivity
                    {
                        IdUser = idUser,
                        DeviceId = deviceId
                    };
                }
                
                loginActivity.LoginDate = DateTime.UtcNow;

                context.LoginActivities.AddOrUpdate(loginActivity);
                context.SaveChanges();
            }
        }
    }
}