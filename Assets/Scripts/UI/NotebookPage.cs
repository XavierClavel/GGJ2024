using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookPage : MonoBehaviour
{
    [SerializeField] private List<RecipeDisplay> recipeDisplays;

    public int getSize() => recipeDisplays.Count;

    public void DisplayRecipes(int startIndex)
    {
        for (int i = 0; i < recipeDisplays.Count; i++)
        {
            if (DataManager.recipes.Count <= startIndex + i) recipeDisplays[i].gameObject.SetActive(false);
            else recipeDisplays[i].DisplayRecipe(DataManager.recipes[startIndex + i]);
        }
    }

    public void PreviousPage()
    {
        Notebook.instance.PreviousPage();
    }

    public void NextPage()
    {
        Notebook.instance.NextPage();
    }
}
