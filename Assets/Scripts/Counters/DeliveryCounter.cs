using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact(Player player)
    {
        //如果玩家手里的是碟子才销毁
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //Only accepts Plates()
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                //销毁递来的盘子
                 player.GetKitchenObject().DestroySelf();
            }
           
        }

    }


}
