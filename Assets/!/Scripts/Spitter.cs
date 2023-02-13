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
        float attackSpeed = GetComponent<Assets.Scripts.Enemy>().EnemyStats.attackSpeed;
        StartCoroutine(Spit(attackSpeed));
    }

    IEnumerator Spit(float attackSpeed)
    {
        while (true)
        {
            var waitTime = 1 / attackSpeed;
            yield return new WaitForSeconds(waitTime);
            if (enemyMovement.targetInRange && !enemyMovement.isMoving)
                Instantiate(acid, transform.position, Quaternion.identity, transform);
        }
    }
}
