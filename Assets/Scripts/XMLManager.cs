using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

//https://www.youtube.com/watch?v=6vl1IYMpwVQ
//https://stackoverflow.com/questions/258960/how-to-serialize-an-object-to-xml-without-getting-xmlns
//https://www.codeproject.com/Articles/483055/XML-Serialization-and-Deserialization-Part-1
//https://unity3dtuts.com/xml-serialization-and-deserialization-in-c-sharp-using-unity/

public static class XMLManager 
{
    public static void Serialize(object item, string path)
    {
        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
        ns.Add("", "");

        XmlSerializer serializer = new XmlSerializer(item.GetType());
        FileStream stream = new FileStream(path, FileMode.Create); //File.Append
        serializer.Serialize(stream, item);
        stream.Close();
    }
    public static T Deserialize<T>(string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StreamReader reader = new StreamReader(path);
        T deserialized = (T)serializer.Deserialize(reader.BaseStream);
        reader.Close();
        return deserialized;
    }
}
