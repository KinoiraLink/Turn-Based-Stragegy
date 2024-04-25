using System;
using UnityEngine;



public class SpinAction : BaseAction
{
    
    private float totalSpinAmount;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        
        float spinAddAmount = 360 * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            isActive = false;
            OnActionComplete?.Invoke();
        }
    }

    public void Spin(Action onSpinComplete)
    {
        this.OnActionComplete = onSpinComplete;
        isActive = true;
        totalSpinAmount = 0f;
    }
}
