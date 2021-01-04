using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeSpawner : MonoBehaviour
{
    public GameObject bossSlime_01;
    public GameObject bossSlime_02;
    public GameObject bossSlime_03;

    private BossSlimeController bossSlimeController;
    private PlayerController playerController;      // 플레이어 컨트롤러 클래스


    private Vector3 targetNotDetect;                    // 플레이어 탐색이 안되었을 때, 랜덤하게 설정된 목표점
    private float moveSpeed;                            // 적 이동 속도
    private bool isDetectWall;
    private GameSystem gameSystem;                      // 게임 시스템 클래스

    private void Start()
    {
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        bossSlimeController = GameObject.Find("BossSlime").GetComponent<BossSlimeController>();

        for (int i = 0; i < gameSystem.enemyList.Count; i++)
        {
            if (gameSystem.enemyList[i][1].Equals("BossSlime"))
            {
                moveSpeed = float.Parse(gameSystem.enemyList[i][5]);
                break;
            }
        }

    }

    private void Update()
    {
        Move();
        Spawn();
    }

    private void Spawn()
    {
        float bossCurrentHealth = bossSlimeController.currentHealth;
        float bossMaxHealth = bossSlimeController.maxHealth;
        if ((transform.gameObject.name.Equals("BossSlime_01") || transform.gameObject.name.Equals("BossSlime_01(Clone)")) && bossCurrentHealth / bossMaxHealth <= 0.5)
        {
            Instantiate(bossSlime_02, transform.position, transform.rotation).transform.parent = transform.parent;
            Instantiate(bossSlime_02, transform.position, transform.rotation).transform.parent = transform.parent;
            Destroy(this.gameObject);
        }
        if ((transform.gameObject.name.Equals("BossSlime_02") || transform.gameObject.name.Equals("BossSlime_02(Clone)")) && bossCurrentHealth / bossMaxHealth <= 0.25)
        {
            Instantiate(bossSlime_03, transform.position, transform.rotation).transform.parent = transform.parent;
            Instantiate(bossSlime_03, transform.position, transform.rotation).transform.parent = transform.parent;
            Destroy(this.gameObject);
        }
        if ((transform.gameObject.name.Equals("BossSlime_03") || transform.gameObject.name.Equals("BossSlime_03(Clone)")) && bossCurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    private void Move()
    {

        if (transform.position == targetNotDetect)
        {
            targetNotDetect = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetNotDetect, moveSpeed * Time.deltaTime);

        if (isDetectWall)
        {
            targetNotDetect = new Vector3(0, 0, 0);
            isDetectWall = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Wall"))
        {
            isDetectWall = true;
        }

        if (collision.tag.Equals("Skill"))
        {
            for (int i = 0; i < gameSystem.skillList.Count; i++)
            {
                if ((gameSystem.skillList[i][1] + "(Clone)").Equals(collision.name) && gameSystem.skillList[i][2].Equals("Attack"))
                {
                    Debug.Log("a");
                    int skillCode = int.Parse(gameSystem.skillList[i][0]);
                    bossSlimeController.currentHealth -= int.Parse(gameSystem.skillList[i][5]) * playerController.skillCondition[skillCode];
                    break;
                }
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))         // 플레이어와 충돌한 경우
        {
            if (!playerController.isInvincible)     // 플레이어가 무적 상태가 아닌 경우 데미지 가함
            {
                playerController.currentHealth -= bossSlimeController.damage;
                playerController.invincibleTime = 1f;
                playerController.isInvincible = true;
            }
        }
    }
}
