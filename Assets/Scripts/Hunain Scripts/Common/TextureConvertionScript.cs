using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

public class TextureConvertionScript : MonoBehaviour
{
    public static string base64Texture;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }


	// Compress

	public static byte[] Compress(byte[] data)
	{
		using (var compressedStream = new MemoryStream())
		using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
		{
			zipStream.Write(data, 0, data.Length);
			zipStream.Close();
			return compressedStream.ToArray();
		}
	}

	//Encode
	public static string base64_encode(byte[] data)
	{
		if (data == null)
			throw new ArgumentNullException("data");
		return Convert.ToBase64String(data);
	}


	//Decode 
	public static byte[] base64_decode(string encodedData)
	{
		byte[] encodedDataAsBytes = Convert.FromBase64String(encodedData);
		return encodedDataAsBytes;
	}

	public static byte[] Decompress(byte[] data)
	{
		using (var compressedStream = new MemoryStream(data))
		using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
		using (var resultStream = new MemoryStream())
		{
			var buffer = new byte[4096];
			int read;

			while ((read = zipStream.Read(buffer, 0, buffer.Length)) > 0)
			{
				resultStream.Write(buffer, 0, read);
			}

			return resultStream.ToArray();
		}
	}
	//Decompressing Above

	public static string Texture2DToBase64(Texture2D texture)
	{
		byte[] imageData = texture.EncodeToJPG();
		//Debug.LogError("Convert.ToBase64String(imageData).Length: " +Convert.ToBase64String(imageData));
		return Convert.ToBase64String(imageData);
	}

	public static Texture2D Base64ToTexture2D(string encodedData)
	{
		byte[] imageData = Convert.FromBase64String(encodedData);

		int width, height;
		GetImageSize(imageData, out width, out height);

		Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
		texture.hideFlags = HideFlags.HideAndDontSave;
		texture.filterMode = FilterMode.Point;
		texture.LoadImage(imageData);

		return texture;
	}

	private static void GetImageSize(byte[] imageData, out int width, out int height)
	{
		width = ReadInt(imageData, 3 + 15);
		height = ReadInt(imageData, 3 + 15 + 2 + 2);
	}

	private static int ReadInt(byte[] imageData, int offset)
	{
		return (imageData[offset] << 8) | imageData[offset + 1];
	}


}
