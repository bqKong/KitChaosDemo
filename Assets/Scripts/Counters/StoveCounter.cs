using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CuttingCounter;

public class StoveCounter : BaseCounter,IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private State state;
    private float fryingTimer;
    private FryingRecipeSO fryingRecipeSO;

    private float burningTimer;
    private BurningRecipeSO burningRecipeSO;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {

        if (HasKitchenObject())
        {
            switch (state)
            {
               
                case State.Idle:
                    break;

                case State.Frying:
                    fryingTimer += Time.deltaTime;

                    //OnprogressChanged这个事件会在ProgressUI中完成订阅
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                    if (fryingTimer > fryingRecipeSO.fryingTimerMax)
                    {
                        //Fried(煎好了)
                        //剪好了就销毁当前的生肉
                        GetKitchenObject().DestroySelf();

                        //生成煎好的肉(熟肉)，并设置其父对象
                        KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

                        //Debug.Log("Object fried!");

                        //切换状态，到-->BurnedMeat
                        state = State.Fried;

                        burningTimer = 0f;
                        burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state});

                    }
                    break;

                case State.Fried:
                    burningTimer += Time.deltaTime;

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
                    });

                    if (burningTimer > burningRecipeSO.burningTimerMax)
                    {
                        //Burned(焦了),销毁熟肉
                        GetKitchenObject().DestroySelf();
                        //生成烤焦的肉
                        KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);

                        //Debug.Log("Object Burned!");
                        state = State.Burned;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                        //完全煮好了，清零
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });

                    }
                    break;

                case State.Burned:

                    break;

            }
            //Debug.Log(state);
        }

    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //There is no kitchenObject here(StoveCounter柜台上没有东西)
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Player carrying something that can be Fried(角色拿着的物品可以被煎)
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());

                    //切换煎炸状态
                    state = State.Frying;
                    fryingTimer = 0f;

                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                        progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
                    });

                }

            }
            else
            {
                //Player not carrying anything(角色没有拿着物品)
            }

        }
        else
        {
            //There is a KitchenObject here(StoveCounter柜台上有东西)
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

                        //重置状态
                        state = State.Idle;

                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                        //玩家举起来了，进度清零
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });

                    }


                }

            }
            else
            {
                //Player is not carrying anything(Player手里没有物品)
                //那就可以让玩家拾取物品
                GetKitchenObject().SetKitchenObjectParent(player);

                state = State.Idle;

                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });

                //玩家举起来了，进度清零
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
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
        FryingRecipeSO fryRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        return fryRecipeSO != null;

    }

    /// <summary>
    /// 从输入菜谱获取对应的输出菜谱
    /// </summary>
    /// <param name="inputKitchenObjectSO"></param>
    /// <returns></returns>
    private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
    {
        FryingRecipeSO fryRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
        if (fryRecipeSO != null)
        {
            return fryRecipeSO.output;
        }
        else
        {
            return null;
        }

    }


    /// <summary>
    /// 获取煎炸配方
    /// </summary>
    /// <param name="inputKitchenObjectSO"></param>
    /// <returns></returns>
    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO fryRecipeSO in fryingRecipeSOArray)
        {
            if (fryRecipeSO.input == inputKitchenObjectSO)
            {
                return fryRecipeSO;
            }
        }

        return null;
    }


    /// <summary>
    /// 获取burnning配方
    /// </summary>
    /// <param name="inputKitchenObjectSO"></param>
    /// <returns></returns>
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
        {
            if (burningRecipeSO.input == inputKitchenObjectSO)
            {
                return burningRecipeSO;
            }
        }

        return null;
    }

    public bool IsFried()
    {
           return state == State.Fried; 
    }

}
