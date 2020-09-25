using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GradientHealth : MonoBehaviour
{
    
    public Image healthBar;
    public float currentHealth;
    public float maxHealth;
    public Gradient gradient;
    public GameObject healthOf;
    public AI.StatePointAI pointAI;

    private void IRLHealth()
    {
        if (healthOf.TryGetComponent<Player.PlayerController>(out Player.PlayerController player))
        {
            currentHealth = player.health;
            maxHealth = player.maxHealth;
        }
        else if (healthOf.TryGetComponent<AI.StatePointAI>(out AI.StatePointAI ai))
        {
            currentHealth = ai.health;
            maxHealth = ai.maxHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        IRLHealth();
        pointAI.DeathCheck();
        SetHealth(currentHealth);
    }
    public void SetHealth(float health)
    {
        healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
        healthBar.color = gradient.Evaluate(healthBar.fillAmount);
    }
}






