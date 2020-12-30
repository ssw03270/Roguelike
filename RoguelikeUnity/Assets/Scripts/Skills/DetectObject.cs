using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 장애물과의 충돌을 체크하기 위한 함수
    /// Object 혹은 Enemy와 충돌한 경우 이 오브젝트를 삭제한다.
    /// </summary>
    /// <param name="collision">충돌한 오브젝트</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Object") || collision.tag.Equals("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
