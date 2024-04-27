using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ActionBusyUI : MonoBehaviour
    {
        private void Start()
        {
            UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
            Hide();
        }

        private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
        {
            if(isBusy)
                Show();
            else 
                Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}