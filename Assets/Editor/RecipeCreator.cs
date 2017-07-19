using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditorInternal;

public class RecipeCreator : EditorWindow
{
    
    [MenuItem("Recipes/Recipe Creator")]

    static void Init()
    {
        /* Create a window and display it as a GUI. Checck for the needed folders
         * if they don't exist, create them.
         * 
         * */
        RecipeCreator recipeWindow = (RecipeCreator)EditorWindow.CreateInstance(typeof(RecipeCreator));
        recipeWindow.Show();
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Crafting/Recipes"))
        {
            Directory.CreateDirectory("Assets/Resources/Crafting/Recipes");
        }
        

    }
    
    private ReorderableList reorderableList;
    Recipe tempRecipe = null;
    RecipeManager recipeManager = null;
    IngredientManager ingredientManager = null;
    List<Ingredient> tempIngredients = new List<Ingredient>();

    void AddItemToList(object _ingredient)
    {
        tempIngredients.Add((Ingredient)_ingredient);
    }

    void OnEnable()
    {
        reorderableList = new ReorderableList(tempIngredients, typeof(Ingredient), true, true, true, true);


        reorderableList.onAddDropdownCallback = (Rect buttonRect, ReorderableList l) => {
            var menu = new GenericMenu();
            var guids = ingredientManager.ingredientList;
            foreach (var guid in guids)
            {
                menu.AddItem(new GUIContent("Ingredients/" + guid), false, AddItemToList, guid);
            }
            menu.ShowAsContext();
        };
    }
    
    void OnGUI()
    {
        if (recipeManager == null)
        {
            if (GameObject.FindObjectOfType<RecipeManager>() == null)
            {
                GameObject tempRecipeManger = new GameObject("RecipeManager");
                tempRecipeManger.AddComponent<RecipeManager>();
                recipeManager = tempRecipeManger.GetComponent<RecipeManager>();
            }
            else
                recipeManager = GameObject.FindObjectOfType<RecipeManager>().GetComponent<RecipeManager>();

            if (GameObject.FindObjectOfType<IngredientManager>() == null)
            {
                GameObject tempIngredientManger = new GameObject("IngredientManager");
                tempIngredientManger.AddComponent<IngredientManager>();
                ingredientManager = tempIngredientManger.GetComponent<IngredientManager>();
            }
            else
                ingredientManager = GameObject.FindObjectOfType<IngredientManager>().GetComponent<IngredientManager>();
        }
        
        
        if (tempRecipe)
        {
            tempRecipe.recipeName = EditorGUILayout.TextField("Recipe Name", tempRecipe.recipeName);
            tempRecipe.recipeID = EditorGUILayout.IntField("Recipe ID", tempRecipe.recipeID);
            tempRecipe.ingredientsRequired = tempIngredients;
            
            reorderableList.DoLayoutList();


        }

        if (tempRecipe == null)
        {

            if (GUILayout.Button("Create Recipe"))
            {
                tempRecipe = (Recipe)ScriptableObject.CreateInstance<Recipe>();
            }

        }
        else
        {

            if (GUILayout.Button("Create Recipe"))
            {
                foreach (Recipe _recipe in recipeManager.recipeList)
                {
                    if (_recipe.recipeID == tempRecipe.recipeID)
                    {
                        EditorUtility.DisplayDialog("Warning", "The recipe with such an ID already exists. Recipe name : " + _recipe.recipeName + " at position : " + recipeManager.recipeList.IndexOf(_recipe), "OK");
                        return;
                    }
                }
                AssetDatabase.CreateAsset(tempRecipe,
                   "Assets/Resources/Crafting/Recipes/" + tempRecipe.recipeName + ".asset");
                AssetDatabase.SaveAssets();
                recipeManager.recipeList.Add(tempRecipe);
                Selection.activeObject = tempRecipe;

                tempRecipe = null;

            }
        }
    }

    
}
