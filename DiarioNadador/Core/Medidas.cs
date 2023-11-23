using System;

namespace DiarioNadador.Core;

[Serializable]
public class Medidas
{
    public Medidas(double peso, double circunferenciaAbdominal, string notas)
    {
        Peso = peso;
        CircunferenciaAbdominal = circunferenciaAbdominal;
        Notas = notas;
    }

    public Medidas()
    {
        Peso = 0;
        CircunferenciaAbdominal = 0;
        Notas = "";
    }

    public double Peso { get; set; }
    public double CircunferenciaAbdominal { get; set; }
    public string Notas { get; set; }
}