using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        //清除kitchenobje绑定的
        //清除当前绑定的柜台前一个kitchenObject
        if (this.kitchenObjectParent != null)
        {
            this.kitchenObjectParent.ClearKitchenObject();
        }

        //1.重新绑定柜台
        //2.初始化绑定柜台
        this.kitchenObjectParent = kitchenObjectParent;

        //检查现在的kitchenObject是否为空
        if (kitchenObjectParent.HasKitchenObject())
        {
            Debug.LogError("IKitchenObjectParent already has a KitchenObject!");

        }
        //设置新的kitchenObject
        kitchenObjectParent.SetKitchenObject(this);

        //完成父物体的变换
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;

    }

    public IKitchenObjectParent GetKitchenObjectParent()
    {
        return kitchenObjectParent;
    }

    /// <summary>
    /// 销毁函数(写在kitchenobject自身里面)
    /// </summary>
    public void DestroySelf()
    {
        //销毁前清除父物体的kitchenobject，不然会nullreference
        kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    //KitchenObject SpawnKitchenObject()

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject  kitchenObject= kitchenObjectTransform.GetComponent<KitchenObject>();
        
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }

}
