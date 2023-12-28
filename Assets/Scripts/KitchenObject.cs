using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private ClearCounter clearCounter;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }

    public void SetClearCounter(ClearCounter clearCounter)
    {
        //清除前一个kitchenObject
        if (this.clearCounter != null)
        {
            this.clearCounter.ClearKitchenObject();
        }

        this.clearCounter = clearCounter;

        //检查现在的kitchenObject是否为空
        if (clearCounter.HasKitchenObject())
        {
            Debug.LogError("Counter already has a KitchenObject!");

        }
        //设置新的kitchenObject
        clearCounter.SetKitchenObject(this);

        //完成父物体的变换
        transform.parent = clearCounter.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;

    }

    public ClearCounter GetClearCounter()
    {
        return clearCounter;
    }

}
