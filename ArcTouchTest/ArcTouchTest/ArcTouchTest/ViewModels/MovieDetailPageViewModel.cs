using Acr.UserDialogs;
using ArcTouchTest.Abstractions;
using ArcTouchTest.Model;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcTouchTest.ViewModels
{
    public class MovieDetailPageViewModel : ViewModelBase
    {
        private IMoviesService MoviesService { get; set; }

        public MovieDetailPageViewModel(INavigationService navigationService,
                                        IMoviesService moviesService) : base(navigationService)
        {
            MoviesService = moviesService;
        }

        private Movie _model;
        public Movie Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        private List<Genre> _genres;
        public List<Genre> Genres
        {
            get { return _genres; }
            set { SetProperty(ref _genres, value); }
        }

        private List<Company> _productionCompanies;
        public List<Company> ProductionCompanies
        {
            get { return _productionCompanies; }
            set { SetProperty(ref _productionCompanies, value); }
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey(nameof(Movie)))
            {
                Model = parameters[nameof(Movie)] as Movie;
            }

            await LoadItems();
        }

        async Task LoadItems()
        {
            IsBusy = true;
            var response = await MoviesService.GetAsync<MovieDetail>($"movie/{Model.Id}");
            if (response.IsSuccess)
            {
                ProductionCompanies = response.Object.Companies;
                Genres = response.Object.Genres;
            }
            else
            {
                string msg = "Ops... Something is wrong. Try again later";
                if (CrossConnectivity.Current.IsConnected)
                    msg = "Ops... No internet connection, please verify...";

                UserDialogs.Instance.Toast(msg);
            }
            IsBusy = false;
        }

        public override async void InternetConnectionChanged(bool isConnected)
        {
            if (!isConnected && Genres?.Count == 0)
                UserDialogs.Instance.Toast("Ops... No internet connection, please verify");
            else if (isConnected && Genres?.Count == 0)
                await LoadItems();
        }
    }
}
