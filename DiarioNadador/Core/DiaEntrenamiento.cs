using System.Collections.Generic;

namespace DiarioNadador.Core;

public class DiaEntrenamiento
{
    public DiaEntrenamiento(List<Actividad> actividades, Medidas? medidas)
    {
        Actividades = actividades;
        Medidas = medidas;
    }

    public List<Actividad> Actividades { get; set; }
    public Medidas? Medidas { get; set; }
}