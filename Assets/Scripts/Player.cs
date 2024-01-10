using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    //单例模式
    public static Player Instance { get; private set; }

    //C#标准事件处理EventHandler
    //专门处理selectedcounter的视觉变化
    public event EventHandler<OnSelectedCounterChangedArgs> OnSelectedCounterChanged;

    //自定义事件的参数OnSelectedCounterChangedArgs，必须继承EventArgs
    public class OnSelectedCounterChangedArgs : EventArgs
    {
        public BaseCounter selectedCounter;
    }

    public event EventHandler OnPickSomething;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;

    [Tooltip("厨房物体")]
    private KitchenObject kitchenObject;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("不止一个玩家！");
        }

        Instance = this;
    }

    private void Start()
    {
        //订阅事件
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        //不是在游戏进行期间无法互动
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }

    }

    /// <summary>
    /// 输入事件处理函数
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        //不是在游戏进行期间无法互动
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null)
        {
            //Interact这个函数在ClearCounter里面
            selectedCounter.Interact(this);
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleInteractions();

    }

    public bool IsWalking()
    {
        return isWalking;
    }

    /// <summary>
    /// 互动处理
    /// </summary>
    private void HandleInteractions()
    {
        //向量归一化处理
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        //记录最后一次moveDir
        //防止停止的时候moveDir为0，无法进行Raycast
        if (moveDir != Vector3.zero)
        { 
            lastInteractDir = moveDir;
        }

        //互动的距离
        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            //用这种方法，而不用标签去获取
            //获取射线碰撞到的物体中的BaseCounter组件
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //有计数器
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }

        }
        else
        {
            SetSelectedCounter(null);
        }

        //Debug.Log(selectedCounter);
    }


    /// <summary>
    /// 移动处理
    /// </summary>
    private void HandleMovement()
    {
        //向量归一化处理
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        //移动的距离
        float moveDistance = moveSpeed * Time.deltaTime;

        float PlayerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, PlayerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //当不能向moveDir方向移动时
            //Attempt only X movement(尝试往X轴移动)
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            //canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, PlayerRadius, moveDirX, moveDistance);
            canMove = (moveDir.x < -.5f || moveDir.x > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, PlayerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                //Can move only on the X
                moveDir = moveDirX;
            }
            else
            {
                //Cannot move only on the X(不能往X轴方向移动，尝试向Z轴方向移动)

                //Attempt only Z movement(尝试往Z轴移动)
                Vector3 moveDirZ = new Vector3(moveDir.z, 0, 0).normalized;
                //canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, PlayerRadius, moveDirZ, moveDistance);
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, PlayerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //Can move only on the Z(可以向Z轴方向移动)
                    moveDir = moveDirZ;
                }
                else
                {
                    //Can move only on any direction(不能朝任何方向移动)
                }

            }

        }

        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        //moveDir 不能为 0(即方向键有输入)
        isWalking = moveDir != Vector3.zero;

        //修改前向旋转
        //transform.forward = moveDir;

        //旋转
        float rotateSpeed = 10f;
        //插值函数
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);

    }

    /// <summary>
    /// 设置当前选中的Counter(柜台)
    /// </summary>
    /// <param name="selectedCounter"></param>
    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        //玩家拿起物体 有音效！
        if (kitchenObject != null)
        {
            OnPickSomething?.Invoke(this, EventArgs.Empty);
        }

    }

    public KitchenObject GetKitchenObject()
    {
        return this.kitchenObject;
    }

    public void ClearKitchenObject()
    {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
