using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{

    //静态事件
    public static event EventHandler OnAnyObjectTrash;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        { 
            player.GetKitchenObject().DestroySelf();
            OnAnyObjectTrash?.Invoke(this, EventArgs.Empty);    
        }
    }

}
