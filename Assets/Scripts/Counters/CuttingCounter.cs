using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter,IHasProgress
{
    //静态事件，防止每个挂载该脚本的物体都播放音频
    public static event EventHandler OnAnyCut;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler OnCut;

    [SerializeField] private CuttingRecipeSO[] cutKitchenObjectSOArray;

    [Tooltip("切割进度")]
    private int cuttingProgress;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no kitchenObject here
            //clearcounter柜台上没有东西
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Player carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    //确保每次放下食材都是切割次数都是0
                    cuttingProgress = 0;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput((GetKitchenObject().GetKitchenObjectSO()));

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    }) ;
                }

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
        //要加上检查当前柜台食材是否在菜谱里，不然会报nullreference
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            //There is  a KitchenObject here AND it can be cut
            cuttingProgress++;

            OnCut?.Invoke(this, EventArgs.Empty);
            //静态事件调用
            OnAnyCut?.Invoke(this, EventArgs.Empty);    

            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput((GetKitchenObject().GetKitchenObjectSO()));

            //触发事件
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });


            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());

                //自毁
                GetKitchenObject().DestroySelf();

                //Transform kitchenObjectTransform = Instantiate(cutKitchenObjectSO.prefab);
                //kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
            }


        }


    }


    /// <summary>
    /// 检查当前食材是否在菜谱里面
    /// </summary>
    /// <param name="inputKitchenObjectSO"></param>
    /// <returns></returns>
    private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        return cuttingRecipeSO != null;

    }

    /// <summary>
    /// 从输入菜谱获取对应的输出菜谱
    /// </summary>
    /// <param name="inputKitchenObjectSO"></param>
    /// <returns></returns>
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }
        else
        {
            return null;
        }

    }


    /// <summary>
    /// 获取切割配方
    /// </summary>
    /// <param name="inputKitchenObjectSO"></param>
    /// <returns></returns>
    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO cuttingRecipeSO in cutKitchenObjectSOArray)
        {
            if (cuttingRecipeSO.input == inputKitchenObjectSO)
            {
                return cuttingRecipeSO;
            }
        }

        return null;
    }



}
