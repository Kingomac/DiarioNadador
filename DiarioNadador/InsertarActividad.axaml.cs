using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace DiarioNadador;
using DiarioNadador.Core;




public partial class InsertarActividad : Window

{
    public InsertarActividad(List<Actividad> listaActividades)
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif


        var opSalir = this.FindControl<MenuItem>("OpSalir");
        opSalir!.Click += (_, _) => this.OnSalir();

        
    
        var btGuardar = this.FindControl<Button>("btGuardar");
        btGuardar.Click += (_, _) => this.GuardaActividad(listaActividades);
        
        
        var edTiempo = this.FindControl<TextBox>("edTiempo");
        edTiempo.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1)
            {
                edTiempo.Text = "";
            }
        }, RoutingStrategies.Tunnel);

        //Escribe la distancia
        var edDistancia = this.FindControl<TextBox>("edDistancia");
        edDistancia.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1)
            {
                edDistancia.Text = "";
            }
        }, RoutingStrategies.Tunnel);


        //Escribe las notas
        var edNotas = this.FindControl<TextBox>("edNotas");
        edNotas.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1)
            {
                edNotas.Text = "";
            }
        }, RoutingStrategies.Tunnel);
        
        //Escribe el circuito
        var edCircuito = this.FindControl<TextBox>("edCircuito");
        edCircuito.AddHandler(PointerPressedEvent, (sender, e) =>
        {
            if (e.ClickCount == 1)
            {
                edCircuito.Text = "";
            }
        }, RoutingStrategies.Tunnel); 
        
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }


    private void GuardaActividad(List<Actividad> actividads)
    {
        string tiempoStr = this.FindControl<TextBox>("edTiempo").Text;
        string distanciaStr = this.FindControl<TextBox>("edDistancia").Text;
        string circuitoStr = this.FindControl<TextBox>("edCircuito").Text;
        string notas = this.FindControl<TextBox>("edNotas").Text;

        if (!TimeSpan.TryParse(tiempoStr, out TimeSpan tiempo))
        {
            Console.WriteLine("Error: El tiempo no es un formato válido.");
            return;
        }

        if (!int.TryParse(distanciaStr, out int distancia))
        {
            Console.WriteLine("Error: La distancia no es un número válido.");
            return;
        }

        Circuito circuito = new Circuito(distancia, circuitoStr, notas, "");
        
        Actividad newActividad = new Actividad(tiempo, distancia, circuito, notas);
        actividads.Add(newActividad);
        

        
    }


    private void OnSalir()
    {
        this.Close();
    }
    
}
