using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public int index;
    private void OnMouseEnter()
    {
        Debug.Log("Mouse enter");
        Player.setSelectedCardHolder(this);
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse exit");
        if (Player.getSelectedCardHolder() != this) return;
        Player.setSelectedCardHolder(null);
    }

    public Vector3 getPosition()
    {
        return transform.position;
    }

    private void OnMouseUp()
    {
        Debug.Log("Mouse Up");
        
    }

}
