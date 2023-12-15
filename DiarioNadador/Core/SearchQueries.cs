using System;

namespace DiarioNadador.Core;

/// <summary>
/// Clase Decorador de <see cref="DiarioNadador.Core.DiarioEntrenamiento"/> que extrae la información del diccionario para mostrarla en gráficas
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
        var date = new DateOnly(ano, mes, 1);
        date.AddDays(2);
        for (var dia = 1; dia <= totalDias; dia++)
            if (DiarioEntrenamiento.TryGetValue(new DateOnly(ano, mes, dia), out var diaEntrenamiento))
                toret[dia - 1] = diaEntrenamiento.Medidas ?? Medidas.Default;
            else
                toret[dia - 1] = Medidas.Default;

        return toret;
    }
}