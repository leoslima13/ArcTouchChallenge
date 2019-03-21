using System;
using System.Collections.Generic;
using System.Text;

namespace ArcTouchTest.Utils
{
  public class Constants
  {
    public static class Settings
    {
      public static string ApiKey = "1f54bd990f1cdfb230adb312546d765d";
      public static string MovieDBBaseUrl = "https://api.themoviedb.org/3/";
    }

    public static class Pages
    {
      public static string MainPage = nameof(MainPage);
      public static string MovieDetailPage = nameof(MovieDetailPage);
    }
  }
}
