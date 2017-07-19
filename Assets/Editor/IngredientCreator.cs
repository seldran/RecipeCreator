using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class IngredientCreator : EditorWindow
{
    [MenuItem("Recipes/Ingredient Creator")]
    static void Init()
    {
        IngredientCreator ingredientWindow = (IngredientCreator)EditorWindow.CreateInstance(typeof(IngredientCreator));
        ingredientWindow.Show();
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Crafting/Ingredients"))
        {
            Directory.CreateDirectory("Assets/Resources/Crafting/Ingredients");
        }
    }

    Ingredient tempIngredient = null;
    IngredientManager ingredientManager = null;
    int availableID = 0;

    void OnGUI()
    {
        if(ingredientManager == null)
        {
            if (GameObject.FindObjectOfType<IngredientManager>() == null)
            {
                GameObject tempIngredientManger = new GameObject("IngredientManager");
                tempIngredientManger.AddComponent<IngredientManager>();
                ingredientManager = tempIngredientManger.GetComponent<IngredientManager>();
            }
            else
                ingredientManager = GameObject.FindObjectOfType<IngredientManager>().GetComponent<IngredientManager>();
        }

        if(tempIngredient)
        {
            tempIngredient.ingredientName = EditorGUILayout.TextField("Ingredient Name", tempIngredient.ingredientName);
            tempIngredient.ingredientID = EditorGUILayout.IntField("Ingredient ID", availableID);
        }

        EditorGUILayout.Space();

        if (tempIngredient == null)
        {

            if (GUILayout.Button("Create Ingredient"))
            {
                tempIngredient = (Ingredient)ScriptableObject.CreateInstance<Ingredient>();
                int tempID = 0;
                for(int x = 0; x < ingredientManager.ingredientList.Count; x++)
                {
                    if(ingredientManager.ingredientList[x].ingredientID == tempID)
                    {
                        tempID++;
                        x = 0;
                    }
                }
                availableID = tempID;
                tempID = 0;
            }

        }
        else
        {

            if (GUILayout.Button("Create Ingredient"))
            {
                foreach(Ingredient _ingredient in ingredientManager.ingredientList)
                {
                    if(_ingredient.ingredientID == tempIngredient.ingredientID)
                    {
                        EditorUtility.DisplayDialog("Warning", "The ingredient with such an ID already exists. Ingredient name : " + _ingredient.ingredientName + " at position : " + ingredientManager.ingredientList.IndexOf(_ingredient), "OK");
                        return;
                    }
                }
                AssetDatabase.CreateAsset(tempIngredient,
                   "Assets/Resources/Crafting/Ingredients/" + tempIngredient.ingredientName + ".asset");
                AssetDatabase.SaveAssets();
                ingredientManager.ingredientList.Add(tempIngredient);
                Selection.activeObject = tempIngredient;

                tempIngredient = null;

            }
        }
    }
}
