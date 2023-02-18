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
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z);
            if (enemyMovement.targetInRange && !enemyMovement.isMoving)
                Instantiate(acid, pos, Quaternion.identity, transform);
        }
    }
}
