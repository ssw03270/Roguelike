using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireBall : MonoBehaviour
{
    private Animator animator;                      // 스킬 애니메이션
    private Rigidbody2D rigidbody2D;                // 스킬의 rigidbody2d 정보
    private Vector2 movePos;                        // 스킬이 날아가야하는 방향

    private int skillCode = 0;                      // 스킬 코드 번호, 화염구 == 0
    private int usedMana;                           // 스킬이 사용하는 마나
    private int skillDamage;                        // 스킬이 가하는 데미지
    private float usedDelay;                        // 스킬이 주는 딜레이
    private float moveSpeed;                        // 스킬이 날라가는 속도

    private PlayerController playerController;      // 플레이어 컨트롤러 클래스
    private GameSystem gameSystem;                  // 게임 시스템 클래스

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        playerController = playerObject.GetComponent<PlayerController>();
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        usedMana = int.Parse(gameSystem.skillList[skillCode][3]);
        usedDelay = float.Parse(gameSystem.skillList[skillCode][4]);
        skillDamage = int.Parse(gameSystem.skillList[skillCode][5]);

        if (playerController.currentMana < usedMana)
        {
            Destroy(this.gameObject);
        }

        movePos = playerController.lastMove;
        moveSpeed = 5f;

        animator.SetFloat("MoveX", movePos.x);
        animator.SetFloat("MoveY", movePos.y);
        
        playerController.currentMana -= usedMana;

        playerController.delaySkill = usedDelay;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2D.velocity = movePos * moveSpeed;
    }

}
