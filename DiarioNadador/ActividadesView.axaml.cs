using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            new FuncDataTemplate<Actividad>((value, _) =>
            {
                var expander = new ActividadExpander { Actividad = value };
                expander.Delete += (_, _) =>
                {
                    if (DiarioEntrenamiento.TryGetValue(DateOnly.FromDateTime(Calendar.SelectedDate.Value),
                            out var diaEntrenamiento)) diaEntrenamiento.Actividades.Remove(value);
                    ActualizarActividadesMedidas();
                };
                return expander;
            });
        MedidasControl.MedidasModificadas += (_, medidas) => { };
    }

    public required Action SaveXml { get; init; }

    public required DiarioEntrenamiento DiarioEntrenamiento
    {
        get => GetValue(DiarioEntrenamientoProperty);
        set => SetValue(DiarioEntrenamientoProperty, value);
    }

    private void Calendar_OnSelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
    {
        ActualizarActividadesMedidas();
    }

    private void ActualizarActividadesMedidas()
    {
        Debug.WriteLine("Actualizando actividades y medidas");
        ListaActividades.Items.Clear();
        var date = Calendar.SelectedDate ?? DateTime.Now;
        if (DiarioEntrenamiento.TryGetValue(DateOnly.FromDateTime(date), out var diaEntrenamiento))
        {
            Debug.WriteLine("Existen datos para el dia seleccionado");
            foreach (var act in diaEntrenamiento.Actividades) ListaActividades.Items.Add(act);
            MedidasControl.Medidas = diaEntrenamiento.Medidas ?? new Medidas(0, 0, "");
        }
        else
        {
            Debug.WriteLine("No existen datos para el dia seleccionado");
            MedidasControl.Medidas = new Medidas(0, 0, "");
        }
    }

    private void Control_OnLoaded(object? sender, RoutedEventArgs e)
    {
        Calendar.SelectedDate = DateTime.Now;
    }

    public void InsertarActividad(object? sender, RoutedEventArgs e)
    {
        var winActividades = new InsertarActividad(Calendar.SelectedDate);
        winActividades.Insertar += (_, args) =>
        {
            Debug.WriteLine("InsertarActividad");
            if (DiarioEntrenamiento.TryGetValue(args.Fecha, out var diaEntrenamiento))
                diaEntrenamiento.Actividades.Add(args.Actividad);
            else
                DiarioEntrenamiento.Add(args.Fecha,
                    new DiaEntrenamiento { Actividades = new List<Actividad> { args.Actividad } });
            ActualizarActividadesMedidas();
            SaveXml();
        };
        winActividades.Show();
    }

    private void MedidasControl_OnMedidasModificadas(object? sender, Medidas e)
    {
        if (Calendar.SelectedDate is null)
            throw new NullReferenceException("Calendar.SelectedDate es null en GuardarMedidas");
        var date = DateOnly.FromDateTime(Calendar.SelectedDate.Value);
        if (DiarioEntrenamiento.TryGetValue(date, out var diaEntrenamiento))
            diaEntrenamiento.Medidas = MedidasControl.Medidas;
        else
            DiarioEntrenamiento[date] = new DiaEntrenamiento
            {
                Actividades = new List<Actividad>(),
                Medidas = MedidasControl.Medidas
            };
        SaveXml();
        Debug.WriteLine("Guardadas medidas para " + date + ": " + MedidasControl.Medidas);
    }
}