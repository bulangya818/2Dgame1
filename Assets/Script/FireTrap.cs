using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    [Header("动画组件")]
    public Animator animator;
    
    [Header("触发延迟")]
    public float activationDelay = 1.0f;
    
    [Header("伤害值")]
    public float damage = 100f;
    
    [Header("火焰子对象")]
    public HuoYan huoYan;
    
    private bool isActive = false;
    private bool playerOnTop = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // 获取动画组件（如果在Inspector中未指定）
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        // 查找火焰子对象
        if (huoYan == null)
        {
            huoYan = GetComponentInChildren<HuoYan>();
            if (huoYan != null)
            {
                huoYan.fireTrap = this;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// 当玩家碰撞到陷阱时
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
                // 玩家踩到陷阱，触发Hit动画
                if (animator != null)
                {
                    animator.SetTrigger("Hit");
                }
                
                // 延迟一段时间后激活陷阱
                Invoke("ActivateTrap", activationDelay);
                
                // 记录玩家在陷阱上
                playerOnTop = true;
            }
            else if (isActive)
            {
                // 如果陷阱已激活且玩家从侧面或下方碰到，则造成伤害
                Player3 player = collision.gameObject.GetComponent<Player3>();
                if (player != null)
                {
                    player.TakeHurt(damage);
                }
            }
        }
    }
    
    /// <summary>
    /// 当玩家离开陷阱时
    /// </summary>
    /// <param name="collision">碰撞信息</param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 检查离开的对象是否是玩家
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnTop = false;
        }
    }
    
    /// <summary>
    /// 激活陷阱，播放On动画并激活火焰
    /// </summary>
    private void ActivateTrap()
    {
        if (playerOnTop)
        {
            isActive = true;
            
            // 触发On动画
            if (animator != null)
            {
                animator.SetTrigger("On");
            }
            
            // 激活火焰子对象
            if (huoYan != null)
            {
                huoYan.Activate();
            }
        }
    }
    
    /// <summary>
    /// 禁用陷阱
    /// </summary>
    public void DeactivateTrap()
    {
        isActive = false;
        playerOnTop = false;
        
        // 禁用火焰子对象
        if (huoYan != null)
        {
            huoYan.Deactivate();
        }
    }
    
    /// <summary>
    /// 当触发器被碰撞时（用于检测玩家是否碰到激活的陷阱）
    /// </summary>
    /// <param name="other">碰撞的物体</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰撞的对象是否是玩家且陷阱已激活
        if (other.CompareTag("Player") && isActive)
        {
            // 对玩家造成伤害
            Player3 player = other.GetComponent<Player3>();
            if (player != null)
            {
                player.TakeHurt(damage);
            }
        }
    }
}