using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;

namespace DiarioNadador.Core.XML;

public class XmlCircuito
{
    private static readonly string RutaArchivoXml = "circuitos.xml";

    public static void ToXml(List<Circuito> circuito)
    {
        try
        {
            var serializer = new XmlSerializer(typeof(List<Circuito>));

            using (TextWriter writer = new StreamWriter(RutaArchivoXml))
            {
                serializer.Serialize(writer, circuito);
            }

            Debug.WriteLine($"Archivo XML guardado en: {Path.GetFullPath(RutaArchivoXml)}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error al crear el archivo XML de circuitos: {ex.Message}");
            Debug.WriteLine(ex.StackTrace);
        }
    }

    public static void CircuitosToXml(double distancia, string lugar, string notas, string urlMapa)
    {
        try
        {
            var nuevoCircuito = new Circuito(distancia, lugar, notas, urlMapa);

            var listaCircuitos = XmlToCircuitos();

            listaCircuitos.Add(nuevoCircuito);

            var serializer = new XmlSerializer(typeof(List<Circuito>));

            using (TextWriter writer = new StreamWriter(RutaArchivoXml))
            {
                serializer.Serialize(writer, listaCircuitos);
            }

            Console.WriteLine($"Archivo XML guardado en: {Path.GetFullPath(RutaArchivoXml)}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al crear el archivo XML: {ex.Message}");
        }
    }


    public static List<Circuito> XmlToCircuitos()
    {
        try
        {
            if (File.Exists(RutaArchivoXml))
            {
                var serializer = new XmlSerializer(typeof(List<Circuito>));

                using (var fileStream = new FileStream(RutaArchivoXml, FileMode.Open))
                {
                    return (List<Circuito>)serializer.Deserialize(fileStream);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar la lista de circuitos desde el archivo XML: {ex.Message}");
        }

        return new List<Circuito>();
    }
}