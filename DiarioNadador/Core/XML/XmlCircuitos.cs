using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Circuito
{ 
 public class XmlCircuito
    {
        private static string RutaArchivoXml = "circuitos.xml";

        public static void CircuitosToXml(double distancia, string lugar, string notas, string urlMapa)
        {
            try
            {
                Circuito nuevoCircuito = new Circuito(distancia, lugar, notas, urlMapa);

                List<Circuito> listaCircuitos = XmlToCircuitos();

                listaCircuitos.Add(nuevoCircuito);

                XmlSerializer serializer = new XmlSerializer(typeof(List<Circuito>));

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
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Circuito>));

                    using (FileStream fileStream = new FileStream(RutaArchivoXml, FileMode.Open))
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
}