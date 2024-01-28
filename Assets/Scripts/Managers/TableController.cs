using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public Transform[] playerCardsPositions;
    public Transform[] showCardPositions;
    public Transform[] wonCardTransform;

    public GameObject cardAreaBorder;
    private RectTransform cardAreaRect;

    public GameObject topTable;
    public GameObject sideTable;


    public static TableController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        cardAreaRect = cardAreaBorder.GetComponent<RectTransform>();
    }

    public void ShowSideTable(bool showSide=true)
    {
        topTable.SetActive(!showSide);
        sideTable.SetActive(showSide);
    }

    public Transform GetPlayerCardsTransform(int playerNumber)
    {
        return playerCardsPositions[playerNumber];
    }

    public Transform GetPlayerShowCardTransform(int playerNumber)
    {
        return showCardPositions[playerNumber];
    }

    public Transform GetPlayerWonCardTransform(int playerTablePos)
    {
        return wonCardTransform[playerTablePos];
    }

    public bool IsCardPlaceable(Vector3 pos, Camera cam)
    {
    //    RectTransformUtility.RectangleContainsScreenPoint(cardAreaRect, pos, cam);
        return RectTransformUtility.RectangleContainsScreenPoint(cardAreaRect, pos, cam);


        //      // Check if the card is dropped on the table area
        //if (RectTransformUtility.RectangleContainsScreenPoint(TableRectTransform, eventData.position, eventData.pressEventCamera))
        //{
        //    // Card is dropped on the table, do something
        //    Debug.Log("Card dropped on the table!");
        //}
        //else
        //{
        //    // Card is dropped elsewhere, return it to the initial position
        //    transform.position = initialPosition;
        //}

    }

    //public Transform GetPlayerWinCardTransform()
    //{

    //}

    public void ClearCards()
    {
        foreach(Transform child in wonCardTransform)
        {
            foreach(Transform subChild in child)
            {
            //    Destroy(subChild.gameObject);
            }
        }
    }

    public void ClearAllContainers()
    {
        foreach (Transform child in playerCardsPositions)
        {
            foreach (Transform subChild in child)
            {
                Destroy(subChild.gameObject);
            }
        }


        foreach (Transform child in showCardPositions)
        {
            foreach (Transform subChild in child)
            {
                Destroy(subChild.gameObject);
            }
        }

        foreach (Transform child in wonCardTransform)
        {
            foreach(Transform subChild in child)
            {
                Destroy(subChild.gameObject);
            }
        }
}

    public void ShowTableCardArea(bool show)
    {
        cardAreaBorder.SetActive(show);
    }
}
