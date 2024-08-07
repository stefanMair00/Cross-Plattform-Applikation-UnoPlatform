using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using CrossPlatformApplicationUnoPlatform;
using Microsoft.UI.Dispatching;
using Windows.UI.ViewManagement;

namespace CrossPlatformApplicationUnoPlatform;

public sealed partial class MainPage : Page
{
    private const string SecondsUnit = "s";
    private const string MicroSecondsUnit = "μs";
    private GraphControl? _leftGraphControl;
    private GraphControl? _rightGraphControl;

    public MainPage()
    {
        this.InitializeComponent();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        Setup();
        StartUpLoading();

        stopwatch.Stop();

        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            TextBlock? startUpTimeTextBlock = FindName("StartUpTimeTextBlock") as TextBlock;
            if (startUpTimeTextBlock == null)
                return;

            startUpTimeTextBlock.Text = $"{Math.Round(stopwatch.Elapsed.TotalSeconds, 2)} {SecondsUnit}";
        });
    }

    private void Setup()
    {
        _leftGraphControl = FindName("LeftGraphCanvas") as GraphControl;
        _rightGraphControl = FindName("RightGraphCanvas") as GraphControl;
    }

    private void StartUpLoading()
    {
        _leftGraphControl?.LoadData();
        _rightGraphControl?.LoadData();
    }

    private async void LoadData(object sender, RoutedEventArgs e)
    {
        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            TextBlock? loadTimeTextBlock = FindName("LoadTimeTextBlock") as TextBlock;
            if (loadTimeTextBlock == null)
                return;

            loadTimeTextBlock.Text = $"0 {SecondsUnit}";

            Grid? loadingGridVisible = FindName("LoadingGrid") as Grid;
            if (loadingGridVisible == null)
                return;

            loadingGridVisible.Visibility = Visibility.Visible;
        });

        await Task.Delay(10);

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        _leftGraphControl?.LoadData();
        _rightGraphControl?.LoadData();

        stopwatch.Stop();

        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            TextBlock? loadTimeTextBlock = FindName("LoadTimeTextBlock") as TextBlock;
            if (loadTimeTextBlock == null)
                return;

            loadTimeTextBlock.Text = $"{Math.Round(stopwatch.Elapsed.TotalSeconds, 2)} {SecondsUnit}";

            Grid? loadingGridVisible = FindName("LoadingGrid") as Grid;
            if (loadingGridVisible == null)
                return;

            loadingGridVisible.Visibility = Visibility.Collapsed;
        });
    }

    private async void StartMultiThreadingAsync(object sender, RoutedEventArgs e)
    {
        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            TextBlock? multiThreadingTimeTextBlock = FindName("MultiThreadingTimeTextBlock") as TextBlock;
            if (multiThreadingTimeTextBlock == null)
                return;

            multiThreadingTimeTextBlock.Text = $"0 {SecondsUnit}";

            Button? isDeleteGraphEnabled = FindName("DeleteGraphButton") as Button;
            if (isDeleteGraphEnabled == null)
                return;

            isDeleteGraphEnabled.IsEnabled = false;

            Button? isLoadDataEnabled = FindName("LoadDataButton") as Button;
            if (isLoadDataEnabled == null)
                return;

            isLoadDataEnabled.IsEnabled = false;

            Button? isMultiThreadingEnabled = FindName("MultiThreadingButton") as Button;
            if (isMultiThreadingEnabled == null)
                return;

            isMultiThreadingEnabled.IsEnabled = false;
        });

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            await Task.WhenAll(_leftGraphControl?.LoadDataAsync() ?? throw new InvalidOperationException(), _rightGraphControl?.LoadDataAsync() ?? throw new InvalidOperationException());
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }

        stopwatch.Stop();

        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            TextBlock? multiThreadingTimeTextBlock = FindName("MultiThreadingTimeTextBlock") as TextBlock;
            if (multiThreadingTimeTextBlock == null)
                return;

            multiThreadingTimeTextBlock.Text = $"{Math.Round(stopwatch.Elapsed.TotalSeconds, 2)} {SecondsUnit}";

            Button? isDeleteGraphEnabled = FindName("DeleteGraphButton") as Button;
            if (isDeleteGraphEnabled == null)
                return;

            isDeleteGraphEnabled.IsEnabled = true;

            Button? isLoadDataEnabled = FindName("LoadDataButton") as Button;
            if (isLoadDataEnabled == null)
                return;

            isLoadDataEnabled.IsEnabled = true;

            Button? isMultiThreadingEnabled = FindName("MultiThreadingButton") as Button;
            if (isMultiThreadingEnabled == null)
                return;

            isMultiThreadingEnabled.IsEnabled = true;
        });
    }

    private void ClearGraph(object sender, RoutedEventArgs e)
    {
        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            TextBlock? deleteTimeTextBlock = FindName("DeleteTimeTextBlock") as TextBlock;
            if (deleteTimeTextBlock == null)
                return;

            deleteTimeTextBlock.Text = $"0 {MicroSecondsUnit}";
        });

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        _leftGraphControl?.ClearGraph();
        Task.Delay(1);
        _rightGraphControl?.ClearGraph();

        stopwatch.Stop();

        DispatcherQueue.GetForCurrentThread().TryEnqueue(() =>
        {
            TextBlock? deleteTimeTextBlock = FindName("DeleteTimeTextBlock") as TextBlock;
            if (deleteTimeTextBlock == null)
                return;

            deleteTimeTextBlock.Text = $"{stopwatch.Elapsed.Microseconds} {MicroSecondsUnit}";
        });
    }
}
