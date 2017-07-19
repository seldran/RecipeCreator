using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recipe : ScriptableObject
{
    public string recipeName = "";
    public List<Ingredient> ingredientsRequired = new List<Ingredient>();
    public int recipeID = 0;
}
