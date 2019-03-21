using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcTouchTest.Model
{
  public class MovieDetail
  {
    public List<Genre> Genres { get; set; }

    [JsonProperty("production_companies")]
    public List<Company> Companies { get; set; }

  }
}
