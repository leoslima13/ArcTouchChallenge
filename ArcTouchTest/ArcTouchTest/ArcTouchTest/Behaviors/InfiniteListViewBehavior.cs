using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace ArcTouchTest.Behaviors
{
  /// <summary>
  /// Behavior to infinite listview, scrolling and get more data
  /// </summary>
  public class InfiniteListViewBehavior : Behavior<ListView>
  {
    /// <summary>
    /// Object that will be use behavior
    /// </summary>
    public ListView AssociatedObject { get; private set; }

    private CancellationTokenSource ListCancelationToken = new CancellationTokenSource();

    public static readonly BindableProperty ThresholdProperty =
    BindableProperty.Create(nameof(Threshold), typeof(int), typeof(InfiniteListViewBehavior), 0);

    public static readonly BindableProperty LoadMoreCommandProperty =
        BindableProperty.Create(nameof(LoadMoreCommand), typeof(ICommand), typeof(InfiniteListViewBehavior), null);

    public int Threshold
    {
      get { return (int)GetValue(ThresholdProperty); }
      set { SetValue(ThresholdProperty, value); }
    }

    public ICommand LoadMoreCommand
    {
      get { return (ICommand)GetValue(LoadMoreCommandProperty); }
      set { SetValue(LoadMoreCommandProperty, value); }
    }

    protected override void OnAttachedTo(ListView bindable)
    {
      base.OnAttachedTo(bindable);

      AssociatedObject = bindable;
      AssociatedObject.ItemAppearing += OnItemAppearing;
      AssociatedObject.ItemSelected += OnItemSelected;
    }

    protected override void OnDetachingFrom(ListView bindable)
    {
      base.OnDetachingFrom(bindable);
      AssociatedObject.ItemAppearing -= OnItemAppearing;
      AssociatedObject.ItemSelected -= OnItemSelected;
      AssociatedObject = null;
    }

    private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      (sender as ListView).SelectedItem = null;
    }

    private void OnItemAppearing(object sender, ItemVisibilityEventArgs e)
    {
      var items = AssociatedObject.ItemsSource as IList;

      if (items == null || Threshold > items?.Count - 1)
        return;

      if(e.Item == items[items.Count - 1 - Threshold])
        if(LoadMoreCommand != null && LoadMoreCommand.CanExecute(null) && !AssociatedObject.IsRefreshing)
        {
          LoadMoreCommand.Execute(null);
        }
    }

  }
}
