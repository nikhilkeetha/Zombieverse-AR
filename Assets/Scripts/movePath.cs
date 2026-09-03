using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePath : MonoBehaviour
{
    public GameObject zombie;
    public GameObject destination;
    public float speed;

    Animator animator;
    private Vector3 actualPos;

    PlayerHealth health;
    public float zombieDamage;

    public bool isZombieDead=false;

    public AudioClip attackClip;
    float time = 1.1f;

    bool once=true;

    private void Start()
    {
        destination=GameObject.Find("zombieDestination");
        animator=GetComponent<Animator>();
        health=GameObject.Find("zombieDestination").GetComponent<PlayerHealth>();
    }

    private void Update()
    {   
        actualPos=zombie.transform.position;
        zombie.transform.position=Vector3.MoveTowards(actualPos, destination.transform.position, speed * Time.deltaTime);
        if(actualPos==destination.transform.position)
        {


            //Playing zombie attacking sound effect 
            AudioManager sourceAd = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            //AudioClip audioClip = Resources.Load<AudioClip>("zombie attack");
            sourceAd.PlaySFX2(attackClip,1);
        
            
            //zombie reached player then attack the player
            animator.SetFloat("walk",1);
            if(once==true)
            {
                once=false;
                StartCoroutine(attak());
            }
            
        }
    }
    IEnumerator attak()
    {
        while(true)
        {
            yield return new WaitForSeconds(time);
            time=2.8f;
            health.TakeDamage(zombieDamage);
        }
    }

}
