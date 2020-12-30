using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private GameSystem gameSystem;
    private float moveSpeed;
    private bool isEnter;
    private float timer;
    private Vector3 targetNotDetect;

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

    private void Move()
    {
        timer += Time.deltaTime;
        if (!isEnter)
        {
            if (Mathf.RoundToInt(timer) % 2 == 0)
            {
                targetNotDetect = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0) + transform.position;
                timer += 0.5f;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetNotDetect, moveSpeed);
        }
        if(targetNotDetect.x <= -6.5f || targetNotDetect.x >= 7.5f || targetNotDetect.y >= 2.5f || targetNotDetect.y <= -3.5)
        {
            targetNotDetect = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0) + transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
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
