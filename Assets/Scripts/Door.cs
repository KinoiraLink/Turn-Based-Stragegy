using System;
using System.Threading;
using UnityEngine;

namespace Assets
{
    public class Door : MonoBehaviour,IInteractable
    {
        [SerializeField] private bool isOpen;

        private const string IS_OPEN = "IsOpen";
        
        private GridPosition gridPosition;

        private Animator animator;

        private Action onInteractComplete;

        private float timer;

        private bool isActive;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition,this);

            if (isOpen)
            {
                OpenDoor();
            }
            else
            {
                CloseDoor();
            }
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }
            
            timer -= Time.deltaTime;

            if (timer < 0f)
            {
                isActive = false;
                onInteractComplete?.Invoke();
            }
        }

        public void Interact(Action onInteractionComplete)
        {
            this.onInteractComplete = onInteractionComplete;
            isActive = true;
            timer = 0.5f;
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }

        private void OpenDoor()
        {
            isOpen = true;
            animator.SetBool(IS_OPEN,true);
            Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition,true);
        }

        private void CloseDoor()
        {
            isOpen = false;
            animator.SetBool(IS_OPEN,false);
            Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition,false);
        }
    }
}