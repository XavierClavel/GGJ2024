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
        dragged = false;
        if (cardHolder == null)
        {
            transform.position = startPos;
        }
        else
        {
            transform.position = cardHolder.getPosition();
            Player.placeCard();
        }
        
        
    }


    public CardHandler getCardInfo()
    {
        return cardHandler;
    }
}
 