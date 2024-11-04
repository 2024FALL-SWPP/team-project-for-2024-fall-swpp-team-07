using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlow : MonoBehaviour
{
    public Vector3 localFlowDirection = Vector3.forward;
    public float flowSpeed = 50f;

    public Vector3 GetFlowForce()
    {
        // 월드 좌표계에서의 물살 힘을 반환
        return transform.TransformDirection(localFlowDirection.normalized) * flowSpeed;
    }
}
