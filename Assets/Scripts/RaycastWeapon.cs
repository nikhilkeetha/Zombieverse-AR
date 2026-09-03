using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem.XR;

public class RaycastWeapon : MonoBehaviour
{
    public TrailRenderer tracerEffect;
    public bool isFiring =false;
    public ParticleSystem muzzleFlash, hitEffect;
    public Transform raycastOrigin,raycastDestination;
    Ray ray;
    RaycastHit hitInfo;

    public float damage;

    [SerializeField] private AudioClip m_BulletSound;

    public GameObject arCamera;

    public Rigidbody gunRigidbody;

    public Animator animator;

    public GameObject bullet;

    public Transform gunplace;

    public string type;

    public int Bullets;

    public TMP_Text bulletsDisp;

    bool isReload=true;

    float reloadTime,reloadSpeed=0.6f;

    public GameObject fireBtn;

    public float forceMagnitude;

    public AudioClip reClip,machineClip;

    // Start is called before the first frame update

    void Start()
    {
        if(type=="p")
        {
            Bullets=10;
            reloadTime=2.533f;
        }
        else if(type=="a")
        {
            Bullets=30;
            reloadTime=3.033f;
        }else if(type=="m")
        {
            Bullets=50;
        }
        bulletsDisp.text=Bullets+"/"+"∞";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartFiring() {

        isFiring =true;
        animator.SetBool("tests",true);
       
       if(type=="p")
       {
            singleBulletMode();
       }else if(type=="a")
       {
           InvokeRepeating("singleBulletMode", 0f, 0.2f);
       }else{
        bulletsDisp.text="";
            InvokeRepeating("singleBulletMode", 0f, 0.1f);
       }
    }

    private void singleBulletMode()
    {
        if(Bullets!=0)
        {
            muzzleFlash.Emit(1);
            ray.origin = raycastOrigin.position;
            ray.direction = raycastDestination.position - raycastOrigin.position;
            //ray.direction=transform.forward;
            
            var tracer = Instantiate(tracerEffect, ray.origin, Quaternion.identity);
            tracer.AddPosition(ray.origin);

            if (Physics.Raycast(arCamera.transform.position, arCamera.transform.forward, out hitInfo)) {
                //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);

/*
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1); 
                */
                tracer.transform.position = hitInfo.point;
                Destroy(tracer,2.0f);
            }

            //used bullets
            GameObject instance = Instantiate(bullet, gunplace.transform.position, Quaternion.identity);
            instance.transform.Rotate(new Vector3(0, 106, -14.944f));

            Rigidbody rb = instance.GetComponent<Rigidbody>();
            Vector3 dir=new Vector3(-2,2,-2);
            rb.AddForce(dir.normalized * forceMagnitude, ForceMode.Impulse);
            Destroy(instance,2.0f);

            AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            if(type=="m")
            {
                audioManager.PlaySFX(machineClip,0.9f);
            }else if(type=="a"){
                audioManager.PlaySFX(machineClip,1.0f);
            }
            else{
                audioManager.PlaySFX(m_BulletSound,2.0f);
            }

            if(type!="m"){
                Bullets--;
                bulletsDisp.text=Bullets+"/"+"∞";
            }

            var rb2d = hitInfo.collider.GetComponent<Rigidbody>();
            if(rb2d) {
                rb2d.AddForceAtPosition(ray.direction * 20, hitInfo.point, ForceMode.Impulse);
            }
    
            var hitBox = hitInfo.collider.GetComponent<HitBox>();
            if(hitBox) { 
                hitBox.OnRaycastHit(this,ray.direction);
            }

        }
        else{
            StartCoroutine(reload());
            StopFiring();
        }
    }

    IEnumerator reload()
    {
        while(true)
        {
            if(isReload==true)
            {
                fireBtn.SetActive(false);
                isReload=false;
                animator.SetBool("reload",true);
                
                //playing reload sound
                AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                audioManager.PlaySFX(reClip,reloadSpeed);
                

            }else{
                animator.SetBool("reload",false);
                isReload=true;
                if(type=="p")
                {
                    Bullets=10;
                }
                else if(type=="a")
                {
                    Bullets=30;
                }else if(type=="m")
                {
                    Bullets=50;
                }
                bulletsDisp.text=Bullets+"/"+"∞";
                fireBtn.SetActive(true);
                break;
            }
            yield return new WaitForSeconds(reloadTime);
        }
    }

    public void Reloader()
    {
        StartCoroutine(reload());
    }

    public void StopFiring() {
        isFiring=false;
        animator.SetBool("tests",false);
        CancelInvoke("singleBulletMode");
    }
}
