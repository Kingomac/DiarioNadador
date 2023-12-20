using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DiarioNadador.Components;

public partial class Confirmacion : Window
{
    public static readonly StyledProperty<string> TituloProperty = AvaloniaProperty.Register<Confirmacion, string>(
        nameof(Titulo));

    public static readonly StyledProperty<string> CuerpoProperty = AvaloniaProperty.Register<Confirmacion, string>(
        nameof(Cuerpo));

    public Confirmacion()
    {
        InitializeComponent();
    }

    public string Titulo
    {
        get => GetValue(TituloProperty);
        set
        {
            SetValue(TituloProperty, value);
            if (TituloTxt is not null) TituloTxt.Text = value;
        }
    }

    public string Cuerpo
    {
        get => GetValue(CuerpoProperty);
        set
        {
            SetValue(CuerpoProperty, value);
            if (CuerpoTxt is not null) CuerpoTxt.Text = value;
        }
    }

    private void AceptarBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Debug.WriteLine("aceptar btn");
        Close();
        Aceptar?.Invoke(this, e);
    }

    private void CancelarBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
        Cancelar?.Invoke(this, e);
    }

    public event EventHandler<RoutedEventArgs>? Aceptar;
    public event EventHandler<RoutedEventArgs>? Cancelar;
}