using System;
using UnityEngine;

namespace UI
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private bool invert;
        
        private Transform cameraTransform;
        private Vector3 dirToCamera;
        private void Awake()
        {
            cameraTransform = Camera.main.transform;
            dirToCamera = (cameraTransform.position - transform.position).normalized;

        }

        private void LateUpdate()
        {
            if (invert)
            {
                transform.LookAt(transform.position + dirToCamera * -1);
            }
            else
            {
                transform.LookAt(cameraTransform);
            }
        }
    }
}