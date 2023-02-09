using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPuddle : MonoBehaviour
{
    private float timeToLive = 5f;
    private float timeAlive = 0f;
    private float damage = 1f;
    
    void Start()
    {
        
    }
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > timeToLive)
            Destroy(gameObject);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Player in acid");
            //TODO: Damage player
        }
    }

}
