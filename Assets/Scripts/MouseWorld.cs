using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
   
    [SerializeField]
    private LayerMask mousePlaneLayerMask;//MousePlane 

    public static MouseWorld instance { get; private set; }

    public void Awake()
    {
        instance = this;
    }

    private void Update()
    {
      
    }

    public static Vector3 GetPostion() 
    {
        //Input.mousePosition 获取鼠标在屏幕（摄像机镜头）的位置
        //ScreenPointToRay() 从屏幕位置反射一条射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance.mousePlaneLayerMask);
        return raycastHit.point;
    }
}
