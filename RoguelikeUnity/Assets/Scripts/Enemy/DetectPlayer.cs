using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private GameSystem gameSystem;                      // 게임 시스템 클래스
    private float moveSpeed;                            // 적 이동 속도
    private bool isEnter;                               // 플레이어와 충돌 여부 확인 변수
    private float timer;                                // 랜덤 이동 초기화 시간
    private Vector3 targetNotDetect;                    // 플레이어 탐색이 안되었을 때, 랜덤하게 설정된 목표점

    // Start is called before the first frame update
    void Start()
    {
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();
        string enemyName = transform.name;
        for(int i = 0; i < gameSystem.enemyList.Count; i++)
        {
            if ((gameSystem.enemyList[i][1] + "(Clone)").Equals(enemyName))
            {
                moveSpeed = float.Parse(gameSystem.enemyList[i][5]);
                break;
            }
        }
        isEnter = false;
        targetNotDetect = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0) + transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /// <summary>
    /// 적 이동 함수
    /// 플레이어와 충돌하지 않은 경우, 랜덤하게 생성된 좌표로 이동한다.
    /// 단, 맵 밖으로는 나가지 않는다.
    /// </summary>
    private void Move()
    {
        timer += Time.deltaTime;
        if (!isEnter)
        {
            if (Mathf.RoundToInt(timer) % 2 == 0)
            {
                targetNotDetect = new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0) + transform.position;
                timer += 0.5f;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetNotDetect, moveSpeed);
        }
        if(targetNotDetect.x <= -6.5f || targetNotDetect.x >= 7.5f || targetNotDetect.y >= 2.5f || targetNotDetect.y <= -3.5)
        {
            targetNotDetect = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))            // 플레이어를 감지한 경우, 쫒아간다.
        {
            isEnter = true;
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, moveSpeed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            isEnter = false;
        }
    }
}
