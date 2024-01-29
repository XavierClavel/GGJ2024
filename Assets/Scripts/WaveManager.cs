using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaveManager
{
    private static int currentWave = 0;

    public static void IncreaseWave() => currentWave++;
    public static void ResetWave() => currentWave = 4;
    public static int getCurrentWave() => currentWave;
    public static WaveData getWaveData() => DataManager.dictIndexToWaveData[getCurrentWave()];

    public static bool isWaveShop() => currentWave is 5 or 10 or 15 or 19;

    public static List<string> getAvailableEmotions()
    {
        List<string> availableEmotions = new List<string>
        {
            "Fear",
            "Anger",
            "Sadness",
        };
        if (currentWave >= 4) availableEmotions.Add("Disdain");
        if (currentWave >= 8) availableEmotions.Add("Disgust");
        if (currentWave >= 12) availableEmotions.Add("Depression");
        return availableEmotions;
    }

    public static List<string> getAvailableCards()
    {
        List<string> availableCards = new List<string>
        {
            "Joke",
            "Cuddle",
            "Talk",
        };
        if (currentWave >= 2) availableCards.Add("Sing");
        if (currentWave >= 4) availableCards.Add("Soft");
        if (currentWave >= 6) availableCards.Add("Dance");
        if (currentWave >= 8) availableCards.Add("Loud");
        if (currentWave >= 10) availableCards.Add("Tell");
        if (currentWave >= 12) availableCards.Add("Playful");
        return availableCards;
    }
}
