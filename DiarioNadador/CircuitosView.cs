using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DiarioNadador.Components;
using DiarioNadador.Core;

namespace DiarioNadador;

public partial class CircuitosView : UserControl
{
    public ObservableCollection<Circuito> ListaDeCircuitos { get; set; }
    private ListBox ListBoxCircuitos { get; }

    public CircuitosView()
    {
        InitializeComponent();
        ListaDeCircuitos = new ObservableCollection<Circuito>();

        var circuitosDesdeXml = Core.XML.XmlCircuito.XmlToCircuitos();
        foreach (var circuito in circuitosDesdeXml)
        {
            ListaDeCircuitos.Add(circuito);
        }

        ListBoxCircuitos = this.FindControl<ListBox>("listBoxCircuitos");
        ListBoxCircuitos.ItemsSource = ListaDeCircuitos;


        var btGuardar = this.FindControl<Button>("BtGuardar");
        btGuardar.Click += (_, _) => OnGuardarCircuito();

        var btEliminar = this.FindControl<Button>("BtEliminar");
        btEliminar.Click += BtEliminar_Click;

        var DistanciaTextBox = this.FindControl<TextBox>("DistanciaTextBox");
        DistanciaTextBox.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1) DistanciaTextBox.Text = "";
        }, RoutingStrategies.Tunnel);

        var LugarTextBox = this.FindControl<TextBox>("LugarTextBox");
        LugarTextBox.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1) LugarTextBox.Text = "";
        }, RoutingStrategies.Tunnel);

        var NotasTextBox = this.FindControl<TextBox>("NotasTextBox");
        NotasTextBox.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1) NotasTextBox.Text = "";
        }, RoutingStrategies.Tunnel);

        var UrlGoogleMapsTextBox = this.FindControl<TextBox>("UrlGoogleMapsTextBox");
        UrlGoogleMapsTextBox.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1) UrlGoogleMapsTextBox.Text = "";
        }, RoutingStrategies.Tunnel);
    }

    private void OnGuardarCircuito()
    {
        var distanciaTexto = this.FindControl<TextBox>("DistanciaTextBox").Text;
        if (!EsDistanciaValida(distanciaTexto))
        {
            var dialog = new DialogWindow("Por favor, introduzca un número válido para la distancia.");
            dialog.ShowDialog(GetWindow());
            return;
        }

        var nuevoCircuito = new Circuito(
            double.Parse(this.FindControl<TextBox>("DistanciaTextBox").Text),
            this.FindControl<TextBox>("LugarTextBox").Text,
            this.FindControl<TextBox>("NotasTextBox").Text,
            this.FindControl<TextBox>("UrlGoogleMapsTextBox").Text
        );

        ShowSelfClosingDialog("Circuito guardado exitosamente.", 1500);
        ListaDeCircuitos.Add(nuevoCircuito);
        Core.XML.XmlCircuito.CircuitosToXml(nuevoCircuito);

        this.FindControl<TextBox>("DistanciaTextBox").Text = "";
        this.FindControl<TextBox>("LugarTextBox").Text = "";
        this.FindControl<TextBox>("NotasTextBox").Text = "";
        this.FindControl<TextBox>("UrlGoogleMapsTextBox").Text = "";
    }

    private void BtEliminar_Click(object sender, RoutedEventArgs e)
    {
        var circuitoSeleccionado = ListBoxCircuitos.SelectedItem as Circuito;
        if (circuitoSeleccionado != null)
        {
            ListaDeCircuitos.Remove(circuitoSeleccionado);
            Core.XML.XmlCircuito.EliminarCircuitoDelXml(circuitoSeleccionado.Distancia, circuitoSeleccionado.Lugar);
        }
    }
    private bool EsDistanciaValida(string distanciaTexto)
    {
        return double.TryParse(distanciaTexto, out _);
    }
    private Window GetWindow()
    {
        return this.VisualRoot as Window;
    }

    private void ShowSelfClosingDialog(string message, int milliseconds)
    {
        var dialog = new DialogWindow(message);

        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(milliseconds) };
        timer.Tick += (sender, args) =>
        {
            timer.Stop();
            dialog.Close();
        };
        timer.Start();

        dialog.ShowDialog(GetWindow());
    }
}