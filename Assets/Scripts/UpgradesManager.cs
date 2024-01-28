using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private List<UpgradeButton> upgradeButtons;
    private static UpgradesManager instance;
    public static float visiblePos = -475.28f;
    public static float hiddenPos = 500f;
    [SerializeField] private Sprite upgradeHearts;
    [SerializeField] private Sprite upgradeHand;


    public void DisplayUpgrades()
    {
        instance = this;
        rectTransform.DOAnchorPosY(visiblePos, 1f).SetEase(Ease.InOutQuad);
        upgradeButtons.ForEach(it => it.Activate());
        upgradeButtons.Shuffle();
        foreach (var upgradeButton in upgradeButtons)
        {
            GenerateUpgrade(upgradeButton);
            upgradeButton.transform.eulerAngles = Random.Range(-5f, 5f) * Vector3.forward;
        }
    }


    private void GenerateUpgrade(UpgradeButton upgrade)
    {
        float value = Random.Range(0f,1f);
        if (value < 0.33f)
        {
            upgrade.Setup("Increase hand size", DeckManager.IncreaseHandSize, upgradeHand, Color.white);
            return;
        }

        if (value < 0.50f)
        {
            upgrade.Setup("More health", Player.IncreaseMaxHealth, upgradeHearts, Color.white);
            return;
        }
        string key = WaveManager.getAvailableCards().getRandom();
        Debug.Log($"Selected new card {key}");
        upgrade.Setup($"Add to deck", delegate
        {
            DeckManager.AddCardToDeck(key);
        }, DataManager.dictKeyToCard[key].getIcon(), 
            DataManager.dictKeyToCard[key].getAccentColor()
            );
    }
    
    public static void CloseUpgradesPanel()
    {
        instance.upgradeButtons.ForEach(it => it.Deactivate());
        instance.rectTransform.DOAnchorPosY(hiddenPos, 1f)
            .SetEase(Ease.InOutQuad)
            .OnComplete(Player.instance.PrepareWave);
    }

}
