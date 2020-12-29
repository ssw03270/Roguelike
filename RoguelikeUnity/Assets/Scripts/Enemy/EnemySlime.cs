﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    private string enemyName = "Slime";

    private int currentHealth;
    private int currentMana;
    private int maxHealth;
    private int maxMana;
    private int damage;

    private UIHealthBar uIHealthBar;
    public GameObject objHealthBar;
    public GameObject canvas;
    RectTransform healthBar;
    private float height = 0.5f;

    public GameSystem gameSystem;

    // Start is called before the first frame update
    void Start()
    {
        SetState();

        healthBar = Instantiate(objHealthBar, canvas.transform).GetComponent<RectTransform>();
        uIHealthBar = healthBar.gameObject.GetComponent<UIHealthBar>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        SetHealthBar();
    }
    /// <summary>
    /// 적의 상태를 설정하는 함수
    /// GameSystem에서 enemyList 정보를 가져온다.
    /// 해당 정보를 바탕으로 적의 상태를 설정한다.
    /// </summary>
    private void SetState()
    {
        for (int i = 0; i < gameSystem.enemyList.Count; i++)
        {
            if (gameSystem.enemyList[i][1] == enemyName)
            {
                maxHealth = int.Parse(gameSystem.enemyList[i][2]);
                currentHealth = maxHealth;
                maxMana = int.Parse(gameSystem.enemyList[i][3]);
                currentMana = maxMana;
                damage = int.Parse(gameSystem.enemyList[i][4]);

                break;
            }
        }
    }
    /// <summary>
    /// 적의 상태를 체크하는 함수
    /// 체력이 0 이하가 되면 체력바와 적을 삭제한다.
    /// </summary>
    private void CheckState()
    {
        if(currentHealth <= 0)
        {
            Destroy(healthBar.gameObject);
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 적의 체력바를 설정하는 함수
    /// 체력바의 위치 및 체력 정보를 바탕으로 체력바를 설정한다.
    /// </summary>
    private void SetHealthBar()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0f));
        healthBar.position = healthBarPos;

        uIHealthBar.SetMaxHealth(maxHealth);
        uIHealthBar.SetCurrentHealth(currentHealth);
    }

    /// <summary>
    /// 스킬에 맞은 경우 해당 스킬에 해당하는 정보를 GameSystem의 skillList에서 가져온다.
    /// 이후 해당 스킬에 대한 데미지를 체력에 가한다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Skill")
        {
            for (int i = 0; i < gameSystem.skillList.Count; i++)
            {
                if (gameSystem.skillList[i][1] + " (Clone)" == collision.name)
                {
                    currentHealth -= int.Parse(gameSystem.skillList[i][5]);
                    break;
                }
            }
        }
    }
}
