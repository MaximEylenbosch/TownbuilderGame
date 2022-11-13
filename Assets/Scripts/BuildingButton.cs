using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private BuildingSO _buildingSO;

    [SerializeField] private Image _buildingImage;
    [SerializeField] private Image _resourceCostImage;
    [SerializeField] private Text _buildingText;
    [SerializeField] private Text _buildingCost;

    private void Start()
    {
        _buildingImage.sprite = _buildingSO.Sprite;
        _buildingText.text = _buildingSO.Name;

        //_resourceCostImage.sprite = _buildingSO.Sprite;
        //_buildingCost.text = _buildingSO.BuildingCost.ToString();
    }
}
