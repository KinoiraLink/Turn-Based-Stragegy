using System;
using Cinemachine;
using UnityEngine;

namespace Assets
{
    public class ScreenShake : MonoBehaviour
    {
        public static ScreenShake Instance { get; private set; }
        private CinemachineImpulseSource cinemachineImpulseSource;

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.Log("There's more than one ScreenShake!" + transform + "-" + Instance);
                Destroy(this.gameObject);
                return;
            }
            Instance = this;
            
            cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }
        

        public void Shake(float intensity = 1f)
        {
            cinemachineImpulseSource.GenerateImpulse(intensity);
        }
    }
}