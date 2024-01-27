using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI titleDisplay;
    private bool dragged = false;
    private Vector3 startPos = new Vector3(0, -4, -9);
    private CardHandler cardHandler;

    private void Start()
    {
        startPos = transform.position;
    }

    public void setup(string key)
    {
        cardHandler = DataManager.dictKeyToCard[key];
        titleDisplay.SetText(cardHandler.getKey());
    }

    private void Update()
    {
        if (!dragged) return;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
    }

    public CardHandler getCardInfo()
    {
        return cardHandler;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer down");
        dragged = true;
        Player.setSelectedCard(this);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer up");
        CardHolder cardHolder = Player.getSelectedCardHolder();
        Debug.Log(cardHolder);
        dragged = false;
        if (cardHolder == null)
        {
            transform.position = startPos;
            Player.removeCard();
        }
        else
        {
            transform.position = cardHolder.getPosition();
            Player.placeCard();
        }
        if (Player.getSelectedCard() == this) Player.setSelectedCard(null);
    }
}
 