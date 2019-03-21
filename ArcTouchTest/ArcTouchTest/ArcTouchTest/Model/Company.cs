using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcTouchTest.Model
{
  public class Company
  {
    public string Name { get; set; }

    [JsonProperty("logo_path")]
    public string LogoPath { get; set; }
  }
}
