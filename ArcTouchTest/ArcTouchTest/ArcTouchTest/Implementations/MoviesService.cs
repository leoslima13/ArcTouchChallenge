using ArcTouchTest.Abstractions;
using ArcTouchTest.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcTouchTest.Implementations
{
  public class MoviesService : ServiceBase, IMoviesService
  {
    public MoviesService() : base(Constants.Settings.MovieDBBaseUrl)
    {
    }
  }
}
