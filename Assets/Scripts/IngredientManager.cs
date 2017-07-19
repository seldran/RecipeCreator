using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//[InitializeOnLoad]
public class IngredientManager : MonoBehaviour
{
    public List<Ingredient> ingredientList = new List<Ingredient>();


    void AddIngredientsToList()
    {
        
        DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Crafting/Ingredients/");
        FileInfo[] info = dir.GetFiles("*.asset");
        foreach(FileInfo f in info)
        {
            Ingredient t = (Ingredient)AssetDatabase.LoadAssetAtPath(f.ToString(), typeof(Ingredient));
            ingredientList.Add(t);
        }
    }
}

