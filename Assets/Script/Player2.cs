using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Player3类控制玩家角色的移动、跳跃和碰撞检测
/// </summary>
public class Player2 : MonoBehaviour
{
    /// <summary>
    /// 水平输入轴的值 (-1到1之间)
    /// </summary>
    private float h;
    
    /// <summary>
    /// 玩家的刚体组件，用于物理运动控制
    /// </summary>
    private Rigidbody2D rig;
    
    /// <summary>
    /// 跳跃次数计数器，限制连续跳跃次数
    /// </summary>
    private int jumpCount;
    
    /// <summary>
    /// 玩家的动画控制器组件
    /// </summary>
    private Animator an;
    
    /// <summary>
    /// 粒子系统组件
    /// </summary>
    public ParticleSystem partical;
    
    /// <summary>
    /// 标记玩家是否面朝右边
    /// </summary>
    private bool faceRight;
    
    /// <summary>
    /// 标记角色是否正在移动
    /// </summary>
    private bool isMoving;
    
    /// <summary>
    /// 各种音效剪辑：吃水果、脚步声、跳跃声、受伤声
    /// </summary>
    public AudioClip eat, foot, jump, hit;
    
    /// <summary>
    /// 音频源组件
    /// </summary>
    private AudioSource au;
    
    /// <summary>
    /// 玩家初始位置
    /// </summary>
    public GameObject startpos;
    
    /// <summary>
    /// 分数显示文本组件
    /// </summary>
    public Text text;
    
    /// <summary>
    /// 玩家当前分数
    /// </summary>
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        // 获取玩家对象的各种组件
        faceRight = true;
        isMoving = false; // 初始化移动状态为false
        rig = GetComponent<Rigidbody2D>(); // 获取2D刚体组件用于物理控制
        an = GetComponent<Animator>(); // 获取动画控制器组件
        au = GetComponent<AudioSource>(); // 获取音频源组件
        jumpCount = 2; // 初始化跳跃次数为2，允许双跳
        transform.position = startpos.transform.position;
        score = 0;
        text.text = "得分:" + score + "/3";
    }

    // Update is called once per frame
    void Update()
    {
        /*flip();
        Move();*/
        // 获取水平轴输入（A/D键或方向键左右），返回-1到1之间的值
        h = Input.GetAxis("Horizontal");
        an.SetFloat("speed", Mathf.Abs(h)); // 设置动画参数speed为水平输入的绝对值

        // 设置玩家的水平速度，保持垂直速度不变
        // h * 2f 控制移动速度，数值越大移动越快
        rig.velocity = new Vector2(h * 2f, rig.velocity.y);

        // 如果玩家向右移动，设置玩家面向右侧
        if (h > 0 /*&&faceRight==false*/)
        {
            // 只有当角色从静止状态转为移动状态时才播放脚步声
            if (!isMoving && Mathf.Abs(h) > 0.2f)
            {
                au.clip = foot; // 设置脚步音效
                au.Play(); // 播放音效
                isMoving = true; // 标记为正在移动
            }

            Vector2 v = transform.localScale;
            v.x = 1; // 设置X轴缩放为正数，角色面向右侧
            transform.localScale = v;
            /*partical.Play();
            faceRight = true;*/
        }

        // 如果玩家向左移动，设置玩家面向左侧
        else if (h < 0 /*&&faceRight==true*/)
        {
            // 只有当角色从静止状态转为移动状态时才播放脚步声
            if (!isMoving && Mathf.Abs(h) > 0.2f)
            {
                au.clip = foot; // 设置脚步音效
                au.Play(); // 播放音效
                isMoving = true; // 标记为正在移动
            }

            Vector2 v = transform.localScale;
            v.x = -1; // 设置X轴缩放为负数，角色面向左侧
            transform.localScale = v;
            /*partical.Play();
            faceRight = false;*/
        }
        // 如果玩家停止移动
        else if (Mathf.Abs(h) <= 0.2f)
        {
            isMoving = false; // 标记为停止移动
        }

        // 检测空格键是否被按下，用于跳跃，同时检查是否还有跳跃次数
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            jumpCount--; // 减少可跳跃次数
            au.clip = jump; // 设置跳跃音效
            au.Play(); // 播放音效
            // 给玩家施加向上的力，实现跳跃效果
            // Vector2.up * 5 是跳跃力度，ForceMode2D.Impulse表示瞬间力
            rig.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }

        // 根据玩家垂直速度调整重力大小，实现更自然的跳跃
        if (rig.velocity.y > 0)
        {
            rig.gravityScale = 1; // 上升时重力较小，让跳跃更柔和
        }
        else
        {
            rig.gravityScale = 2; // 下降时重力较大，让下落更快
        }

        // 根据水平移动速度设置角色动画状态
        // state = 0: 待机状态, state = 1: 跑步状态
        if (Mathf.Abs(h) > 0.2f)
        {
            an.SetInteger("state", 1); // 水平移动时设为跑步动画
        }
        else
        {
            an.SetInteger("state", 0); // 静止时设为待机动画
        }

        // 根据垂直速度设置跳跃和下落动画状态
        // state = 2: 起跳状态, state = 3: 下落状态, state = 4: 二段跳状态
        if (rig.velocity.y > 0.3f)
        {
            an.SetInteger("state", 2); // 向上移动时设为起跳动画
            if (jumpCount == 0)
            {
                an.SetInteger("state", 4); // 如果没有剩余跳跃次数则设为二段跳动画
            }
        }
        else if (rig.velocity.y < -0.3f)
        {
            an.SetInteger("state", 3); // 向下移动时设为下落动画
        }
    }
    

    /// <summary>
    /// 当玩家触发 OnTriggerEnter2D 事件时调用
    /// 触发器用于检测不产生物理反应的碰撞，如收集物品
    /// </summary>
    /// <param name="collision">与玩家发生触发的碰撞体</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player triggered with: " + collision.gameObject.name + " with tag: " + collision.tag);

        // 如果玩家碰到了水果标签的对象，则播放音效并销毁该对象
        if (collision.tag == "fruit")
        {
            au.clip = eat; // 设置吃水果音效
            au.Play(); // 播放音效
            Destroy(collision.gameObject); // 销毁水果对象
            score++;
            text.text = "得分:" + score + "/3";
        }

        // 如果玩家碰到了墙壁标签的对象，则重新加载当前场景（游戏重启）
        if (collision.tag == "wall")
        {
            Debug.Log("Player hit the wall, reloading scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景
        }

        // 如果玩家碰到旗子标签的对象，触发动画效果
        if (collision.tag == "flag")
        {
            collision.transform.GetComponent<Animator>().SetBool("flag", true); // 激活旗子动画
            Invoke("showsuccpanel", 1f);
        }
    }

    /// <summary>
    /// 显示成功面板
    /// </summary>
    private void showsuccpanel()
    {
        UIController.instance.showpanel("succ");
    }

    /// <summary>
    /// 当玩家发生物理碰撞时调用OnCollisionEnter2D方法
    /// 物理碰撞会产生实际的物理反应，如站在地面上
    /// </summary>
    /// <param name="collision">碰撞信息</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果玩家与地面发生碰撞，则重置跳跃次数
        if (collision.transform.tag == "ground")
        {
            jumpCount = 2; // 重置跳跃次数为2，允许再次双跳
        }

        // 如果玩家碰到尖刺，播放受伤动画并在1秒后显示失败界面
        if (collision.transform.tag == "jianci")
        {
            au.clip = hit; // 设置音效
            au.Play();
            an.SetTrigger("hit"); // 播放受伤动画
            // 1秒后显示失败界面
            Invoke("ShowFailPanel", 1f);
        }

        // 如果玩家碰到齿轮，播放受伤动画并在1秒后显示失败界面
        if (collision.transform.tag == "chilun")
        {
            au.clip = hit; // 设置音效
            au.Play();
            an.SetTrigger("hit"); // 播放受伤动画
            // 1秒后显示失败界面
            Invoke("ShowFailPanel", 1f);
        }

        // 如果玩家碰到开始平台，激活平台动画
        if (collision.transform.tag == "Start")
        {
            Animator an = collision.transform.GetComponent<Animator>(); // 获取平台的动画组件
            an.SetBool("Start", true); // 激活平台动画
        }
    }

    /// <summary>
    /// 当玩家离开碰撞时调用OnCollisionExit2D方法
    /// </summary>
    /// <param name="collision">离开碰撞的物体信息</param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 如果离开的是开始平台，关闭平台动画
        if (collision.transform.tag == "Start")
        {
            Animator an = collision.transform.GetComponent<Animator>(); // 获取平台的动画组件
            an.SetBool("Start", false); // 关闭平台动画
        }
    }

    /// <summary>
    /// 显示失败界面
    /// </summary>
    private void ShowFailPanel()
    {
        // 显示失败界面
        if (UIController.instance != null)
        {
            UIController.instance.showpanel("fail");
        }
    }

    /// <summary>
    /// 切换场景方法，重新加载当前场景
    /// </summary>
    private void changescene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景
    }
}