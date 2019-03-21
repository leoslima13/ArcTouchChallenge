using Acr.UserDialogs;
using ArcTouchTest.Abstractions;
using ArcTouchTest.Model;
using ArcTouchTest.Utils;
using Plugin.Connectivity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArcTouchTest.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private int _currentPage;
        private IMoviesService MoviesService { get; set; }
        private IDeviceInfo DeviceInfo { get; set; }
        private DeviceOrientation CurrentOrientation { get; set; }
        private List<Movie> AllItems { get; set; }
        public SearchBar SearchBar { get; set; }

        public MainPageViewModel(INavigationService navigationService,
                             IMoviesService moviesService,
                             IDeviceInfo deviceInfo) : base(navigationService)
        {
            Title = "Upcoming Movies";
            MoviesService = moviesService;
            DeviceInfo = deviceInfo;
            CurrentOrientation = DeviceInfo.Orientation;
        }

        #region Properties
        private int _columnCount;
        public int ColumnCount
        {
            get { return _columnCount; }
            set { SetProperty(ref _columnCount, value); }
        }

        private ObservableCollection<Movie> _items;
        public ObservableCollection<Movie> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private bool _isBusyInfinite;
        public bool IsBusyInfinite
        {
            get { return _isBusyInfinite; }
            set { SetProperty(ref _isBusyInfinite, value); }
        }

        private bool _hasItems = true;
        public bool HasItems
        {
            get { return _hasItems; }
            set { SetProperty(ref _hasItems, value); RaisePropertyChanged(nameof(NoItemsAndNoConnection)); }
        }

        private int _totalRecords;
        public int TotalRecords
        {
            get { return _totalRecords; }
            set { SetProperty(ref _totalRecords, value); }
        }

        private bool _isSearching;
        public bool IsSearching
        {
            get { return _isSearching; }
            set { SetProperty(ref _isSearching, value); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); SearchTextChanged(); }
        }

        public bool NoItemsAndNoConnection => !HasItems && !CrossConnectivity.Current.IsConnected;
        public bool NoItemsFound => AllItems?.Count > 0 && (Items == null || Items?.Count == 0);
        #endregion

        #region Commands
        public DelegateCommand LoadMoreCommand =>
       new DelegateCommand(async () => await LoadMore());

        public DelegateCommand RefreshCommand =>
          new DelegateCommand(() => IsBusy = false);

        public DelegateCommand<Movie> SelectedItemCommand =>
          new DelegateCommand<Movie>(async (obj) =>
          {
              var navParams = new NavigationParameters { { nameof(Movie), obj } };
              await NavigationService.NavigateAsync(Constants.Pages.MovieDetailPage, navParams);
          });

        public DelegateCommand SearchButtonCommand =>
            new DelegateCommand(() =>
            {
                IsSearching = !IsSearching;
                if (IsSearching)
                    SearchBar.Focus();
                else
                    SearchBar.Unfocus();
            });

        public DelegateCommand SearchCommand =>
            new DelegateCommand(SearchItem);
        #endregion

        #region Methods

        private void SearchItem()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
                Items = new ObservableCollection<Movie>(AllItems);
            else
                Items = new ObservableCollection<Movie>(AllItems.Where(i => i.Title.ToUpper()
                            .StartsWith(SearchText.ToUpper().TrimEnd(), StringComparison.Ordinal)));

            HasItems = Items?.Count > 0;
            RaisePropertyChanged(nameof(NoItemsFound));
        }

        public void SearchTextChanged()
        {
            var searchCancelationToken = new CancellationTokenSource();
            Interlocked.Exchange(ref searchCancelationToken, new CancellationTokenSource()).Cancel();

            Task.Delay(1000, searchCancelationToken.Token)
                .ContinueWith(delegate
                {
                    SearchItem();
                },
                  CancellationToken.None,
                  TaskContinuationOptions.OnlyOnRanToCompletion,
                  TaskScheduler.FromCurrentSynchronizationContext()
                 );
        }

        public void SetCurrentOrientation(DeviceOrientation orientation)
        {
            if (orientation != CurrentOrientation)
            {
                CurrentOrientation = orientation;
                if (orientation == DeviceOrientation.Landscape)
                    ColumnCount = 3;
                else
                    ColumnCount = 2;
            }
        }

        public override async void OnNavigatingTo(INavigationParameters parameters)
        {
            if (CurrentOrientation == DeviceOrientation.Landscape)
                ColumnCount = 3;
            else
                ColumnCount = 2;

            if ((Items == null || Items.Count == 0) && CrossConnectivity.Current.IsConnected)
            {

                IsBusy = true;
                var result = await LoadItems(++_currentPage);
                if (result != null)
                {
                    AllItems = result;
                    Items = new ObservableCollection<Movie>(AllItems);
                }
                IsBusy = false;

            }
            HasItems = Items?.Count > 0;
        }

        private async Task LoadMore()
        {
            if (IsSearching || !CrossConnectivity.Current.IsConnected)
                return;

            IsBusyInfinite = true;
            var result = await LoadItems(++_currentPage);

            if (result != null)
            {
                AllItems.AddRange(result);
                foreach (var item in result)
                    Items.Add(item);
            }

            IsBusyInfinite = false;
        }

        async Task<List<Movie>> LoadItems(int page)
        {
            var result = await MoviesService.GetAsync<BaseJsonResponse<Movie>>("movie/upcoming", $"page={page}");
            if (result.IsSuccess)
            {
                TotalRecords = result.Object.TotalResults;
                return result.Object.Results.ToList();
            }
            else
            {
                UserDialogs.Instance.Toast("Ops... Something is wrong. Try again later", TimeSpan.FromSeconds(3));
                return null;
            }
        }

        public override async void InternetConnectionChanged(bool isConnected)
        {
            RaisePropertyChanged(nameof(NoItemsAndNoConnection));

            if (!isConnected && HasItems)
                UserDialogs.Instance.Toast("Ops! Check your internet connection...");
            else if (isConnected && (AllItems == null || AllItems?.Count == 0))
            {
                IsBusy = true;
                var result = await LoadItems(1);
                HasItems = result?.Count > 0;
                if (result != null)
                {
                    AllItems = result;
                    Items = new ObservableCollection<Movie>(AllItems);
                }
                IsBusy = false;
            }
        }
        #endregion
    }
}
