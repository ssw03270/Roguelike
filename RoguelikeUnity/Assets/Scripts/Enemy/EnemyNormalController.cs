using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormalController : MonoBehaviour
{
    public string enemyName;                        // 적 이름

    private int currentHealth;                      // 적 현재 체력
    private int maxHealth;                          // 적 최대 체력
    private int damage;                             // 적이 가진 공격력

    private UIHealthBar uIHealthBar;                // 적의 체력바 UI
    public GameObject objHealthBar;                 // 체력바 오브젝트
    private GameObject canvas;                      // 캔버스

    RectTransform healthBar;                        // 캔버스에서의 체력바

    private float heightHealth = 0.5f;             // 체력바 배치 위치

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
            Destroy(this.transform.parent.gameObject);
        }
    }

    /// <summary>
    /// 아이템 드롭과 관련된 함수
    /// 해당 아이템과 그 아이템이 떨어질 확률 등을 통해 아이템을 드랍한다.
    /// </summary>
    private void DropItem()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            float probability = Random.Range(0f, 1f);
            if (probability < itemProbability[i])
            {
                Instantiate(itemList[i], transform.position, transform.rotation).transform.parent = transform.parent.parent;
            }
        }
    }
    /// <summary>
    /// 적의 체력바와 마나바를 설정하는 함수
    /// 체력바와 마나바의 위치 및 자원 정보를 바탕으로 값을 설정한다.
    /// </summary>
    private void SetResourceBar()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + heightHealth, 0f));
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
        if (collision.tag.Equals("Skill"))
        {
            for (int i = 0; i < gameSystem.skillList.Count; i++)
            {
                if ((gameSystem.skillList[i][1] + "(Clone)").Equals(collision.name))
                {
                    int skillCode = int.Parse(gameSystem.skillList[i][0]);
                    currentHealth -= int.Parse(gameSystem.skillList[i][5]) * playerController.skillCondition[skillCode];
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
        }
        catch
        {
            Debug.Log("Error while destoy the enemy resource bar.");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))         // 플레이어와 충돌한 경우
        {
            if (!playerController.isInvincible)     // 플레이어가 무적 상태가 아닌 경우 데미지 가함
            {
                playerController.currentHealth -= damage;
                playerController.invincibleTime = 1f;
                playerController.isInvincible = true;
            }
        }
    }
}
