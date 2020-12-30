using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    private string enemyName = "Slime";             // 적 이름

    private int currentHealth;                      // 적 현재 체력
    private int currentMana;                        // 적 현재 마나
    private int maxHealth;                          // 적 최대 체력
    private int maxMana;                            // 적 최대 마나
    private int damage;                             // 적이 가진 공격력

    private UIHealthBar uIHealthBar;                // 적의 체력바 UI
    private UIManaBar uIManaBar;                    // 적의 마나바 UI
    public GameObject objHealthBar;                 // 체력바 오브젝트
    public GameObject objManaBar;                   // 마나바 오브젝트
    private GameObject canvas;                      // 캔버스

    RectTransform healthBar;                        // 캔버스에서의 체력바
    RectTransform manaBar;                          // 캔버스에서의 마나바

    private float heightHealth = 0.65f;             // 체력바 배치 위치
    private float heightMana = 0.5f;                // 마나바 배치 위치

    private GameSystem gameSystem;                  // 게임 시스템 클래스
    private PlayerController playerController;      // 플레이어 컨트롤러 클래스

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        healthBar = Instantiate(objHealthBar, canvas.transform).GetComponent<RectTransform>();
        manaBar = Instantiate(objManaBar, canvas.transform).GetComponent<RectTransform>();
        uIHealthBar = healthBar.GetComponent<UIHealthBar>();
        uIManaBar = manaBar.GetComponent<UIManaBar>();

        SetState();

    }

    // Update is called once per frame
    void Update()
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
            Destroy(this.transform.parent.gameObject);
        }
    }

    /// <summary>
    /// 적의 체력바와 마나바를 설정하는 함수
    /// 체력바와 마나바의 위치 및 자원 정보를 바탕으로 값을 설정한다.
    /// </summary>
    private void SetResourceBar()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + heightHealth, 0f));
        Vector3 manaBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + heightMana, 0f));
        healthBar.position = healthBarPos;
        manaBar.position = manaBarPos;

        uIHealthBar.SetMaxHealth(maxHealth);
        uIManaBar.SetMaxMana(maxMana);
        uIHealthBar.SetCurrentHealth(currentHealth);
        uIManaBar.SetCurrentMana(currentMana);
    }

    /// <summary>
    /// 스킬에 맞은 경우 해당 스킬에 해당하는 정보를 GameSystem의 skillList에서 가져온다.
    /// 이후 해당 스킬에 대한 데미지를 체력에 가한다.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Skill"))
        {
            for (int i = 0; i < gameSystem.skillList.Count; i++)
            {
                if ((gameSystem.skillList[i][1] + " (Clone)").Equals(collision.name))
                {
                    currentHealth -= int.Parse(gameSystem.skillList[i][5]);
                    break;
                }
            }
        }
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
            Destroy(manaBar.gameObject);
        }
        catch
        {
            Debug.Log("Error while destoy the enemy resource bar.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.name.Equals("Player"))
        {
            if (!playerController.isInvincible)
            {
                playerController.currentHealth -= damage;
                playerController.invincibleTime = 1f;
                playerController.isInvincible = true;
            }
        }
    }
}
