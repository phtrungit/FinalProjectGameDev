﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainChar : MonoBehaviour {
    public Text lv;
    public Text HPText;
    public Text ManaText;
    public Text ExperenceText;
    public Text Note;
    public Text Note2;
    public LayerMask NPCLayer;
    public GameObject Skill2;
    public int skill2CD;
    public int skill1CD;
    public float skill2PhamVi;
    public GameObject Skill3;
    public int skill3CD;
    public float skill3PhamVi;
    public Transform Skill3CheckPoint;
    public GameObject Skill4;
    public int skill4CD;
    public float skill4PhamVi;
    public Transform Skill4CheckPoint;
    public string name;
    string filename = "dataplayer.gd";
    public int level;
    public int dame;
    public string whoIsUsing;
    public bool isAttacking;
    public Camera Camera;
    public int HPMax;
    public int ManaMax;
    public float speed = 5f;
    public float jumpSpeed = 10f;
    private float moment = 0f;
    private Rigidbody2D rigidBody;
    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    private Animator playerAnimation;
    int attack1;
    public LayerMask QuaiLayer;
    public Transform QuaiCheckPoint;
    public float QuaiCheckRadius;
    public Transform pfHealthBar;
    Transform healthBarTransform;
    HealthBar healthBar;
    public QuaiHPManager HP;
    public Transform pfManaBar;
    Transform ManaBarTransform;
    HealthBar ManaBar;
    public QuaiHPManager Mana;
    public Transform pfLVBar;
    Transform LVBarTransform;
    LevelBar LVBar;
    public int experenceCurence;
    public ExperenceManager Experence;
    public float speedCurrence;
    public GameObject HPBar;
    public GameObject manaBar;
    public GameObject expBar;
    int skill2Attack = 0;
    int skill1Attack = 0;
    int skill3Attack = 0;
    int skill4Attack = 0;
    // Use this for initialization
    void Start () {
        this.tag = "Player";
        speedCurrence = speed;
        database dataBase = readData();
        level = dataBase.lv;
        //Debug.Log(ExperenceManager.getHealthMax(level));
        name = dataBase.name;
        experenceCurence = dataBase.experence;
        lv.text = (level+1).ToString();
        Debug.Log(level);
        Debug.Log(experenceCurence);
        Debug.Log(dataBase.lvMap);
        isAttacking = false;
        whoIsUsing = "";
        rigidBody = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        attack1 = -1;
        HP = new QuaiHPManager(HPMax);
        healthBar = HPBar.GetComponent<HealthBar>();
        healthBar.setup(HP);
        Mana = new QuaiHPManager(ManaMax);
        ManaBar = manaBar.GetComponent<HealthBar>();
        ManaBar.setup(Mana);
        Experence = new ExperenceManager(level, experenceCurence,ref HP, ref Mana);
        LVBar = expBar.GetComponent<LevelBar>();
        LVBar.setup(Experence);
    }

    // Update is called once per frame
    void Update() {
        if (skill2Attack > 0)
            skill2Attack--;
        if (skill2Attack == 50)
        {
            Skill2.GetComponent<Animator>().SetBool("skill2", false);
            playerAnimation.SetBool("skill2", false);
        }
        if (skill1Attack > 0)
            skill1Attack--;
        if (skill1Attack == 0)
        {
            playerAnimation.SetBool("attack1", false);
        }
        if (skill3Attack > 0)
            skill3Attack--;
        if (skill3Attack == 0)
        {
            Skill3.GetComponent<Animator>().SetBool("skill3", false);
            playerAnimation.SetBool("skill3", false);
        }
        if (skill4Attack > 0)
            skill4Attack--;
        if (skill4Attack == 0)
        {
            Skill4.GetComponent<Animator>().SetBool("skill4", false);
            playerAnimation.SetBool("skill4", false);
        }
        if (isAttacking == true)
        {
            Note.text = "Tiến nhập chiến đấu trung.";
        }
        else
        {
            Note.text = "";
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        //database dataBase = readData();
        //level = dataBase.lv;
        lv.text = Experence.getLevel().ToString();
        int HPCurrenceTemp = (int)HP.getHP();
        int HPMaxTemp = (int)HP.getHPMax();
        HPText.text = HPCurrenceTemp.ToString() + "/" + HPMaxTemp.ToString();
        int ManaCurrenceTemp = (int)Mana.getHP();
        int ManaMaxTemp = (int)Mana.getHPMax();
        ManaText.text = ManaCurrenceTemp.ToString() + "/" + ManaMaxTemp.ToString();
        ExperenceText.text = Experence.getExperence().ToString() + "/" + Experence.getExperenceNextLV().ToString();
        Collider2D[] NPC = Physics2D.OverlapCircleAll(QuaiCheckPoint.position, QuaiCheckRadius, NPCLayer);
        if (NPC.Length > 0)
        {
            writeData(2);
        }
        else
        {
            Debug.Log("---");
            writeData();
        } 
       
        if (HP.getHP() <= 0)
        {
            HP.returnHP();
            transform.position = new Vector3(0.09f, -3.170004f, transform.position.z);
        }
        isTouchingGround = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, QuaiLayer);
        moment = Input.GetAxis("Horizontal");
        if (moment > 0f)
        {
            playerAnimation.SetFloat("Speed", 1);
            rigidBody.velocity = new Vector2(moment * speedCurrence, rigidBody.velocity.y);
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (moment < 0f)
        {
            playerAnimation.SetFloat("Speed", 1);
            rigidBody.velocity = new Vector2(moment * speedCurrence, rigidBody.velocity.y);
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else
        {
            playerAnimation.SetFloat("Speed", 0);
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && isTouchingGround && isAttacking == false)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
        }


        playerAnimation.SetBool("onGround", isTouchingGround);
        if (Input.GetKeyDown(KeyCode.Alpha4) && skill4Attack == 0)
        {
            skill4Attack = skill4CD;
            Mana.Damage(5);
            playerAnimation.SetBool("skill4", true);
            Skill4.GetComponent<Animator>().SetBool("skill4", true);
            Collider2D[] enemiesToDamege = Physics2D.OverlapCircleAll(Skill4CheckPoint.transform.position, skill4PhamVi, QuaiLayer);
            if (enemiesToDamege.Length > 0)
            {
                enemiesToDamege[0].GetComponent<AutoAttack>().HP.Damage(dame);
                float damg = dame + (Experence.getLevel() - enemiesToDamege[0].GetComponent<AutoAttack>().Level) * 0.1f * dame;
                int damgBlood = (int)damg;
                if (damgBlood > 0)
                    enemiesToDamege[0].GetComponent<AutoAttack>().HP.Damage(damgBlood);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && skill3Attack == 0)
        {
            skill3Attack = skill3CD;
            Mana.Damage(5);
            playerAnimation.SetBool("skill3", true);
            Skill3.GetComponent<Animator>().SetBool("skill3", true);
            Collider2D[] enemiesToDamege = Physics2D.OverlapCircleAll(Skill3CheckPoint.transform.position, skill3PhamVi, QuaiLayer);
            if (enemiesToDamege.Length > 0)
            {
                enemiesToDamege[0].GetComponent<AutoAttack>().HP.Damage(dame);
                float damg = dame + (Experence.getLevel() - enemiesToDamege[0].GetComponent<AutoAttack>().Level) * 0.1f * dame;
                int damgBlood = (int)damg;
                if (damgBlood > 0)
                    enemiesToDamege[0].GetComponent<AutoAttack>().HP.Damage(damgBlood);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && skill2Attack == 0)
        {
            skill2Attack = skill2CD;
            Mana.Damage(5);
            playerAnimation.SetBool("skill2", true);
            Skill2.GetComponent<Animator>().SetBool("skill2", true);
            Collider2D[] enemiesToDamege = Physics2D.OverlapCircleAll(transform.position, skill2PhamVi, QuaiLayer);
            for (int i = 0; i < enemiesToDamege.Length; i++)
            {
                enemiesToDamege[i].GetComponent<AutoAttack>().HP.Damage(dame);
                float damg = dame + (Experence.getLevel() - enemiesToDamege[0].GetComponent<AutoAttack>().Level) * 0.1f * dame;
                int damgBlood = (int)damg;
                if (damgBlood > 0)
                    enemiesToDamege[0].GetComponent<AutoAttack>().HP.Damage(damgBlood);
            }

        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && skill1Attack == 0)
        {
            skill1Attack = skill1CD;
            Mana.Damage(2);
            playerAnimation.SetBool("attack1", true);
            Collider2D[] enemiesToDamege = Physics2D.OverlapCircleAll(QuaiCheckPoint.position, QuaiCheckRadius, QuaiLayer);
            if (enemiesToDamege.Length > 0)
            {
                enemiesToDamege[0].GetComponent<AutoAttack>().HP.Damage(dame);
                float damg = dame + (Experence.getLevel() - enemiesToDamege[0].GetComponent<AutoAttack>().Level) * 0.1f * dame;
                int damgBlood = (int)damg;
                if (damgBlood > 0)
                    enemiesToDamege[0].GetComponent<AutoAttack>().HP.Damage(damgBlood);
            }
        }


    }

    public void setSpeedCurrence(float cre)
    {
        this.speedCurrence = speed * cre;
    }

    public void setAttacking(string name)
    {
        if (whoIsUsing == "")
        {
            this.whoIsUsing = name;
            isAttacking = true;
        }
    }

    public void cancalAttacking(string name)
    {
        if (whoIsUsing == name)
        {
            this.whoIsUsing = "";
            this.speedCurrence = this.speed;
            isAttacking = false;
        }
    }

    public void writeData(int lvMap)
    {
        database dataBase = new database(name, Experence.getLevel()-1, Experence.getExperence(), lvMap);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
        bf.Serialize(file, dataBase);
        file.Close();
    }

    public void writeData()
    {
        database dataBase = new database(name, Experence.getLevel() - 1, Experence.getExperence());
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
        bf.Serialize(file, dataBase);
        file.Close();
    }

    database readData()
    {
        Debug.Log(Application.persistentDataPath + "/" + filename);
        database dataBase = new database(name, 1, 0, 1);
        if(File.Exists(Application.persistentDataPath + "/" + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filename, FileMode.Open);
            dataBase = (database)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            dataBase = new database(name, 0, 0, 1);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/" + filename);
            bf.Serialize(file, dataBase);
            file.Close();
        }
        return dataBase;
    }
}
