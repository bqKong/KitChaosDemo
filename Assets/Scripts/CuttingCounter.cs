using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no kitchenObject here
            //clearcounter柜台上没有东西
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);

            }
            else
            {
                //Player not carrying anything
            }

        }
        else
        {
            //There is a KitchenObject here
            //clearCounter柜台上有东西
            if (player.HasKitchenObject())
            {
                //player is carrying something
                //player提着东西
            }
            else
            {
                //Player is not carrying anything
                //Player没有提着东西

                //那就可以让玩家举着
                GetKitchenObject().SetKitchenObjectParent(player);
            }

        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        { 
            //There is  a KitchenObject here
            //自毁
            GetKitchenObject().DestroySelf();

            //Transform kitchenObjectTransform = Instantiate(cutKitchenObjectSO.prefab);
            //kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
            KitchenObject.SpawnKitchenObject(cutKitchenObjectSO,this);
        }


    }


}
