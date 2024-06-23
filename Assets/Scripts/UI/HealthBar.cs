using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private float MAX_HEALTH = 300f;

    private float health = 300f;
    public Image healthBar;

    public float GetHealth() => health;
    public float GetMaxHealth() => MAX_HEALTH;

    public void Damage(float damage)
    {
        // Validar entrada
        if (damage < 0)
            return;

        health -= damage;
        health = Mathf.Clamp(health, 0, MAX_HEALTH);
        UpdateHealthBar();
    }

    public void IncrementMaxHealth(float extraMaxHealth)
    {
        this.MAX_HEALTH += extraMaxHealth;
        health = Mathf.Clamp(health, 0, MAX_HEALTH); // Opcional: Ajuste a saúde atual para manter a proporção
        UpdateHealthBar();
    }

    public void Heal(float percentage)
    {
        // Converter o valor da porcentagem para a forma decimal
        float decimalPercentage = percentage / 100f;

        // Validar entrada
        if (decimalPercentage < 0 || decimalPercentage > 1)
            return;

        float healAmount = MAX_HEALTH * decimalPercentage;
        health += healAmount;
        health = Mathf.Clamp(health, 0, MAX_HEALTH);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = health / MAX_HEALTH;
    }
}
