using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DiarioNadador.Core;

namespace DiarioNadador;

public partial class CircuitosView : UserControl
{
    public CircuitosView()
    {
        InitializeComponent();
        ListaDeCircuitos = new ObservableCollection<Circuito>();

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

    public ObservableCollection<Circuito> ListaDeCircuitos { get; set; }
    private ListBox ListBoxCircuitos { get; }

    private void OnGuardarCircuito()
    {
        var nuevoCircuito = new Circuito(
            double.Parse(this.FindControl<TextBox>("DistanciaTextBox").Text),
            this.FindControl<TextBox>("LugarTextBox").Text,
            this.FindControl<TextBox>("NotasTextBox").Text,
            this.FindControl<TextBox>("UrlGoogleMapsTextBox").Text
        );

        ListaDeCircuitos.Add(nuevoCircuito);

        this.FindControl<TextBox>("DistanciaTextBox").Text = "";
        this.FindControl<TextBox>("LugarTextBox").Text = "";
        this.FindControl<TextBox>("NotasTextBox").Text = "";
        this.FindControl<TextBox>("UrlGoogleMapsTextBox").Text = "";
    }

    private void BtEliminar_Click(object sender, RoutedEventArgs e)
    {
        var circuitoSeleccionado = ListBoxCircuitos.SelectedItem as Circuito;
        if (circuitoSeleccionado != null) ListaDeCircuitos.Remove(circuitoSeleccionado);
    }
}