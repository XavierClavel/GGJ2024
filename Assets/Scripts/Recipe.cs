using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{
    private List<string> cards = new List<string>();
    private Dictionary<string, int> emotions = new Dictionary<string, int>();

    public void addInput(string key)
    {
        if (key != null) cards.Add(key);
    }
    public void addOutput(string key, int amount)
    {
        if (amount > 0) emotions[key] = amount;
    }

    public void addOutput(Recipe other)
    {
        foreach (var output in other.getOutput())
        {
            addOutput(output.Key, output.Value);
        }
    }
    
    public List<string> getInput() => cards;
    public int getInputSize() => cards.Count;

    public Dictionary<string, int> getOutput() => emotions;

    public bool matchesInput(List<string> keys)
    {
        if (keys.Count != getInputSize()) return false;
        if (keys.Count >= 1 && keys[0] != cards[0]) return false;
        if (keys.Count >= 2 && keys[1] != cards[1]) return false;
        if (keys.Count >= 3 && keys[2] != cards[2]) return false;
        return true;
    }

    public bool matchesInput(string key)
    {
        return cards[0] == key;
    }
}