/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player类控制玩家角色的移动、跳跃、动画和碰撞检测
/// </summary>
public class Player : MonoBehaviour
{
    private float h; // 水平输入轴的值 (-1到1之间)
    private Rigidbody2D rig; // 玩家的2D刚体组件，用于物理运动控制
    private Animator an; // 玩家的动画控制器组件
    private int jumpCount; // 跳跃次数计数器，限制连续跳跃次数
    private float sepeed; // 角色移动速度（注：此处拼写有误，应为speed）
    public ParticleSystem partical; // 粒子系统，用于播放特效
    private bool faceRight; // 角色面向状态，true为向右，false为向左
    public GameObject blood; // 血迹特效预制体
    public AudioClip eat, foot, jump, hit; // 各种音效剪辑：吃水果、脚步声、跳跃声、受伤声
    private AudioSource au; // 音频源组件
    public Text text; // UI文本组件，用于显示分数
    private int score; // 玩家得分
    public GameObject startpos; // 起始位置对象

    /// <summary>
    /// 角色状态枚举，用于控制动画状态
    /// </summary>
    private enum state
    {
        idle,       // 待机状态
        run,        // 跑步状态
        jump,       // 跳跃状态
        fall,       // 下落状态
        doublejump  // 二段跳状态
    }

    /// <summary>
    /// Start is called before the first frame update
    /// 初始化玩家角色的各种属性和组件
    /// </summary>
    void Start()
    {
        faceRight = true; // 初始化角色面向右侧
        rig = GetComponent<Rigidbody2D>(); // 获取2D刚体组件用于物理控制
        an = GetComponent<Animator>(); // 获取动画控制器组件
        au = GetComponent<AudioSource>(); // 获取音频源组件
        jumpCount = 2; // 初始化跳跃次数为2，允许双跳
        sepeed = 2; // 设置移动速度为2
        score = 0; // 初始化分数为0
        text.text = "score:" + score + "/3"; // 更新UI分数显示
        transform.position = startpos.transform.position; // 将玩家位置设置为起始位置
    }

    /// <summary>
    /// Update is called once per frame
    /// 每帧调用，处理玩家输入和状态更新
    /// </summary>
    void Update()
    {
        Move();    // 处理角色移动
        Jump();    // 处理角色跳跃
        flip();    // 处理角色翻转（面向）
        playerAni(); // 处理角色动画状态
    }

    /// <summary>
    /// 控制角色水平移动
    /// </summary>
    private void Move()
    {
        h = Input.GetAxis("Horizontal"); // 获取水平轴输入（A/D键或方向键左右）
        rig.velocity = new Vector2(h * sepeed, rig.velocity.y); // 设置角色水平速度
    }

    /// <summary>
    /// 控制角色跳跃
    /// </summary>
    private void Jump()
    {
        // 检测空格键是否被按下且还有跳跃次数
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount > 0)
        {
            jumpCount--; // 减少可跳跃次数
            au.clip = jump; // 设置跳跃音效
            au.Play(); // 播放音效
            rig.AddForce(Vector2.up * 5, ForceMode2D.Impulse); // 给玩家施加向上的力实现跳跃
            PlayPS(); // 播放粒子特效
        }
    }

    /// <summary>
    /// 控制角色翻转（面向）
    /// </summary>
    private void flip()
    {
        // 当向右移动且当前面向左侧时，翻转角色
        if (h > 0 && faceRight == false)
        {
            flipc(); // 执行翻转操作
        }

        // 当向左移动且当前面向右侧时，翻转角色
        if (h < 0 && faceRight == true)
        {
            flipc(); // 执行翻转操作
        }
    }

    /// <summary>
    /// 执行角色翻转的具体操作
    /// </summary>
    private void flipc()
    {
        au.clip = foot; // 设置脚步音效
        au.Play(); // 播放音效
        Vector2 v = transform.localScale; // 获取当前缩放
        v.x *= -1; // X轴缩放取反实现翻转
        transform.localScale = v; // 应用新的缩放
        PlayPS(); // 播放粒子特效
        faceRight = !faceRight; // 更新面向状态
    }

    /// <summary>
    /// 播放粒子特效
    /// </summary>
    private void PlayPS()
    {
        partical.Play(); // 播放粒子系统
    }

    /// <summary>
    /// 根据角色状态设置动画
    /// </summary>
    private void playerAni()
    {
        state playerstate; // 定义角色状态变量
        
        // 根据水平移动速度判断是待机还是跑步状态
        if (Mathf.Abs(h) > 0.1f)
        {
            playerstate = state.run; // 水平移动时设为跑步状态
        }
        else
        {
            playerstate = state.idle; // 静止时设为待机状态
        }

        // 根据垂直速度判断跳跃、下落或二段跳状态
        if (rig.velocity.y > 0.3f)
        {
            playerstate = state.jump; // 向上移动时设为跳跃状态
            if (jumpCount == 0)
            {
                playerstate = state.doublejump; // 如果没有剩余跳跃次数则设为二段跳状态
            }
        }
        else if (rig.velocity.y < -0.2f)
        {
            playerstate = state.fall; // 向下移动时设为下落状态
        }

        an.SetInteger("state", (int)playerstate); // 将状态转换为整数并设置给动画控制器
    }

    /// <summary>
    /// 当玩家发生物理碰撞时调用
    /// </summary>
    /// <param name="collision">碰撞信息</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果玩家与地面发生碰撞，则重置跳跃次数
        if (collision.transform.tag == "ground")
        {
            jumpCount = 2; // 重置跳跃次数为2，允许再次双跳
        }

        // 如果玩家碰到水果，增加分数并销毁水果
        if (collision.transform.tag == "fruit")
        {
            score++; // 分数加1
            text.text = "score:" + score + "/3"; // 更新UI分数显示
            au.clip = eat; // 设置吃水果音效
            au.Play(); // 播放音效
            Destroy(collision.gameObject); // 销毁水果对象
        }

        // 如果玩家碰到墙壁（当前未实现任何逻辑）
        if (collision.transform.tag == "wall")
        {
        }

        // 如果玩家碰到尖刺，播放受伤效果并在1秒后显示失败界面
        if (collision.transform.tag == "spikes")
        {
            au.clip = hit; // 设置受伤音效
            au.Play(); // 播放音效
            GameObject e = Instantiate(blood, transform.position, Quaternion.identity); // 在玩家位置生成血迹特效
            Destroy(e, 0.5f); // 0.5秒后销毁血迹特效
            an.SetTrigger("hit"); // 触发受伤动画

            // 1秒后显示失败界面
            Invoke("ShowFailPanel", 1f);
        }

        // 如果玩家碰到起始平台，触发动画效果
        if (collision.transform.tag == "startpos")
        {
            Animator startan = collision.transform.GetComponent<Animator>(); // 获取平台的动画组件
            startan.SetInteger("ismove", 1); // 设置平台移动动画
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

    /*private void showpanell()
    {
        UIcontroller.instance.showpanel("failpanel");
    }#1#
}*/