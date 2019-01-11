﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaiHuongDan : MonoBehaviour
{
    public float PhamViToiThieu;
    public float PhamViMaxCurrence;
    public float cre;
    public float high = 2.5f;
    public float weigh = -2f;
    public int Level;
    public int experence;
    public string name;
    public LayerMask PlayerLayer;
    public float PhamViMin;
    public float PhamViMax;
    public float PlayerCheckRadius;
    public Transform PlayerCheckPoint;
    public GameObject enemy;
    public int bloodLoss;
    public GameObject player;
    public float speed = 5f;
    public float jumpSpeed = 10f;
    private Rigidbody2D rigidBody;
    private Animator playerAnimation;
    bool flag;
    private HPPlayerManager gameHPManager;
    public int count;
    public int temp;
    int temp1;
    public float PhamViAttack;
    public int HPinput;
    public QuaiHPManager HP;
    //public HealthBar healthBar;
    public Transform pfHealthBar;
    Transform healthBarTransform;
    HealthBar healthBar;
    Collider2D[] enemiesToDamege;
    bool died = false;
    // Use this for initialization
    void Start()
    {
        
        PhamViMaxCurrence = PhamViMax;
        rigidBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        gameHPManager = FindObjectOfType<HPPlayerManager>();
        flag = false;
        temp1 = temp;
        HP = new QuaiHPManager(HPinput);

        healthBarTransform = Instantiate(pfHealthBar, new Vector3(transform.position.x + weigh, transform.position.y + high), Quaternion.identity);
        healthBar = healthBarTransform.GetComponent<HealthBar>();
        healthBar.setup(HP);
    }

    // Update is called once per frame
    void Update()
    {
        enemiesToDamege = Physics2D.OverlapCircleAll(PlayerCheckPoint.position, PlayerCheckRadius, PlayerLayer);
        if (enemiesToDamege.Length > 0)
        {
            flag = true;
        }
        else
            flag = false;
        Debug.Log(flag);
        healthBar.transform.position = new Vector3(transform.position.x + weigh, transform.position.y + high);
        if (HP.getHP() <= 0 && died == false)
        {
            died = true;
            Debug.Log("--------------");
            Debug.Log(Level);
            Debug.Log(player.GetComponent<HuongDan>().Experence.getLevel());
            if (player.GetComponent<HuongDan>().Experence.getLevel() <= Level)
            {
                experence = experence - (int)(experence * (player.GetComponent<HuongDan>().Experence.getLevel() - Level) * 0.1);
            }
            else if ((player.GetComponent<HuongDan>().Experence.getLevel() - Level > 0) && (player.GetComponent<HuongDan>().Experence.getLevel() - Level <= 3))
            {
                experence = experence - (int)(experence * (Level - player.GetComponent<HuongDan>().Experence.getLevel()) * 0.1);
            }
            else
            {
                experence = 0;
            }
            if (experence > 0)
            {
                Debug.Log(experence);
                player.GetComponent<HuongDan>().Experence.increase(experence);
            }
            Debug.Log(experence);
            player.GetComponent<HuongDan>().cancalAttacking(name);
            enemy.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        }
        if (Mathf.Abs(Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2))) < PhamViMaxCurrence && Mathf.Abs(Mathf.Sqrt(Mathf.Pow(player.transform.position.x - transform.position.x, 2) + Mathf.Pow(player.transform.position.y - transform.position.y, 2))) >= PhamViMin)
        {
            PhamViMaxCurrence = 20;
            if (HP.getHP() > 0)
            {
                player.GetComponent<HuongDan>().setAttacking(name);
            }
            if (flag)
            {
                if (count == temp)
                {
                    float damg = bloodLoss + (Level - player.GetComponent<HuongDan>().Experence.getLevel()) * 0.1f * bloodLoss;
                    int damgBlood = (int)damg;
                    if (damgBlood > 0)
                    {
                        if (cre * player.GetComponent<HuongDan>().speed < player.GetComponent<HuongDan>().speedCurrence)
                            player.GetComponent<HuongDan>().setSpeedCurrence(cre);
                        player.GetComponent<HuongDan>().HP.Damage(damgBlood);
                    }
                    temp = 0;
                }
                else
                {
                    temp++;
                }
                playerAnimation.SetBool("Attack", true);
            }
            else
            {
                temp = temp1;
                if (player.transform.position.x - transform.position.x < -PhamViToiThieu)
                {
                    rigidBody.velocity = new Vector2(-Mathf.Abs(speed), rigidBody.velocity.y);
                    transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
                }
                else if (player.transform.position.x - transform.position.x > PhamViToiThieu)
                {
                    rigidBody.velocity = new Vector2(Mathf.Abs(speed), rigidBody.velocity.y);
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                }
                else
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
                playerAnimation.SetFloat("Speed", 0.2f);
                playerAnimation.SetBool("Attack", false);
            }
        }
        else
        {
            PhamViMaxCurrence = PhamViMax;
            player.GetComponent<HuongDan>().cancalAttacking(name);
            if (HP.getHP() > 0)
                HP.returnHP();
            playerAnimation.SetBool("Attack", false);
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            playerAnimation.SetFloat("Speed", 0f);
        }

    }
    void OnTriggerEnter2D(Collider2D collision)
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

    }


}
