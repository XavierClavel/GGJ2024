
using System;
using System.Collections;
using UnityEngine;

public static class SaveManager
{
    public static void Save()
    {
        BitArray bitArray = new BitArray(DataManager.recipes.Count);
        for (int i = 0; i < bitArray.Count; i++)
        {
            bitArray[i] = RecipeManager.isRecipeUncovered(i);
        }

        string s = bitArray.toBase64String();
        PlayerPrefs.SetString("Notebook", s);
    }

    public static void Load()
    {
        if (!PlayerPrefs.HasKey("Notebook"))
        {
            Save();
            return;
        }
        
        string s = PlayerPrefs.GetString("Notebook");
        Byte[] b = Convert.FromBase64String(s);
        BitArray bits = new BitArray(b);
        for (int i = 0; i < bits.Count; i++)
        {
            if (!bits[i]) continue;
            RecipeManager.uncoverRecipe(i);
            Debug.Log($"{i}");
        }
    }

    public static void Erase()
    {
        RecipeManager.eraseRecipes();
        Save();
    }
}