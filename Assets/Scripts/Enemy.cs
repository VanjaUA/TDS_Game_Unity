using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create(Vector3 position) 
    {
        Transform enemyTransform = Instantiate(GameAssets.Instance.enemyPrefab, position, Quaternion.identity);
        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }
    private Transform targetTransform;
    private Rigidbody2D rigidbody2d;
    private HealthSystem healthSystem;

    private float lookForTargetTimer;
    private float lookForTargetTimerMax = 0.2f;

    void Start()
    {
        if (BuildingManager.Instance.GetHQBuilding() != null)
        {
            targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
        }
        rigidbody2d = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDied += HealthSystem_OnDied;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        lookForTargetTimer = Random.Range(0f,lookForTargetTimerMax);
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.Shake(4f,0.07f);
        ChromaticAberrationEffect.Instance.SetWeight(0.1f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        CinemachineShake.Instance.Shake(6f, 0.15f);
        ChromaticAberrationEffect.Instance.SetWeight(0.2f);
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        Instantiate(GameAssets.Instance.enemyDieParticles,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleMovement();

        HandleTargeting();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building;
        if (collision.gameObject.TryGetComponent<Building>(out building))
        {
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            this.healthSystem.Damage(9999);
        }
    }

    private void HandleMovement() 
    {
        if (targetTransform != null)
        {
            Vector3 moveDir = (targetTransform.position - transform.position).normalized;

            float moveSpeed = 6f;
            rigidbody2d.velocity = moveDir * moveSpeed;
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
        }
    }

    private void HandleTargeting() 
    {
        lookForTargetTimer -= Time.deltaTime;
        if (lookForTargetTimer < 0f)
        {
            lookForTargetTimer += lookForTargetTimerMax;
            LookForTargets();
        }
    }

    private void LookForTargets() 
    {
        float targetMaxRadius  = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position,targetMaxRadius);

        foreach (Collider2D collider in collider2DArray)
        {
            Building building;
            if (collider.TryGetComponent<Building>(out building))
            {
                if (targetTransform == null)
                {
                    targetTransform = building.transform;
                }
                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position,targetTransform.position))
                    {
                        //closer
                        targetTransform = building.transform;
                    }
                }
            }
        }
        if (targetTransform == null)
        {
            if (BuildingManager.Instance.GetHQBuilding() != null)
            {
                targetTransform = BuildingManager.Instance.GetHQBuilding().transform;
            }
        }
    }
}
