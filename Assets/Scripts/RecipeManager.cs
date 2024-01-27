using System;
using System.Collections;
using System.Collections.Generic;

public static class RecipeManager
{
   private static List<Recipe> recipes;

   public static Recipe findRecipe(string key)
   {
      foreach (var recipe in recipes)
      {
         if (recipe.matchesInput(key)) return recipe;
      }

      return null;
   }

   public static Recipe getRecipe(List<string> cardKeys)
   {
      //Look for exact recipe
      foreach (var recipe in recipes)
      {
         if (recipe.matchesInput(cardKeys)) return recipe;
      }
      
      //If one card is intonation => fail
      foreach (var cardKey in cardKeys)
      {
         if (DataManager.dictKeyToCard[cardKey].isIntonation()) return null;
      }
      
      //Build output based on cards
      Recipe newRecipe = new Recipe();
      foreach (var cardKey in cardKeys)
      {
         newRecipe.addOutput(findRecipe(cardKey));
      }

      return newRecipe;

   }

   public static Dictionary<string, int> getRecipeEmotions(List<string> cardKeys)
   {
      return getRecipe(cardKeys).getOutput();
   }
}
