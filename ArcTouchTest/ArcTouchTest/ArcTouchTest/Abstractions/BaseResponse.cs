using System;
using System.Collections.Generic;
using System.Text;

namespace ArcTouchTest.Abstractions
{
  public class BaseResponse<T> where T : class, new()
  {
    public bool IsSuccess => Exception == null;

    public T Object { get; set; }

    public int StatusCode { get; set; }

    public Exception Exception { get; set; }
  }
}
