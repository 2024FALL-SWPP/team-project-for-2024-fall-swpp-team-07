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
            // Stage Clear. Trigger such info
            key.SetActive(false);
        }
    }
}
