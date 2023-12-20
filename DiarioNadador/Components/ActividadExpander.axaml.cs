using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using DiarioNadador.Core;

namespace DiarioNadador.Components;

public partial class ActividadExpander : UserControl
{
    public static readonly StyledProperty<Actividad> ActividadProperty =
        AvaloniaProperty.Register<ActividadExpander, Actividad>(
            nameof(Actividad));

    public RoutedEvent<RoutedEventArgs> DeleteEvent = RoutedEvent.Register<ActividadExpander, RoutedEventArgs>(
        nameof(Delete),
        RoutingStrategies.Bubble);

    public ActividadExpander()
    {
        InitializeComponent();
        DataContext = this;
    }

    public Actividad? Actividad
    {
        get => GetValue(ActividadProperty);
        set
        {
            if (value is null) return;
            SetValue(ActividadProperty, value);
            DuracionTxt.Text = value.TiempoEmpleado.ToString(@"m\'ss\""");
            LugarDistanciaTxt.Text = $"{value.Circuito.Lugar} ({value.Distancia}km)";
            MapaBtn.IsVisible = !string.IsNullOrEmpty(value.Circuito.UrlMapa);
            /*NotesBtn.IsVisible = !string.IsNullOrEmpty(value.Notas);*/
        }
    }

    /*public double NotesOpacity
    {
        get => NotasTxt.Opacity;
        set
        {
            NotasTxt.Opacity = value;
            NotasTxt.IsVisible = value >= 1;
        }
    }

    private void NotesBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        NotesOpacity = NotesOpacity == 0 ? 1 : 0;
    }*/

    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var confirmacion = new Confirmacion
        {
            Titulo = "Eliminar actividad",
            Cuerpo = "¿Estás seguro de que quieres eliminar esta actividad?"
        };
        confirmacion.Aceptar += (_, _) =>
        {
            Debug.WriteLine("delete event");
            RaiseEvent(new RoutedEventArgs(DeleteEvent));
        };
        var root = this.GetVisualRoot();
        if (root is not null)
        {
            var window = root as Window;
            await confirmacion.ShowDialog(window!);
        }
        else
        {
            confirmacion.Show();
        }
    }

    public event EventHandler Delete
    {
        add => AddHandler(DeleteEvent, value);
        remove => RemoveHandler(DeleteEvent, value);
    }

    private void MapaBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (Actividad != null)
            Process.Start(new ProcessStartInfo(Actividad.Circuito.UrlMapa) { UseShellExecute = true });
    }
}