using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kolcordWebApi.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace kolcordWebApi.Tests.Helpers
{
    public class TestDataGenerator
    {
        public static ApplicationUser CreateTestUser(string id = "user1", string userName = "testuser")
        {
            return new ApplicationUser
            {
                Id = id,
                UserName = userName,
                Email = $"{userName}@test.com"
            };
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }
    }
}
