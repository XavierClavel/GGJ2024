using System;
using System.Collections;
using System.Collections.Generic;

public static class RecipeManager
{
   private static List<Recipe> recipes;

   public static Recipe getRecipe(List<string> cardKeys)
   {
      foreach (var recipe in recipes)
      {
         if (recipe.matchesInput(cardKeys)) return recipe;
      }

      throw new ArgumentOutOfRangeException("Recipe not found");
   }

   public static Dictionary<string, int> getRecipeEmotions(List<string> cardKeys)
   {
      return getRecipe(cardKeys).getOutput();
   }
}
