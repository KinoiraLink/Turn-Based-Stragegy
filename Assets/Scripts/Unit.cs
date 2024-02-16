using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private Animator unitAnimator;

    private const string IS_WALKING = "IsWalking";

    private Vector3 targetPosition;//移动目标位置
    private float moveSpeed = 4f;
    private float stoppingDistance = 0.1f;
    private float rotationSpeed = 10f;

    private void Awake()
    {
        targetPosition = transform.position;
    }
    private void Update()
    {
        //防止移动后误差异常带来的困扰
        if (Vector3.Distance(transform.position,targetPosition)> stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            
            //旋转
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotationSpeed);

            unitAnimator.SetBool(IS_WALKING, true);
        }
        else 
        {
            unitAnimator.SetBool (IS_WALKING, false);
        }
        


    }

    /// <summary>
    /// 获取目标位置
    /// </summary>
    /// <param name="targetPositon"></param>
    public void Move(Vector3 targetPositon){
        this.targetPosition = targetPositon;
        
    }
}
