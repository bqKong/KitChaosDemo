using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    //静态事件
    public static event EventHandler OnAnyObjectTrash;

    /// <summary>
    /// 释放静态事件
    /// </summary>
    new public static void ResetStaticData()
    {
        OnAnyObjectTrash = null;
    }

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        { 
            player.GetKitchenObject().DestroySelf();
            OnAnyObjectTrash?.Invoke(this, EventArgs.Empty);    
        }
    }

}
