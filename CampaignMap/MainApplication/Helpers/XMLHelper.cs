﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MainApplication
{
    public class XMLHelper<T>
    {
        //public static void Serialize(string path, List<T> liste)
        //{
        //    try
        //    {
        //        XmlSerializer xs = new XmlSerializer(typeof(List<T>));
        //        StreamWriter stm = new StreamWriter(path);

        //        xs.Serialize(stm, liste);
        //        stm.Close();
        //    }
        //    catch (Exception)
        //    {

        //        throw new Exception("Fehler beim speichern des xml.");
        //    }
        //}

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

        //public static List<T> Deserialize(string path)
        //{
        //    try
        //    {
        //        XmlSerializer xs = new XmlSerializer(typeof(List<T>));
        //        StreamReader stm = new StreamReader(path);
        //        object obj = xs.Deserialize(stm);
        //        stm.Close();
        //        return (List<T>)obj;
        //    }
        //    catch (Exception)
        //    {

        //        throw new Exception("Fehler beim öffnen des xml. Xml scheint nicht korrekt zu sein.");
        //    }
        //}

        public static T Deserialize(string path)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(List<T>));
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
