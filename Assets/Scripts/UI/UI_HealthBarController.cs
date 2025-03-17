using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBarController : MonoBehaviour
{
    private Entity entity;
    private StatsController statsCtrl;
    private RectTransform rectTransform;
    private Slider slider;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        statsCtrl = GetComponentInParent<StatsController>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponent<Slider>();

        // Add subscription to the onFlipped event
        entity.onFlipped += Flip;

        // Setup initialized value for the slider
        UpdateHealthBarSlider();

        // Add subscription to update the health bar ui
        statsCtrl.onHealthChanged += UpdateHealthBarSlider;
    }


    private void OnDisable()
    {
        entity.onFlipped -= Flip;
        statsCtrl.onHealthChanged -= UpdateHealthBarSlider;
    }

    // Flip is called right after the entity is flipped to correct the rotation value
    private void Flip()
    {
        rectTransform.Rotate(new Vector3(0, 180, 0));
    }

    // UpdateHealthBarUI is called right after the health value of the entity is changed
    private void UpdateHealthBarSlider()
    {
        slider.maxValue = statsCtrl.GetTotalMaxHealth();
        slider.value = statsCtrl.GetCurrentHealth();
    }
}
