using Learni.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data;

namespace Learni.Core.Interfaces.DataAccess
{
    public interface IPackageRepository
    {
        IEnumerable<Package> GetByCategoryId(int categoryId);
        IEnumerable<Package> GetFeatured(); 
        Package GetById(int packageId);
        Package Save(Package package);
        int Delete(int packageId);
    }
}
