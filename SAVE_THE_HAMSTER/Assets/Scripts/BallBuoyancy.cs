using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBuoyancy : MonoBehaviour
{
    public float buoyancyForce = 25f;

    // 부력 최솟값, 최댓값 및 주기적인 변화 속도
    public float buoyancyMinForce = 25f;
    public float buoyancyMaxForce = 50f;
    public float buoyancyCycleSpeed = 2f;

    // 물에 들어갔을 때의 물 저항 및 각 저항 (너무 낮으면 수면 위로 떠오르지 않고 가라앉음)
    public float waterDrag = 5f;
    public float waterAngularDrag = 3f;

    private Rigidbody rb;
    private bool isInWater = false;
    private float waterSurfaceHeight;
    private Collider objectCollider;
    private Vector3 currentFlowDirection = Vector3.zero; // 물살 방향

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        objectCollider = GetComponent<Collider>();
    }

    void FixedUpdate()
    {
        if (isInWater)
        {
            UpdateBuoyancyForce();
            ApplyBuoyancy();
            ApplyWaterFlow();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            // 물 표면 높이를 저장하고 물에 들어갔다고 표시
            isInWater = true;
            waterSurfaceHeight = other.bounds.max.y;
            rb.drag = waterDrag;
            rb.angularDrag = waterAngularDrag;

            // 물살 정보를 가져와서 저장
            WaterFlow waterFlow = other.GetComponent<WaterFlow>();
            currentFlowDirection = waterFlow != null ? waterFlow.GetFlowForce() : Vector3.zero;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            // 물에서 나왔다고 표시 및 drag, angularDrag를 원래대로 복구
            isInWater = false;
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
            currentFlowDirection = Vector3.zero; // 물살 방향 초기화
        }
    }

    void UpdateBuoyancyForce()
    {
        // 부력을 주기적으로 변화시킴 (sin 함수를 이용하여 주기적인 변화)
        float sinValue = Mathf.Sin(Time.time * buoyancyCycleSpeed);
        buoyancyForce = Mathf.Lerp(buoyancyMinForce, buoyancyMaxForce, (sinValue + 1) / 2);
    }

    void ApplyBuoyancy()
    {
        float difference = transform.position.y - waterSurfaceHeight;

        if (difference < 0)
        {
            float displacement = Mathf.Clamp01(-difference / GetObjectHeight());
            rb.AddForce(Vector3.up * buoyancyForce * displacement, ForceMode.Acceleration);
        }
    }

    void ApplyWaterFlow()
    {
        if (currentFlowDirection != Vector3.zero)
        {
            rb.AddForce(currentFlowDirection * 10, ForceMode.Acceleration);
        }
    }

    float GetObjectHeight()
    {
        // 오브젝트의 형태에 따라 높이를 계산하는 방법을 다르게 처리
        if (objectCollider is SphereCollider sphereCollider)
        {
            return sphereCollider.radius * 2 * transform.localScale.y;
        }
        else if (objectCollider is BoxCollider boxCollider)
        {
            return boxCollider.size.y * transform.localScale.y;
        }
        else if (objectCollider is CapsuleCollider capsuleCollider)
        {
            return capsuleCollider.height * transform.localScale.y;
        }
        else if (objectCollider is MeshCollider meshCollider)
        {
            return meshCollider.bounds.size.y;
        }
        else
        {
            return objectCollider.bounds.size.y;
        }
    }
}
