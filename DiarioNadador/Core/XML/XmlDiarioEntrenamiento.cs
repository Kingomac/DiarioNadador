using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DiarioNadador.Core.XML;

public class XmlDiarioEntrenamiento
{
    public string RutaArchivoXml { get; init; } = "diarioEntrenamiento.xml";

    public void Save(DiarioEntrenamiento diario)
    {
        Debug.WriteLine("Guardando archivo XML");
        var serializer = new XmlSerializer(typeof(DiarioEntrenamiento));
        try
        {
            using (TextWriter writer = new StreamWriter(RutaArchivoXml))
            {
                serializer.Serialize(writer, diario);
            }

            Debug.WriteLine($"Archivo XML guardado en: {Path.GetFullPath(RutaArchivoXml)}");
        }
        catch (XmlException e)
        {
            throw new XmlFileException(e)
                { FilePath = Path.GetFullPath(RutaArchivoXml), OperationType = XmlFileException.Operation.Save };
        }
    }

    public DiarioEntrenamiento Load()
    {
        Debug.WriteLine("Cargando archivo XML");

        var serializer = new XmlSerializer(typeof(DiarioEntrenamiento));

        try
        {
            using (var fileStream = new FileStream(RutaArchivoXml, FileMode.Open))
            {
                return (DiarioEntrenamiento)(serializer.Deserialize(fileStream) ??
                                             throw new XmlException(
                                                 "No se ha podido deserializar el archivo XML"));
            }
        }
        catch (Exception e)
        {
            throw new XmlFileException(e)
                { FilePath = Path.GetFullPath(RutaArchivoXml), OperationType = XmlFileException.Operation.Load };
        }
    }

    public DiarioEntrenamiento LoadIfExists()
    {
        return !File.Exists(RutaArchivoXml) ? new DiarioEntrenamiento() : Load();
    }
}