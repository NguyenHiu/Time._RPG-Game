using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class SkillCooldown
{
    public Image img;
    public Skill skill;
    public float timer;
    public float cooldown;

    public SkillCooldown(Image img, Skill skill)
    {
        this.img = img;
        this.skill = skill;
        this.timer = 0;
        this.cooldown = 0;
    }
}

public class InGame_UI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider healthBarSlider;

    [Header("Skills")]
    [SerializeField] private Image dashImg;
    [SerializeField] private Image parryImg;
    [SerializeField] private Image crystalImg;
    [SerializeField] private Image throwSwordImg;
    [SerializeField] private Image blackHoleImg;
    [SerializeField] private Image flaskImg;
    private readonly List<SkillCooldown> skillCooldowns = new();

    private void Start()
    {
        playerStats.onHealthChanged += UpdateHealthBarSlider;
        UpdateHealthBarSlider();

        SkillManager sm = SkillManager.instance;
        skillCooldowns.Add(new(dashImg, sm.dashSkill));
        skillCooldowns.Add(new(parryImg, sm.parrySkill));
        skillCooldowns.Add(new(crystalImg, sm.crystalSkill));
        skillCooldowns.Add(new(throwSwordImg, sm.throwSwordSkill));
        skillCooldowns.Add(new(blackHoleImg, sm.blackholeSkill));

        foreach (SkillCooldown skillCD in skillCooldowns)
        {
            skillCD.skill.OnCooldownUpdated += (cooldown) =>
            {
                skillCD.cooldown = cooldown;
                skillCD.timer = cooldown;
                skillCD.img.fillAmount = 1;
            };
        }

        SkillCooldown flaskSkillCooldown = new(flaskImg, null);
        Inventory.instance.OnFlaskCooldownUpdated += (cooldown) =>
        {
            flaskSkillCooldown.cooldown = cooldown;
            flaskSkillCooldown.timer = cooldown;
            flaskSkillCooldown.img.fillAmount = 1;
        };
        skillCooldowns.Add(flaskSkillCooldown);
    }

    private void Update()
    {
        foreach (SkillCooldown skillCD in skillCooldowns)
        {
            skillCD.timer -= Time.deltaTime;
            UpdateSkill(skillCD.img, skillCD.timer, skillCD.cooldown);
        }
    }

    private void UpdateSkill(Image _img, float _timer, float _cooldown)
    {
        if (_timer < 0)
            _img.fillAmount = 0;
        else
            _img.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

    private void UpdateHealthBarSlider()
    {
        healthBarSlider.maxValue = playerStats.GetTotalMaxHealth();
        healthBarSlider.value = playerStats.GetCurrentHealth();
    }
}
