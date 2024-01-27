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

    public void Setup(string text, UnityAction action)
    {
        textDisplay.SetText(text);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
        button.onClick.AddListener(UpgradesManager.CloseUpgradesPanel);
    }

    
}
