﻿using System;
using Actions;
using UnityEngine;

namespace Assets
{
    public class ScreenShakeActions : MonoBehaviour
    {
        private void Start()
        {
            ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
            GrenadeProjectile.OnAnyGrenadeExploded += GrenadeProjectile_OnAnyGrenadeExploded;
            SwordAction.OnAnySwordHit += SwordAction_OnAnySwordHit;
        }

        private void OnDestroy()
        {
            ShootAction.OnAnyShoot -= ShootAction_OnAnyShoot;
            GrenadeProjectile.OnAnyGrenadeExploded -= GrenadeProjectile_OnAnyGrenadeExploded;
            SwordAction.OnAnySwordHit -= SwordAction_OnAnySwordHit;

        }


        private void ShootAction_OnAnyShoot(object sender, OnShootEventArgs e)
        {
            ScreenShake.Instance.Shake();
        }
        
        private void GrenadeProjectile_OnAnyGrenadeExploded(object sender, EventArgs e)
        {
            ScreenShake.Instance.Shake(5f);
        }
        
        
        private void SwordAction_OnAnySwordHit(object sender, EventArgs e)
        {
            ScreenShake.Instance.Shake(2f);

        }


    }
}