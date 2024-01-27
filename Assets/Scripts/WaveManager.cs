using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaveManager
{
    private static int currentWave = 0;

    public static void IncreaseWave() => currentWave++;
    public static void ResetWave() => currentWave = 0;
    public static int getCurrentWave() => currentWave;
    public static WaveData getWaveData() => DataManager.dictIndexToWaveData[getCurrentWave()];

    public static bool isWaveShop() => currentWave is 5 or 10 or 15 or 19;

    public static List<string> getAvailableEmotions()
    {
        List<string> availableEmotions = new List<string>
        {
            "Sadness",
            "Anger",
            "Fear",
        };
        if (currentWave >= 5) availableEmotions.Add("Disdain");
        return availableEmotions;
    }

    public static List<string> getAvailableCards()
    {
        List<string> availableCards = new List<string>
        {
            "Talk",
            "Sing",
            "Dance",
        };
        if (currentWave >= 4) availableCards.Add("jsp");
        if (currentWave >= 8) availableCards.Add("jsp");
        if (currentWave >= 12) availableCards.Add("jsp");
        return availableCards;
    }
}
