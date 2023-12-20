using System;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using DiarioNadador.Core;

namespace DiarioNadador;

public partial class InformeAnualView : UserControl
{
    private const int Margen = 25;
    private const int FilasY = 10;
    private const int ValorFila = 15;

    private static readonly string[] Meses =
        { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };

    public static readonly StyledProperty<DiarioEntrenamiento> DiarioEntrenamientoProperty =
        AvaloniaProperty.Register<InformeAnualView, DiarioEntrenamiento>(
            nameof(DiarioEntrenamiento));

    private readonly IImmutableSolidColorBrush _colorMaya = new ImmutableSolidColorBrush(Colors.Black);

    private IImmutableSolidColorBrush _colorDatos = new ImmutableSolidColorBrush(Colors.Blue);
    private IImmutableSolidColorBrush _colorLabel = new ImmutableSolidColorBrush(Colors.Blue);

    public InformeAnualView()
    {
        InitializeComponent();

        ColorDatos.ColorChanged += ActualizarColor;
        ColorLabel.ColorChanged += ActualizarColor;
    }

    public required DiarioEntrenamiento DiarioEntrenamiento
    {
        get => GetValue(DiarioEntrenamientoProperty);
        set => SetValue(DiarioEntrenamientoProperty, value);
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
            Stroke = _colorMaya,
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
                Stroke = _colorMaya,
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
                Foreground = _colorLabel,
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
                Stroke = _colorMaya,
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
                Foreground = _colorLabel,
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
            Stroke = _colorDatos,
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
                Fill = _colorDatos
            };
            Canvas.SetLeft(rectangulo, puntos[i].X - 10);
            Canvas.SetTop(rectangulo, puntos[i].Y);

            var textBlock = new TextBlock
            {
                Text = valores[i] + "",
                Foreground = _colorLabel,
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

    private void ActualizarColor(object? sender, ColorChangedEventArgs e)
    {
        if (sender is ColorPicker selec)
        {
            if (selec.Name.Equals("ColorDatos")) _colorDatos = new ImmutableSolidColorBrush(e.NewColor);

            if (selec.Name.Equals("ColorLabel")) _colorLabel = new ImmutableSolidColorBrush(e.NewColor);
        }

        ActualizarGraficos();
    }

    private void Calendario_OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        ActualizarGraficos();
    }

    private async void ActualizarGraficos()
    {
        var pesos =
            new SearchQueries { DiarioEntrenamiento = DiarioEntrenamiento }.GetPesos(Calendario.SelectedDate!.Value
                .Year);

        Text(Meses, pesos);

        DrawGrid(CanvasLineal, Meses, GraficoLineal.Width, GraficoLineal.Height);
        DrawLines(CanvasLineal, GraficoLineal.Width, GraficoLineal.Height, pesos);

        DrawGrid(CanvasColumnas, Meses, GraficoColumnas.Width, GraficoColumnas.Height);
        DrawRectangle(CanvasColumnas, GraficoColumnas.Width, GraficoColumnas.Height, pesos);
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        Calendario.SelectedDate = DateTime.Now;
    }
}