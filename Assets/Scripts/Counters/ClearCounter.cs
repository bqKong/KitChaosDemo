using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    //[SerializeField,Tooltip("要转移的柜台")] private ClearCounter secondClearCounter;
    //[SerializeField] private bool testing = true;
    //private void Update()
    //{
    //    if (testing && Input.GetKeyDown(KeyCode.T))
    //    {
    //        if (kitchenObject != null)
    //        { 
    //            kitchenObject.SetKitchenObjectParent(secondClearCounter);
    //        }
    //    }
    //}

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

                //玩家是否拿着盘子
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        //销毁自身
                        GetKitchenObject().DestroySelf();
                    }
                }
                //玩家没拿着盘子，拿的是其他
                else
                {
                    //Player is  not carrying Plate but somthing else
                    if (GetKitchenObject().TryGetPlate(out  plateKitchenObject))
                    {
                        //Counter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }

                    }
                    
                }


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

}
