using System;
using UnityEngine;

public class TextureConverter : MonoBehaviour
{
	public Texture2D defaultTexture;

	public static TextureConverter instance;

	public static byte[] bytesData = null;

	private void Awake()
    {
        instance = this;
    }
    public static string Texture2DToBase64(Texture2D texture)
	{
        if (texture == null)
        {
			texture = instance.defaultTexture;
		}
		byte[] imageData = null;

		if (texture.isReadable)
        {
			imageData = duplicateTexture(texture).EncodeToJPG();
        }
		string imageString = Convert.ToBase64String(imageData);
		SaveBase64Image(imageString);
		return Convert.ToBase64String(imageData);
	}

	public static byte[] Texture2DToBytes(Texture2D texture)
	{
        if (texture == null)
        {
			texture = instance.defaultTexture;
		}


		if (texture.isReadable)
        {
			bytesData = duplicateTexture(texture).EncodeToPNG();
		}
		return bytesData;
	}

	static Texture2D duplicateTexture(Texture2D source)
	{
		RenderTexture renderTex = RenderTexture.GetTemporary(
					source.width,
					source.height,
					0,
					RenderTextureFormat.Default,
					RenderTextureReadWrite.Linear);

		Graphics.Blit(source, renderTex);
		RenderTexture previous = RenderTexture.active;
		RenderTexture.active = renderTex;
		Texture2D readableText = new Texture2D(source.width, source.height);
		readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
		readableText.Apply();
		RenderTexture.active = previous;
		RenderTexture.ReleaseTemporary(renderTex);
		return readableText;
	}
	public static Texture2D Base64ToTexture2D(string encodedData)
	{
        if (string.IsNullOrEmpty(encodedData))
        {
			return instance.defaultTexture;
        }
		byte[] imageData = Convert.FromBase64String(encodedData);

		int width, height;
		GetImageSize(imageData, out width, out height);

		Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
		texture.hideFlags = HideFlags.HideAndDontSave;
		texture.filterMode = FilterMode.Point;
		texture.LoadImage(imageData);

		return texture;
	}

	public static void SaveBase64Image(string base64String)
	{
		PlayerPrefs.SetString("Picture", base64String);
		PlayerPrefs.Save();
	}

	public static string Get_Base64Image()
	{
		string base64String = PlayerPrefs.GetString("Picture");
		return base64String;
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
