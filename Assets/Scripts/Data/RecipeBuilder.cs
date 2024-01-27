using System.Collections;
using System.Collections.Generic;


public class RecipeBuilder : DataBuilder<Recipe>
{
    protected override Recipe BuildData(List<string> s)
    {
        Recipe recipe = new Recipe();
        
        string keyA = null;
        string keyB = null;
        string keyC = null;
        
        SetValue(ref keyA, "SlotA");
        SetValue(ref keyB, "SlotB");
        SetValue(ref keyC, "SlotC");
        
        recipe.addInput(keyA);
        recipe.addInput(keyB);
        recipe.addInput(keyC);

        foreach (var emotionKey in DataManager.dictKeyToEmotion.Keys)
        {
            int amount = 0;
            SetValue(ref amount, emotionKey);
            recipe.addOutput(emotionKey, amount);
        }

        return recipe;
    }
}
