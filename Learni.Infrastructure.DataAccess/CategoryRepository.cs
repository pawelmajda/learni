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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly dynamic _db = Database.Open();
        public IEnumerable<Category> Get()
        {
            return _db.Category.All();
        }
        public Category GetById(int id)
        {
            return _db.Category.FindById(id);
        }
    }
}
