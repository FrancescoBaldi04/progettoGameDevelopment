using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthText; // riferimento al testo

    public float animationSpeed = 5f; // velocità di movimento della barra

    private float targetHealth; // il valore verso cui deve animarsi
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
        if (slider.value != targetHealth) // se la barra non è ancora arrivata a targetHealth si muove verso targetHealth
        {
            slider.value = Mathf.Lerp(slider.value, targetHealth, Time.deltaTime * animationSpeed); // muovo la barra
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