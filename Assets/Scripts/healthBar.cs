using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    private Image Bar;
    public float currentHealth;
    private float MaxHealth;

    PlayerHealth health;

    private void Start()
    {
        Bar =GetComponent<Image>();
        health=GameObject.Find("zombieDestination").GetComponent<PlayerHealth>();
        MaxHealth=health.maxHealth;
    }

    private void Update()
    {
        currentHealth=health.currentHealth;
        Bar.fillAmount=currentHealth/MaxHealth;
    }

}
