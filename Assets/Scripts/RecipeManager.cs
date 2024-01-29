using System;
using System.Collections;
using System.Collections.Generic;

public static class RecipeManager
{
   private static List<int> uncoveredRecipes = new List<int>();

   public static void uncoverRecipe(int index) => uncoveredRecipes.TryAdd(index);

   public static bool isRecipeUncovered(int index) => uncoveredRecipes.Contains(index);

   public static Recipe findRecipe(string key)
   {
      foreach (var recipe in DataManager.recipes)
      {
         if (recipe.matchesInput(key)) return recipe;
      }

      return null;
   }

   private static void UncoverRecipe(Recipe recipe, int recipeIndex)
   {
      uncoveredRecipes.Add(recipeIndex);
      AudioManager.PlaySfx("New");
      Notebook.instance.dictIndexToRecipeDisplay[recipeIndex].DisplayRecipe(recipe, recipeIndex);
      Player.ShowRecipePanel();
      Player.instance.recipeDisplay.DisplayRecipe(recipe, recipeIndex);
      SaveManager.Save();
   }

   public static Recipe getRecipe(List<string> cardKeys)
   {
      //Look for exact recipe
      foreach (var recipe in DataManager.recipes)
      {
         if (!recipe.matchesInput(cardKeys)) continue;
         int recipeIndex = DataManager.recipes.IndexOf(recipe);
         if (!uncoveredRecipes.Contains(recipeIndex)) UncoverRecipe(recipe, recipeIndex);
         return recipe;
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
