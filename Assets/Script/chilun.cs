using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 齿轮控制器类，控制齿轮沿着预设路径移动并持续旋转
/// </summary>
public class chilun : MonoBehaviour
{
    /// <summary>
    /// 齿轮移动的路径点数组
    /// </summary>
    public GameObject[] points;
    
    /// <summary>
    /// 齿轮旋转速度
    /// </summary>
    public float rotationSpeed = 100f;
    
    /// <summary>
    /// 齿轮在每个路径点的等待时间（秒）
    /// </summary>
    public float waitTime = 1.0f;

    /// <summary>
    /// 当前目标点的索引
    /// </summary>
    private int num;
    
    /// <summary>
    /// 标记齿轮是否正在等待
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
            // 等待期间仍然旋转
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            return; // 等待期间不执行移动逻辑
        }

        // 正常移动逻辑
        transform.position += (points[num].transform.position - transform.position).normalized * Time.deltaTime;
        if (Vector2.Distance(transform.position, points[num].transform.position) < 0.1f)
        {
            // 到达目标点，开始等待
            isWaiting = true;
        }
        
        // 添加旋转逻辑，让齿轮持续旋转
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// 当其他物体进入齿轮触发区域时调用
    /// </summary>
    /// <param name="collision">进入触发区域的碰撞体</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 如果碰撞的是齿轮标签的物体，则将其设置为齿轮的子物体
        if (collision.tag == "chilun")
        {
            collision.transform.parent = transform;
        }
    }
    
    /// <summary>
    /// 当其他物体离开齿轮触发区域时调用
    /// </summary>
    /// <param name="collision">离开触发区域的碰撞体</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 如果离开的是齿轮标签的物体，则将其从齿轮的子物体中移除
        if (collision.tag == "chilun")
        {
            collision.transform.parent = null;
        }
    }
}