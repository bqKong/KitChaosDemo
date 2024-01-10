using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{

    [SerializeField] private Transform container;

    [Tooltip("菜谱模版")]
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliverManager_OnRecipeSpawned;
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;

        UpdateViusal();
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, EventArgs e)
    {
        UpdateViusal();
    }

    private void DeliverManager_OnRecipeSpawned(object sender, EventArgs e)
    {
        UpdateViusal();
    }

    private void UpdateViusal()
    {
        //这个函数
        //这其实相当于销毁掉除recipeTemplate外的所有对象
        //再以recipeTemplate每次生成相应的clone体

        foreach (Transform child in container)
        {
            //Debug.LogWarning("child Name : " + child.name);
            if (child == recipeTemplate) continue;
            Destroy(child.gameObject);
        }


        //这个遍历
        //其实是生成一份份菜单的UI
        //菜单的UI里面所需的食材 --> 交给DeliveryManagerSingleUI的SsetRecipeSO()方法
        foreach (RecipeSO recipeSO in DeliveryManager.Instance.GetWaittingRecipeSOList())
        {
            Transform recipeTransform = Instantiate(recipeTemplate, container);
            recipeTransform.gameObject.SetActive(true);
          
            recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
        }

    }

}
