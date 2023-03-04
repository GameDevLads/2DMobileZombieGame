using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZombieSpawnController : MonoBehaviour
{
    public GameObject[] zombies;
    public GameObject terrain;
    public TilemapRenderer tilemapRenderer; //this should be a collider as below, currently our map has no tilemap collider component set up
   // public TilemapCollider2D tilemapCollider;
    // Start is called before the first frame update
    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();//change this to collider eventually
        StartCoroutine(DelayZombieSpawnsBySeconds(3));
    }

    void SpawnZombie(GameObject go, int amount)
    {
        if(go == null) return;

        for(int i = 0; i< amount; i++)
        {
            Vector2 randomPoint = GetRandomPoint();
            GameObject tmp = Instantiate(go);//instantiate the object such as zombie
            tmp.gameObject.transform.position = new Vector2(randomPoint.x, randomPoint.y); //create copy of game object at random point on the map
        }
    }

    Vector2 GetRandomPoint()
    {
        int xRandom = 0;
        int yRandom = 0;

        xRandom = (int)Random.Range(0, 8);//tilemapRenderer.bounds.min.x, tilemapRenderer.bounds.max.x);
        yRandom = (int)Random.Range(0, 8);//tilemapRenderer.bounds.min.y, tilemapRenderer.bounds.max.y);

        return new Vector2(xRandom, yRandom);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator DelayZombieSpawnsBySeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SpawnZombie(zombies[0], 1);
    }
}
