using Learni.UI.Mobile.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learni.UI.Mobile.DataProviders
{
    public class DataProviderBase
    {
        //public static string ApiUrl = "http://169.254.80.80:59643/api/"; //Emulator
        public static string ApiUrl = "http://192.168.1.40:59643/api/"; //Device

        protected static RestClient RestClient = new RestClient(ApiUrl);
        protected static bool IsAuthenticated = false;
        protected static IList<RestResponseCookie> Cookies;
        
        public DataProviderBase()
        {
        }

        protected async Task<bool> Login()
        {
            if(!IsAuthenticated)
            {
                var loginModel = new LoginModel()
                {
                };

                var request = new RestRequest("login", Method.POST).AddBody(loginModel);

                TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();
                RestClient.ExecuteAsync(request, response =>
                {
                    if (response.ResponseStatus != ResponseStatus.Error)
                    {
                        IsAuthenticated = true;
                        Cookies = response.Cookies;
                    }

                    taskCompletionSource.SetResult(IsAuthenticated);

                });
                return await taskCompletionSource.Task;
            }

            return IsAuthenticated;
        }
    }
}
