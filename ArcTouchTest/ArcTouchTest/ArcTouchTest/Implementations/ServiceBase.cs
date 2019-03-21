using ArcTouchTest.Abstractions;
using ArcTouchTest.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArcTouchTest.Implementations
{
  /// <summary>
  /// Implementations of IServiceBase
  /// </summary>
  public abstract class ServiceBase : IServiceBase
  {
    /// <summary>
    /// Constructor with base url
    /// </summary>
    /// <param name="baseURL">Base Url that will be requested</param>
    public ServiceBase(string baseURL)
    {
      BaseUrl = baseURL;
    }

    private string _baseUrl;
    public string BaseUrl
    {
      get => _baseUrl;
      private set => _baseUrl = value;
    }

    public async Task<BaseResponse<T>> GetAsync<T>(string url, string queryString = null) where T : class, new()
    {
      var response = new HttpResponseMessage();

      try
      {
        using (var httpClient = new HttpClient())
        {
          httpClient.BaseAddress = new Uri(BaseUrl);

          url = $"{url}?api_key={Constants.Settings.ApiKey}";

          if (!string.IsNullOrEmpty(queryString))
            url = $"{url}&{queryString}";

          response = await httpClient.GetAsync(url);

          if (response.IsSuccessStatusCode)
          {
            var responseString = await response.Content.ReadAsStringAsync();

            var obj = JsonHelper<T>.JsonToObject(responseString);

            return new BaseResponse<T> { Object = obj };
          }
          else
          {
            return new BaseResponse<T>
            {
              StatusCode = (int)response.StatusCode,
              Exception = new Exception(await response.Content.ReadAsStringAsync())
            };
          }
        }
      }
      catch (Exception ex)
      {
        return new BaseResponse<T> { Exception = ex, StatusCode = 0 };
      }
    }
  }
}
