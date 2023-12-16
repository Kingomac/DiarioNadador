using Avalonia;
using Avalonia.Controls;
using DiarioNadador.Core;
using DiarioNadador.Core.XML;

namespace DiarioNadador;

public partial class MainWindow : Window
{
    private readonly DiarioEntrenamiento _diarioEntrenamiento;
    private readonly XmlDiarioEntrenamiento _file;

    public MainWindow()
    {
        InitializeComponent();
        _file = new XmlDiarioEntrenamiento();
        _diarioEntrenamiento = _file.LoadIfExists();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void MenuViewList_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems[0] is not ListBoxItem newSelected) return;
        MainViewContent.Content = newSelected.Name switch
        {
            nameof(MenuViewListActividades) => new ActividadesView
            {
                DiarioEntrenamiento = _diarioEntrenamiento,
                SaveXml = () => _file.Save(_diarioEntrenamiento)
            },
            nameof(MenuViewListInformeAnual) => new InformeAnualView { DiarioEntrenamiento = _diarioEntrenamiento },
            nameof(MenuViewListGraficaMedidas) => new GraficoMedidasUserControl(_diarioEntrenamiento),
            _ => MainViewContent.Content
        };
    }
}