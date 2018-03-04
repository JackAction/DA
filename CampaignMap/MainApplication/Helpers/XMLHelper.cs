using System;
using System.IO;
using System.Xml.Serialization;

namespace MainApplication
{
    public class XMLHelper<T>
    {
        /// <summary>
        /// Speichert <paramref name="singleObject"/> als XML im <paramref name="path"/> ab.
        /// </summary>
        /// <param name="path">Speicherpfad</param>
        /// <param name="singleObject">Zu speicherndes Objekt</param>
        public static void Serialize(string path, T singleObject)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                StreamWriter stm = new StreamWriter(path);

                xs.Serialize(stm, singleObject);
                stm.Close();
            }
            catch (Exception)
            {

                throw new Exception("Fehler beim speichern des xml.");
            }
        }

        /// <summary>
        /// Liefert ein Objekt einer XML Datei im <paramref name="path"/>.
        /// </summary>
        /// <param name="path">Pfad der XML Datei</param>
        /// <returns>Deserialisiertes Objekt</returns>
        public static T Deserialize(string path)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                StreamReader stm = new StreamReader(path);
                object obj = xs.Deserialize(stm);
                stm.Close();
                return (T)obj;
            }
            catch (Exception)
            {

                throw new Exception("Fehler beim öffnen des xml. Xml scheint nicht korrekt zu sein.");
            }
        }
    }
}
