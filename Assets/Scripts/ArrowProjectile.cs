using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public static ArrowProjectile Create(Vector3 position,Enemy enemy)
    {
        Transform arrowPrefab = GameAssets.Instance.arrrowProjectile;
        Transform arrowTransform = Instantiate(arrowPrefab, position, Quaternion.identity);
        ArrowProjectile arrow = arrowTransform.GetComponent<ArrowProjectile>();

        arrow.SetEnemy(enemy);

        return arrow;
    }

    private Enemy targetEnemy;
    private Vector3 lastMoveDir;

    private float timeToDie = 2f;

    private void Update()
    {
        Vector3 moveDir;
        if (targetEnemy != null)
        {
            moveDir = (targetEnemy.transform.position - transform.position).normalized;
            lastMoveDir = moveDir;
        }
        else
        {
            moveDir = lastMoveDir;
        }

        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

        float moveSpeed = 20f;
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        timeToDie -= Time.deltaTime;
        if (timeToDie <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void SetEnemy(Enemy targetEnemy) 
    {
        this.targetEnemy = targetEnemy;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy;
        if (collision.TryGetComponent<Enemy>(out enemy))
        {
            int damageAmount = 10;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);

            Destroy(gameObject);
        }
    }
}
