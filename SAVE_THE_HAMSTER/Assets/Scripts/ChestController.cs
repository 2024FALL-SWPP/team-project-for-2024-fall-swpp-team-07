using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Animator animator;
    private bool isOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("ToggleChestState", 0.0f, 5.0f);
    }

    // Update is called once per frame
    void ToggleChestState()
    {
        isOpened = !isOpened;
        animator.SetBool("OpenChest", isOpened);
    }
}
