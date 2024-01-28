using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    public static CardsController instance;

    public Card currentCard;
    private Vector3 currentCardInitPosition;
    private Vector3 currentCardInitScale;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void OnSelectCard(Card card)
    {

        if (this.currentCard != null)
        {
            LeanTween.scale(this.currentCard.gameObject, currentCardInitScale, 0.1f);
            LeanTween.moveLocal(this.currentCard.gameObject, currentCardInitPosition, 0.1f).setEase(LeanTweenType.easeInOutQuad);

        }
     
        this.currentCard = card;
        currentCardInitPosition = this.currentCard.transform.localPosition;
        currentCardInitScale = this.currentCard.transform.localScale;

        Vector3 newPos = currentCardInitPosition;
        Vector3 newScale = currentCardInitScale;

        newPos.y = 200;
        newScale.x = 3.5f;
        newScale.y = 3.5f;

        LeanTween.scale(this.currentCard.gameObject, newScale, 0.1f);
        LeanTween.moveLocal(this.currentCard.gameObject, newPos, 0.1f).setEase(LeanTweenType.easeInOutQuad);
        }

}
