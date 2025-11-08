using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [Header("弹跳参数")]
    public float bounceForce = 10f; // 弹跳力度
    
    [Header("动画组件")]
    public Animator animator; // 跳板动画控制器
    
    // Start is called before the first frame update
    void Start()
    {
        // 获取动画组件（如果在Inspector中未指定）
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        // 确保跳板有合适的物理组件设置
        SetupPhysics();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// 设置跳板的物理属性，防止被推动
    /// </summary>
    private void SetupPhysics()
    {
        // 获取或添加Rigidbody2D组件
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // 设置为运动学刚体，这样不会受到其他力的影响，但可以影响其他物体
        rb.isKinematic = true;
        rb.freezeRotation = true;
        
        // 获取或确保有Collider2D组件
        if (GetComponent<Collider2D>() == null)
        {
            // 如果没有碰撞体，则添加一个BoxCollider2D
            gameObject.AddComponent<BoxCollider2D>();
        }
    }
    
    /// <summary>
    /// 当玩家碰撞到跳板时触发弹跳效果
    /// </summary>
    /// <param name="collision">碰撞信息</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 检查碰撞的对象是否是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            // 检查玩家是否从上方碰撞（通过比较位置）
            if (collision.transform.position.y > transform.position.y + 0.5f)
            {
                // 触发跳板动画
                if (animator != null)
                {
                    animator.SetTrigger("Bounce");
                }
                
                // 获取玩家的刚体组件
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                
                if (playerRb != null)
                {
                    // 重置玩家的Y轴速度并施加向上的力
                    playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
                    playerRb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
                    
                    // 播放弹跳音效（如果需要）
                    AudioSource audioSource = GetComponent<AudioSource>();
                    if (audioSource != null)
                    {
                        audioSource.Play();
                    }
                }
            }
        }
    }
}