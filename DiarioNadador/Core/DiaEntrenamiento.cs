using System;
using System.Collections.Generic;

namespace DiarioNadador.Core;

[Serializable]
public class DiaEntrenamiento
{
    public DiaEntrenamiento(List<Actividad> actividades, Medidas? medidas)
    {
        Actividades = actividades;
        Medidas = medidas;
    }

    public DiaEntrenamiento()
    {
        Actividades = new List<Actividad>();
    }

    public List<Actividad> Actividades { get; set; }
    public Medidas? Medidas { get; set; }
}