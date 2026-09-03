using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndlessLevels : MonoBehaviour
{
    public GameObject zombiePrefab;
    float spawnInterval = 5f;

    public Transform obj;

    public float minX;
    public float maxX;

    public float minZ,maxZ;
    float angle=180;

    public GameObject[] prefabs;

    public GameObject[] Guns;

    RaycastWeapon raycastWe;

    int totalZombies;
    string level="p";
    int levelInt;

    bool easy=true,mid=true,hard=true;

    public Image gunImg;

    public Sprite pistol,akm,mg;

    public TMP_Text gunNm,warnTimer;

    public GameObject Ui,wUi,reloadBtn,bulletFireBtn;

    int timer=5;

    Vector3 scale = new Vector3(0.5f,0.5f,0.5f);

    public gameManager manager;
    public AudioClip levelUpClip,timerClip;

    void Start()
    {
        Time.timeScale = 1;
        DisableGuns();
        gameStart(true,1.0f);
    }

    public void StartFire()
    {
        raycastWe=Guns[levelInt].GetComponent<RaycastWeapon>();
        raycastWe.StartFiring();
    }

    public void StopFiring()
    {
        raycastWe=Guns[levelInt].GetComponent<RaycastWeapon>();
        raycastWe.StopFiring();
    }

    public void Reload()
    {
        raycastWe=Guns[levelInt].GetComponent<RaycastWeapon>();
        raycastWe.Reloader();
    }

    public void gameStart(bool isAr,float time)
    {
        if(isAr==true)
        {
            wUi.SetActive(true);
        }
        StartCoroutine(LoadAR(time));
    }
    IEnumerator LoadAR(float time)
    {
        while(true)
        {
            yield return new WaitForSeconds(time);
            if(timer>0)
            {
                AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                audioManager.PlaySFX(timerClip,1.0f);
                timer--;
                warnTimer.text="Game starts in "+timer+"...";
            }else if(timer==0)
            {
                StartCoroutine(SpawnZombies());
                break;
            }
        }   
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            wUi.SetActive(false);
            Ui.SetActive(true);
            if(totalZombies<=10)
            {
                EasyLevel();
                // MidLevel();
            }
            else if(totalZombies<=20)
            {
                MidLevel();
            }
            else if(totalZombies>20)
            {
                HardLevel();
            }
            totalZombies++;
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void EasyLevel()
    {
        //Easy Level
        level="p"; //setting level
        levelInt=0;
        

        if(easy==true)
        {
            easy=false;
            DisableGuns();
            Guns[0].SetActive(true);
            gunImg.sprite=pistol;
            gunNm.text="Pistol";
            bulletFireBtn.SetActive(true);

            manager.showNotifier("Easy Level",3.0f,"s");
        }

        spawnInterval=3f;

        //Generating a random co-ordinate for the zombiess
        float randomX = Random.Range(minX, maxX); //random x co-ordinate generator
        float ranZ = Random.Range(minZ,maxZ); //ramdom z co-rodinates generator

        /*
        float opposite = distanceCalculator(new Vector3(0,0,0), new Vector3(0,0,ranZ));
        float adj = distanceCalculator(new Vector3(randomX,0,ranZ), new Vector3(0,0,ranZ));
        AngleCalculator(opposite,adj);
        */
        

        //random vector 3 generator
        Vector3 randomPosition = new Vector3(randomX, obj.position.y, ranZ);
        obj.position = randomPosition; 

        GameObject instance = Instantiate(prefabs[0], randomPosition, Quaternion.identity);
        instance.transform.Rotate(new Vector3(0, angle, 0));
        //instance.transform.localScale=scale;
    }

    private void MidLevel()
    {
        //Medium Level
        level="a";
        levelInt=1;

        if(mid==true)
        {
            mid=false;
            DisableGuns();
            Guns[1].SetActive(true);
            gunImg.sprite=akm;
            gunImg.transform.rotation=Quaternion.Euler(0f, 0f, -45f);
            gunNm.text="Akm";
            bulletFireBtn.SetActive(true);

            playLevelUpSFX();
            manager.showNotifier("Medium Level",3.0f,"s");
        }

        spawnInterval=2f;

        float randomX = Random.Range(minX, maxX); //random x co-ordinate generator
        float ranZ =Random.Range(minZ,maxZ);
        
        //random vector 3 generator
        Vector3 randomPosition = new Vector3(randomX, obj.position.y, ranZ);
        obj.position = randomPosition; 

        int zombiePrefabRandom=Random.RandomRange(0,2); //random zombies for mid level

        GameObject instance = Instantiate(prefabs[zombiePrefabRandom], randomPosition, Quaternion.identity);
        instance.transform.Rotate(new Vector3(0, angle, 0));
    }

    private void HardLevel()
    {
        //Hard or Infinity Level
        level="m";
        levelInt=2;
        reloadBtn.SetActive(false);


        if(hard==true)
        {
            hard=false;
            DisableGuns();
            Guns[2].SetActive(true);
            gunImg.sprite=mg;
            gunNm.text="Machine Gun";
            bulletFireBtn.SetActive(true);

            playLevelUpSFX();
            manager.showNotifier("Endless Level",3.0f,"s");
        }

        spawnInterval=1.0f;

        //random postion for front
        float randomX = Random.Range(-10, 10);
        float fmin=10,fmax=20;
        float rmin=-10,rmax=-20;

        float ranZ=Random.Range(fmin,fmax);
        /*
        //if you want to spawn the zombies behind player
        
        if(totalZombies%2==0)
        {
            ranZ =Random.Range(fmin,fmax);
            angle=180;
        }else{
            ranZ=Random.Range(rmin,rmax);
            angle=0;
        }
        */
        
        
        Vector3 randomPosition = new Vector3(randomX, obj.position.y, ranZ);
        obj.position = randomPosition; 


        int zombiePrefabRandom=Random.RandomRange(0,prefabs.Length); //random zombies for hard level

        GameObject instance = Instantiate(prefabs[zombiePrefabRandom], randomPosition, Quaternion.identity);
        instance.transform.Rotate(new Vector3(0, angle, 0));

    }

    private void playLevelUpSFX()
    {
        AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlaySFX(levelUpClip,1.0f);
    }

    private float distanceCalculator(Vector3 a, Vector3 b)
    {
        float distance = Vector3.Distance(a, b);
        return distance;
    }

    private void AngleCalculator(float opp,float adj)
    {
        float angleInRadians = Mathf.Atan(opp/adj);
        float angleInDegrees = angleInRadians * Mathf.Rad2Deg;

        angle = 90+angleInDegrees;
        /*

        if (Mathf.Sign(angleInDegrees) == -1)
        {
            angle=180-angleInDegrees;
        }
        else if (Mathf.Sign(angleInDegrees) == 1)
        {
            angle=-180-angleInDegrees;
        }
        else
        {
            //not an integer its zero
        }
        */
    }

    private void DisableGuns()
    {
        for(int i=0;i<Guns.Length;i++)
        {
            Guns[i].SetActive(false);
        }
    }
}
