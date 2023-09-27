using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretComponent : MonoBehaviour
{
    public GameObject particleSystemToSpawn;
    public GameObject player;
    public Slider healthBar;
    public Slider awarnessBar;
    public Text awarnessText;
    public Vector3 initialFowardVector;
    public Vector3 playerDirection;
    public float maxAngle = 45;
    public float maxDistance = 100;
    public float health = 100;

    public float awarnessTimer = 0.0f;
    public float fullAwarnessTime = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        initialFowardVector = transform.forward;  
        player = GameObject.FindGameObjectWithTag("Player");
        healthBar.maxValue = health;
    }


    // Update is called once per frame
    void Update()
    {
        
        UpdateTurretRotation();
        UpdateTurretAwareness(SeePlayer());
               
    }
    
    public void UpdateTurretAwareness(bool seePlayer)
    {
        awarnessTimer = (seePlayer)?awarnessTimer + Time.deltaTime : awarnessTimer - Time.deltaTime;
        float awarnessRatio = Mathf.Clamp(awarnessTimer/fullAwarnessTime, 0.0f, 1.0f);
        awarnessBar.value = awarnessRatio;

       if (awarnessTimer >= fullAwarnessTime)
       {

            awarnessText.enabled = (true);
       } else
        {
            awarnessText.enabled = (false);
        }
       
    }
    public void ProcessHit()
    {
        health = health - 10;
        healthBar.value = health;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void UpdateTurretRotation()
    {
        if (SeePlayer())
        {
            playerDirection = new Vector3(playerDirection.x,0,playerDirection.z);
            transform.LookAt(player.transform.position + playerDirection);
        }

    }
    public bool SeePlayer()
    {
        playerDirection = player.transform.position - transform.position;
        if (playerDirection.magnitude < maxDistance)
        {
            Vector3 normPlayerDirection = Vector3.Normalize(playerDirection);
            float dotProduct = Vector3.Dot(transform.forward, normPlayerDirection);

            float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;
            
            if (angle < maxAngle) {
                Debug.Log(angle);
                RaycastHit hit = new RaycastHit();
                Ray ray = new Ray(transform.position, normPlayerDirection);
                if(Physics.Raycast(ray, out hit))
                {   
                    if (hit.collider.tag == "Player")
                    {

                        return true;
                    }
                }
            }
        } 
        return false;
       
    }
}