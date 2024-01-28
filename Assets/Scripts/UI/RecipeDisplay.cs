using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplay : MonoBehaviour
{
    [SerializeField] private List<Image> images;
    [SerializeField] private List<TextMeshProUGUI> texts;
    private int recipeIndex;

    public void DisplayRecipe(Recipe recipe, int recipeIndex)
    {
        this.recipeIndex = recipeIndex;
        bool isRecipeUncovered = RecipeManager.isRecipeUncovered(recipeIndex);
        int index = 0;
        foreach (var key in recipe.getInput())
        {
            images[index].sprite = isRecipeUncovered ? DataManager.dictKeyToCard[key].getIcon() : Notebook.instance.unknownIcon;
            images[index].color = isRecipeUncovered ? DataManager.dictKeyToCard[key].getAccentColor() : Color.white;

            index++;

            images[index].sprite = Notebook.instance.plusIcon;
            index++;
        }

        for (int i = index - 1; i < 5; i++)
        {
            images[i].gameObject.SetActive(false);
        }

        index = 5;

        images[index].sprite = Notebook.instance.equalsIcon;

        int textIndex = -1;

        foreach (var emotion in recipe.getOutput())
        {
            if (emotion.Value == 0) continue;
            index++;
            textIndex++;
            
            images[index].sprite = isRecipeUncovered ? DataManager.dictKeyToEmotion[emotion.Key].getIcon() : Notebook.instance.unknownIcon;
            if (!isRecipeUncovered) texts[textIndex].SetText("");
            else if (emotion.Value == 1) texts[textIndex].SetText("");
            else texts[textIndex].SetText(emotion.Value.ToString());
        }

        index++;
        if (index >= images.Count) return;

        for (int i = index; i < images.Count; i++)
        {
            images[i].gameObject.SetActive(false);
        }
    }
}
