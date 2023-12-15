using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DiarioNadador.Core;

namespace DiarioNadador.Components;

public partial class MedidasControl : UserControl
{
    public MedidasControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    public Medidas Medidas
    {
        get => new(Peso, CircunferenciaAbdominal, Notas);
        set
        {
            Peso = value.Peso;
            CircunferenciaAbdominal = value.CircunferenciaAbdominal;
            Notas = value.Notas;
        }
    }

    public double Peso
    {
        get => (double)(PesoTxt.Value ?? 0);
        set => PesoTxt.Value = (decimal)value;
    }

    public double CircunferenciaAbdominal
    {
        get => (double)(CircunferenciaAbdominalTxt.Value ?? 0);
        set => CircunferenciaAbdominalTxt.Value = (decimal)value;
    }

    public string Notas
    {
        get => NotasTxt.Text ?? string.Empty;
        set => NotasTxt.Text = value;
    }

    public event EventHandler<Medidas>? MedidasModificadas;

    public void OnMedidasModificadas(object? sender, RoutedEventArgs ev)
    {
        MedidasModificadas?.Invoke(this, Medidas);
    }
}