using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataManager", menuName = Vault.other.scriptableObjectMenu + "DataManager", order = 0)]
public class DataManager : ScriptableObject
{
    [SerializeField] private TextAsset recipeCsv;
    [SerializeField] private TextAsset waveDataCsv;
    public static Dictionary<string, CardHandler> dictKeyToCard;
    public static Dictionary<string, EmotionHandler> dictKeyToEmotion;
    public static Dictionary<int, WaveData> dictIndexToWaveData;
    public static Dictionary<string, Sfx> dictKeyToSfx;
    public static List<Recipe> recipes;

    public void LoadData()
    {
        dictKeyToCard = new Dictionary<string, CardHandler>();
        CardHandler[] cardHandlers = Resources.LoadAll<CardHandler>("Actions/");
        foreach (var cardHandler in cardHandlers)
        {
            Debug.Log(cardHandler.getKey());
            dictKeyToCard[cardHandler.getKey()] = cardHandler;
        }
        IntonationHandler[] intonationHandlers = Resources.LoadAll<IntonationHandler>("Intonations/");
        foreach (var cardHandler in intonationHandlers)
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

        dictKeyToSfx = new Dictionary<string, Sfx>();
        Sfx[] sfxs = Resources.LoadAll<Sfx>("Sfx/");
        foreach (var sfx in sfxs)
        {
            dictKeyToSfx[sfx.getKey()] = sfx;
        }
        
        RecipeBuilder recipeBuilder = new RecipeBuilder();
        recipes = new List<Recipe>();
        recipeBuilder.loadText(recipeCsv, ref recipes, "Recipes");

        WaveDataBuilder waveDataBuilder = new WaveDataBuilder();
        dictIndexToWaveData = new Dictionary<int, WaveData>();
        waveDataBuilder.loadText(waveDataCsv, ref dictIndexToWaveData, "Wave Data");

        foreach (var input in recipes[0].getInput())
        {
            Debug.Log($"input : {input}");
        }
        
        foreach (var output in recipes[0].getOutput())
        {
            Debug.Log($"output : {output.Key}, amount : {output.Value}");
        }
    }
}
