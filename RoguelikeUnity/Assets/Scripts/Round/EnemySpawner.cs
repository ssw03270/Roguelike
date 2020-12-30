using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyList = new List<GameObject>();     // 적 오브젝트 저장되어 있는 리스트

    /// <summary>
    /// 적 소환 후, 해당 스포너는 삭제
    /// </summary>
    void Start()
    {
        Instantiate(enemyList[Random.Range(0,enemyList.Count)], transform.position, transform.rotation).transform.parent = transform.parent;
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
