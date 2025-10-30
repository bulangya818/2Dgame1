using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAction : MonoBehaviour
{
    // 刚体组件，用于控制角色物理运动
    private Rigidbody2D Rig;

    // 水平方向输入值
    private float H;
    private bool faceRight;
    private bool isJump;

    // Start is called before the first frame update
    void Start()
    {
        // 获取角色的刚体组件
        Rig = GetComponent<Rigidbody2D>();
        faceRight = false;
        isJump = false;
    }

    // Update is called once per frame
    void Update()
    {
        H = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }
    }

    private void FixedUpdate()
    {
        move();
        if (isJump)
        {
            Rig.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }
    }

    private void move()
    {
       
        Rig.velocity = new Vector2(H, Rig.velocity.y);

        if (H > 0 && faceRight == true)
        {
            flip();
        }
        else if (H < 0 && faceRight == false)
        {
            flip();
        }


        /*if (Input.GetKey(KeyCode.A))
        {
            // transform.position += new Vector3(-1 * Time.deltaTime, 0, 0);
            Rig.velocity = Vector2.right * -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            // transform.position += new Vector3(1 * Time.deltaTime, 0, 0);
            Rig.velocity = Vector2.right * 1;
        }*/

        /*// 按空格键时给角色施加向上的力，实现跳跃
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rig.AddForce(Vector2.up * 300);
        }*/

        /*// 按S键时使角色向下移动
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -1 * Time.deltaTime, 0);
        }*/
    }

    private void flip()
    {
        Vector2 v = transform.localScale;
        v.x *= -1;
        faceRight = !faceRight;
        transform.localScale = v;
    }
}