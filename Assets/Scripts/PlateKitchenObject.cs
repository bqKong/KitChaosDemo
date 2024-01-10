using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//扩展KitchenObject
public class PlateKitchenObject : KitchenObject
{

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [Tooltip("能放入盘子的食材列表")]
    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

    [Tooltip("当前盘子里的食材列表")]
    private List<KitchenObjectSO> kitchenObjectSOList;

    private void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        //如果不是食谱里面的菜单
        if (!validKitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        if (kitchenObjectSOList.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            kitchenObjectSOList.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });

            return true;
        }

    }

    /// <summary>
    /// 返回当前盘子里的食材列表(当前盘子有什么食材)
    /// </summary>
    /// <returns></returns>
    public List<KitchenObjectSO> GetKitchenObjectSOList()
    { 
        return kitchenObjectSOList;
    }

}
