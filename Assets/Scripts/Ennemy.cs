using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    [SerializeField] private int patience;
    private Dictionary<string, int> dictEmotions = new Dictionary<string, int>();
    public static List<Ennemy> ennemiesList = new List<Ennemy>();

    private void Awake()
    {
        ennemiesList.Add(this);
        dictEmotions["Sadness"] = 1;
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
            if (dictEmotions[effet.Key] <= 0) dictEmotions.Remove(effet.Key);
        }

        if (dictEmotions.Count == 0)
        {
            Cure();
            return;
        }

        patience--;
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
