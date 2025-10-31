using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;

    public GameObject RangeTrigger;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }
    }

    public void Attack()
    {
        RangeTrigger.SetActive(true);
    }

    public void EndAttack()
    {
        RangeTrigger.SetActive(false);
    }
}