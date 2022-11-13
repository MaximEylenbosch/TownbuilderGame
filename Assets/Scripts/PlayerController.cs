using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera _camera;

    private BuildingSO _selectedBuildingSO = null;
    private bool _buildingSelected = false;
	public bool BuildingSelected { get => _buildingSelected; }

    private GameObject _previewBuiding = null;
    private OverlapChecker _overlapChecker = null;

    [SerializeField] private LayerMask _buildingLayer = 0;
    [SerializeField] private float _buildingRotationSpeed = 10;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (_buildingSelected)
            MoveSelectedBuilding();
    }

	private void MoveSelectedBuilding()
	{
        if (Input.GetMouseButtonDown(0))
        {
            if (_overlapChecker.CanPlace())
                BuildSelectedBuilding();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            DeselectBuilding();
            return;
        }
        else if (Input.mouseScrollDelta.y != 0)
        {
            RotateSelectedBuilding();
            return;
        }

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, _buildingLayer.value))
        {
            if (_previewBuiding != null)
            {
                _previewBuiding.transform.position = hit.point;
            }
        }
    }

    private void RotateSelectedBuilding()
    {
        _previewBuiding.transform.Rotate(Vector3.up, Input.mouseScrollDelta.y * _buildingRotationSpeed);
    }

    private void BuildSelectedBuilding()
    {
        Transform transformPreviewBuilding = _previewBuiding.transform;
        Instantiate(_selectedBuildingSO.Building, transformPreviewBuilding.position, transformPreviewBuilding.rotation);

        DeselectBuilding();
    }

    public void SelectBuilding(BuildingSO buildingSO)
    {
        DeselectBuilding();
        ShowBuildingPreview(buildingSO);
    }

    private void DeselectBuilding()
    {
        if (_previewBuiding)
        {
            Destroy(_previewBuiding);
            _previewBuiding = null;
        }
        _buildingSelected = false;
        _selectedBuildingSO = null;
    }

    private void ShowBuildingPreview(BuildingSO buildingSO)
	{
        _selectedBuildingSO = buildingSO;
        _buildingSelected = true;
        _previewBuiding = Instantiate(buildingSO.PreviewBuilding);
        _previewBuiding.GetComponent<RadiusChecker>().Radius = buildingSO.Radius;
        _overlapChecker = _previewBuiding.GetComponentInChildren<OverlapChecker>();
    }
}
