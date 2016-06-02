using Learni.Core.Interfaces.DataAccess;
using Learni.Core.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learni.Infrastructure.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly dynamic _db = Database.Open();

        public User GetByGuid(Guid guid)
        {
            return _db.Users.FindByGuid(guid.ToString());
        }

        public User GetByName(string name)
        {
            return _db.Users.FindByName(name);
        }

        public IEnumerable<User> Get()
        {
            return _db.Users.All();
        }

        public User Save(User user)
        {
            if (user.Id > 0)
            {
                _db.Users.Update(user);
                return user;
            }
            else
            {
                return _db.Users.Insert(user);
            }
        }
    }
}
