using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    //C#的标准事件流
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;

    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        Instance = this;

        playerInputAction = new PlayerInputAction();
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
        OnInteractAlternateAction?.Invoke(this,EventArgs.Empty);
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        //调用互动的事件
        OnInteractAction?.Invoke(this,EventArgs.Empty);
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
}
