using System;

namespace DiarioNadador.Core;

public class SearchQueries
{
    public required DiarioEntrenamiento DiarioEntrenamiento { get; init; }

    public double[] GetPesos(int ano)
    {
        var pesos = new double[12];
        for (var mes = 1; mes <= 12; mes++)
        {
            var sumaPesos = 0.0;
            var numPesos = 0;
            for (var dia = 1; dia <= DateTime.DaysInMonth(ano, mes); dia++)
            {
                var key = new DateOnly(ano, mes, dia);
                if (DiarioEntrenamiento.TryGetValue(key, out var diaEntrenamiento))
                    if (diaEntrenamiento.Medidas != null && diaEntrenamiento.Medidas.Peso > 1)
                    {
                        sumaPesos += diaEntrenamiento.Medidas.Peso;
                        numPesos++;
                    }
            }

            pesos[mes - 1] = numPesos == 0 ? 0 : sumaPesos / numPesos;
        }

        return pesos;
    }
}