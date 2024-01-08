using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualGameObjectArray;

    private void Start()
    {
        //监听事件
        Player.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChange;
    }

    /// <summary>
    /// 选中的柜台变更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Instance_OnSelectedCounterChange(object sender, Player.OnSelectedCounterChangedArgs e)
    {
        //判断Player中检测到的Counter与视觉中的Counter是否一致
        if (e.selectedCounter == baseCounter)
        {
            Show();
        }
        else
        { 
            Hide();
        }

    }


    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray) 
        {
            visualGameObject.SetActive(true);
        }
        
    }

    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }

}
