using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    private Slider slider;

    // Start is called before the first frame update

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void SetMaxHealth(int health)
    {

        slider.maxValue = health;
        slider.value = health;
    }

    // Update is called once per frame
    public void SetCurrentHealth(int health)
    {
        slider.value = health;
    }
}
