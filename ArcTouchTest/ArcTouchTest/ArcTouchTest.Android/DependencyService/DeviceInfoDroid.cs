using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ArcTouchTest.Abstractions;

namespace ArcTouchTest.Droid.DependencyService
{
  public class DeviceInfoDroid : IDeviceInfo
  {
    public DeviceOrientation Orientation
    {
      get
      {
        IWindowManager windowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
        var rotation = windowManager.DefaultDisplay.Rotation;
        bool isLandscape = rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270;
        return isLandscape ? DeviceOrientation.Landscape : DeviceOrientation.Portrait;
      }
    }
  }
}