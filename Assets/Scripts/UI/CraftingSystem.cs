using UnityEngine;
using System.Collections.Generic;
using CosmicYarnCat.Core;

namespace CosmicYarnCat.Systems
{
    [System.Serializable]
    public class CraftingRecipe
    {
        public string ItemName;
        public int ThreadCost;
        public int FragmentCost;
        public GameObject ResultPrefab;
    }

    public class CraftingSystem : MonoBehaviour
    {
        public List<CraftingRecipe> Recipes;

        public void Craft(string itemName)
        {
            var recipe = Recipes.Find(r => r.ItemName == itemName);
            if (recipe == null) return;

            if (ResourceManager.Instance.StellarThread >= recipe.ThreadCost &&
                ResourceManager.Instance.MoonFragments >= recipe.FragmentCost)
            {
                ResourceManager.Instance.SpendThread(recipe.ThreadCost);
                // Deduct fragments if they are consumable, or just check requirement
                
                Debug.Log($"Crafted {itemName}!");
                // Give item to player
            }
            else
            {
                Debug.Log("Not enough resources!");
            }
        }
    }
}
