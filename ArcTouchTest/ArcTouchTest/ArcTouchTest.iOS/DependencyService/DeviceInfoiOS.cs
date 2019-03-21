using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArcTouchTest.Abstractions;
using Foundation;
using UIKit;

namespace ArcTouchTest.iOS.DependencyService
{
  public class DeviceInfoiOS : IDeviceInfo
  {
    public DeviceOrientation Orientation
    {
      get
      {
        var currentOrientation = UIApplication.SharedApplication.StatusBarOrientation;
        bool isLandscape = currentOrientation == UIInterfaceOrientation.LandscapeLeft ||
                           currentOrientation == UIInterfaceOrientation.LandscapeRight;

        return isLandscape ? DeviceOrientation.Landscape : DeviceOrientation.Portrait;
      }
    }
  }
}