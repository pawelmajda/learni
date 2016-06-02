using System.Linq;
using Learni.API.Models;
using Learni.Core.Interfaces.DataAccess;
using Learni.Core.Models;
using Learni.Infrastructure.DataAccess;
using Nancy;
using Nancy.Security;
using System.Collections.Generic;
using Nancy.ModelBinding;

namespace Learni.API.Modules
{
    public class PackagesModule : NancyModule
    {
        public PackagesModule(IPackageRepository pacakgeRepository)
        {
            Get["/categories/{id}/packages"] = p => Response.AsJson(pacakgeRepository.GetByCategoryId((int)p.id));
            Get["/packages/{id}"] = p => Response.AsJson(pacakgeRepository.GetById((int)p.id));

            Get["/packages/featured"] = _ => Response.AsJson(pacakgeRepository.GetFeatured());

            Post["/packages"] = _ =>
                {
                    this.RequiresAuthentication();
                    return Response.AsJson(pacakgeRepository.Save(this.Bind<Package>()));
                };

            Delete["/packages/{id}"] = p =>
                {
                    this.RequiresAuthentication();
                    return Response.AsJson(pacakgeRepository.Delete((int)p.id));
                };
        }
    }
}