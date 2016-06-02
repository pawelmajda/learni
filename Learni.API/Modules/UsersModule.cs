using Learni.API.Models;
using Learni.Core.Interfaces.DataAccess;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Authentication.Forms;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Learni.Core.Models;
using Learni.API.Helpers;

namespace Learni.API.Modules
{
    public class UsersModule : NancyModule
    {
        public UsersModule(IUserRepository userRepository)
        {
            Post["/register"] = _ =>
            {
                var registerModel = this.Bind<RegisterModel>();

                var user = new User()
                {
                    Name = registerModel.UserName,
                    Guid = Guid.NewGuid().ToString(),
                    Password = Sha1Helper.CalculateHash(registerModel.Password),
                    Email = registerModel.Email
                };

                userRepository.Save(user);

                return HttpStatusCode.OK;
            };

            Post["/login"] = _ =>
            {
                if (this.Context.CurrentUser != null)
                {
                    this.LogoutWithoutRedirect();
                }

                var loginModel = this.Bind<LoginModel>();
                var user = userRepository.GetByName(loginModel.UserName);

                if (user != null && user.Password == Sha1Helper.CalculateHash(loginModel.Password))
                    return this.LoginWithoutRedirect(Guid.Parse(user.Guid));

                return HttpStatusCode.Unauthorized;
            };

            Get["/logout"] = p =>
            {
                this.RequiresAuthentication();

                if (this.Context.CurrentUser != null)
                    return this.LogoutWithoutRedirect();

                return HttpStatusCode.Conflict;
            };
        }
       
    }
}