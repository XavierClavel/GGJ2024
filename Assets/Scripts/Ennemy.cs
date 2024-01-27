using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public static List<Ennemy> ennemiesList = new List<Ennemy>();
    
    [SerializeField] private int patience;
    [SerializeField] private GameObject patiencePoint;
    [SerializeField] private Transform patienceLayout;
    [SerializeField] private EmotionDisplay prefabEmotionDisplay;
    [SerializeField] private Transform emotionLayout;
    private List<GameObject> patiencePoints = new List<GameObject>();
    private Dictionary<string, int> dictEmotions = new Dictionary<string, int>();

    private Dictionary<string, EmotionDisplay> dictKeyToEmotionDisplay = new Dictionary<string, EmotionDisplay>();
    

    private void Awake()
    {
        ennemiesList.Add(this);
        dictEmotions["Sadness"] = 2;
    }

    private void Start()
    {
        for (int i = 0; i < patience; i++)
        {
            GameObject go = Instantiate(patiencePoint, patienceLayout);
            patiencePoints.Add(go);
        }

        foreach (var value in dictEmotions)
        {
            EmotionDisplay emoDisplay = Instantiate(prefabEmotionDisplay, emotionLayout);
            emoDisplay.setup(value.Key, value.Value);
            dictKeyToEmotionDisplay[value.Key] = emoDisplay;
        }
    }

    private void OnDestroy()
    {
        ennemiesList.Remove(this);
    }

    public void ApplyEffect(Recipe recipe)
    {
        ApplyEffect(recipe.getOutput());
    }

    public void ApplyEffect(Dictionary<string, int> input)
    {
        foreach (var effet in input)
        {
            if (!dictEmotions.ContainsKey(effet.Key)) continue;
            dictEmotions[effet.Key] -= effet.Value;
            dictKeyToEmotionDisplay[effet.Key].setValue(dictEmotions[effet.Key]);
            if (dictEmotions[effet.Key] <= 0) dictEmotions.Remove(effet.Key);
        }

        if (dictEmotions.Count == 0)
        {
            Cure();
            return;
        }

        patience--;
        GameObject go = patiencePoints.Last();
        patiencePoints.Remove(go);
        go.SetActive(false);
        if (patience == 0)
        {
            Fail();
        }
    }

    private void Cure()
    {
        Debug.Log("Patient is cured !");
        Destroy(gameObject);
    }

    private void Fail()
    {
        Debug.Log("Ennemy has left");
        Destroy(gameObject);
    }
}
