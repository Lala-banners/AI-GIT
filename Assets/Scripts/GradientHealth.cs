using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("AI/Gradient Health")]

public class GradientHealth : MonoBehaviour
{
    #region Variables
    [Header("Health")]
    public Image healthBar;
    public float currentHealth;
    public float maxHealth;
    public Gradient gradient;
    public GameObject healthOf;
    public AI.StatePointAI pointAI;
    
    #endregion

    //Function that allows Player and AI to impact each others health
    private void IRLHealth()
    {
        //Player health reference to PlayerController script to connect variables in GradientHealth to PlayerController script without having to write in Player and AI scripts
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
        SetHealth(currentHealth);
    }
    public void SetHealth(float health)
    {
        healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
        healthBar.color = gradient.Evaluate(healthBar.fillAmount);
    }
}






