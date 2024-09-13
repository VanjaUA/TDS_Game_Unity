using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishBtn : MonoBehaviour
{
    [SerializeField] private Building building;
    private void Awake()
    {
        transform.Find("button").GetComponent<Button>().onClick.AddListener(() => 
        {
            BuildingTypeSO buildingType = building.GetComponent<BuildingTypeHolder>().buildingType;
            foreach (ResourceAmount resource in buildingType.constructionResourceCostArray)
            {
                ResourceManager.Instance.AddResource(resource.resourceType,Mathf.FloorToInt(resource.amount * 0.6f));
            }
            Destroy(building.gameObject);
        });
    }
}
