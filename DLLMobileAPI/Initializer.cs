using System;
using System.Data.Entity;

namespace DLLMobileAPI
{
    internal class Initializer : DropCreateDatabaseIfModelChanges<ApiContext>
    {
        protected override void Seed(ApiContext context)
        {
            var newUser = new ApplicationUser
            {
                Id = 1,
                Name = "Lucas Felix de Oliveira",
                UserName = "lucasfelix",
                Password = "123123"
            };

            context.Users.Add(newUser);

            var newLoginActivity = new LoginActivity
            {
                Id = 1,
                DeviceId = "123123",
                IdUser = 1,
                LoginDate = DateTime.Now
            };

            context.LoginActivities.Add(newLoginActivity);

            context.SaveChanges();
        }
    }
}