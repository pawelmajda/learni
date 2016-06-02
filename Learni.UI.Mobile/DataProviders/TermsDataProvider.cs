using Learni.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learni.UI.Mobile.Extensions;
using Learni.UI.Mobile.ViewModels;
using RestSharp;

namespace Learni.UI.Mobile.DataProviders
{
    public interface ITermsDataProvider
    {
        Task<List<TermViewModel>> GetTermsByPackageId(int packageId);
        Task<Term> CreateTerm(Term newTerm);
    }

    public class TermsDataProvider : DataProviderBase, ITermsDataProvider
    {
        public async Task<List<TermViewModel>> GetTermsByPackageId(int packageId)
        {
            var request = new RestRequest("packages/{packageId}/terms?refresh=" + Guid.NewGuid().ToString(), Method.GET);
            request.AddUrlSegment("packageId", packageId.ToString());

            return await RestClient.ExecuteTask<List<TermViewModel>>(request);
        }

        public async Task<Term> CreateTerm(Term newTerm)
        {
            var isAuthenticated = await Login();

            if (isAuthenticated)
            {
                var request = new RestRequest("terms", Method.POST).AddBody(newTerm);
                foreach (var cookie in Cookies)
                {
                    request.AddCookie(cookie.Name, cookie.Value);
                }

                return await RestClient.ExecuteTask<Term>((RestRequest)request);
            }

            return null;
        }
    }
}
