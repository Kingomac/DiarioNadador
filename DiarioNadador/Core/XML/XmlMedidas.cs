using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DiarioNadador.Core.XML;

public class XmlMedidas
{
    private static string rutaArchivoXml = "medidas.xml";

    public static void MedidasToXml(double peso, double circunferencia, string notas)
    {
        try
        {
            Medidas nuevaMedida = new Medidas(peso, circunferencia, notas);

            List<Medidas> listaMedidas = XmlToMedidas();

            listaMedidas.Add(nuevaMedida);

            XmlSerializer serializer = new XmlSerializer(typeof(List<Medidas>));

            using (TextWriter writer = new StreamWriter(rutaArchivoXml))
            {
                serializer.Serialize(writer, listaMedidas);
            }

            Console.WriteLine($"Archivo XML guardado en: {Path.GetFullPath(rutaArchivoXml)}");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear el archivo XML: {ex.Message}");
        }
    }

    public static List<Medidas> XmlToMedidas()
    {
        try
        {
            if (File.Exists(rutaArchivoXml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Medidas>));

                using (FileStream fileStream = new FileStream(rutaArchivoXml, FileMode.Open))
                {
                    return (List<Medidas>)serializer.Deserialize(fileStream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar la lista de medidas desde el archivo XML: {ex.Message}");
        }

        return new List<Medidas>();
    }
}