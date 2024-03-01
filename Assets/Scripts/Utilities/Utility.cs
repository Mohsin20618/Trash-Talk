using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public static class Utility
{
  //  public static Utility instance;

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //}

    public static void DisableButtonChilds(Button obj)   //Alpha child according to parent alpha
    {
        
        if (obj == null)
            return;

        float alpha = obj.interactable? obj.colors.normalColor.a:obj.colors.disabledColor.a;
        foreach (Transform child in obj.transform)
        {
            Image image = child.GetComponent<Image>();

            if (image != null)
            {
                Color color = image.color;
                color.a = alpha;
                image.color = color;
            }
        }
    }

    public static int GetRandom(int min, int max)
    {
        return Random.Range(min, max);
    }
    public static List<int> CalculateUserLevel(int winnings)
    {
        int level = 1;
        int winningsThreshold = 5; // Initial threshold for level 1


        while (winnings >= winningsThreshold)
        {
            winnings -= winningsThreshold;
            level++;
            winningsThreshold += 5 + (level - 1) * 2;
        }

        List<int> result = new List<int>();
        result.Add(level);
        result.Add(winnings);
        result.Add(winningsThreshold);

        return result;
    }
}
