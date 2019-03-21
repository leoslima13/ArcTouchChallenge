using ArcTouchTest.Abstractions;
using ArcTouchTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArcTouchTest.Views
{
  public partial class MainPage : ContentPage
  {
    public MainPage()
    {
      InitializeComponent();
    }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null && BindingContext is MainPageViewModel mpvm)
                mpvm.SearchBar = searchBar;
        }

        protected override void OnSizeAllocated(double width, double height)
    {
      base.OnSizeAllocated(width, height);

      if(BindingContext != null && BindingContext is MainPageViewModel mpvm)
      {
        var orientation = DeviceOrientation.Portrait;
        if (width > height)
          orientation = DeviceOrientation.Landscape;

        mpvm.SetCurrentOrientation(orientation);
      }
    }
  }
}