using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;

    public static GameAssets Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = Resources.Load<GameAssets>("GameAssets");
            }
            return instance; 
        }
    }


    public Transform enemyPrefab;
    public Transform enemyDieParticles;
    public Transform arrrowProjectile;
    public Transform buildingDestroyedParticles;
    public Transform buidlingConstruction;
    public Transform buildingPlacedParticles;
}
