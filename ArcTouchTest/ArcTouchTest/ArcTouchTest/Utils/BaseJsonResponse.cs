using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcTouchTest.Utils
{
  public class BaseJsonResponse<T>
  {
    [JsonProperty("results")]
    public IList<T> Results { get; set; }

    [JsonProperty("page")]
    public int Page { get; set; }

    [JsonProperty("total_results")]
    public int TotalResults { get; set; }

    [JsonProperty("total_pages")]
    public int TotalPages { get; set; }
  }
}
