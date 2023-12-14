using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using DiarioNadador.Components;
using DiarioNadador.Core;
using DiarioNadador.Core.XML;

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
        ActualizarActividadesMedidas();
    }

    private void ActualizarActividadesMedidas()
    {
        Console.WriteLine("Actualizar actividades y medidas");
        ListaActividades.Items.Clear();
        var date = Calendar.SelectedDate ?? DateTime.Now;
        if (DiarioEntrenamiento.TryGetValue(DateOnly.FromDateTime(date), out var diaEntrenamiento))
        {
            Console.WriteLine("Dia entrenamiento encontrado");
            foreach (var act in diaEntrenamiento.Actividades) ListaActividades.Items.Add(act);
            MedidasControl.Medidas = diaEntrenamiento.Medidas ?? new Medidas(0, 0, "");
        }
        else
        {
            Console.WriteLine("Dia entrenamiento no encontrado");
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
            Console.WriteLine("InsertarActividad");
            if (DiarioEntrenamiento.TryGetValue(args.Fecha, out var diaEntrenamiento))
                diaEntrenamiento.Actividades.Add(args.Actividad);
            else
                DiarioEntrenamiento.Add(args.Fecha,
                    new DiaEntrenamiento { Actividades = new List<Actividad> { args.Actividad } });
            ActualizarActividadesMedidas();
        };
        winActividades.Show();
    }

    public void GuardarMedidas(object? sender, RoutedEventArgs e)
    {
        double peso = Convert.ToDouble(MedidasControl.FindControl<NumericUpDown>("PesoTxt").Text);
        double circunferencia = Convert.ToDouble(MedidasControl.FindControl<NumericUpDown>("CircunferenciaAbdominalTxt").Text);
        string notas = Convert.ToString(MedidasControl.FindControl<TextBox>("NotasTxt").Text);

        var date = Calendar.SelectedDate ?? DateTime.Now;
        if (DiarioEntrenamiento.TryGetValue(DateOnly.FromDateTime(date), out var diaEntrenamiento))
        {
            diaEntrenamiento.Medidas = new Medidas(peso, circunferencia, notas);
            Console.WriteLine("Medidas guardadas");
        }
        ActualizarActividadesMedidas();

        XmlMedidas.MedidasToXml(peso, circunferencia, notas);
    }

}