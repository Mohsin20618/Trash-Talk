using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AvatarUpdater : MonoBehaviour
{
    public int maxSize = 256;
 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private static Texture2D texture = null;
    public void PickImage()
    {
        Debug.Log("Image path: ");
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                Debug.Log("Image path: " + path);
                // Create Texture from selected image
                texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    MesgBar.instance.show("Couldn't load Image", true);
                    return;
                }
            }
        }, "Select a JPG image", "image/jpg");
        if (permission == NativeGallery.Permission.Denied)
        {
            MesgBar.instance.show("Permission Denied! Allow Permission From Phone Setting", true);

        }
        Debug.Log("Permission result: " + permission);
    }
}
