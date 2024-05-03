
using System;
using Actions;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;
    
    private const string IS_WALKING = "IsWalking";
    private const string SHOOT = "Shoot";


    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += MoveAction_OnShoot;
           
        }
    }



    private void Start()
    {
        animator.SetBool(IS_WALKING,false);
    }
    
    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool(IS_WALKING,true);
    }
    
    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool(IS_WALKING,false);
    }
    private void MoveAction_OnShoot(object sender, OnShootEventArgs e)
    {
        animator.SetTrigger(SHOOT);

        Transform projectileTransform = 
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = projectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;
        
        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

}
