using Learni.Core.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learni.UI.Mobile.Extensions;

namespace Learni.UI.Mobile.DataProviders
{
    public interface ICategoriesDataProvider
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategory(int id);
    }

    public class CategoriesDataProvider : DataProviderBase, ICategoriesDataProvider
    {
        public async Task<List<Category>> GetCategories()
        {
            var request = new RestRequest("categories", Method.GET);

            return await RestClient.ExecuteTask<List<Category>>(request);
        }

        public async Task<Category> GetCategory(int id)
        {
            var request = new RestRequest("categories/{categoryId}", Method.GET);
            request.AddUrlSegment("categoryId", id.ToString());

            return await RestClient.ExecuteTask<Category>(request);
        }
    }
}
