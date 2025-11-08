using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;

    public GameObject RangeTrigger;
    
    private bool _attacking = false;

    public Player3 player3;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !_attacking)
        {
            animator.SetTrigger("Attack");
            _attacking = true;
            player3.enabled = false;
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

    public void AttackAnimationFinished()
    {
        _attacking = false;
        player3.enabled = true;
    }
}