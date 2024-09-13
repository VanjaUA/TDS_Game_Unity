using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResourceType")]
public class ResourceTypeSO : ScriptableObject
{
    public string resourceName;
    public string shortName;
    public Sprite sprite;
    public string coloreHex = "";
}
