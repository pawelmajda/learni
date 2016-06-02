using Learni.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learni.Core.Interfaces.DataAccess
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> Get();
        Category GetById(int id);
    }
}
