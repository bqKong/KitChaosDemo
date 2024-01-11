using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);

        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        //取消游戏暂停，防止切换场景后场景一直处于暂停状态
        Time.timeScale = 1f;

    }




}
