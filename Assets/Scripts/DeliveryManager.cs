using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;

    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSO recipeListSO;

    //菜单，里面包含多个菜谱
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    
    //最大菜单数
    private int waittingRecipeMax = 4;
    private int successfulRecipesAmount;


    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            //新增：KitchenGameManager.Instance.IsGamePlaying()
            //保证在游戏playing的时候才生成菜谱
            if (KitchenGameManager.Instance.IsGamePlaying() &&(waitingRecipeSOList.Count < waittingRecipeMax))
            {
                //随机将菜单里的菜谱，赋值给waitingRecipeSO
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeSOList.Add(waitingRecipeSO);
                //Debug.Log(waitingRecipeSO.name);

                //调用事件，只有在赋值给菜谱的时候才调用
                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }

        }

    }

    //函数在跟DeliveryCounter交互的时候才会调用
    /// <summary>
    /// 传递菜谱
    /// </summary>
    /// <param name="plateKitchenObject"></param>
    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            //
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            //如果菜谱里面的厨房物体(食材)数量等于盘子里的厨房物体(食材)数量
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                //Has the same number of ingredients


                //盘子里的菜是否匹配菜谱
                bool plateContentMatchesRecipe = true;

                //遍历 比对 菜谱里面的每一种食材
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    //Cycling through all ingredients in the recipe

                    //是否找到对应的食材
                    bool ingredientFound = false;

                    //遍历 比对 盘子里的每一种食材
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //Cycling through all ingredients in the plate

                        //如果盘子里的食材 匹配 菜谱的菜
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            //Ingredient matches!

                            //匹配状态置为true 退出这层循环
                            ingredientFound = true;
                            break;

                        }
                    }

                    //如果盘子里的食材 和 菜谱的食材不配
                    if (!ingredientFound)
                    {
                        //This Recipe ingredient was not found the Plate

                        //配方不匹配
                        plateContentMatchesRecipe = false;
                    }

                }

                //如果配方完全匹配
                if (plateContentMatchesRecipe)
                {
                    //Player delivered the correct recipe!
                    Debug.Log("Player delivered the correct recipe!");

                    successfulRecipesAmount++;

                    //移除当前的菜谱，完成状态
                    waitingRecipeSOList.RemoveAt(i);

                    //调用事件，完全匹配，交付菜
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);

                    //调用事件，播放成功的音效
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);

                    return;
                }

            }

        }
        //No matches found!
        //Player did not deliver a correct recipe

        //完全不对
        //Debug.Log("Player did not deliver a correct recipe!");

        //调用事件，播放失败音效
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }


    /// <summary>
    /// 获取菜谱，用于UI
    /// </summary>
    /// <returns></returns>
    public List<RecipeSO> GetWaittingRecipeSOList()
    { 
        return waitingRecipeSOList;
    }

    /// <summary>
    /// 返回成功做好菜的数量
    /// </summary>
    /// <returns></returns>
    public int GetSuccessfulRecipesAmount()
    { 
        return successfulRecipesAmount;
    }


}
