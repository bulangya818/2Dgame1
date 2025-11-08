using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public GameObject[] points;
    public float waitTime = 1.0f;
    private int num;


    private bool isWaiting = false;


    private float waitTimer = 0f;
    
    // 用于保存初始缩放
    private Vector3 originalScale;
    
    // 添加仇恨和攻击相关字段
    public float chaseRange = 5f;         // 仇恨范围
    public float attackRange = 1.5f;      // 攻击范围
    public float moveSpeed = 2f;          // 追踪速度
    private Transform player;             // 玩家引用
    private Player3 player3;              // 玩家引用
    private enum EnemyState { Patrolling, Chasing, Attacking }
    private EnemyState currentState = EnemyState.Patrolling;
    private bool isAttacking = false;     // 是否正在攻击
    private float attackCooldown = 2f;    // 攻击冷却时间
    private float lastAttackTime = 0f;    // 上次攻击时间
    private bool isDead = false;          // 是否已经死亡
    
    // 添加攻击力属性
    public int minDamage = 20;
    public int maxDamage = 30;

    // Start is called before the first frame update
    void Start()
    {
        num = 0;
        originalScale = transform.localScale;
        
        // 查找玩家对象
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            player3 = playerObject.GetComponent<Player3>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        
        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrol();
                CheckForPlayer();
                break;
            case EnemyState.Chasing:
                ChasePlayer();
                CheckAttackRange();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
        }
    }
    
    // 巡逻行为
    void Patrol()
    {
        // 如果正在等待，则更新等待计时器
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                // 移动到下一个点
                num++;
                if (num == points.Length)
                {
                    num = 0;
                }
            }

            // 等待期间不执行移动逻辑，设置动画为Idle
            animator.SetFloat("Speed", 0f);
            return;
        }

        // 计算移动方向
        Vector3 direction = points[num].transform.position - transform.position;

        // 根据移动方向翻转敌人
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // 正常移动逻辑
        transform.position += direction.normalized * Time.deltaTime;

        // 设置移动动画
        animator.SetFloat("Speed", direction.normalized.magnitude);

        if (Vector2.Distance(transform.position, points[num].transform.position) < 0.1f)
        {
            // 到达目标点，开始等待
            isWaiting = true;
        }
    }
    
    // 检查玩家是否进入仇恨范围
    void CheckForPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= chaseRange)
            {
                currentState = EnemyState.Chasing;
            }
        }
    }
    
    // 追踪玩家
    void ChasePlayer()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // 如果玩家离开了仇恨范围，回到巡逻状态
        if (distanceToPlayer > chaseRange)
        {
            currentState = EnemyState.Patrolling;
            return;
        }
        
        // 如果进入攻击范围，切换到攻击状态
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
            return;
        }
        
        // 追踪玩家
        Vector3 direction = (player.position - transform.position).normalized;
        
        // 根据移动方向翻转敌人
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        
        // 移动敌人
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        // 设置移动动画
        animator.SetFloat("Speed", direction.magnitude);
    }
    
    // 检查是否可以攻击
    void CheckAttackRange()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // 如果玩家离开了仇恨范围，回到巡逻状态
        if (distanceToPlayer > chaseRange)
        {
            currentState = EnemyState.Patrolling;
            return;
        }
        
        // 如果进入攻击范围，切换到攻击状态
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
        // 如果远离了攻击范围但仍在仇恨范围内，继续追踪
        else if (distanceToPlayer > attackRange && distanceToPlayer <= chaseRange)
        {
            currentState = EnemyState.Chasing;
        }
    }
    
    // 攻击玩家
    void AttackPlayer()
    {
        if (player == null) return;
        
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        
        // 如果玩家离开了仇恨范围，回到巡逻状态
        if (distanceToPlayer > chaseRange)
        {
            currentState = EnemyState.Patrolling;
            isAttacking = false;
            animator.SetBool("isAttacking", false);
            return;
        }
        
        // 如果超出了攻击范围，回到追踪状态
        if (distanceToPlayer > attackRange)
        {
            currentState = EnemyState.Chasing;
            isAttacking = false;
            animator.SetBool("isAttacking", false);
            return;
        }
        
        // 面向玩家
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        
        // 攻击冷却完毕后执行攻击
        if (Time.time - lastAttackTime >= attackCooldown && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            lastAttackTime = Time.time;
        }
    }
    
    // 动画事件调用的方法，攻击动画开始时激活攻击检测
    public void OnAttackStart()
    {
        // 对玩家造成伤害
        DealDamageToPlayer();
    }
    
    // 动画事件调用的方法，攻击动画结束时重置状态
    public void OnAttackEnd()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        
        // 攻击结束后检查玩家位置以确定下一步行动
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer > attackRange && distanceToPlayer <= chaseRange)
            {
                currentState = EnemyState.Chasing;
            }
            else if (distanceToPlayer > chaseRange)
            {
                currentState = EnemyState.Patrolling;
            }
        }
    }
    
    // 处理对玩家的伤害
    void DealDamageToPlayer()
    {
        if (player3 != null)
        {
            // 生成随机伤害值
            int damage = UnityEngine.Random.Range(minDamage, maxDamage + 1);
            // 对玩家造成伤害
            player3.TakeHurt(damage);
        }
    }
    
    public void Hurt()
    {
        // 如果已经死亡，不再处理受伤
        if (isDead) return;
        
        // 直接死亡
        Die();
    }
    
    // 敌人死亡处理
    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        // 禁用碰撞器
        /*Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }*/
        // 2秒后销毁游戏对象
        Destroy(gameObject, 0.88f);
    }
    
    // 在Scene视图中绘制敌人检测范围的辅助线
    void OnDrawGizmosSelected()
    {
        // 绘制仇恨范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        
        // 绘制攻击范围
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}