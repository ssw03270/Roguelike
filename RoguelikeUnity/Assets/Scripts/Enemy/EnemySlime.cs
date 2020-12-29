using System.Collections;
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
    private UIManaBar uIManaBar;
    public GameObject objHealthBar;
    public GameObject objManaBar;
    private GameObject canvas;

    RectTransform healthBar;
    RectTransform manaBar;

    private float heightHealth = 0.65f;
    private float heightMana = 0.5f;

    private GameSystem gameSystem;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        SetState();

        healthBar = Instantiate(objHealthBar, canvas.transform).GetComponent<RectTransform>();
        manaBar = Instantiate(objManaBar, canvas.transform).GetComponent<RectTransform>();
        uIHealthBar = healthBar.gameObject.GetComponent<UIHealthBar>();
        uIManaBar = manaBar.gameObject.GetComponent<UIManaBar>();

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
            Destroy(manaBar.gameObject);
            Destroy(this.gameObject);
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.name == "Player")
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
