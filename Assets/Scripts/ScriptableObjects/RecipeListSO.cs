using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//菜单，菜单里面有多个菜谱
[CreateAssetMenu()]
public class RecipeListSO : ScriptableObject
{
    public List<RecipeSO> recipeSOList;
}
