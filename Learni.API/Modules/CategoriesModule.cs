using Learni.Core.Interfaces.DataAccess;
using Nancy;
using Nancy.Security;

namespace Learni.API.Modules
{
    public class CategoriesModule : NancyModule
    {
        public CategoriesModule(ICategoryRepository categoryRepository) : base("/categories")
        {
            Get["/"] = _ => Response.AsJson(categoryRepository.Get());
            Get["/{id}"] = p => Response.AsJson(categoryRepository.GetById((int)p.id));
        }
    }
}