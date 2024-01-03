using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    //公共静态事件，我们只想让每一个计数器都监听一个事件,而不希望监听每一个计数器
    public static event EventHandler OnAnyObjectPlaceHere;

    /// <summary>
    /// 释放静态事件
    /// </summary>
    public static void ResetStaticData()
    {
        OnAnyObjectPlaceHere = null;
    }

    [SerializeField] private Transform counterTopPoint;
    private KitchenObject kitchenObject;

    //PS:也可以将方法声明为抽象方法
    public virtual void Interact(Player player) 
    {
        Debug.LogError("BaseCounter.Interact()!");
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("BaseCounter.InteractAlternate()!");
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    //调用是在KitchenObject中的
    /// <summary>
    /// 给父物体 设置新的kitchenObject
    /// </summary>
    /// <param name="kitchenObject"></param>
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnAnyObjectPlaceHere?.Invoke(this, EventArgs.Empty);
        }

    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }

}
