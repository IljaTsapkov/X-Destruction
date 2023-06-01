using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    public static PlayerHealth Instance;
    public float health;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float chipSpeed = 0f;
    public Image frontHealthBar;
    public Image backHealthBar;

    public int Armor;
    public int SuperArmor;


    public Text ArmorText;
    public Text SuperArmorText;

    public float invincibilityDuration = 1f; // The duration of the invincibility in seconds
    private float invincibilityTimer = 0f; // A timer to track how long the player has been invincible
    void Start()
    {
        frontHealthBar = GameObject.Find("FrontHealthBar").GetComponent<Image>();
        backHealthBar = GameObject.Find("BackHealthBar").GetComponent<Image>();
        health = maxHealth;
        Armor = 0;
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(Random.Range(10, 30));
        }

        invincibilityTimer -= Time.deltaTime;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateHealthUI()
    {

        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        if (fillF < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, backHealthBar.fillAmount, percentComplete);
        }
        if (frontHealthBar.fillAmount == hFraction && backHealthBar.fillAmount == hFraction)
        {
            lerpTimer = 0f;
        }
    }



    public void TakeDamage(float damage)
    {
        if (Armor > 0)
        {
            Armor -= (int)damage;
            if (Armor <= 0)
            {
                Armor = 0;
                float remainingDamage = Mathf.Abs(Armor);
                health -= remainingDamage;
            }
        }
        else
        {
            health -= damage;
        }
        UpdateHealthUI();
        UpdateArmorUI(); 
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;
        lerpTimer = 0f;
    }

    public void RestoreArmor(int value)
    {
        Armor += value;
        ArmorText.text = $"Armor:{Armor}";
    }

    public void UpdateArmorUI()
    {
        ArmorText.text = "Armor: " + Armor.ToString();


    }

    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject.tag == "MutantRightHand")
        {
            if (invincibilityTimer <= 0f)
            {
                health -= 15;
                invincibilityTimer = invincibilityDuration;
            }
        }

        if (enemy.gameObject.tag == "MutantLeftHand")
        {
            if (invincibilityTimer <= 0f)
            {
                health -= 25;
                invincibilityTimer = invincibilityDuration;
            }
        }
    }

    public void LoadData(GameData data)
    {
        health = data.playerHealth;
        Armor = data.playerArmor;
        ArmorText.text = $"Armor:{Armor}";
    }

    public void SaveData(ref GameData data)
    {
        data.playerHealth = health;
        data.playerArmor = Armor;
    }

}