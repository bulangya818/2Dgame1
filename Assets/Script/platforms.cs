using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台控制器类，控制平台沿着预设路径移动
/// </summary>
public class platforms : MonoBehaviour
{
    /// <summary>
    /// 平台移动的路径点数组
    /// </summary>
    public GameObject[] points;
    
    /// <summary>
    /// 平台在每个路径点的等待时间（秒）
    /// </summary>
    public float waitTime = 1.0f;

    /// <summary>
    /// 当前目标点的索引
    /// </summary>
    private int num;
    
    /// <summary>
    /// 标记平台是否正在等待
    /// </summary>
    private bool isWaiting = false;
    
    /// <summary>
    /// 等待计时器
    /// </summary>
    private float waitTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        num = 0;
    }

    // Update is called once per frame
    void Update()
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
            return; // 等待期间不执行移动逻辑
        }

        // 正常移动逻辑
        transform.position += (points[num].transform.position - transform.position).normalized * Time.deltaTime;
        if (Vector2.Distance(transform.position, points[num].transform.position) < 0.1f)
        {
            // 到达目标点，开始等待
            isWaiting = true;
        }
    }

    /// <summary>
    /// 当其他物体进入平台触发区域时调用
    /// </summary>
    /// <param name="collision">进入触发区域的碰撞体</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果碰撞的是玩家，则将玩家设置为平台的子物体
        // 这样玩家会随着平台一起移动
        if (collision.tag == "Player")
        {
            collision.transform.parent = transform;
        }
    }
    
    /// <summary>
    /// 当其他物体离开平台触发区域时调用
    /// </summary>
    /// <param name="collision">离开触发区域的碰撞体</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 如果离开的是玩家，则将玩家从平台的子物体中移除
        if (collision.tag == "Player")
        {
            collision.transform.parent = null;
        }
    }
}