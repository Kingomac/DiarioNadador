using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DiarioNadador.Core;

namespace DiarioNadador.Components;

public partial class ActividadExpander : UserControl
{
    public static readonly StyledProperty<Actividad> ActividadProperty =
        AvaloniaProperty.Register<ActividadExpander, Actividad>(
            nameof(Actividad));

    public RoutedEvent<RoutedEventArgs> DeleteEvent = RoutedEvent.Register<ActividadExpander, RoutedEventArgs>(
        nameof(DeleteEvent),
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
            /*MapaBtn.IsVisible = !string.IsNullOrEmpty(value.Circuito.UrlMapa);
            NotesBtn.IsVisible = !string.IsNullOrEmpty(value.Notas);*/
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
    }

    private void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        RaiseEvent(new RoutedEventArgs(DeleteEvent));
    }*/

    public event EventHandler Delete
    {
        add => AddHandler(DeleteEvent, value);
        remove => RemoveHandler(DeleteEvent, value);
    }

    /*private void MapaBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo(Actividad.Circuito.UrlMapa) { UseShellExecute = true });
    }*/
}