using Learni.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learni.Core.Interfaces.DataAccess
{
    public interface IUserRepository
    {
        User GetByGuid(Guid guid);
        User GetByName(string name);
        IEnumerable<User> Get();
        User Save(User user);
    }
}
