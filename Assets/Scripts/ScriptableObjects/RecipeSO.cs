using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//菜谱
[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    //KitchenObjectSOList
    public List<KitchenObjectSO> kitchenObjectSOList;
    public string recipeName;
}
