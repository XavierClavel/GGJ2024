using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataManager", menuName = Vault.other.scriptableObjectMenu + "DataManager", order = 0)]
public class DataManager : ScriptableObject
{
    [SerializeField] private TextAsset recipeCsv;
    public static Dictionary<string, CardHandler> dictKeyToCard;
    public static Dictionary<string, EmotionHandler> dictKeyToEmotion;

    public void LoadData()
    {
        dictKeyToCard = new Dictionary<string, CardHandler>();
        CardHandler[] cardHandlers = Resources.LoadAll<CardHandler>("Cards/");
        foreach (var cardHandler in cardHandlers)
        {
            Debug.Log(cardHandler.getKey());
            dictKeyToCard[cardHandler.getKey()] = cardHandler;
        }
        
        dictKeyToEmotion = new Dictionary<string, EmotionHandler>();
        EmotionHandler[] emotionHandlers = Resources.LoadAll<EmotionHandler>("Emotions/");
        foreach (var emotionHandler in emotionHandlers)
        {
            Debug.Log(emotionHandler.getKey());
            dictKeyToEmotion[emotionHandler.getKey()] = emotionHandler;
        }
    }
}
