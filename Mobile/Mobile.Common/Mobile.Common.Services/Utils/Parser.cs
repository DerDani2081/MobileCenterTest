using System;
using System.IO;
using System.Xml.Serialization;

namespace Mobile.Common.Services.Utils
{
	public static class Parser
	{
		/// <summary>
		/// Parses the object from bytes.
		/// </summary>
		/// <returns>The object from byte array.</returns>
		/// <param name="b">The byte array representing the serialized object.</param>
		/// <typeparam name="T">Type in which the object should be parsed.</typeparam>
		public static T ParseObjectFromBytes<T>(byte[] b)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			MemoryStream ms = new MemoryStream(b);
			return (T)serializer.Deserialize(ms);
		}

		/// <summary>
		/// Parses the object from string.
		/// </summary>
		/// <returns>The object from string variable.</returns>
		/// <param name="b">The string variable representing the serialized object.</param>
		/// <typeparam name="T">Type in which the object should be parsed.</typeparam>
		public static T ParseObjectFromString<T>(string b)
		{
			if (String.IsNullOrEmpty(b))
			{
				return default(T);
			}

			XmlSerializer serializer = new XmlSerializer(typeof(T));
			StringReader reader = new StringReader(b);
			return (T)serializer.Deserialize(reader);
		}

		/// <summary>
		/// Parses the object to byte array.
		/// </summary>
		/// <returns>The byte array which represents the object.</returns>
		/// <param name="o">The object which should be serialized.</param>
		/// <typeparam name="T">Type of the current object.</typeparam>
		public static byte[] ParseObjectToBytes<T>(T o)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			MemoryStream ms = new MemoryStream();
			serializer.Serialize(ms, o);
			return ms.ToArray();
		}

		/// <summary>
		/// Parses the object to string.
		/// </summary>
		/// <returns>The string which represents the object.</returns>
		/// <param name="o">The object which should be serialized.</param>
		/// <typeparam name="T">Type of the current object.</typeparam>
		public static string ParseObjectToString<T>(T o)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			StringWriter writer = new StringWriter();
			serializer.Serialize(writer, o);
			String test = writer.ToString();
			return test;
		}
	}
}
