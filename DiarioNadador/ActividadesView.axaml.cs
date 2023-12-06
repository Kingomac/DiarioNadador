using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using DiarioNadador.Components;
using DiarioNadador.Core;

namespace DiarioNadador;

public partial class ActividadesView : UserControl
{
    public static readonly StyledProperty<DiarioEntrenamiento> DiarioEntrenamientoProperty =
        AvaloniaProperty.Register<ActividadesView, DiarioEntrenamiento>(
            nameof(DiarioEntrenamiento));

    public ActividadesView()
    {
        InitializeComponent();

        ListaActividades.ItemTemplate =
            new FuncDataTemplate<Actividad>((value, _) => new ActividadExpander { Actividad = value });
    }

    public required DiarioEntrenamiento DiarioEntrenamiento
    {
        get => GetValue(DiarioEntrenamientoProperty);
        set => SetValue(DiarioEntrenamientoProperty, value);
    }

    private void Calendar_OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        var date = e.AddedItems[0] as DateTime? ?? throw new NullReferenceException();
        if (DiarioEntrenamiento.TryGetValue(DateOnly.FromDateTime(date), out var diaEntrenamiento))
        {
            ListaActividades.ItemsSource = diaEntrenamiento.Actividades;
            MedidasControl.Medidas = diaEntrenamiento.Medidas ?? new Medidas(0, 0, "");
        }
        else
        {
            ListaActividades.ItemsSource = new List<Actividad>();
            MedidasControl.Medidas = new Medidas(0, 0, "");
        }
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        Calendar.SelectedDate = DateTime.Now;
    }
}