using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class Card : MonoBehaviour,ICard
{
    public CardData data;
    public GameObject front;
    public GameObject back;
    public bool isLeadingCard = false;

    private Image frontImage;
    private Sprite frontSprite;

    private Button button;
    private CardDragHandler dragHandler;

    public Player cardOwner;

    Transform parent;

    public enum Suit
    {
        Diamonds = 0,
        Spades = 1,
        Hearts = 2,
        Clubs = 3,
        Joker = 4
    }

    public Suit suit;

    private void Awake()
    {
        parent = transform.parent;
        frontImage = front.GetComponent<Image>();
        dragHandler = GetComponent<CardDragHandler>();
        button = GetComponent<Button>();
    }

    private void Start()
    {
        dragHandler.enabled = false;
        button.interactable = false;
    }

    public  void SetData(CardData data)
    {
        this.data = data;
        this.suit = (Suit)Enum.Parse(typeof(Card.Suit), data.suit);
        SetCardUI();
    }

    public void SetOwner(Player player)
    {
        this.cardOwner = player;
    }

    void SetCardUI()
    {
        frontSprite = Resources.Load<Sprite>($"Cards/{this.data.shortCode}");

        if (frontSprite != null)
            frontImage.sprite = frontSprite;
    }

    public void SwitchSide(bool showOnlyFront = false, bool animated=false, bool isVertical=false)
    {
        if (animated)
        {
            SwichSideAnimated(isVertical);
            return;
        }

        bool isFrontActive = front.activeSelf && !showOnlyFront;

        front.SetActive(!isFrontActive);
        back.SetActive(isFrontActive);
    }

    public void SwichSideAnimated(bool isVertical=false)
    {
        float x = isVertical?90:0;
        float y = isVertical ? 0 : 90;


        if (front.activeSelf)
        {
            RotateObjectAnimated(front, Vector2.zero, new Vector2(x, y), () =>
            {
                front.SetActive(false);
                front.transform.localEulerAngles = Vector2.zero;

                RotateObjectAnimated(back, new Vector2(x, y), Vector2.zero, () =>
                {
                    back.SetActive(true);
                });
            });
        }
        else
        {
            RotateObjectAnimated(back, Vector2.zero, new Vector2(x, y), () =>
            {
                back.SetActive(false);
                back.transform.localEulerAngles = Vector2.zero;

                RotateObjectAnimated(front, new Vector2(x, y), Vector2.zero, () =>
                {
                    front.SetActive(true);
                });
            });           
        }
    }

    private void RotateObjectAnimated(GameObject obj, Vector3 startRotation, Vector3 endRotation, System.Action onComplete)
    {
        obj.transform.localEulerAngles = startRotation;
        obj.SetActive(true);

        LeanTween.rotateLocal(obj, endRotation, 0.2f).setOnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void SetInitialParent()
    {
        transform.SetParent(parent);
    }

    public void MoveCard(Transform trans, float customSize = 1, bool makeParent = true, bool hideOnReach = false, System.Action onComplete=null)
    {
        LeanTween.scale(gameObject, new Vector2(customSize, customSize), 0.1f);

        LeanTween.move(gameObject, trans.position, 0.1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            if (makeParent)
            {
                transform.SetParent(trans);
                trans.SetAsLastSibling();
            }
            gameObject.SetActive(!hideOnReach);

            transform.eulerAngles = Vector3.zero;
            onComplete?.Invoke();
        });
    }

    public void MoveToPlayerPosition(int playerIndex, bool makeParent=true)
    {
        Transform currentPlayerDeckTransform = TableController.instance.GetPlayerCardsTransform(playerIndex);

        LeanTween.rotate(gameObject, Vector2.zero, 0.1f).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.scale(gameObject, Vector2.one * (this.cardOwner.isOwn? 3f:1),0.1f);
        LeanTween.move(gameObject, currentPlayerDeckTransform.position, 0.1f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            if (makeParent)
            {
                if (this.cardOwner.isOwn)
                {
                    transform.SetParent(currentPlayerDeckTransform);
                }
                else
                {
                    gameObject.SetActive(false);
                    UIEvents.UpdateData(Panel.PlayersUIPanel, null, "UpdateCardCount", playerIndex, this.cardOwner.hand.Count);
                }

                transform.localEulerAngles = Vector3.zero;

                HandCardsUI hand = transform.GetComponentInParent<HandCardsUI>();
                if (hand != null)
                {
                    hand.UpdateCardArrangement();
                }
            }
        });
    }

    public void PlaceCardOnTable()
    {
        ActiveButton(false);
        ActiveDragable(false);
        GameplayManager.instance.PlaceCardOnTable(cardOwner,this);
    }

    public void ActiveButton(bool interactable=true)
    {
        this.button.interactable = interactable;
    }

    public void ActiveDragable(bool enabled = true)
    {
        this.dragHandler.enabled = enabled;

    }

    public void HighlightCard(bool normal)
    {
        Color color = normal ? Color.white : Color.gray;
        frontImage.color = color;
    }
}
