﻿using System;
using UnityEngine;

namespace Assets
{
    public class InteractSphere : MonoBehaviour,IInteractable
    {
        [SerializeField] private Material greenMaterial;
        [SerializeField] private Material redMaterial;

        [SerializeField]private MeshRenderer meshRenderer;

        private bool isGreen;

        private GridPosition gridPosition;
        private Action onInteractComplete;
        private bool isActive;
        private float timer;

        private void Start()
        {

            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition,this);
            
            SetColorGreen();
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                isActive = false;
                onInteractComplete?.Invoke();
            }

        }

        private void SetColorGreen()
        {
            isGreen = true;
            meshRenderer.material = greenMaterial;
        }

        private void SetColorRed()
        {
            isGreen = false;
            meshRenderer.material = redMaterial;
        }

        public void Interact(Action onInteractionComplete)
        {
            this.onInteractComplete = onInteractionComplete;
            isActive = true;
            timer = 0.5f;
            if (isGreen)
            {
                SetColorRed();
            }
            else
            {
                SetColorGreen();
            }
        }
    }
}