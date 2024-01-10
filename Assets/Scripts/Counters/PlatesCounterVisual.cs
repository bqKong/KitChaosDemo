using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlateCounter plateCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;

    private List<GameObject> plateVisualGameObjectList;


    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        plateCounter.OnPlateSpawned += PlateCounter_OnPlateSpawned;
        plateCounter.OnPlateRemoved += PlateCounter_OnPlateRemoved;
    }

    /// <summary>
    /// 销毁盘子
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PlateCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    /// <summary>
    /// 生成盘子
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PlateCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab,counterTopPoint);

        float plateOffsetY = .1f;
        //生成多个的时候，视觉看起来像叠在一起
        plateVisualTransform.localPosition = new Vector3(0,plateOffsetY * plateVisualGameObjectList.Count,0);

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);

    }
}
