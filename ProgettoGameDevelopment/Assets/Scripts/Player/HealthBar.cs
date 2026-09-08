using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthText; 

    public float animationSpeed = 5f; // Bar movement speed

    private float targetHealth; 
    private float maxHealth; 

    public void SetMaxHealth(float health)
    { 
        slider.maxValue = health;
        slider.value = health;
        
        targetHealth = health;
        maxHealth = health;
        
        UpdateText(health);
    }

    public void SetHealth(float health)
    {
        targetHealth = health; 
        UpdateText(health);
    }

    void Update()
    {
        if (slider.value != targetHealth) // If the bar has not reached targetHealth yet, move it towards targetHealth
        {
            slider.value = Mathf.Lerp(slider.value, targetHealth, Time.deltaTime * animationSpeed); // Bar movement
        }
    }

    private void UpdateText(float currentHealth)
    {
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }
}