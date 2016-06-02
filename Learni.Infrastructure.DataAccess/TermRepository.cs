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
    public class TermRepository : ITermRepository
    {
        private readonly dynamic _db = Database.Open();
        public IEnumerable<Term> GetByPackageId(int packageId)
        {
            return _db.Terms.FindAllByPackageId(packageId);
        }

        public Term Save(Term term)
        {
            if (term.Id > 0)
            {
                _db.Terms.Update(term);
                return term;
            }
            else
            {
                return _db.Terms.Insert(term);
            }
        }

        public int Delete(int termId)
        {
            return _db.Terms.DeleteById(termId);
        }
    }
}
