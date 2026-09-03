using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class ZombieHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    SkinnedMeshRenderer[] skinnedMeshRenderer;
    public float blinkIntensity;
    public float blinkDuration;
    float blinkTimer;

    Ragdoll ragdoll;

    public Animator animator;

    public TMP_Text score,highScore;

    int kills;

    GameObject zombie;
    movePath mp;
    bool once=true;

    // Start is called before the first frame update
    void Start()
    {
        mp=GetComponent<movePath>();
        ragdoll = GetComponent<Ragdoll>();
        skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        currentHealth = maxHealth;

        var rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach(var rigidbody in rigidbodies) {
            HitBox hitBox = rigidbody.gameObject.AddComponent<HitBox>();
            hitBox.health=this;
        }

        score = GameObject.Find("currentKills").GetComponent<TMP_Text>();
        highScore = GameObject.Find("top kiils").GetComponent<TMP_Text>();
        
        highScore.text="Highest Kills : "+PlayerPrefs.GetInt("high kill",0).ToString();
    }

    public void TakeDamage(float amount, Vector3 direction) {
        currentHealth -= amount;

        //playing hit reaction animation
        animator.SetBool("hit",true); 
        StartCoroutine(hitReaction());

        if (currentHealth == 0.0f) {
            Die(direction);
        }
        blinkTimer = blinkDuration;

    }

    IEnumerator hitReaction()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("hit",false);
    }

    private void Die(Vector3 direction)
    {
      animator.SetBool("die",true);
      //on zombie dead
      Destroy(gameObject,2.5f);

      mp.isZombieDead=true;
      kills=int.Parse(score.text);
      kills++;
      score.text=kills.ToString();

      if(kills > PlayerPrefs.GetInt("high kill",0))
      {
        PlayerPrefs.SetInt("high kill",kills);
        highScore.text="Highest Kills : "+PlayerPrefs.GetInt("high kill",0).ToString();

        //showing 
        gameManager noti=GameObject.Find("UI").GetComponent<gameManager>();
        noti.showNotifier("New High Kills",2.0f,"high score");

      }

      //total zombie kills
      int totalKills=PlayerPrefs.GetInt("total zombies killed",0);
      PlayerPrefs.SetInt("total zombies killed",++totalKills);
    }

    private void Update() {
        /*
        foreach(var skinMesh in skinnedMeshRenderer) {
            blinkTimer -= Time.deltaTime;
            float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
            float intensity = (lerp * blinkIntensity) + 1.0f;
            skinMesh.material.color = Color.white * intensity;
        }
        */
    
    }
}
