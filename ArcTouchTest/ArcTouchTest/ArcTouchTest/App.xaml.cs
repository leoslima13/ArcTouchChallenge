using Prism;
using Prism.Ioc;
using ArcTouchTest.ViewModels;
using ArcTouchTest.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using DLToolkit.Forms.Controls;
using System;
using ArcTouchTest.Abstractions;
using ArcTouchTest.Implementations;
using DryIoc;
using Plugin.Connectivity;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ArcTouchTest
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            FlowListView.Init();

            CrossConnectivity.Current.ConnectivityChanged += (sender, e) =>
            {
                var bindingContext = (MainPage as NavigationPage).CurrentPage.BindingContext;
                if (bindingContext != null && bindingContext is ViewModelBase vmb)
                    vmb.InternetConnectionChanged(e.IsConnected);
            };
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterServices(containerRegistry);
            RegisterPages(containerRegistry);
        }

        private void RegisterPages(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MovieDetailPage, MovieDetailPageViewModel>();
        }

        private void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMoviesService, MoviesService>();
        }
    }
}
