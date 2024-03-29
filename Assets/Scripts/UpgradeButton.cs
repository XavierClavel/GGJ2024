using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private Button button;
    [SerializeField] private Image icon;

    public void Setup(string text, UnityAction action, Sprite sprite, Color color)
    {
        icon.sprite = sprite;
        icon.color = color;
        textDisplay.SetText(text);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
        button.onClick.AddListener(UpgradesManager.CloseUpgradesPanel);
    }


    public void Deactivate()
    {
        button.interactable = false;
    }

    public void Activate()
    {
        button.interactable = true;
    }

    
}
