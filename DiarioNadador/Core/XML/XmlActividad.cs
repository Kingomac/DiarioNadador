using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Actividad
{
 public class XmlActividades
    {
        private static string RutaArchivoXml = "actividades.xml";

        public static void ActividadesToXml(DateTime fecha, double tiempo, double distancia, string notas)
        {
            try
            {
                Actividad nuevaActividad = new Actividad(fecha, tiempo, distancia, notas);

                List<Actividad> listaActividades = XmlToActividades();

                listaActividades.Add(nuevaActividad);

                XmlSerializer serializer = new XmlSerializer(typeof(List<Actividad>));

                using (TextWriter writer = new StreamWriter(RutaArchivoXml))
                {
                    serializer.Serialize(writer, listaActividades);
                }

                Console.WriteLine($"Archivo XML guardado en: {Path.GetFullPath(RutaArchivoXml)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el archivo XML: {ex.Message}");
            }
        }

        public static List<Actividad> XmlToActividades()
        {
            try
            {
                if (File.Exists(RutaArchivoXml))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Actividad>));

                    using (FileStream fileStream = new FileStream(RutaArchivoXml, FileMode.Open))
                    {
                        return (List<Actividad>)serializer.Deserialize(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar la lista de actividades desde el archivo XML: {ex.Message}");
            }

            return new List<Actividad>();
        }
    }
}