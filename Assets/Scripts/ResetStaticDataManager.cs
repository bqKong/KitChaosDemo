using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{

    private void Awake()
    {
        //静态事件在切换场景中是不会被销毁的
        //这里为了防止切换为GameScene场景出现问题
        //手动将这些静态事件置为null
        CuttingCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
    }

}
