using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    [SerializeField] private UpgradeButton upgradeA;
    [SerializeField] private UpgradeButton upgradeB;
    [SerializeField] private UpgradeButton upgradeC;
    private static UpgradesManager instance;

    public void DisplayUpgrades()
    {
        instance = this;
        List<UpgradeButton> upgradeButtons = new List<UpgradeButton>
        {
            upgradeA,
            upgradeB,
            upgradeC
        };
        upgradeButtons.Shuffle();
        foreach (var upgradeButton in upgradeButtons)
        {
            GenerateUpgrade(upgradeButton);
        }
    }

    private void GenerateUpgrade(UpgradeButton upgrade)
    {
        
        if (Helpers.ProbabilisticBool(0.33f))
        {
            upgrade.Setup("Increase hand size", DeckManager.IncreaseHandSize);
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
        Player.instance.PrepareWave();
        instance.gameObject.SetActive(false);
    }

}
