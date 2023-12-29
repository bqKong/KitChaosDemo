using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        //解决无限生成预制体的问题
        //思路：检查player手上是否有物体
        if (!player.HasKitchenObject())
        {
            //Player is not carrying anything
            //Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
            //kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            //事件拥有者启动事件
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }

        else
        {

        }



    }

}
