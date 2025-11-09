using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuoYan : MonoBehaviour
{
    [Header("动画组件")]
    public Animator animator;
    
    [Header("伤害值")]
    public float damage = 100f;
    
    [Header("火陷阱主体引用")]
    public FireTrap fireTrap;
    
    private bool isOn = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // 获取动画组件（如果在Inspector中未指定）
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        // 确保碰撞体初始为触发器且禁用
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
            collider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// 激活火焰，开始造成伤害
    /// </summary>
    public void Activate()
    {
        isOn = true;
        
        // 激活动画
        if (animator != null)
        {
            animator.SetTrigger("On");
        }
        
        // 启用碰撞体以检测玩家
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
    
    /// <summary>
    /// 禁用火焰
    /// </summary>
    public void Deactivate()
    {
        isOn = false;
        
        // 禁用碰撞体
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
    
    /// <summary>
    /// 当触发器被碰撞时（用于检测玩家是否碰到火焰）
    /// </summary>
    /// <param name="other">碰撞的物体</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查碰撞的对象是否是玩家且火焰处于激活状态
        if (other.CompareTag("Player") && isOn)
        {
            // 对玩家造成伤害
            Player3 player = other.GetComponent<Player3>();
            if (player != null)
            {
                player.TakeHurt(damage);
            }
        }
    }
    
    /// <summary>
    /// 当触发器退出碰撞时
    /// </summary>
    /// <param name="other">离开的物体</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        // 可以在这里添加额外的逻辑
    }
}