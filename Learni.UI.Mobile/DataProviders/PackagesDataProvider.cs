using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learni.UI.Mobile.Extensions;
using Learni.Core.Models;
using Newtonsoft.Json;

namespace Learni.UI.Mobile.DataProviders
{
    public interface IPackagesDataProvider
    {
        Task<Package> GetPackage(int id);
        Task<List<Package>> GetPackagesByCategoryId(int categoryId);
        Task<List<Package>> GetFeaturedPackages();
        Task<Package> CreatePackage(Package newPackage);
        Task<bool> RemovePackage(int id);
    }

    public class PackagesDataProvider : DataProviderBase, IPackagesDataProvider
    {
        public async Task<Package> GetPackage(int id)
        {
            var request = new RestRequest("packages/{packageId}", Method.GET);
            request.AddUrlSegment("packageId", id.ToString());

            return await RestClient.ExecuteTask<Package>(request);
        }

        public async Task<List<Package>> GetPackagesByCategoryId(int categoryId)
        {
            var request = new RestRequest("categories/{categoryId}/packages", Method.GET);
            request.AddUrlSegment("categoryId", categoryId.ToString());

            return await RestClient.ExecuteTask<List<Package>>(request);
        }

        public async Task<List<Package>> GetFeaturedPackages()
        {
            var request = new RestRequest("packages/featured", Method.GET);

            return await RestClient.ExecuteTask<List<Package>>(request);
        }

        public async Task<Package> CreatePackage(Package newPackage)
        {
            var isAuthenticated = await Login();

            if(isAuthenticated)
            {
                var request = new RestRequest("packages", Method.POST).AddBody(newPackage);
                foreach(var cookie in Cookies)
                {
                    request.AddCookie(cookie.Name, cookie.Value);
                }
                
                return await RestClient.ExecuteTask<Package>((RestRequest)request);
            }

            return null;            
        }

        public async Task<bool> RemovePackage(int packageId)
        {
            var isAuthenticated = await Login();

            if (isAuthenticated)
            {
                var request = new RestRequest("packages/{packageId}", Method.DELETE);
                request.AddUrlSegment("packageId", packageId.ToString());

                foreach (var cookie in Cookies)
                {
                    request.AddCookie(cookie.Name, cookie.Value);
                }

                var deletedPackageResponse = await RestClient.ExecuteTask(request);
                var result = JsonConvert.DeserializeObject<bool>(deletedPackageResponse.Content);

                return result;
            }

            return false;
        }
    }
}
