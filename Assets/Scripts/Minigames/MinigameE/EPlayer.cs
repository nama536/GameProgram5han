using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class EPlayer : MonoBehaviour
{
    //コントローラーの左スティック入力受け取り
    private Vector2 _axis;
    //プレイヤーの物理
    private Rigidbody2D _rb;
    //プレイヤーの速度とジャンプ力
    [SerializeField] float _playerSpeed;
    [SerializeField] float _playerJump;
    //弾
    [SerializeField] GameObject _bullet;

    //地面に着いているか
    private bool _isGround = false;
    //どちらを向いているか
    private bool _doRight;


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerMove();
    }

    //左スティック取得
    void OnMove(InputValue value)
    {
        _axis = value.Get<Vector2>();
    }

    //横移動
    void PlayerMove()
    {
        _rb.velocity = new Vector2(_axis.x * _playerSpeed,_rb.velocity.y);
        Vector3 nowPos = this.transform.position;
        nowPos.x = Mathf.Clamp(nowPos.x,-7.0f,7.0f);
        this.transform.position = nowPos;
        
        //プレイヤーの向きを変える
        if(_axis.x > 0.0f)
        {
            this.transform.eulerAngles = new Vector3(0.0f,0.0f,0.0f);
            _doRight = true;
        }
        else if(_axis.x < 0.0f)
        {
            this.transform.eulerAngles = new Vector3(0.0f,180.0f,0.0f);
            _doRight = false;
        }
    }

    //ジャンプ
    void OnJump()
    {
        if(_isGround == true)
        {
            _rb.velocity = new Vector2(_rb.velocity.x,_playerJump);
            _isGround = false;
        }
    }

    //射撃
    void OnShoot()
    {
        if(_doRight == true)
        {
            Instantiate(_bullet,transform.position + new Vector3(0.6f,0.0f,0.0f),transform.rotation);
        }
        else if(_doRight == false)
        {
            Instantiate(_bullet,transform.position + new Vector3(-0.6f,0.0f,0.0f),transform.rotation);
        }
    }

    //地面についたら
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" )
        {
            //飛べるようにする
           _isGround = true;
        }
    }
}
