using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace DiarioNadador.Core.XML;

public class XmlMedidas
{
    private static readonly string rutaArchivoXml = "medidas.xml";

    public static void MedidasToXml(double peso, double circunferencia, string notas)
    {
        try
        {
            var nuevaMedida = new Medidas(peso, circunferencia, notas);

            var listaMedidas = XmlToMedidas();

            listaMedidas.Add(nuevaMedida);

            var serializer = new XmlSerializer(typeof(List<Medidas>));

            using (TextWriter writer = new StreamWriter(rutaArchivoXml))
            {
                serializer.Serialize(writer, listaMedidas);
            }

            Debug.WriteLine($"Archivo XML guardado en: {Path.GetFullPath(rutaArchivoXml)}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al crear el archivo XML: {ex.Message}");
        }
    }

    public static List<Medidas> XmlToMedidas()
    {
        try
        {
            if (File.Exists(rutaArchivoXml))
            {
                var serializer = new XmlSerializer(typeof(List<Medidas>));

                using (var fileStream = new FileStream(rutaArchivoXml, FileMode.Open))
                {
                    return (List<Medidas>)serializer.Deserialize(fileStream);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al cargar la lista de medidas desde el archivo XML: {ex.Message}");
        }

        return new List<Medidas>();
    }
}