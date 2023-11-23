using System;
using System.IO;
using System.Xml.Serialization;

namespace DiarioNadador.Core.XML;

public class XmlDiarioEntrenamiento
{
    private static readonly string RutaArchivoXml = "diarioEntrenamiento.xml";

    public static void DiarioEntrenamientoToXml(DiarioEntrenamiento diario)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(DiarioEntrenamiento));

            using (TextWriter writer = new StreamWriter(RutaArchivoXml))
            {
                serializer.Serialize(writer, diario);
            }

            Console.WriteLine($"Archivo XML guardado en: {Path.GetFullPath(RutaArchivoXml)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear el archivo XML: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine(ex.InnerException!.StackTrace);
        }
    }

    public static DiarioEntrenamiento XmlToDiarioEntrenamiento()
    {
        try
        {
            if (File.Exists(RutaArchivoXml))
            {
                var serializer = new XmlSerializer(typeof(DiarioEntrenamiento));

                using (var fileStream = new FileStream(RutaArchivoXml, FileMode.Open))
                {
                    return (DiarioEntrenamiento)serializer.Deserialize(fileStream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar el diario de entrenamiento desde el archivo XML: {ex.Message}");
        }

        return new DiarioEntrenamiento();
    }
}