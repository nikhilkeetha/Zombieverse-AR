using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class gameManager : MonoBehaviour
{
    // Start is called before the first frame update
    bool isPause=false,isMusic=false;
    public Image img,musImg,UIPanel;
    public Sprite pause,resume,mOn,mOff;
    public GameObject pauseUI,presentUI,modeUi,exitUI;

    public GameObject terrian;
    public AudioClip btnClip;

    public GameObject notifier;

    public TMP_Text notfierTxt,antidotesCountTxt;
    public bool once=true,isAntidoteOn=false,isHighScore=false;
    public int andidoteTime;

    public Color antidoteEffectColor,defaultColor;

    public GameObject antidoteBar; 
    void Start()
    {
        Time.timeScale = 1;

        if(PlayerPrefs.GetString("music","de")=="de")
        {
            musImg.sprite=mOn;
        }
        else{
            isMusic=true;
            musImg.sprite=mOff;
        }
        antidotesCountTxt.text=""+PlayerPrefs.GetInt("antidotes",0);

        setCamera(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onBackPressOrExit();
        }
    }

    public void RestartGame()
    {
        Scene scene= SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void ARmode()
    {
        //playing btn sound effect
        AudioManager audMan = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audMan.PlaySFX(btnClip,1);

        terrian.SetActive(false);
        setCamera(true);
        modeUi.SetActive(false);
        EndlessLevels endless = GameObject.Find("zombieSpawnPoint").GetComponent<EndlessLevels>();
        endless.gameStart(true,2.0f);
    }

    public void VirtualMode()
    {
        //playing btn sound effect
        AudioManager audMan = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audMan.PlaySFX(btnClip,1);

        terrian.SetActive(true);
        setCamera(false);


        GameObject cam = GameObject.Find("AR Camera");
        if (cam != null)
        {
            cam.AddComponent<GyroCameraController>();
        }

        modeUi.SetActive(false);
        EndlessLevels endless = GameObject.Find("zombieSpawnPoint").GetComponent<EndlessLevels>();
        endless.gameStart(false,0.0f);
    }


    private void setCamera(bool en)
    {
        ARSessionOrigin ar = GameObject.Find("AR Session Origin").GetComponent<ARSessionOrigin>();
        ar.enabled=en;
    
        ARCameraManager cameraManager = GameObject.Find("AR Camera").GetComponent<ARCameraManager>();
        cameraManager.enabled=en;

        ARCameraBackground cameraBackground = GameObject.Find("AR Camera").GetComponent<ARCameraBackground>();
        cameraBackground.enabled=en;

        GameObject arSess = GameObject.Find("AR Session");
        arSess.SetActive(en);
    }


    public void PauseScene()
    {
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlaySFX(btnClip,1);
        if(isPause==false)
        {
            Time.timeScale = 0;
            isPause=true;
            pauseUI.SetActive(true);
            presentUI.SetActive(false);
            // img.sprite=resume;
        }else{
            ResumeScene();
        }
    }

    public void ResumeScene()
    {
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlaySFX(btnClip,1);
        Time.timeScale = 1;
        isPause=false;
        pauseUI.SetActive(false);
        presentUI.SetActive(true);
        // img.sprite=pause;
    }

    public void musicBtn()
    {
        AudioManager adm = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        adm.PlaySFX(btnClip,1);
        if(isMusic==false)
        {
            AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            audioManager.PauseMusic();
            isMusic=true;
            PlayerPrefs.SetString("music","unmute");
            musImg.sprite=mOff;
        }else{
            AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            audioManager.PlayMusic();
            isMusic=false;
            PlayerPrefs.SetString("music","de");
            musImg.sprite=mOn;
        }
    }

    public void onBackPressOrExit()
    {
        pauseUI.SetActive(false);
        exitUI.SetActive(true);
    }

    public void exitGame()
    {
        SceneManager.LoadScene("Home");
    }

    public void StayInGame()
    {
        ResumeScene();
        exitUI.SetActive(false);
    }

    public void showNotifier(string message,float time,string type)
    {
        if(type=="high score")
        {
            if(once==true)
            {
                once=false;
                notfierTxt.text=message;
                notifier.SetActive(true);
                isHighScore=true;
                StartCoroutine(timerNotifer(time,type));
            }
        }
        else{
            notfierTxt.text=message;
            notifier.SetActive(true);
            StartCoroutine(timerNotifer(time,"else"));
        }
    }

    IEnumerator timerNotifer(float time,string type)
    {
        yield return new WaitForSeconds(time);
        notifier.SetActive(false);
        if(type=="high score")
        {
            showNotifier("Antidote Bonus",2f,"esle");
            int totalAntidotes= PlayerPrefs.GetInt("antidotes",0);
            ++totalAntidotes;
            PlayerPrefs.SetInt("antidotes",totalAntidotes);
            antidotesCountTxt.text=""+totalAntidotes;
        }
    }


    public void activateAntidoteMode()
    {
        if(isAntidoteOn==false)
        {
            int totalAntidotes=PlayerPrefs.GetInt("antidotes",0);
            if(totalAntidotes>0)
            {
                andidoteTime=200;
                isAntidoteOn=true;
                antidoteBar.SetActive(true);
                totalAntidotes--;
                antidotesCountTxt.text=""+totalAntidotes;
                PlayerPrefs.SetInt("antidotes",totalAntidotes);
                UIPanel.color=antidoteEffectColor;
            
                notfierTxt.text="Anitdote Mode";
                notifier.SetActive(true);
                StartCoroutine(antidoteTimer(0.1f));
            }
        }
    }

    IEnumerator antidoteTimer(float time)
    {
        while(true)
        {
            if(andidoteTime>0)
            {
                andidoteTime--;
                //AntidoteBar bar = GameObject.Find("Fill").GetComponent<AntidoteBar>();
                //bar.Update();
            }else if(andidoteTime==0){
                isAntidoteOn=false;
                UIPanel.color=defaultColor;
                antidoteBar.SetActive(false);
                notifier.SetActive(false);
                break;
            }
            yield return new WaitForSeconds(time);
        }
    }


}
