using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int itemCode;                                // 해당 스펠의 스킬 코드
    private PlayerController playerController;          // 플레이어 컨트롤 클래스  
    private GameSystem gameSystem;                      // 게임 시스템 클래스

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            switch (gameSystem.itemList[itemCode][2])
            {
                case "spell":
                    bool codeChecker = false;
                    int skillCode = int.Parse(gameSystem.itemList[itemCode][3]);

                    foreach (int code in playerController.skillCondition.Keys)
                    {
                        if (code == skillCode)
                        {
                            codeChecker = true;
                        }
                    }

                    if (!codeChecker)
                    {
                        playerController.skillCondition.Add(skillCode, 1);
                        playerController.skillConditionChecker.Add(skillCode);
                    }
                    else
                    {
                        playerController.skillCondition[skillCode] += 1;
                    }
                    break;
                case "health":
                    playerController.healthPotionCount += Random.Range(1, 3);
                    break;
                case "mana":
                    playerController.manaPotionCount += Random.Range(1, 3);
                    break;
                case "essence":
                    if (playerController.stageNum == 5 || playerController.stageNum == 15)
                    {
                        playerController.essenceCount += Random.Range(1, 10);
                    }
                    else
                    {
                        playerController.essenceCount += Random.Range(1, 3);
                    }
                    break;
            }
            Destroy(this.gameObject);

        }
    }
}
