using System;
using System.Collections.Generic;
using System.Text;

namespace ArcTouchTest.Abstractions
{
  public interface IDeviceInfo
  {
    DeviceOrientation Orientation { get; }
  }

  public enum DeviceOrientation
  {
    Landscape,
    Portrait
  }
}
