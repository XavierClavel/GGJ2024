using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnnemyManager : MonoBehaviour
{
    [SerializeField] private Ennemy ennemyPrefab;
    [SerializeField] private Transform ennemiesLayout;
    public RectTransform canvas;
    public static EnnemyManager instance;
    [SerializeField] private List<Sprite> spritesPaysans;
    [SerializeField] private List<Sprite> spritesBasic;
    [SerializeField] private List<Sprite> spritesNobles;
    [SerializeField] private List<Sprite> spritesKings;

    private void Awake()
    {
        instance = this;
    }

    public static void SpawnEnnemies()
    {
        List<float> spawnPos = new List<float>
        {
            200f,
            0f,
            -200f
        };
        WaveData waveData = WaveManager.getWaveData();
        int ennemiesAmount = waveData.ennemies.getRandom();
        for (int i = 0; i < ennemiesAmount; i++)
        {
            instance.SpawnEnnemy(spawnPos.popRandom(), waveData, WaveManager.getCurrentWave() == 20);   
        }
    }
    

    private void SpawnEnnemy(float position, WaveData waveData, bool king = false)
    {
        Ennemy ennemy = Instantiate(ennemyPrefab, ennemiesLayout, king);
        int maxPoints = waveData.emotionsPoints.getRandom();
        ennemy
            .setup(position, GenerateDictEmotions(waveData, maxPoints))
            .setSprite(getSprite(maxPoints))
            ;
    }

    private Sprite getSprite(int maxPoints)
    {
        switch (maxPoints)
        {
            case <=6:
                return spritesPaysans.getRandom();
            
            case <=13:
                return spritesBasic.getRandom();
            
            case <=19:
                return spritesNobles.getRandom();
            
            case 20:
                return spritesKings.popRandom();
            
           default:
               return spritesPaysans.getRandom();
        }
    }

    private Dictionary<string, int> GenerateDictEmotions(WaveData waveData, int maxPoints)
    {
        Dictionary<string, int> dictEmotions = new Dictionary<string, int>();
        
        List<string> emotions = WaveManager.getAvailableEmotions().getRandomList(waveData.emotionsAmount.getRandom());
        
        int pointsToSpend = maxPoints;
        int maxPerEmotion = emotions.Count == 1 ? maxPoints : maxPoints / 2 + 1;
        
        for (int i = 0; i < emotions.Count; i++)
        {
            int amountEmotionsLeft = emotions.Count - i - 1;
            
            //Make sure that if all remaining emotions spend max points total is maxPerEmotion
            int min = Mathf.Max(1,pointsToSpend - maxPerEmotion * amountEmotionsLeft);
            
            //Make sure that if all remaining emotions can spend at least 1 point
            int max = Mathf.Min(maxPerEmotion, pointsToSpend - amountEmotionsLeft);
            int amount = Random.Range(min, max+1);
            pointsToSpend -= amount;

            dictEmotions[emotions[i]] = amount;
        }

        if (pointsToSpend == 0) return dictEmotions;
        Debug.LogError("Points left not equal to zero");
        Debug.LogError($"Points to spend : {maxPoints}");
        foreach (var v in dictEmotions)
        {
            Debug.Log($"Emotion {v.Key}, Value {v.Value}");
        }

        return null;
    }
}
