using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManaBar : MonoBehaviour
{
    private Slider slider;

    // Start is called before the first frame update

    private void Start()
    {
        slider = GetComponent<Slider>();
    }
    public void SetMaxMana(int Mana)
    {

        slider.maxValue = Mana;
        slider.value = Mana;
    }

    // Update is called once per frame
    public void SetCurrentMana(int Mana)
    {
        slider.value = Mana;
    }
}
