using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;

using medidas.Core;
using Avalonia.Media;
using Avalonia.Controls.Shapes;

namespace DiarioNadador.Components;

public partial class GraficoMedidasUserControl : UserControl
{
    public GraficoMedidasUserControl()
    {
        InitializeComponent();
        CrearGrafico();
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void CrearGrafico()
    {
        List<Medidas> medidas = XmlMedidas.XmlToMedidas();
        medidas = medidas.OrderBy(m => m.Fecha).ToList();
        
        foreach (var medida in medidas)
        {
            Console.WriteLine($"Peso: {medida.Peso}, Circunferencia: {medida.CircunferenciaAbdominal}, Notas: {medida.Notas}");
        }

        if (medidas.Count > 0)
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
            StrokeThickness = 2,    
        };

        ejes.Points = new Points
        {
            new Point(0,0),
            new Point(0,height),
            new Point(width, height)
        };
        
        canvas.Children.Add(ejes);

        
        for (int i = 0; i < dates.Count(); i++)
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
        double distanciaY = height / 5;
        
        for (int i = 0; i <= 5; i++)
        {
            double valor = i * interval;
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
        List<Point> puntos = ObtenerPuntos(values, canvas.Width, canvas.Height);
        
        var polyline = new Polyline
        {
            Stroke = Brushes.Black,
            StrokeThickness = 2,    
        };

        foreach (var punto in puntos)
        {
            polyline.Points.Add(punto);
        }
        
        canvas.Children.Add(polyline);
    }

    private List<Point> ObtenerPuntos(IEnumerable<double> values, double width, double height)
    {
        var points = new List<Point>();
        var count = 0;

        double distanciaX = width / (values.Count() - 1);
        double distanciaY = height / 5;
        
        foreach (var value in values)
        {
            var x = count * distanciaX;
            var y = height - (value/20) * distanciaY;

            points.Add(new Point(x, y));
            count++;
        }

        return points;
    }
}
