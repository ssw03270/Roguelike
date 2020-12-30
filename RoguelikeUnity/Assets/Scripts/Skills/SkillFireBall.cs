using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFireBall : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private Vector2 movePos;

    private int skillCode = 0;
    public int usedMana;
    public int skillDamage;
    private float usedDelay;
    private float moveSpeed;

    private PlayerController playerController;
    private UIManaBar uIManaBar;
    private GameSystem gameSystem;

    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        playerController = playerObject.GetComponent<PlayerController>();
        GameObject uiManaBarObject = GameObject.Find("ManaBar");
        uIManaBar = uiManaBarObject.GetComponent<UIManaBar>();
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        movePos = playerController.lastMove;
        moveSpeed = 5f;

        animator.SetFloat("MoveX", movePos.x);
        animator.SetFloat("MoveY", movePos.y);
        
        playerController.currentMana -= usedMana;
        uIManaBar.SetCurrentMana(playerController.currentMana);

        usedDelay = float.Parse(gameSystem.skillList[skillCode][4]);
        playerController.delaySkill = usedDelay;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2D.velocity = movePos * moveSpeed;
    }

}
