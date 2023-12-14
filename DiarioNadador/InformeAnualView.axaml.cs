namespace DiarioNadador;

using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

public partial class InformeAnualView : UserControl
{
    const int Margen = 25;
    const int FilasY = 10;
    const int ValorFila = 15;
    
    public InformeAnualView()
    {
        InitializeComponent();
        
        var canvas = new Canvas();
        double[] pesos = { 70.5, 55.2, 80.0, 65.8, 90.3, 72.1, 68.9, 75.4, 82.6, 60.7, 70.5, 70.5 };
        string[] meses = { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        
        this.Text(meses, pesos);

        Lineal!.Click += (_, _) =>
        {
            DrawGrid(canvas, meses);
            DrawLines(canvas, Grafico.Width, Grafico.Height, pesos);
            Grafico.Child = canvas;
            ZonaTexto!.IsVisible = true;
        };

        Columnas!.Click += (_, _) =>
        {
            DrawGrid(canvas, meses);
            DrawRectangle(canvas, Grafico.Width, Grafico.Height, pesos);
            Grafico.Child = canvas;
            ZonaTexto!.IsVisible = true;
        };
    }
    
    private void DrawGrid(Canvas canvas,string[] meses)
    {
        canvas.Children.Clear();
        this.DrawX(canvas, Grafico.Width, Grafico.Height, meses);
    }

    private void DrawX(Canvas canvas, double width,double height, string[] meses)
    {
        var ejes = new Polyline
        {
            Stroke = Brushes.White,
            StrokeThickness = 2.5,    
        };
        
        ejes.Points = new Points
        {
            new Point(Margen,Margen),
            new Point(Margen, height-Margen),
            new Point(width, height-Margen),
        };
        
        double distanciaX = (width - 2 * Margen)/meses.Length;
        double distanciaY = -((height - 2 * Margen) / FilasY);
        
        for (int i = 0; i < meses.Length; i++)
        {
            var ejeX = new Polyline
            {
                Stroke = Brushes.White,
                StrokeThickness = 0.5, 
            };

            ejeX.Points = new Points
            {
                new Point((i+1) * distanciaX + Margen, height - Margen),
                new Point((i+1) * distanciaX + Margen, Margen)
            };
            
            var textBlock = new TextBlock
            {
                Text = meses[i],
                Foreground = Brushes.Aquamarine,
                Margin = new Thickness((i+1)*distanciaX + Margen - 10, height - 20, 0, 0)
            };
        
            canvas.Children.Add(textBlock);
            canvas.Children.Add(ejeX);
        }
        
        for (int i = 0; i <= FilasY; i++)
        {
            int valor = i * ValorFila;
            
            var ejeY = new Polyline
            {
                Stroke = Brushes.White,
                StrokeThickness = 0.5, 
            };

            ejeY.Points = new Points
            {
                new Point(Margen, Margen - (i) * distanciaY),
                new Point(width, Margen - (i) * distanciaY)
            };
            
            var textBlock = new TextBlock
            {
                Text = valor+"",
                Foreground = Brushes.Aquamarine,
                Margin = new Thickness(0, (height - Margen) + ((i)*distanciaY), 0, 0)
            };
            
            canvas.Children.Add(textBlock);
            canvas.Children.Add(ejeY);
        }
        
        canvas.Children.Add(ejes);
    }

    private void DrawLines(Canvas canvas, double width,double height, double[] valores)
    {
        Point[] puntos = Points(width, height, valores);
        var polyline = new Polyline
        {
            Stroke = Brushes.Blue,
            StrokeThickness = 2,
        };

        foreach (var punto in puntos)
        {
            polyline.Points.Add(punto);
        }
        
        canvas.Children.Add(polyline);
    }

    private void DrawRectangle(Canvas canvas, double width,double height, double[] valores)
    {
        Point[] puntos = Points(width, height, valores);
        
        for (int i = 0; i < puntos.Length; i++)
        {
            var rectangulo = new Rectangle
            {
                Width = 20,
                Height = (height - Margen) - puntos[i].Y,
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

    private Point[] Points(double width,double height, double[] valores)
    {
        double distanciaX = (width - 2 * Margen)/valores.Length;
        double distanciaY = (height - 2 * Margen) / FilasY;
        Point[] puntos = new Point[valores.Length];
        
        for (int i = 0; i < valores.Length; i++)
        {
            puntos[i] = new Point(Margen + ((i+1)*distanciaX), (height - Margen) - distanciaY*(valores[i]/ValorFila));
        }

        return puntos;
    }
    
    private void Text(string[] meses, double [] pesos)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<----Informe Anual---->");
        
        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();
 
        for (int i = 0; i < meses.Length; i++)
        {
            sb.AppendLine($"En {meses[i]} -> {pesos[i]} Kilogramos.");
            sb.AppendLine();
        }

        Texto.Text = sb.ToString();
    }
}