using System;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DiarioNadador.Core;
using DiarioNadador.Core.XML;

namespace DiarioNadador;

public partial class InsertarActividad : Window
{
    public static readonly RoutedEvent<InsertarActividadEventArgs> InsertarEvent =
        RoutedEvent.Register<InsertarActividad, InsertarActividadEventArgs>(
            nameof(Insertar),
            RoutingStrategies.Bubble);

    public InsertarActividad(DateTime? fechaCalendario)
    {
        InitializeComponent();

        fecha.SelectedDate = new DateTimeOffset(fechaCalendario ?? DateTime.Now); // aquí marca el error

        var opSalir = this.FindControl<MenuItem>("OpSalir");
        OpSalir.Click += (_, _) => OnSalir();


        var btGuardar = this.FindControl<Button>("btGuardar");
        btGuardar.Click += (_, _) => GuardaActividad();


        var edTiempo = this.FindControl<TextBox>("edTiempo");
        edTiempo.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1) edTiempo.Text = "";
        }, RoutingStrategies.Tunnel);

        //Escribe la distancia
        var edDistancia = this.FindControl<TextBox>("edDistancia");
        edDistancia.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1) edDistancia.Text = "";
        }, RoutingStrategies.Tunnel);


        //Escribe las notas
        var edNotas = this.FindControl<TextBox>("edNotas");
        edNotas.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1) edNotas.Text = "";
        }, RoutingStrategies.Tunnel);

        //Escribe el circuito
        //var comboCircuitos = this.FindControl<ComboBox>("edCircuitos");
        var listaCircuitos = XmlCircuito.XmlToCircuitos();
        edCircuitos.ItemsSource = listaCircuitos;
    }

    public event EventHandler<InsertarActividadEventArgs> Insertar
    {
        add => AddHandler(InsertarEvent, value);
        remove => RemoveHandler(InsertarEvent, value);
    }

    private void GuardaActividad()
    {
        var tiempoStr = this.FindControl<TextBox>("edTiempo").Text;
        var distanciaStr = this.FindControl<TextBox>("edDistancia").Text;
        var notas = this.FindControl<TextBox>("edNotas").Text;

        if (!TimeSpan.TryParse(tiempoStr, out var tiempo))
        {
            Debug.WriteLine("Error: El tiempo no es un formato válido.");
            return;
        }

        if (!int.TryParse(distanciaStr, out var distancia))
        {
            Debug.WriteLine("Error: La distancia no es un número válido.");
            return;
        }

        var circuito = edCircuitos.SelectedItem as Circuito;

        var newActividad = new Actividad(tiempo, distancia, circuito, notas);
        //actividads.Add(newActividad);
        OnInsertar(newActividad);
        OnSalir();
    }

    protected virtual void OnInsertar(Actividad newActividad)
    {
        RaiseEvent(new InsertarActividadEventArgs(InsertarEvent)
            { Fecha = DateOnly.FromDateTime(fecha.SelectedDate!.Value.DateTime), Actividad = newActividad });
    }


    private void OnSalir()
    {
        Close();
    }

    public class InsertarActividadEventArgs : RoutedEventArgs
    {
        public InsertarActividadEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {
        }

        public required Actividad Actividad { get; init; }
        public required DateOnly Fecha { get; init; }
    }
}