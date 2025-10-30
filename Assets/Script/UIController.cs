using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// UI控制器类，用于管理游戏中的各种UI面板和场景跳转
/// 负责处理游戏设置面板、成功面板、失败面板的显示与隐藏
/// 同时处理游戏暂停、继续、重新开始、退出等操作
/// </summary>
public class UIController : MonoBehaviour
{
    /// <summary>
    /// 设置面板对象引用
    /// 当玩家按下ESC键或点击设置按钮时显示该面板
    /// 包含继续游戏、重新开始、返回主菜单等选项
    /// </summary>
    public GameObject settingpanel;
    
    /// <summary>
    /// 成功面板对象引用
    /// 当玩家完成关卡目标时显示该面板
    /// 通常包含下一关、重新开始、返回主菜单等选项
    /// </summary>
    public GameObject succpanel;
    
    /// <summary>
    /// 失败面板对象引用
    /// 当玩家失败（如碰到敌人、掉入深渊等）时显示该面板
    /// 通常包含重新开始、返回主菜单等选项
    /// </summary>
    public GameObject failpanel;
    
    /// <summary>
    /// 面板显示状态标记
    /// true表示当前有面板处于显示状态，false表示所有面板都已隐藏
    /// 用于控制面板的显示与隐藏切换逻辑
    /// </summary>
    public bool isshow;
    
    /// <summary>
    /// UIController单例实例
    /// 通过单例模式确保整个游戏中只有一个UIController实例
    /// 其他脚本可以通过UIController.instance访问该实例
    /// </summary>
    public static UIController instance;

    /// <summary>
    /// Awake方法，在脚本实例被载入时调用，用于初始化单例模式
    /// 这个方法在Start方法之前执行，确保单例在其他脚本使用前已经创建
    /// </summary>
    private void Awake()
    {
        // 确保只有一个UIController实例存在，避免重复创建
        // 如果已经存在其他实例且不是当前实例，则销毁当前实例
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject); // 销毁当前游戏对象
            return; // 直接返回，不再执行后续代码
        }

        // 将当前实例赋值给instance，实现单例模式
        instance = this;
    }

    /// <summary>
    /// Start方法，在第一帧更新之前调用，用于初始化变量和游戏状态
    /// 在Awake方法之后执行，此时所有对象都已初始化完成
    /// </summary>
    void Start()
    {
        isshow = false; // 初始化面板显示状态为隐藏
        hidepanels(); // 隐藏所有面板并恢复游戏时间流速
    }

    /// <summary>
    /// 隐藏所有面板并恢复游戏时间流速
    /// 在游戏开始、继续、重新开始等操作时调用
    /// 确保所有UI面板都被正确隐藏，游戏时间恢复正常流速
    /// </summary>
    private void hidepanels()
    {
        if (settingpanel != null) settingpanel.SetActive(false); // 隐藏设置面板
        if (succpanel != null) succpanel.SetActive(false);      // 隐藏成功面板
        if (failpanel != null) failpanel.SetActive(false);      // 隐藏失败面板
        Time.timeScale = 1; // 恢复正常时间流速，取消暂停状态
    }

    /// <summary>
    /// 显示指定名称的面板
    /// 根据传入的面板名称参数激活对应的面板对象
    /// 同时暂停游戏时间，使面板显示时游戏处于暂停状态
    /// </summary>
    /// <param name="panelname">面板名称 ("setting", "succ", "fail")
    /// "setting" - 设置面板
    /// "succ" - 成功面板
    /// "fail" - 失败面板
    /// </param>
    public void showpanel(String panelname)
    {
        // 移除字符串前后的空格，避免因空格导致的匹配失败
        panelname = panelname.Trim();
        
        // 根据面板名称激活对应的面板
        // 使用if语句分别判断，允许扩展更多面板类型
        if (panelname == "setting" && settingpanel != null)
        {
            settingpanel.SetActive(true); // 激活设置面板
        }

        if (panelname == "succ" && succpanel != null)
        {
            succpanel.SetActive(true); // 激活成功面板
        }

        if (panelname == "fail" && failpanel != null)
        {
            failpanel.SetActive(true); // 激活失败面板
        }

        // 暂停游戏时间，使面板显示时游戏逻辑暂停执行
        Time.timeScale = 0;
    }

    /// <summary>
    /// Update方法，每帧调用，用于处理按键输入
    /// 检测玩家的ESC键输入，实现设置面板的快速显示/隐藏
    /// 这是游戏中最重要的输入处理逻辑之一
    /// </summary>
    void Update()
    {
        // 检测Esc键按下，切换设置面板显示状态
        // 使用GetKeyDown确保只在按下瞬间触发一次，避免持续触发
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 切换面板显示状态，true变false，false变true
            isshow = !isshow;
            
            // 根据isshow状态设置设置面板的激活状态
            if (settingpanel != null) settingpanel.SetActive(isshow);
            
            // 根据面板显示状态设置游戏时间流速
            if (isshow)
            {
                Time.timeScale = 0; // 暂停游戏，时间流速为0
            }
            else
            {
                Time.timeScale = 1; // 恢复游戏，时间流速为正常速度
            }
        }
    }

    /// <summary>
    /// 继续游戏按钮点击事件处理
    /// 当玩家在设置面板中点击继续游戏按钮时调用
    /// 隐藏所有面板并恢复游戏正常运行
    /// </summary>
    public void continuegame()
    {
        isshow = !isshow; // 切换面板显示状态
        if (settingpanel != null) settingpanel.SetActive(isshow); // 根据状态设置设置面板激活状态
        hidepanels(); // 隐藏所有面板并恢复游戏
    }

    /// <summary>
    /// 重新开始按钮点击事件处理
    /// 当玩家在设置面板或失败面板中点击重新开始按钮时调用
    /// 重新加载当前关卡，让玩家可以从头开始
    /// </summary>
    public void restart()
    {
        hidepanels(); // 隐藏所有面板并恢复游戏
        SceneManager.LoadScene("SampleScene"); // 重新加载SampleScene场景
    }

    /// <summary>
    /// 退出游戏按钮点击事件处理
    /// 当玩家在设置面板中点击退出游戏按钮时调用
    /// 返回到游戏主菜单场景
    /// </summary>
    public void QuitGame()
    {
        hidepanels(); // 隐藏所有面板并恢复游戏
        SceneManager.LoadScene("start"); // 跳转到开始场景
    }

    /// <summary>
    /// 下一关按钮点击事件处理
    /// 当玩家在成功面板中点击下一关按钮时调用
    /// 跳转到下一个关卡场景
    /// </summary>
    public void next()
    {
        hidepanels(); // 隐藏所有面板并恢复游戏
        // 跳转到下一关卡 SampleScene1
        SceneManager.LoadScene("SampleScene 1");
    }
}