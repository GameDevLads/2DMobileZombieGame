using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : MonoBehaviour
{
    public GameObject acid;
    private EnemyMovement enemyMovement;

    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        StartCoroutine(Spit());
    }

    IEnumerator Spit()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (enemyMovement.targetInRange && !enemyMovement.isMoving)
                Instantiate(acid, transform.position, Quaternion.identity);
        }
    }
}
