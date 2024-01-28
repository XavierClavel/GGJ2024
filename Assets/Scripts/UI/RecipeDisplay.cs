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
    [SerializeField] private Sprite plusIcon;
    [SerializeField] private Sprite equalsIcon;

    public void DisplayRecipe(Recipe recipe)
    {
        int index = 0;
        foreach (var key in recipe.getInput())
        {
            images[index].sprite = DataManager.dictKeyToCard[key].getIcon();
            images[index].color = DataManager.dictKeyToCard[key].getAccentColor();

            index++;

            images[index].sprite = plusIcon;
            index++;
        }

        for (int i = index; i < 5; i++)
        {
            images[i].gameObject.SetActive(false);
        }

        index = 5;

        images[index].sprite = equalsIcon;

        int textIndex = -1;

        foreach (var emotion in recipe.getOutput())
        {
            if (emotion.Value == 0) continue;
            index++;
            textIndex++;
            
            images[index].sprite = DataManager.dictKeyToEmotion[emotion.Key].getIcon();
            texts[textIndex].SetText(emotion.Value.ToString());
        }

        index++;
        if (index >= images.Count) return;

        for (int i = index; i < images.Count; i++)
        {
            images[i].gameObject.SetActive(false);
        }
    }
}
