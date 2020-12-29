using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    private int currentHealth = 10;
    private int currentMana;
    private int maxHealth = 10;
    private int maxMana;

    private UIHealthBar uIHealthBar;
    public GameObject objHealthBar;
    public GameObject canvas;
    RectTransform healthBar;
    private float height = 0.5f;

    private SkillFireBall skillFireBall;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = Instantiate(objHealthBar, canvas.transform).GetComponent<RectTransform>();
        uIHealthBar = healthBar.gameObject.GetComponent<UIHealthBar>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        SetHealthBar();
    }

    private void CheckState()
    {
        if(currentHealth <= 0)
        {
            Destroy(healthBar.gameObject);
            Destroy(this.gameObject);
        }
    }
    private void SetHealthBar()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + height, 0f));
        healthBar.position = healthBarPos;

        uIHealthBar.SetMaxHealth(maxHealth);
        uIHealthBar.SetCurrentHealth(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Skill")
        {
            skillFireBall = collision.GetComponent<SkillFireBall>();
            currentHealth -= skillFireBall.skillDamage;
        }
    }
}
