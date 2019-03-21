using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcTouchTest.Utils
{
  public class JsonHelper<T> where T : class, new()
  {
    public static string ObjectToJson(T obj)
    {
      JsonSerializerSettings _jsonWriter = new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore
      };
      return JsonConvert.SerializeObject(obj, _jsonWriter);
    }

    public static T JsonToObject(string json)
    {
      return JsonConvert.DeserializeObject<T>(json);
    }
  }
}
