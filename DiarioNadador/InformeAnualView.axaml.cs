using System;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using DiarioNadador.Core;

namespace DiarioNadador;

public partial class InformeAnualView : UserControl
{
    private const int Margen = 25;
    private const int FilasY = 10;
    private const int ValorFila = 15;

    private static readonly string[] Meses =
    {
        "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
        "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"
    };

    public static readonly StyledProperty<DiarioEntrenamiento> DiarioEntrenamientoProperty =
        AvaloniaProperty.Register<InformeAnualView, DiarioEntrenamiento>(
            nameof(DiarioEntrenamiento));

    public InformeAnualView()
    {
        InitializeComponent();
    }

    public required DiarioEntrenamiento DiarioEntrenamiento
    {
        get => GetValue(DiarioEntrenamientoProperty);
        set => SetValue(DiarioEntrenamientoProperty, value);
    }

    private async Task<double[]> BuscarPesos(int ano)
    {
        var pesos = new double[12];
        var calcularMediaMes = (int iano, int imes, DiarioEntrenamiento idiarioEntrenamiento) =>
        {
            var sumaPesos = 0.0;
            var numPesos = 0;
            for (var dia = 1; dia <= DateTime.DaysInMonth(iano, imes); dia++)
            {
                var key = new DateOnly(iano, imes, dia);
                if (idiarioEntrenamiento.TryGetValue(key, out var diaEntrenamiento))
                    if (diaEntrenamiento.Medidas != null && diaEntrenamiento.Medidas.Peso > 1)
                    {
                        sumaPesos += diaEntrenamiento.Medidas.Peso;
                        numPesos++;
                    }
            }

            if (numPesos == 0) return 0;
            return sumaPesos / numPesos;
        };

        for (var mes = 1; mes <= 12; mes++)
        {
            var mes1 = mes;
            pesos[mes - 1] = await
                Dispatcher.UIThread.InvokeAsync(() =>
                    calcularMediaMes(ano, mes1, DiarioEntrenamiento));
        }

        return pesos;
    }

    private void DrawGrid(Canvas canvas, string[] meses, double borderWidth, double borderHeight)
    {
        canvas.Children.Clear();
        DrawX(canvas, borderWidth, borderHeight, meses);
    }

    private void DrawX(Canvas canvas, double width, double height, string[] meses)
    {
        var ejes = new Polyline
        {
            Stroke = Brushes.White,
            StrokeThickness = 2.5
        };

        ejes.Points = new Points
        {
            new(Margen, Margen),
            new(Margen, height - Margen),
            new(width, height - Margen)
        };

        var distanciaX = (width - 2 * Margen) / meses.Length;
        var distanciaY = -((height - 2 * Margen) / FilasY);

        for (var i = 0; i < meses.Length; i++)
        {
            var ejeX = new Polyline
            {
                Stroke = Brushes.White,
                StrokeThickness = 0.5
            };

            ejeX.Points = new Points
            {
                new((i + 1) * distanciaX + Margen, height - Margen),
                new((i + 1) * distanciaX + Margen, Margen)
            };

            var textBlock = new TextBlock
            {
                Text = meses[i],
                Foreground = Brushes.Aquamarine,
                Margin = new Thickness((i + 1) * distanciaX + Margen - 10, height - 20, 0, 0)
            };

            canvas.Children.Add(textBlock);
            canvas.Children.Add(ejeX);
        }

        for (var i = 0; i <= FilasY; i++)
        {
            var valor = i * ValorFila;

            var ejeY = new Polyline
            {
                Stroke = Brushes.White,
                StrokeThickness = 0.5
            };

            ejeY.Points = new Points
            {
                new(Margen, Margen - i * distanciaY),
                new(width, Margen - i * distanciaY)
            };

            var textBlock = new TextBlock
            {
                Text = valor + "",
                Foreground = Brushes.Aquamarine,
                Margin = new Thickness(0, height - Margen + i * distanciaY, 0, 0)
            };

            canvas.Children.Add(textBlock);
            canvas.Children.Add(ejeY);
        }

        canvas.Children.Add(ejes);
    }

    private void DrawLines(Canvas canvas, double width, double height, double[] valores)
    {
        var puntos = Points(width, height, valores);
        var polyline = new Polyline
        {
            Stroke = Brushes.Blue,
            StrokeThickness = 2
        };

        foreach (var punto in puntos) polyline.Points.Add(punto);

        canvas.Children.Add(polyline);
    }

    private void DrawRectangle(Canvas canvas, double width, double height, double[] valores)
    {
        var puntos = Points(width, height, valores);

        for (var i = 0; i < puntos.Length; i++)
        {
            var rectangulo = new Rectangle
            {
                Width = 20,
                Height = height - Margen - puntos[i].Y,
                Fill = Brushes.Blue
            };
            Canvas.SetLeft(rectangulo, puntos[i].X - 10);
            Canvas.SetTop(rectangulo, puntos[i].Y);

            var textBlock = new TextBlock
            {
                Text = valores[i] + "",
                Foreground = Brushes.Aquamarine,
                Margin = new Thickness(puntos[i].X - 10, puntos[i].Y - 20, 0, 0)
            };

            canvas.Children.Add(textBlock);
            canvas.Children.Add(rectangulo);
        }
    }

    private Point[] Points(double width, double height, double[] valores)
    {
        var distanciaX = (width - 2 * Margen) / valores.Length;
        var distanciaY = (height - 2 * Margen) / FilasY;
        var puntos = new Point[valores.Length];

        for (var i = 0; i < valores.Length; i++)
            puntos[i] = new Point(Margen + (i + 1) * distanciaX,
                height - Margen - distanciaY * (valores[i] / ValorFila));

        return puntos;
    }

    private void Text(string[] meses, double[] pesos)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<----Informe Anual---->");

        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();

        for (var i = 0; i < meses.Length; i++)
        {
            sb.AppendLine($"En {meses[i]} -> {pesos[i]} Kilogramos.");
            sb.AppendLine();
        }

        Texto.Text = sb.ToString();
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        Calendario.SelectedDate = DateTime.Now;
        ActualizarGraficos();
    }

    private void Calendario_OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        ActualizarGraficos();
    }

    private async void ActualizarGraficos()
    {
        var pesos = await BuscarPesos(Calendario.SelectedDate!.Value.Year
        ); //= { 70.5, 55.2, 80.0, 65.8, 90.3, 72.1, 68.9, 75.4, 82.6, 60.7, 70.5, 70.5 };

        Text(Meses, pesos);

        DrawGrid(CanvasLineal, Meses, GraficoLineal.Width, GraficoLineal.Height);
        DrawLines(CanvasLineal, GraficoLineal.Width, GraficoLineal.Height, pesos);

        DrawGrid(CanvasColumnas, Meses, GraficoColumnas.Width, GraficoColumnas.Height);
        DrawRectangle(CanvasColumnas, GraficoColumnas.Width, GraficoColumnas.Height, pesos);
    }
}