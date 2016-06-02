using Learni.API.Models;
using Learni.Core.Interfaces.DataAccess;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Learni.API.Helpers
{
    public class UserMapper : IUserMapper
    {
        private readonly IUserRepository _userRepository;

        public UserMapper(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var user = _userRepository.GetByGuid(identifier);

            if (user == null)
                return null;

            return new UserModel
            {
                UserName = user.Name,
                Claims = new[]
                {
                    "User"
                }
            };
        }
    }
}