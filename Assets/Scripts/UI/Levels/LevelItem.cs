using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelItem : MonoBehaviour
{
    //public int minMatch;
    //public int maxMatch;
    public int level;
    public RectTransform progressbar;

    private float maxWidth;
    // Start is called before the first frame update
    //void Awake()
    //{
    //    maxWidth = progressbar.sizeDelta.x;
    //}

    public void SetData(int l, float progress)
    {

        maxWidth = progressbar.sizeDelta.x;

        if(l < level)
        {
            progressbar.sizeDelta = new Vector2(0, progressbar.sizeDelta.y);
            return;
        }
        else if (l > level)
        {
            progressbar.sizeDelta = new Vector2(maxWidth, progressbar.sizeDelta.y);
            return;
        }

        progressbar.sizeDelta = new Vector2((progress * maxWidth), progressbar.sizeDelta.y);

        //      if (wonMatches <= minMatch)
        //      {
        //          progressbar.sizeDelta = new Vector2(0, progressbar.sizeDelta.y);
        //          return;
        //      }
        //      else if(wonMatches > maxMatch)
        //      {
        //          progressbar.sizeDelta = new Vector2(maxWidth, progressbar.sizeDelta.y);
        //          return;
        //      }

        ////      progressbar.sizeDelta = new Vector2((((float)min / (float)maxMatch) * maxWidth), progressbar.sizeDelta.y);

        //      progressbar.sizeDelta = new Vector2(((((float)wonMatches - (float)minMatch) / ((float)maxMatch-(float)minMatch)) * maxWidth), progressbar.sizeDelta.y);

    }
}