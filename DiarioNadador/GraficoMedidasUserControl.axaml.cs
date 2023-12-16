using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using DiarioNadador.Core;

namespace DiarioNadador;

public partial class GraficoMedidasUserControl : UserControl
{
    public static readonly StyledProperty<DiarioEntrenamiento> DiarioEntrenamientoProperty =
        AvaloniaProperty.Register<GraficoMedidasUserControl, DiarioEntrenamiento>(
            nameof(DiarioEntrenamiento));

    public GraficoMedidasUserControl(DiarioEntrenamiento diarioEntrenamiento)
    {
        InitializeComponent();
        DiarioEntrenamiento = diarioEntrenamiento;
        Calendar.SelectedDate = DateTime.Now;
        CrearGrafico();
    }

    public DiarioEntrenamiento DiarioEntrenamiento
    {
        get => GetValue(DiarioEntrenamientoProperty);
        set
        {
            if (value is null) Console.WriteLine("set DiarioEntrenamiento es nulo 1");
            else Console.WriteLine("set DiarioEntrenamiento no es nulo 1");
            SetValue(DiarioEntrenamientoProperty, value);
        }
    }


    private void CrearGrafico()
    {
        //List<Medidas> medidas = DiarioEntrenamiento.medidas();
        //medidas = medidas.OrderBy(m => m.Fecha).ToList();
        var ano = Calendar.SelectedDate.Value.Year;
        var mes = Calendar.SelectedDate.Value.Month;
        var medidas = new SearchQueries { DiarioEntrenamiento = DiarioEntrenamiento }.GetMedidas(ano, mes);
        var totalDias = DateTime.DaysInMonth(ano, mes);

        foreach (var medida in medidas)
            Console.WriteLine(
                $"Peso: {medida.Peso}, Circunferencia: {medida.CircunferenciaAbdominal}, Notas: {medida.Notas}");

        if (medidas.Length > 0)
        {
            var chartCircunferencia = this.FindControl<Canvas>("chartCircunferencia");
            var chartPeso = this.FindControl<Canvas>("chartPeso");
            var dates = Enumerable.Range(1, totalDias).Select(i => i.ToString());

            // Dibujar líneas en el chartCircunferencia
            DibujarEjes(chartCircunferencia, dates, chartCircunferencia.Width, chartCircunferencia.Height);
            DibujarLineas(chartCircunferencia, medidas.Select(m => m.CircunferenciaAbdominal));

            // Dibujar líneas en el chartPesos
            DibujarEjes(chartPeso, dates, chartPeso.Width, chartPeso.Height);
            DibujarLineas(chartPeso, medidas.Select(p => p.Peso));
        }
    }

    private void DibujarEjes(Canvas canvas, IEnumerable<string> dates, double width, double height)
    {
        var ejes = new Polyline
        {
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };

        ejes.Points = new Points
        {
            new(0, 0),
            new(0, height),
            new(width, height)
        };

        canvas.Children.Add(ejes);


        for (var i = 0; i < dates.Count(); i++)
        {
            var textBlock = new TextBlock
            {
                Text = dates.ElementAt(i),
                TextAlignment = TextAlignment.Center,
                Width = width / dates.Count(),
                Margin = new Thickness(i * (width / dates.Count()), height + 5, 0, 0)
            };
            canvas.Children.Add(textBlock);
        }

        double minValue = 0;
        double maxValue = 100;
        double interval = 20;
        var distanciaY = height / 5;

        for (var i = 0; i <= 5; i++)
        {
            var valor = i * interval;
            var textBlock = new TextBlock
            {
                Text = valor + " ",
                TextAlignment = TextAlignment.Right,
                Margin = new Thickness(-40, height - i * distanciaY, 0, 0)
            };
            canvas.Children.Add(textBlock);
        }
    }

    private void DibujarLineas(Canvas canvas, IEnumerable<double> values)
    {
        var puntos = ObtenerPuntos(values, canvas.Width, canvas.Height);

        var polyline = new Polyline
        {
            Stroke = Brushes.Black,
            StrokeThickness = 2
        };

        foreach (var punto in puntos) polyline.Points.Add(punto);

        canvas.Children.Add(polyline);
    }

    private List<Point> ObtenerPuntos(IEnumerable<double> values, double width, double height)
    {
        var points = new List<Point>();
        var count = 0;

        var distanciaX = width / (values.Count() - 1);
        var distanciaY = height / 5;

        foreach (var value in values)
        {
            var x = count * distanciaX;
            var y = height - value / 20 * distanciaY;

            points.Add(new Point(x, y));
            count++;
        }

        return points;
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
    }
}