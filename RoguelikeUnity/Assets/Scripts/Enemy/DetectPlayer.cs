using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private GameSystem gameSystem;
    private float moveSpeed;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, moveSpeed);
        }
    }
}
