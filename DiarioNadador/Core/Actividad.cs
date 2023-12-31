using System;

namespace DiarioNadador.Core;

/// <summary>
///     Contiene los detalles de una sesión de natación
/// </summary>
[Serializable]
public class Actividad
{
    public Actividad(TimeSpan tiempoEmpleado, int distancia, Circuito circuito, string notas)
    {
        TiempoEmpleado = tiempoEmpleado;
        Distancia = distancia;
        Circuito = circuito;
        Notas = notas;
    }

    public Actividad()
    {
        TiempoEmpleado = TimeSpan.Zero;
        Distancia = 0;
        Circuito = new Circuito();
        Notas = "";
    }

    // datos de la sesión
    public TimeSpan TiempoEmpleado { get; set; }
    public int Distancia { get; set; }
    public Circuito Circuito { get; set; }
    public string Notas { get; set; }

}