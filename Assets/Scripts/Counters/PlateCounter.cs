using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;

    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int platesSpawnedAmount;
    private int plateSpawnedAmountMax = 4;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        //新增：KitchenGameManager.Instance.IsGamePlaying()
        //保证在游戏playing的时候才生成盘子
        if (KitchenGameManager.Instance.IsGamePlaying() &&(spawnPlateTimer > spawnPlateTimerMax))
        {
            spawnPlateTimer = 0f;

            if (platesSpawnedAmount < plateSpawnedAmountMax)
            {
                platesSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);

            }

        }

    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is empty handed(Player手上没有东西)
            if (platesSpawnedAmount > 0)
            {
                //There's at lease one plate here(PlateCounter上有盘子)
                platesSpawnedAmount--;

                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }

    }

}
