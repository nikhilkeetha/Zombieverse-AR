using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using TMPro;
using System;
public class HomeNav : MonoBehaviour
{

    public TMP_Text totalTxt,highTxt,antidotesTxt;
    public AudioClip btnClick;
    Banner bannerAd;
    string message="Hey, I found this awesome AR Zombie Game. Click on the link to install. https://play.google.com/store/apps/details?id=com.project.X", subject="Game Invite";

    public GameObject dailyBonusUI;

    public AudioClip claimClip,nopeClip;


    // Start is called before the first frame update
    void Start()
    {
        checkDailyLogin();
        totalTxt.text="Total Zombies Kills : "+PlayerPrefs.GetInt("total zombies killed",0);
        highTxt.text="Highest Kills at once : "+PlayerPrefs.GetInt("high kill",0);
        RequestCamera();

        //request banner ad
        bannerAd=GameObject.Find("ad").GetComponent<Banner>();
        bannerAd.RequestBanner();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        antidotesTxt.text=""+PlayerPrefs.GetInt("antidotes",0);
    }

    public void checkDailyLogin()
    {
        DateTime currentDate = DateTime.Now;
        String date = currentDate.ToString("yyyy-MM-dd");

        if(PlayerPrefs.GetString("last logged in date","first time")=="first time")
        {
            PlayerPrefs.SetString("last logged in date",date);
            dailyBonusUI.SetActive(true);
        }
        else if(PlayerPrefs.GetString("last logged in date","first time")!=date)
        {
            //Player Earned a daily login
            Debug.Log("Won a reward");
            dailyBonusUI.SetActive(true);
        }
    }

    public void claim()
    {
        sfx(claimClip);
        DateTime currentDate = DateTime.Now;
        String date = currentDate.ToString("yyyy-MM-dd");
        PlayerPrefs.SetString("last logged in date",date); 

        //here add the claimed antidote
        dailyBonusUI.SetActive(false);

        int antidotes = PlayerPrefs.GetInt("antidotes",0);  //remember to change default value to 0
        PlayerPrefs.SetInt("antidotes",++antidotes);
    }

    public void noThanks()
    {
        sfx(nopeClip);
        dailyBonusUI.SetActive(false);
    }


    public void RequestCamera()
    {
        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
            return; // Exit the method or handle the permission request callback
        }
        #endif
    }

    private void sfx(AudioClip clip)
    {
        AudioSource musicSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        musicSource.clip=clip;
        musicSource.Play();
    }

    public void startGame()
    {
        //playing btn click sound
        AudioSource musicSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
        musicSource.clip=btnClick;
        musicSource.Play();

        SceneManager.LoadScene("ARMain");
    }

    public void rateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.project.X");
    }

    public void moreGames()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=5819743952750979765");
    }

    public void privacy()
    {
        Application.OpenURL("https://sites.google.com/view/projectx-privacy-policy/home");
    }

    public void contact()
    {
        Application.OpenURL("https://www.youtube.com/@Nikhil-Gaming-Production/about");
    }
    

    public void ShareGame()
    {
        // Create an intent to share the game content
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        // Set the sharing action
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

        // Set the content type
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");

        // Set the game message
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

        // Set the subject (optional)
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);

        // Get the current activity
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        // Start the share activity
        AndroidJavaObject chooserIntent = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share via");
        unityActivityObject.Call("startActivity", chooserIntent);
    }
}
