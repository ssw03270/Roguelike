﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeController : MonoBehaviour
{
    public string enemyName;                        // 적 이름

    public int currentHealth;                      // 적 현재 체력
    public int maxHealth;                          // 적 최대 체력
    public int damage;                             // 적이 가진 공격력

    private UIHealthBar uIHealthBar;                // 적의 체력바 UI
    public GameObject objHealthBar;                 // 체력바 오브젝트
    private GameObject canvas;                      // 캔버스

    RectTransform healthBar;                        // 캔버스에서의 체력바

    private GameSystem gameSystem;                  // 게임 시스템 클래스
    private PlayerController playerController;      // 플레이어 컨트롤러 클래스

    public List<GameObject> itemList;               // 드랍할 아이템 리스트
    public List<float> itemProbability;             // 아이템이 드랍될 확률

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        healthBar = Instantiate(objHealthBar, canvas.transform).GetComponent<RectTransform>();
        uIHealthBar = healthBar.gameObject.GetComponent<UIHealthBar>();

        SetState();
    }

    private void Update()
    {
        CheckState();
        SetResourceBar();
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
            if (gameSystem.enemyList[i][1].Equals(enemyName))
            {
                maxHealth = int.Parse(gameSystem.enemyList[i][2]);
                currentHealth = maxHealth;
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
        if (currentHealth <= 0)
        {
            DropItem();
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 아이템 드롭과 관련된 함수
    /// 해당 아이템과 그 아이템이 떨어질 확률 등을 통해 아이템을 드랍한다.
    /// </summary>
    private void DropItem()
    {
        int itemCode = Random.Range(0, gameSystem.itemList.Count - 1);
        GameObject randomItem = Resources.Load<GameObject>("Prefabs/Item/" + gameSystem.itemList[itemCode][1]);
        Instantiate(randomItem, transform.position + new Vector3(0f, 1f, 0f), transform.rotation).transform.parent = transform.parent;
    }
    /// <summary>
    /// 적의 체력바와 마나바를 설정하는 함수
    /// 체력바와 마나바의 위치 및 자원 정보를 바탕으로 값을 설정한다.
    /// </summary>
    private void SetResourceBar()
    {
        uIHealthBar.SetMaxHealth(maxHealth);
        uIHealthBar.SetCurrentHealth(currentHealth);
    }

    /// <summary>
    /// 적이 삭제 (사망) 되었을 때 실행되는 함수
    /// 적이 가진 체력바와 마나바를 삭제한다.
    /// </summary>
    private void OnDestroy()
    {
        try
        {
            Destroy(healthBar.gameObject);
        }
        catch
        {
            Debug.Log("Error while destoy the enemy resource bar.");
        }
    }
}
