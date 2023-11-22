using Avalonia.Controls;
using DiarioNadador.Core;

namespace DiarioNadador;

public partial class DiaEntrenamientoWindow : Window
{
    public DiaEntrenamientoWindow(DiaEntrenamiento diaEntrenamiento)
    {
        InitializeComponent();
        DiaEntrenamiento = diaEntrenamiento;
        DataContext = DiaEntrenamiento;
        MedidasText.IsVisible = HayMedidas;
        MedidasBorder.IsVisible = HayMedidas;
    }

    public DiaEntrenamiento DiaEntrenamiento { get; }

    public bool HayMedidas => DiaEntrenamiento.Medidas is not null;
}