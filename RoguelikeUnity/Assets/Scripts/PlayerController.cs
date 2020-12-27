using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    private float moveSpeed;
    private bool isPlayerMove;
    private bool isPlayerAttack;
    private Vector2 lastMove;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        isPlayerMove = false;
        isPlayerAttack = false;
        if (Input.GetAxisRaw("Horizontal") != 0f) 
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            isPlayerMove = true;
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        if (Input.GetAxisRaw("Vertical") != 0f)
        {
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            isPlayerMove = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }
        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        animator.SetBool("Moving", isPlayerMove);
        animator.SetBool("Attacking", isPlayerAttack);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
    }
}
