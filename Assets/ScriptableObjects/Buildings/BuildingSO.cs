using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BuildingSO")]
public class BuildingSO : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public int BuildingCost;
    public int Radius;

    public GameObject Building;
    public GameObject PreviewBuilding;
}
