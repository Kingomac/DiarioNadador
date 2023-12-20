using Avalonia.Controls;
using Avalonia.Interactivity;

namespace DiarioNadador.Components;

public partial class DialogWindow : Window
{
    public DialogWindow(string message)
    {
        InitializeComponent();
        this.FindControl<TextBlock>("MessageTextBlock").Text = message;
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}