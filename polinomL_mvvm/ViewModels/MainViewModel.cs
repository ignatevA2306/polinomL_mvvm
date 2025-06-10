using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using polinomL_mvvm.Models;
using MyPoint = polinomL_mvvm.Models.Point;
namespace polinomL_mvvm.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private ObservableCollection<MyPoint> _points = new();
    private string _polynomial = "Полином появится здесь";

    public ObservableCollection<Point> Points
    {
        get => _points;
        set
        {
            _points = value;
            OnPropertyChanged(nameof(Points));
            UpdatePolynomial();
        }
    }

    public string Polynomial
    {
        get => _polynomial;
        set
        {
            _polynomial = value;
            OnPropertyChanged(nameof(Polynomial));
        }
    }

    public ICommand AddPointCommand { get; }
    public ICommand ClearCommand { get; }

    public MainViewModel()
    {
        AddPointCommand = new RelayCommand(AddPoint);
        ClearCommand = new RelayCommand(ClearPoints);
    }

    private void AddPoint(object parameter)
    {
        if (parameter is MouseEventArgs mouseArgs)
        {
            // Преобразуем координаты мыши в декартовы
            var canvas = mouseArgs.Source as Canvas;
            var mousePosition = mouseArgs.GetPosition(canvas);

            double x = mousePosition.X - 400;  // Центр по X
            double y = 300 - mousePosition.Y;  // Инвертируем Y

            Points.Add(new Point(x, y));
            UpdatePolynomial();
        }
        else if (parameter is Point point)
        {
            Points.Add(point);
            UpdatePolynomial();
        }
    }

    private void ClearPoints(object parameter)
    {
        Points.Clear();
        Polynomial = "Полином появится здесь";
    }

    private void UpdatePolynomial()
    {
        if (Points.Count < 2)
        {
            Polynomial = "Нужно минимум 2 точки";
            return;
        }
        Polynomial = CalculateLagrangePolynomial();
    }
    /*
    private string CalculateLagrangePolynomial()
    {
        string result = "L(x) = ";
        for (int i = 0; i < Points.Count; i++)
        {
            double xi = Points[i].X;
            double yi = Points[i].Y;

            string term = $"{yi} * ";
            for (int j = 0; j < Points.Count; j++)
            {
                if (j != i)
                {
                    double xj = Points[j].X;
                    term += $"(x - {xj}) / ({xi} - {xj}) * ";
                }
            }
            term = term.TrimEnd(' ', '*');
            result += $"{term} + ";
        }
        return result.TrimEnd(' ', '+');
    }*/

    private string CalculateLagrangePolynomial()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Полином Лагранжа:\n");
        sb.AppendLine("L(x) = ");

        for (int i = 0; i < Points.Count; i++)
        {
            double xi = Math.Round(Points[i].X, 3);
            double yi = Math.Round(Points[i].Y, 3);

            sb.Append($"{yi:0.###} * ");

            for (int j = 0; j < Points.Count; j++)
            {
                if (j != i)
                {
                    double xj = Math.Round(Points[j].X, 3);
                    sb.Append($"(x - {xj:0.###})/({xi:0.###} - {xj:0.###}) * ");
                }
            }

            sb.Remove(sb.Length - 3, 3); // Удаляем последний " * "
            sb.AppendLine(" +");
        }

        sb.Remove(sb.Length - 3, 3); // Удаляем последний " +"
        return sb.ToString();
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}