using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    private GameObject spriteGameObject;
    private ResourceNearbyOverlay resourceNearbyOverlay;

    private void Awake()
    {
        spriteGameObject = transform.Find("sprite").gameObject;
        resourceNearbyOverlay = transform.Find("ResourceNearbyOverlay").GetComponent<ResourceNearbyOverlay>();
        Hide();
        resourceNearbyOverlay.Hide();
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChanged += BuildingManager_OnActiveBuildingTypeChanged;
    }

    private void BuildingManager_OnActiveBuildingTypeChanged(object sender, BuildingManager.OnActiveBuildingTypeChangedEventArgs e)
    {
        if (e.activeBuildingType == null)
        {
            Hide();
            resourceNearbyOverlay.Hide();
        }
        else
        {
            Show(e.activeBuildingType.sprite);
            if (e.activeBuildingType.hasResourceGeneratorData)
            {
                resourceNearbyOverlay.Show(e.activeBuildingType.resourceGeneratorData);
            }
            else
            {
                resourceNearbyOverlay.Hide();
            }
        }
    }

    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite) 
    {
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
        spriteGameObject.SetActive(true);
    }

    private void Hide() 
    {
        spriteGameObject.SetActive(false);
    }
}
