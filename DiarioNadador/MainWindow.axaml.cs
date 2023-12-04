using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DiarioNadador.Components;
using DiarioNadador.Core;

namespace DiarioNadador;

public partial class MainWindow : Window
{
    private readonly DiarioEntrenamiento diarioEntrenamiento = new();

    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif

        var date = DateOnly.FromDateTime(DateTime.Now);

        Actividad act1 = new(
            TimeSpan.FromMinutes(23),
            23,
            new Circuito(23, "Cangas", "Muy bonito", "https://maps.app.goo.gl/JZfgTpoYvfgPWZYh7"),
            "es el lugar más bonito que he visto en mi vida"
        );

        Actividad act2 = new(
            TimeSpan.FromMinutes(12),
            6,
            new Circuito(12, "Marín", "no tan bonito como Cangas", ""),
            "nunca corrí tan rápido como para escapar de Marín"
        );

        diarioEntrenamiento.Add(date, new DiaEntrenamiento(
            new List<Actividad>
            {
                act1, act2
            },
            new Medidas(70, 90, "")
        ));


        ListaActividades.ItemTemplate =
            new FuncDataTemplate<Actividad>((value, _) => new ActividadExpander { Actividad = value });
        ListaActividades.ItemsSource = diarioEntrenamiento[date].Actividades;
        MedidasControl.Medidas = diarioEntrenamiento[date].Medidas ?? new Medidas(0, 0, "");
    }

    private void Calendar_OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        var date = Calendar.SelectedDates[0];
        try
        {
            ListaActividades.ItemsSource = diarioEntrenamiento[DateOnly.FromDateTime(date)].Actividades;
            MedidasControl.Medidas = diarioEntrenamiento[DateOnly.FromDateTime(date)].Medidas ?? new Medidas(0, 0, "");
        }
        catch (Exception _)
        {
            ListaActividades.ItemsSource = new List<Actividad>();
            MedidasControl.Medidas = new Medidas(0, 0, "");
        }
    }
}