using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotebookPage : MonoBehaviour
{
    [SerializeField] private List<RecipeDisplay> recipeDisplays;

    private void Start()
    {
        for (int i = 0; i < recipeDisplays.Count; i++)
        {
            if (DataManager.recipes.Count <= i) recipeDisplays[i].gameObject.SetActive(false);
            else recipeDisplays[i].DisplayRecipe(DataManager.recipes[i]);
        }
    }
}
