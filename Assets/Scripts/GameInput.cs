using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private readonly string PLAYER_PREFS_BINDINGS = "IOnputBindings";

    //C#的标准事件流
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,

        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause,

    }


    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInputAction();

        //加载修改后的键位
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputAction.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        //开启输入系统
        playerInputAction.Player.Enable();

        //输入系统的互动事件
        playerInputAction.Player.Interact.performed += Interact_performed;

        //切菜事件
        playerInputAction.Player.InteractAlternate.performed += InteractAlternate_performed;

        //暂停事件
        playerInputAction.Player.Pause.performed += Pause_performed;

    }

    private void OnDestroy()
    {
        playerInputAction.Player.Interact.performed -= Interact_performed;
        playerInputAction.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputAction.Player.Pause.performed -= Pause_performed;

        playerInputAction.Dispose();
    }


    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(InputAction.CallbackContext context)
    {
        //调用切菜互动事件
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        //调用互动的事件
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        #region 新版操作
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();
        #endregion

        #region 旧版操作
        //Vector2 inputVector = new Vector2(0, 0);

        //if (Input.GetKey(KeyCode.W))
        //{
        //    inputVector.y = 1f;
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    inputVector.y = -1f;
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    inputVector.x = -1f;
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    inputVector.x = 1f;
        //}

        #endregion

        //向量归一化处理
        inputVector = inputVector.normalized;

        return inputVector;
    }


    public string GetBingText(Binding binding)
    {
        switch (binding)
        {
            default:
                return null;
            case Binding.Move_Up:
                return playerInputAction.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputAction.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputAction.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputAction.Player.Move.bindings[4].ToDisplayString();

            case Binding.Interact:
                return playerInputAction.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputAction.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputAction.Player.Pause.bindings[0].ToDisplayString();


            case Binding.Gamepad_Interact:
                return playerInputAction.Player.Interact.bindings[1].ToDisplayString();
            case Binding.Gamepad_InteractAlternate:
                return playerInputAction.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.Gamepad_Pause:
                return playerInputAction.Player.Pause.bindings[1].ToDisplayString();

        }


    }


    /// <summary>
    /// 重新绑定按键
    /// </summary>
    /// <param name="binding"></param>
    /// <param name="onActionRebound"></param>
    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputAction.Player.Disable();

        //减少重复代码的办法
        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 1;
                break;

            case Binding.Move_Down:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 2;
                break;

            case Binding.Move_Left:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 3;
                break;

            case Binding.Move_Right:
                inputAction = playerInputAction.Player.Move;
                bindingIndex = 4;
                break;

            case Binding.Interact:
                inputAction = playerInputAction.Player.Interact;
                bindingIndex = 0;
                break;

            case Binding.InteractAlternate:
                inputAction = playerInputAction.Player.InteractAlternate;
                bindingIndex = 0;
                break;

            case Binding.Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 0;
                break;


            case Binding.Gamepad_Interact:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 1;
                break;

            case Binding.Gamepad_InteractAlternate:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 1;
                break;

            case Binding.Gamepad_Pause:
                inputAction = playerInputAction.Player.Pause;
                bindingIndex = 1;
                break;

        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
                    {
                        //Debug.Log(callback.action.bindings[1].path);
                        //Debug.Log(callback.action.bindings[1].overridePath);
                        callback.Dispose();
                        playerInputAction.Player.Enable();

                        //完成重新绑定之后，调用回调，将PressToRebind界面重新隐藏
                        onActionRebound?.Invoke();

                        //记录重新绑定的按键

                        PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputAction.SaveBindingOverridesAsJson());
                        PlayerPrefs.Save();

                        //重新绑定 --> TutorialUI
                        OnBindingRebind?.Invoke(this,EventArgs.Empty);
                    })
            .Start();

    }


}
