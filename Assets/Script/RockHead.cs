using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockHead : MonoBehaviour
{
    [Header("动画组件")]
    public Animator animator;
    
    [Header("水果预制体")]
    public GameObject fruitPrefab;
    
    [Header("掉落水果数量")]
    [Range(1, 10)]
    public int fruitCount = 3;
    
    private bool isHit = false;

    void Start()
    {
        // 获取动画组件（如果在Inspector中未指定）
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }
    
    /// <summary>
    /// 当玩家进入触发器区域时调用
    /// </summary>
    /// <param name="other">进入触发器的碰撞体</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查进入触发器的对象是否是玩家
        if (other != null && other.CompareTag("Player"))
        {
            // 检查玩家是否在RockHead下方
            if (other.transform != null && other.transform.position.y < transform.position.y)
            {
                // 玩家在下方时触发撞击效果
                HitByPlayer();
            }
        }
    }
    
    /// <summary>
    /// 被玩家撞击时的处理
    /// </summary>
    private void HitByPlayer()
    {
        // 确保只触发一次
        if (isHit) 
        {
            return;
        }
        
        isHit = true;
        
        // 播放TopHit动画
        if (animator != null)
        {
            animator.SetTrigger("TopHit");
        }
        
        // 掉落水果
        DropFruits();
        
        // 更改Sprite Renderer的sorting order为1
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 1;
        }
        
        // 销毁自身（带延迟以便播放动画）
        Destroy(gameObject, 1f);
    }
    
    /// <summary>
    /// 掉落水果
    /// </summary>
    private void DropFruits()
    {
        for (int i = 0; i < fruitCount; i++)
        {
            if (fruitPrefab != null)
            {
                // 在RockHead位置稍微上方一点生成水果
                Vector3 spawnPosition = new Vector3(
                    transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f),
                    transform.position.y + 0.5f,
                    transform.position.z
                );
                
                // 实例化水果
                GameObject fruit = Instantiate(fruitPrefab, spawnPosition, Quaternion.identity);
                
                // 给水果添加一些随机的力，使其散开
                Rigidbody2D fruitRb = fruit.GetComponent<Rigidbody2D>();
                if (fruitRb != null)
                {
                    Vector2 force = new Vector2(
                        UnityEngine.Random.Range(-2f, 2f), 
                        UnityEngine.Random.Range(2f, 4f)
                    );
                    fruitRb.AddForce(force, ForceMode2D.Impulse);
                }
            }
        }
    }
}