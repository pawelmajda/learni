using Learni.API.Helpers;
using Learni.Core.Interfaces.DataAccess;
using Learni.Core.Models;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using System.IO;
using System.Linq;

namespace Learni.API.Modules
{
    public class TermsModule : NancyModule
    {
        private readonly LockScreenGenerator _lockScreenGenerator;

        public TermsModule(ITermRepository termsRepository, IRootPathProvider pathProvider)
        {
            _lockScreenGenerator = new LockScreenGenerator(pathProvider);

            Get["/packages/{id}/terms"] = p => Response.AsJson(termsRepository.GetByPackageId((int)p.id));

            Post["/terms"] = _ => 
                {
                    this.RequiresAuthentication();

                    var term = termsRepository.Save(this.Bind<Term>());
                    _lockScreenGenerator.GenerateLockScreen(term);

                    return Response.AsJson(term);
                };

            Post["/termsimages"] = _ =>
                {
                    //this.RequiresAuthentication();

                    var image = this.Request.Files.FirstOrDefault();

                    if (image != null && image.ContentType.ToLower().Contains("image"))
                    {
                        var imagePath = Path.Combine(pathProvider.GetRootPath(), "Content", "LockScreens", image.Name);

                        using (var fileStream = new FileStream(imagePath, FileMode.Create))
                        {
                            image.Value.CopyTo(fileStream);
                        }

                        return Response.AsJson("Content/LockScreens/" + image.Name, HttpStatusCode.Created);
                    }

                    return HttpStatusCode.BadRequest;
                };

            Delete["/terms/{id}"] = p => 
                {
                    this.RequiresAuthentication();
                    return Response.AsJson(termsRepository.Delete((int)p.id));
                };

        }
    }
}