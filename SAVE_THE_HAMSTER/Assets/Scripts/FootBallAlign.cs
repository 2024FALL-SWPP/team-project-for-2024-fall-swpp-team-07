using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootBallAlign : MonoBehaviour
{
    public Vector3 targetDirection = Vector3.up; // 정렬할 대상 벡터
    // Start is called before the first frame update
    void Start()
    {
        AlignLongAxisToTarget(targetDirection);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AlignLongAxisToTarget(Vector3 targetDirection)
    {
        Vector3 longAxis = GetLongAxis(); // 로컬 긴축 방향
        Vector3 longAxisWorld = transform.TransformDirection(longAxis); // 월드 방향으로 변환

        // 긴축을 대상 벡터와 정렬
        Quaternion rotation = Quaternion.FromToRotation(longAxisWorld, targetDirection);
        transform.rotation = rotation * transform.rotation;
    }

    Vector3 GetLongAxis()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            Debug.LogError("MeshFilter가 없습니다.");
            return Vector3.zero;
        }

        // 메시의 경계 상자
        Bounds bounds = meshFilter.mesh.bounds;

        // 경계 상자의 크기
        Vector3 size = bounds.size;

        // 가장 긴 축 판별
        if (size.x > size.y && size.x > size.z)
            return Vector3.right; // 로컬 X축
        else if (size.y > size.x && size.y > size.z)
            return Vector3.up; // 로컬 Y축
        else
            return Vector3.forward; // 로컬 Z축
    }
}
