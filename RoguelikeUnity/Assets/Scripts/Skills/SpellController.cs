using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public int skillCode;                               // 해당 스펠의 스킬 코드
    private PlayerController playerController;          // 플레이어 컨트롤 클래스
    // private SpriteRenderer spellImage;                           // 스펠 이미지   
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        // spellImage = GetComponent<SpriteRenderer>();
        // spellImage.sprite = Resources.Load<Sprite>("Sprites/Spells/" + skillCode.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            bool codeChecker = false;
            foreach(int code in playerController.skillCondition)
            {
                if(code == skillCode)
                {
                    codeChecker = true;
                }
            }
            if (!codeChecker)
            {
                playerController.skillCondition.Add(skillCode);
                Destroy(this.gameObject);
            }
        }
    }
}
