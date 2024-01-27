using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private List<UpgradeButton> upgradeButtons;
    private static UpgradesManager instance;
    private float visiblePos = -475.28f;
    private float hiddenPos = 500f;


    public void DisplayUpgrades()
    {
        instance = this;
        rectTransform.DOAnchorPosY(visiblePos, 1f).SetEase(Ease.InOutQuad);
        upgradeButtons.ForEach(it => it.Activate());
        upgradeButtons.Shuffle();
        foreach (var upgradeButton in upgradeButtons)
        {
            GenerateUpgrade(upgradeButton);
        }
    }


    private void GenerateUpgrade(UpgradeButton upgrade)
    {
        float value = Random.Range(0f,1f);
        if (value < 0.33f)
        {
            upgrade.Setup("Increase hand size", DeckManager.IncreaseHandSize);
            return;
        }

        if (value < 0.50f)
        {
            upgrade.Setup("More health", Player.IncreaseMaxHealth);
            return;
        }
        string key = WaveManager.getAvailableCards().getRandom();
        Debug.Log($"Selected new card {key}");
        upgrade.Setup($"Add {key} card to deck", delegate
        {
            DeckManager.AddCardToDeck(key);
        });
    }
    
    public static void CloseUpgradesPanel()
    {
        instance.upgradeButtons.ForEach(it => it.Deactivate());
        instance.rectTransform.DOAnchorPosY(instance.hiddenPos, 1f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(Player.instance.PrepareWave);
    }

}
