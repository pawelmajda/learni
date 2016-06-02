using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learni.UI.Mobile.Extensions
{
    public static class RestSharpExtensions
    {
        public static Task<IRestResponse> ExecuteTask(this RestClient client, RestRequest request)
        {
            TaskCompletionSource<IRestResponse> taskCompletionSource = new TaskCompletionSource<IRestResponse>();
            client.ExecuteAsync(request, response =>
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    taskCompletionSource.SetException(response.ErrorException);
                }
                else
                {
                    taskCompletionSource.SetResult(response);
                }
            });
            return taskCompletionSource.Task;
        }

        public static Task<T> ExecuteTask<T>(this RestClient client, RestRequest request) where T : new()
        {
            TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
            client.ExecuteAsync<T>(request, response =>
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    taskCompletionSource.SetException(response.ErrorException);
                }
                else
                {
                    taskCompletionSource.SetResult(response.Data);
                }
            });
            return taskCompletionSource.Task;
        }
    }
}
