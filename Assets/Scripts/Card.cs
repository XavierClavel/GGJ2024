using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    [SerializeField] private Collider collider;
    private bool dragged = false;
    private Vector3 startPos = new Vector3(0, -4, -9);
    [SerializeField] private CardHandler cardHandler;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (!dragged) return;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector3.forward;
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse down");
        dragged = true;
        Player.setSelectedCard(this);
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse Up");
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


    public CardHandler getCardInfo()
    {
        return cardHandler;
    }
}
 