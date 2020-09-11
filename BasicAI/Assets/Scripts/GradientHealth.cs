using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GradientHealth : MonoBehaviour
{
    // all the variables are only for displaying our health
    //not for logic
    public Image healthBar;
    public float currentHealth;
    public float maxHealth;
    public Gradient gradient;

    // Update is called once per frame
    void Update()
    {
        SetHealth(currentHealth);
    }
    public void SetHealth(float health)
    {
        healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
        healthBar.color = gradient.Evaluate(healthBar.fillAmount);
    }
}






