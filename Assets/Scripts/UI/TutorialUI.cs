using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyMoveUpText;
    [SerializeField] private TextMeshProUGUI keyMoveDownText;
    [SerializeField] private TextMeshProUGUI keyMoveLeftText;
    [SerializeField] private TextMeshProUGUI keyMoveRightText;
    [SerializeField] private TextMeshProUGUI keyInteractText;
    [SerializeField] private TextMeshProUGUI keyInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyPauseText;
    [SerializeField] private TextMeshProUGUI keyGamepadMoveText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractText;
    [SerializeField] private TextMeshProUGUI keyGamepadInteractAlternateText;
    [SerializeField] private TextMeshProUGUI keyGamepadPauseText;


    private void Start()
    {
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged;

        UpdateVisual();
        Show();
    }

    /// <summary>
    /// 进入倒计时UI时，隐藏TutorialUI
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KitchenGameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (KitchenGameManager.Instance.IsCountdownToStartActive())
        { 
            Hide();
        }
        
    }

    private void GameInput_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyMoveUpText.text = GameInput.Instance.GetBingText(GameInput.Binding.Move_Up);
        keyMoveDownText.text = GameInput.Instance.GetBingText(GameInput.Binding.Move_Down);
        keyMoveLeftText.text = GameInput.Instance.GetBingText(GameInput.Binding.Move_Left);
        keyMoveRightText.text = GameInput.Instance.GetBingText(GameInput.Binding.Move_Right);
        keyInteractText.text = GameInput.Instance.GetBingText(GameInput.Binding.Interact);
        keyInteractAlternateText.text = GameInput.Instance.GetBingText(GameInput.Binding.InteractAlternate);
        keyPauseText.text = GameInput.Instance.GetBingText(GameInput.Binding.Pause);
        keyGamepadInteractText.text = GameInput.Instance.GetBingText(GameInput.Binding.Interact);
        keyGamepadInteractAlternateText.text = GameInput.Instance.GetBingText(GameInput.Binding.InteractAlternate);
        keyGamepadPauseText.text = GameInput.Instance.GetBingText(GameInput.Binding.Pause);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }


}
