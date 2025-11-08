using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;

    public GameObject[] points;


    public float waitTime = 1.0f;


    private int num;


    private bool isWaiting = false;


    private float waitTimer = 0f;
    
    // 用于保存初始缩放
    private Vector3 originalScale;

    // Start is called before the first frame update
    void Start()
    {
        num = 0;
        originalScale = transform.localScale;
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

            // 等待期间不执行移动逻辑，设置动画为Idle
            animator.SetFloat("Speed", 0f);
            return; 
        }

        // 计算移动方向
        Vector3 direction = points[num].transform.position - transform.position;
        
        // 根据移动方向翻转敌人
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // 正常移动逻辑
        transform.position += direction.normalized * Time.deltaTime;
        
        // 设置移动动画
        animator.SetFloat("Speed", direction.normalized.magnitude);
        
        if (Vector2.Distance(transform.position, points[num].transform.position) < 0.1f)
        {
            // 到达目标点，开始等待
            isWaiting = true;
        }
    }

    public void Hurt()
    {
        animator.SetTrigger("Hurt");
    }
}