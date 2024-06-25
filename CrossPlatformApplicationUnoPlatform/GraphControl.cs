using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Microsoft.UI.Xaml.Shapes;

namespace CrossPlatformApplicationUnoPlatform;
public partial class GraphControl : Canvas
{
    private Point[]? _points;

    public GraphControl()
    {
        this.Loaded += GraphControl_Loaded;
    }

    private void GraphControl_Loaded(object sender, RoutedEventArgs e)
    {
        this.Width = 400;
        this.Height = 200;
    }

    public void LoadData()
    {
        Random random = new Random();
        _points = new Point[500];
        for (int i = 0; i < _points.Length; i++)
        {
            _points[i] = new Point(10 + (400 - 20.0) / (_points.Length - 1) * i,
                10 + random.Next(200 - 20));
            PerformIntensiveCalculations();
        }

        DrawGraph();
    }

    public async Task LoadDataAsync()
    {
        _points = new Point[200];
        Random random = new Random();

        for (int i = 0; i < 200; i++)
        {
            _points[i] = new Point(10 + (400 - 20.0) / (200 - 1) * i, 10 + random.Next(200 - 20));
            DrawGraph();
            await Task.Delay(1);
        }
    }

    public void ClearGraph()
    {
        _points = null;
        this.Children.Clear();
    }

    private void PerformIntensiveCalculations()
    {
        double sum = 0;

        // sum is not used, but it's needed to save calculations
        for (int i = 0; i < 1000000; i++)
            sum += Math.Sqrt(i);
    }

    private void DrawGraph()
    {
        this.Children.Clear();

        // Draw X and Y axes
        Line xAxis = new Line
        {
            X1 = 10,
            Y1 = 190,
            X2 = 390,
            Y2 = 190,
            Stroke = new SolidColorBrush(Microsoft.UI.Colors.Black)
        };

        Line yAxis = new Line
        {
            X1 = 10,
            Y1 = 10,
            X2 = 10,
            Y2 = 190,
            Stroke = new SolidColorBrush(Microsoft.UI.Colors.Black)
        };

        this.Children.Add(xAxis);
        this.Children.Add(yAxis);

        if (_points == null)
            return;

        // Draw the graph lines
        for (int i = 0; i < _points.Length - 1; i++)
        {
            Line line = new Line
            {
                X1 = _points[i].X,
                Y1 = _points[i].Y,
                X2 = _points[i + 1].X,
                Y2 = _points[i + 1].Y,
                Stroke = new SolidColorBrush(Microsoft.UI.Colors.Red),
                StrokeThickness = 2
            };
            this.Children.Add(line);
        }

        // Draw the points
        foreach (Point point in _points)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = 2,
                Height = 2,
                Fill = new SolidColorBrush(Microsoft.UI.Colors.Black)
            };
            Canvas.SetLeft(ellipse, point.X - 1);
            Canvas.SetTop(ellipse, point.Y - 1);
            this.Children.Add(ellipse);
        }
    }
}
