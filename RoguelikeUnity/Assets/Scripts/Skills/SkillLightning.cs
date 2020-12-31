using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLightning : MonoBehaviour
{
    private int skillCode = 1;                      // 스킬 코드 번호, 번개작렬 == 1
    private int usedMana;                           // 스킬이 사용하는 마나
    private int skillDamage;                        // 스킬이 가하는 데미지
    private float usedDelay;                        // 스킬이 주는 딜레이

    private PlayerController playerController;      // 플레이어 컨트롤러 클래스
    private GameSystem gameSystem;                  // 게임 시스템 클래스

    private float playTime;                         // 스킬이 플레이되는 시간
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        playerController = playerObject.GetComponent<PlayerController>();
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        usedMana = int.Parse(gameSystem.skillList[skillCode][3]);
        usedDelay = float.Parse(gameSystem.skillList[skillCode][4]);
        skillDamage = int.Parse(gameSystem.skillList[skillCode][5]);

        if (playerController.currentMana < usedMana)
        {
            Destroy(this.gameObject);
        }

        playerController.currentMana -= usedMana;
        playerController.delaySkill = usedDelay;
    }

    // Update is called once per frame
    void Update()
    {
        playTime += Time.deltaTime;
        if(playTime >= 0.3f)
        {
            Destroy(this.gameObject);
        }
    }
}
