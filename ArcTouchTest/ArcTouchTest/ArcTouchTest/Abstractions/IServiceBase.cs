using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ArcTouchTest.Abstractions
{
  /// <summary>
  /// Base implementation to call service
  /// </summary>
  public interface IServiceBase
  {
    /// <summary>
    /// Url base that will be requested
    /// </summary>
    string BaseUrl { get; }

    /// <summary>
    /// Request via GET using HttpClient
    /// </summary>
    /// <typeparam name="T">Return Type</typeparam>
    /// <param name="url">Url</param>
    /// <param name="queryString">QueryString</param>
    /// <returns></returns>
    Task<BaseResponse<T>> GetAsync<T>(string url, string queryString = null) where T : class, new();
  }
}
