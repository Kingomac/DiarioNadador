using System;

namespace DiarioNadador.Core;

/// <summary>
///     Clase Decorador de <see cref="DiarioNadador.Core.DiarioEntrenamiento" /> que extrae la información del diccionario
///     para mostrarla en gráficas
/// </summary>
public class SearchQueries
{
    public required DiarioEntrenamiento DiarioEntrenamiento { get; init; }

    /// <summary>
    ///     Busca en el diario de entrenamiento las medidas del mes y año indicados
    /// </summary>
    /// <param name="ano">Año</param>
    /// <param name="mes">Mes</param>
    /// <returns>Array con las medidas o <see cref="Medidas.Default" /> siendo índice 0 enero y 11 diciembre</returns>
    public Medidas[] GetMedidas(int ano, int mes)
    {
        var totalDias = DateTime.DaysInMonth(ano, mes);
        var toret = new Medidas[totalDias];
        for (var dia = 1; dia <= totalDias; dia++)
        {
            var key = new DateOnly(ano, mes, dia);
            if (DiarioEntrenamiento is null) Console.WriteLine("DiarioEntrenamiento es nulo");
            else Console.WriteLine("DiarioEntrenamiento no es nulo");
            DiarioEntrenamiento.TryGetValue(key, out var diaEntrenamiento);
            Console.WriteLine(diaEntrenamiento);
            if (diaEntrenamiento is not null)
                toret[dia - 1] = diaEntrenamiento.Medidas ?? Medidas.Default;
            else
                toret[dia - 1] = Medidas.Default;
        }

        return toret;
    }

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