using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    //C#的标准事件流
    public event EventHandler OnInteractAction;

    private PlayerInputAction playerInputAction;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();

        //输入系统的事件
        playerInputAction.Player.Interact.performed += Interact_performed;

    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
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
