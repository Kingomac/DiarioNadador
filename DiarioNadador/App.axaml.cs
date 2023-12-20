using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DiarioNadador.Components;
using DiarioNadador.Core.XML;

namespace DiarioNadador;

public class App : Application
{
    public override void Initialize()
    {
#if DEBUG
        Trace.Listeners.Add(new ConsoleTraceListener());
#endif
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            try
            {
                desktop.MainWindow = new MainWindow();
            }
            catch (XmlFileException e)
            {
                var opType = e.OperationType switch
                {
                    XmlFileException.Operation.Load => "cargar",
                    XmlFileException.Operation.Save => "guardar",
                    _ => "hacer una operación desconocida"
                };
                ;
                var winErr = new Confirmacion
                {
                    Titulo = "Se ha producido un error",
                    Cuerpo = $"No se ha podido {opType} el archivo XML:\n{e.FilePath}\n\n" + e.Message + "\n\n" +
                             e.StackTrace,
                    Width = 800,
                    Height = 500
                };
                winErr.Aceptar += (_, _) => { Environment.Exit(1); };
                winErr.Show();
            }
            catch (Exception e)
            {
                var winErr = new Confirmacion
                {
                    Titulo = "Se ha producido un error",
                    Cuerpo = e.Message + "\n\n" + e.StackTrace + "\n\n",
                    Width = 600,
                    Height = 400
                };
                winErr.Aceptar += (_, _) => Environment.Exit(1);
                winErr.Show();
            }

        base.OnFrameworkInitializationCompleted();
    }
}