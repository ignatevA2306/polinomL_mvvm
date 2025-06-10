using polinomL_mvvm.ViewModels;
using polinomL_mvvm.Models;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace polinomL_mvvm.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext is MainViewModel viewModel)
            {
                var canvas = (Canvas)sender;
                var mousePosition = e.GetPosition(canvas);

                // Явно указываем пространство имён
                var point = new polinomL_mvvm.Models.Point(mousePosition.X, mousePosition.Y);
                viewModel.AddPointCommand.Execute(point);

                var ellipse = new System.Windows.Shapes.Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = System.Windows.Media.Brushes.Red,
                    Stroke = System.Windows.Media.Brushes.Black
                };

                Canvas.SetLeft(ellipse, mousePosition.X - 4);
                Canvas.SetTop(ellipse, mousePosition.Y - 4);
                canvas.Children.Add(ellipse);
            }
        }
    }
    
}