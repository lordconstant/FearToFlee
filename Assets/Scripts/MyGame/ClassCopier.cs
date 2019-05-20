using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

public class ClassCopier {
	public static T CloneClass<T>(T data) where T: class{
		if(data == null){
			return default(T);
		}

		//Serializes the data the deserializes it to create a copy
		XmlSerializer tempXmlSeri = new XmlSerializer(typeof(T));
		MemoryStream stream = new MemoryStream();

		tempXmlSeri.Serialize(stream, data);
		stream.Seek(0, SeekOrigin.Begin);
		T copy = tempXmlSeri.Deserialize(stream) as T;
		return copy;
	}
}
