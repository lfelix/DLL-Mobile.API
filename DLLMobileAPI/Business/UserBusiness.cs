using System;
using System.Collections.Generic;
using System.Configuration;
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
            int tokenTimeoutMins = Int32.Parse(ConfigurationManager.AppSettings["tokenTimeoutMins"]);
            using (ApiContext context = new ApiContext())
            {
                DateTime lastTenMinutes = DateTime.Now.AddMinutes(20);
                isAuthenticatedInAnotherDevice = context.LoginActivities.Any(l => l.IdUser == idUser && l.DeviceId != deviceId && l.LoginDate > lastTenMinutes);
            }

            return isAuthenticatedInAnotherDevice;
        }


        internal void SetLoginActivity(string deviceId, int idUser)
        {
            using (ApiContext context = new ApiContext())
            {
                context.LoginActivities.Add(new LoginActivity
                {
                    IdUser = idUser,
                    DeviceId = deviceId,
                    LoginDate = DateTime.Now
                });
                context.SaveChanges();
            }
        }
    }
}