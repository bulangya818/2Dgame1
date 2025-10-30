using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 倒计时控制类
/// 负责游戏中的倒计时功能，当倒计时结束时重新加载当前场景
/// </summary>
public class daojishi : MonoBehaviour
{
    // 显示倒计时的UI文本组件
    public Text text;

    // 计时器，用于Update方法（当前被注释掉）
    private float timer;

    // 倒计时的数值（秒）
    private int Count;

    // Start is called before the first frame update
    void Start()
    {
        // 初始化UI文本显示
        text.text = "倒计时:";
        // 初始化计时器
        timer = 0;
        // 设置初始倒计时时间为60秒
        Count = 60;
        // 启动协程进行倒计时
        StartCoroutine(jishi());//调用协程方法
    }

    //协程
    /// <summary>
    /// 倒计时协程方法
    /// 每秒减少倒计时数值，并更新UI显示
    /// 当倒计时结束时重新加载当前场景
    /// </summary>
    /// <returns>IEnumerator迭代器</returns>
    IEnumerator jishi()
    {
        // 持续执行倒计时逻辑
        while (true)
        {
            // 等待1秒
            yield return new WaitForSeconds(1);
            // 倒计时减1
            Count--;
            // 如果倒计时小于0，重新加载当前场景
            if (Count < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景
            }

            // 更新UI文本显示
            text.text = "倒计时：" + Count;
        }
    }
    
    // Update is called once per frame
    /*void Update()
    {
        // 使用deltaTime累加计时器
        timer += Time.deltaTime;
        // 当计时器超过1秒时执行倒计时逻辑
        if (timer > 1)
        {
            // 倒计时减1
            Count--;
            // 如果倒计时小于0，重新加载当前场景
            if (Count < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 重新加载当前场景
            }

            // 重置计时器
            timer = 0;
            // 更新UI文本显示
            text.text = "倒计时:" + Count;
        }
    }*/
}