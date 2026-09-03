using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    public GameObject HitEffect;

    public GameObject gameOver,Ui;
    float sX=1,sY=1,sZ=1;

    public Color []bloodColor;

    public Image image;

    public TMP_Text killsTxt,HighTxt,mainKillsTxt;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount) {
        gameManager manager = GameObject.Find("UI").GetComponent<gameManager>();
        if(manager.isAntidoteOn==false)
        {
            currentHealth -= amount;
            if (currentHealth <= 0.0f) {
                Die();
            }else if(currentHealth > 75)
            {
                image.color=bloodColor[0];
            }else if(currentHealth > 50)
            {
                image.color=bloodColor[1];
            }
            else if(currentHealth > 25)
            {
                image.color=bloodColor[2];
            }
            else if(currentHealth>1)
            {
                image.color=bloodColor[3];
            }

        }
    }

    private void Die()
    {
        Ui.SetActive(false);
        gameOver.SetActive(true);

        Time.timeScale = 0; //pauses the game

        //
        gameManager manager = GameObject.Find("UI").GetComponent<gameManager>();
        string high="";
        if(manager.isHighScore==true)
        {
            high="(Highest Kills)";
        }
        killsTxt.text="Kills : "+mainKillsTxt.text+high;
        HighTxt.text="Highest Kills : "+PlayerPrefs.GetInt("high kill",0);
    }

    public void gameO()
    {
        //rewardResume();
       //after watching the ad game will be resumed
       //also add a  three seconds timer to indicate player
       RewardedAdManager manager = GameObject.Find("rewardAd").GetComponent<RewardedAdManager>();
       if(manager.isAdLoaded==true)
       {
        manager.ShowRewardedAd();
       }

    }

    public void rewardResume(){

        // Get all zombies
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("zombie");

        // Destroy all zombies
        foreach (GameObject obj in objectsWithTag)
        {
            if(obj!=null){
                Destroy(obj);
            }
        }

        gameOver.SetActive(false);
        Ui.SetActive(true);
        currentHealth=maxHealth;
        Time.timeScale = 1; //resume scene 
        image.color=bloodColor[4];

        //antidote bonus
        int totalAntidotes= PlayerPrefs.GetInt("antidotes",0);
        ++totalAntidotes;
        PlayerPrefs.SetInt("antidotes",totalAntidotes);

        gameManager manager = GameObject.Find("UI").GetComponent<gameManager>();
        manager.antidotesCountTxt.text=""+totalAntidotes;
        manager.showNotifier("Antidote Bonus for Watching Ad",2.0f,"no");

    }

    private void Update() {
        //play here some take damage animation
    }
}
