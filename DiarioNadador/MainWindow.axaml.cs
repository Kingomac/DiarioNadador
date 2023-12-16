using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DiarioNadador.Core;
using DiarioNadador.Core.XML;

namespace DiarioNadador;

public partial class MainWindow : Window
{
    private readonly DiarioEntrenamiento _diarioEntrenamiento = new();

    public MainWindow()
    {
        InitializeComponent();
        Views = new ReadOnlyDictionary<string, UserControl>(new Dictionary<string, UserControl>
        {
            { nameof(MenuViewListActividades), new ActividadesView { DiarioEntrenamiento = _diarioEntrenamiento } },
            //{ nameof(MenuViewListCircuitos), },
            //{nameof(MenuViewListGraficaActividades), },
            //{ nameof(MenuViewListGraficaMedidas), },
            { nameof(MenuViewListInformeAnual), new InformeAnualView { DiarioEntrenamiento = _diarioEntrenamiento } }
        });
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

        _diarioEntrenamiento.Add(date, new DiaEntrenamiento(
            new List<Actividad>
            {
                act1, act2
            },
            new Medidas(70, 90, "")
        ));
    }

    private ReadOnlyDictionary<string, UserControl> Views { get; }


    private void MenuViewList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        //TODO: Una vez que estén implementadas todas las vistas quitar la gestión de excepciones de aquí
        try
        {
            var newSelected = e.AddedItems[0] as ListBoxItem;
            if (newSelected?.Name is null)
                throw new NullReferenceException(
                    $"Menú seleccionado no válido, comprueba que está en el diccionario {nameof(Views)} en {nameof(MainWindow)} y que tiene un Name asignado");
            //MainViewContent.Content = Views[newSelected.Name];
            MainViewContent.Content = newSelected.Name switch
            {
                nameof(MenuViewListActividades) => new ActividadesView { DiarioEntrenamiento = _diarioEntrenamiento },
                nameof(MenuViewListInformeAnual) => new InformeAnualView { DiarioEntrenamiento = _diarioEntrenamiento },
                _ => MainViewContent.Content
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error al cambiar de vista: " + ex.Message);
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        XmlDiarioEntrenamiento.DiarioEntrenamientoToXml(_diarioEntrenamiento);
    }

    private void Button_OnClick2(object? sender, RoutedEventArgs e)
    {
        var diario = XmlDiarioEntrenamiento.XmlToDiarioEntrenamiento();
        foreach (var x in diario.Keys) Console.WriteLine(x);
    }
}