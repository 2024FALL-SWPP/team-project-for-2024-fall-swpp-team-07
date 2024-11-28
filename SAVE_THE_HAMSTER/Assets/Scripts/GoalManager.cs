using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public GameObject key;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // change on tag change
        {
            // Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            // if (otherRb.useGravity) //대포에 붙은 채로 닿으면 인식 안하도록
            // {
            key.SetActive(false);
            // }
            // Stage Clear. Trigger such info
        }
    }
}
