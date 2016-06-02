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
    public class PackageRepository : IPackageRepository
    {
        private readonly dynamic _db = Database.Open();
        
        public IEnumerable<Package> GetByCategoryId(int categoryId)
        {
            return _db.Packages.FindAllByCategoryId(categoryId);
        }

        public IEnumerable<Package> GetFeatured()
        {
            return _db.Packages.FindAllBy(IsFeatured: true).Select(_db.Packages.AllColumns(), _db.Packages.Category.Name.As("CategoryName"));
        }

        public Package GetById(int packageId)
        {
            return _db.Packages.FindById(packageId);
        }

        public Package Save(Package package)
        {
            if (package.Id > 0)
            {
                _db.Packages.Update(package);
                return package;
            }
            else
            {
                return _db.Packages.Insert(package);
            }
        }

        public int Delete(int packageId)
        {
            IEnumerable<Term> relatedTerms = _db.Terms.FindAllByPackageId(packageId);
            foreach (var term in relatedTerms)
            {
                _db.Terms.DeleteById(term.Id);
            }

            return _db.Packages.DeleteById(packageId);
        }
    }
}
