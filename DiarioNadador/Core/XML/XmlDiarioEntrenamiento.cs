using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace DiarioNadador.Core.XML;

public class XmlDiarioEntrenamiento
{
    public string RutaArchivoXml { get; init; } = "diarioEntrenamiento.xml";

    public void Save(DiarioEntrenamiento diario)
    {
        Debug.WriteLine("Guardando archivo XML");
        var serializer = new XmlSerializer(typeof(DiarioEntrenamiento));

        using (TextWriter writer = new StreamWriter(RutaArchivoXml))
        {
            serializer.Serialize(writer, diario);
        }

        Console.WriteLine($"Archivo XML guardado en: {Path.GetFullPath(RutaArchivoXml)}");
    }

    public DiarioEntrenamiento Load()
    {
        Debug.WriteLine("Cargando archivo XML");
        
        var serializer = new XmlSerializer(typeof(DiarioEntrenamiento));

        using (var fileStream = new FileStream(RutaArchivoXml, FileMode.Open))
        {
            return (DiarioEntrenamiento)(serializer.Deserialize(fileStream) ??
                                         throw new NullReferenceException(
                                             "No se ha podido deserializar el archivo XML"));
        }
    }
    
    public DiarioEntrenamiento LoadIfExists()
    {
        return !File.Exists(RutaArchivoXml) ? new DiarioEntrenamiento() : Load();
    }
}