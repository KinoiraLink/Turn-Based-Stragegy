using System;

namespace Assets
{
    public interface IInteractable
    {
        void Interact(Action onInteractionComplete);
    }
}