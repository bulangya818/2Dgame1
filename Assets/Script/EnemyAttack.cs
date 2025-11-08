using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;

    public GameObject RangeTrigger;
    
    private bool _attacking = false;

    public Player3 player3;
    
    private bool playerInRange = false;
    private float attackCooldown = 2f;
    private float lastAttackTime = 0f;
    
    // 添加攻击力属性
    public int minDamage = 20;
    public int maxDamage = 30;

    // Update is called once per frame
    void Update()
    {
        // 检查是否可以攻击玩家
        if (playerInRange && Time.time - lastAttackTime >= attackCooldown && !_attacking)
        {
            Attack();
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        _attacking = true;
        // 不再禁用玩家脚本，而是通过其他方式处理攻击
        lastAttackTime = Time.time;
        
        // 造成伤害
        DealDamage();
    }
    
    // 处理伤害逻辑
    void DealDamage()
    {
        if (player3 != null)
        {
            // 生成20-30之间的随机伤害值
            int damage = Random.Range(minDamage, maxDamage + 1);
            // 对玩家造成伤害
            player3.TakeHurt(damage);
        }
    }

    public void EndAttack()
    {
        RangeTrigger.SetActive(false);
    }

    public void AttackAnimationFinished()
    {
        _attacking = false;
        // 不再需要启用玩家脚本，因为它从未被禁用
    }
    
    // 当玩家进入攻击范围时调用
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            RangeTrigger.SetActive(true);
        }
    }
    
    // 当玩家离开攻击范围时调用
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            RangeTrigger.SetActive(false);
        }
    }
}