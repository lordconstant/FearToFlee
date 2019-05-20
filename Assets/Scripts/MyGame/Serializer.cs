using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
#if NETFX_CORE
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Storage.Streams;
#endif

//Allows saving and loading of a file from an xml file
public class Serializer : MonoBehaviour{
	public static T Load<T>(string filename) where T: class
	{
		T tempVar;
		#if NETFX_CORE
			tempVar = LoadWinPhone<T>(filename).Result;
		#elif UNITY_ANDROID
			tempVar = LoadAndroid<T>(filename);
		#else
			tempVar = LoadWin<T>(filename);
		#endif

		return tempVar;
	}
	
	public static void Save<T>(string filename, T data) where T: class
	{	
		#if NETFX_CORE
			SaveWinPhone<T>(filename, data);
		#elif UNITY_ANDROID
			SaveAndroid<T>(filename, data);
		#else
			SaveWin<T>(filename, data);
		#endif
	}

	public static bool FileExists(string filename){
		#if NETFX_CORE
			bool myFile = ExistsWinPhone(filename).Result;
			return myFile;
		#elif UNITY_ANDROID
			return ExistsAndroid(filename);
		#else
			return ExistsWin(filename);
		#endif
	}

#if NETFX_CORE
	private static async Task<T> LoadWinPhone<T>(string filename) where T: class
	{
		XmlSerializer tempXmlSeri = new XmlSerializer(typeof(T));
		Stream tempStreamRead;
		var uri = new Uri("ms-appx:///Data/StreamingAssets/"+filename);
		var myFile = await StorageFile.GetFileFromApplicationUriAsync(uri).AsTask().ConfigureAwait(false);
		tempStreamRead = await myFile.OpenStreamForReadAsync().ConfigureAwait(false);

		T loadedData = tempXmlSeri.Deserialize(tempStreamRead) as T;
		tempStreamRead.Dispose();
		return loadedData;
	}

	private static async void SaveWinPhone<T>(string filename, T data) where T: class
	{	
		var uri = new Uri("ms-appx:///Data/StreamingAssets/"+filename);
		XmlSerializer tempXmlSeri = new XmlSerializer(typeof(T));
		var myFile = await StorageFile.GetFileFromApplicationUriAsync(uri).AsTask().ConfigureAwait(false);
		Stream tempStream = await myFile.OpenStreamForWriteAsync().ConfigureAwait(false);
		//Used to encode the file to a windows phone 8 acceptable format
		XmlTextWriter tempTextWrite = new XmlTextWriter(tempStream, Encoding.UTF8);
		tempXmlSeri.Serialize(tempTextWrite, data);
		tempStream.Dispose();
	}

	private static async Task<bool> ExistsWinPhone(string filename){
		try{
		var uri = new Uri("ms-appx:///Data/StreamingAssets/"+filename);
		var myFile = await StorageFile.GetFileFromApplicationUriAsync(uri).AsTask().ConfigureAwait(false);
			return true;
		}
		catch(FileNotFoundException){
			return false;
		}
	}
#elif UNITY_ANDROID
	private static T LoadAndroid<T>(string filename) where T: class{
		string filePath = Application.streamingAssetsPath + "/" + filename;
		WWW www = new WWW(filePath);

		while(!www.isDone){
			//if(www.error != ""){
			//	return default(T);
			//}
		}

		//StringReader tempStringReader = new StringReader(www.text);
		Stream tempStream = GenerateStreamFromString(www.text);
		XmlSerializer tempXmlSeri = new XmlSerializer(typeof(T));
		T loadedData = tempXmlSeri.Deserialize(tempStream) as T;
		www.Dispose ();
		tempStream.Close();
		return loadedData;
	}
	
	private static void SaveAndroid<T>(string filename, T data) where T: class{	
		string filePath = Application.streamingAssetsPath + "/" + filename;
		WWW www = new WWW(filePath);
		
		while(!www.isDone){
			//if(www.error != ""){
			//	return default(T);
			//}
		}

		Stream tempStream = GenerateStreamFromString(www.text);
		XmlSerializer tempXmlSeri = new XmlSerializer(typeof(T));
		//Used to encode the file to a windows phone 8 acceptable format
		XmlTextWriter tempTextWrite = new XmlTextWriter(tempStream, Encoding.UTF8);
		tempXmlSeri.Serialize(tempTextWrite, data);
		www.Dispose ();
		tempStream.Close();
	}

	private static bool ExistsAndroid(string filename){
		return true;
		string filePath = Application.streamingAssetsPath + "/" + filename;
		
		WWW www = new WWW(filePath);
		
		while(!www.isDone){
			//if(www.error != ""){
			//	return;
			//}
		}

		return true;
	}
#else
    private static T LoadWin<T>(string filename) where T: class{
		string filePath = Application.streamingAssetsPath + "/" + filename;
		XmlSerializer tempXmlSeri = new XmlSerializer(typeof(T));
		Stream tempStreamRead;
		tempStreamRead = File.OpenText(filePath).BaseStream;
		T loadedData = tempXmlSeri.Deserialize(tempStreamRead) as T;
		tempStreamRead.Close();
		return loadedData;
	}

	private static void SaveWin<T>(string filename, T data) where T: class{	
		string filePath = Application.streamingAssetsPath + "/" + filename;
		
		XmlSerializer tempXmlSeri = new XmlSerializer(typeof(T));
		Stream tempStream;
		tempStream = File.Create (filePath);
		//Used to encode the file to a windows phone 8 acceptable format
		XmlTextWriter tempTextWrite = new XmlTextWriter(tempStream, Encoding.UTF8);
		tempXmlSeri.Serialize(tempTextWrite, data);
		tempTextWrite.Close();
	}

	private static bool ExistsWin(string filename){
		string filePath = Application.streamingAssetsPath + "/" + filename;

		return File.Exists (filePath);
	}
#endif

	private static Stream GenerateStreamFromString(string s)
	{
		MemoryStream stream = new MemoryStream();
		StreamWriter writer = new StreamWriter(stream);
		writer.Write(s);
		writer.Flush();
		stream.Position = 0;
		return stream;
	}
}