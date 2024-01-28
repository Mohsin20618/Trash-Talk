using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine;

public class CardDragHandler : MonoBehaviour,IPointerDownHandler, IDragHandler,IPointerUpHandler, IBeginDragHandler
{
    private Vector2 initialPosition;
    private bool isDragging;
    private bool isPlaceable = false;
    private Card card;
 //   private SortingGroup sortingGroup;
 //   private int intialSorting;

    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<Card>();
     //   sortingGroup = GetComponent<SortingGroup>();
     //   intialSorting = sortingGroup.sortingOrder;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("card selected");
        if(CardsController.instance.currentCard == card)
        {
            UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowHideYourTurnHeading", false);
            card.PlaceCardOnTable();

        }
        else
            CardsController.instance.OnSelectCard(card);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        return;
        if (!isDragging)
        {
            initialPosition = transform.position;
            isDragging = true;
            TableController.instance.ShowTableCardArea(true);
            UIEvents.UpdateData(Panel.PlayersUIPanel, null, "ShowHideYourTurnHeading", false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        return;
        if (isDragging)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out Vector2 localPosition);
            isPlaceable = TableController.instance.IsCardPlaceable(eventData.position, eventData.enterEventCamera);
            transform.localPosition = localPosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        return;
        TableController.instance.ShowTableCardArea(false);

        if (!isDragging)
            return;

        isDragging = false;

        if (isPlaceable)
        {
            card.PlaceCardOnTable();
        //    isDragging = false;

            //    // Check if the card is dropped on the table area
            //    if (RectTransformUtility.RectangleContainsScreenPoint(TableRectTransform, eventData.position, eventData.pressEventCamera))
            //    {
            //        // Card is dropped on the table, do something
            //        Debug.Log("Card dropped on the table!");
            //    }
            //    else
            //    {
            //        // Card is dropped elsewhere, return it to the initial position
            //        transform.position = initialPosition;
            //    }
        }
        else
        {
            transform.position = initialPosition;
        }

    }

}
