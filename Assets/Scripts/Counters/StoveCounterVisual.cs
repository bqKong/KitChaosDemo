using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter ;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    /// <summary>
    /// StoverCounter的特效显示
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showViusal = (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried);
        stoveOnGameObject.gameObject.SetActive(showViusal);
        particlesGameObject.gameObject.SetActive(showViusal);

    }
}
