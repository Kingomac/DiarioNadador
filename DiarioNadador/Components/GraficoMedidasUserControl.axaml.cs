using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
<<<<<<< HEAD
=======
using Avalonia.Markup.Xaml;
using System.Linq;

using DiarioNadador.Core;
using Avalonia.Media;
>>>>>>> aff2b982adcea99dcc2a06aab8f6d0dbe5ce2f15
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DiarioNadador.Core;

namespace DiarioNadador.Components;

public partial class GraficoMedidasUserControl : UserControl
{
    public static readonly StyledProperty<DiarioEntrenamiento> DiarioEntrenamientoProperty =
        AvaloniaProperty.Register<GraficoMedidasUserControl, DiarioEntrenamiento>(
            nameof(DiarioEntrenamiento));

    public GraficoMedidasUserControl()
    {
        InitializeComponent();
        Calendar.SelectedDate = DateTime.Now;
        CrearGrafico();
    }

    public required DiarioEntrenamiento DiarioEntrenamiento
    {
        get => GetValue(DiarioEntrenamientoProperty);
        set => SetValue(DiarioEntrenamientoProperty, value);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CrearGrafico()
    {
        //List<Medidas> medidas = DiarioEntrenamiento.medidas();
        //medidas = medidas.OrderBy(m => m.Fecha).ToList();
<<<<<<< HEAD
        var queries = new SearchQueries { DiarioEntrenamiento = DiarioEntrenamiento };
        var ano = Calendar.SelectedDate.Value.Year;
        var mes = Calendar.SelectedDate.Value.Month;
=======
        var queries = new SearchQueries(DiarioEntrenamiento);
>>>>>>> aff2b982adcea99dcc2a06aab8f6d0dbe5ce2f15
        var medidas = queries.GetMedidas(ano, mes);
        var totalDias = DateTime.DaysInMonth(ano, mes);

        foreach (var medida in medidas)
            Console.WriteLine(
                $"Peso: {medida.Peso}, Circunferencia: {medida.CircunferenciaAbdominal}, Notas: {medida.Notas}");

        if (medidas.Length > 0)
        {
            var chartCircunferencia = this.FindControl<Canvas>("chartCircunferencia");
            var chartPeso = this.FindControl<Canvas>("chartPeso");
            var dates = medidas.Select(m => m.Fecha.ToString("yyyy-MM-dd"));

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
}