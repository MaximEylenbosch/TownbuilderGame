using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyBoolEvent : UnityEvent<bool>
{
}

public class OverlapChecker : MonoBehaviour
{
    [SerializeField] private LayerMask _overlapLayer;
    [SerializeField] private Collider _ownCollider = null;

    [SerializeField] private Material _overlappingMaterial;
    [SerializeField] private Material _notOverlappingMaterial;

    [SerializeField] private Transform[] _boundingPoints;

    private bool _isOverlapping;
    private bool _isOnGround;
    private Renderer[] _meshRenderers;

    [HideInInspector]
    public MyBoolEvent OnOverlapChange;

    public bool IsOverlapping { get => _isOverlapping; }

    private void Start()
    {
        _meshRenderers = this.transform.parent.GetComponentsInChildren<Renderer>();

        if (OnOverlapChange == null)
            OnOverlapChange = new MyBoolEvent();

        OnOverlapChange.AddListener(ChangeColor);
    }

    private void ChangeColor(bool hasOverlap)
    {
        Debug.Log("change color");
        Material material = _notOverlappingMaterial;
        if (hasOverlap)
            material = _overlappingMaterial;

        Material[] materials;

        for (int i = 0; i < _meshRenderers.Length; i++)
        {
            materials = new Material[_meshRenderers[i].materials.Length];
            for (int j = 0; j < materials.Length; j++)
            {
                materials[j] = material;
            }
            _meshRenderers[i].materials = materials;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_isOverlapping)
            return;

        if (other.gameObject.layer == 0 && other.GetComponent<Collider>() != _ownCollider)
        {
            _isOverlapping = false;
            OnOverlapChange.Invoke(_isOverlapping);
        }         
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isOverlapping)
            return;

        if (other.gameObject.layer == 0 && other.GetComponent<Collider>() != _ownCollider)
        {
            _isOverlapping = true;
            OnOverlapChange.Invoke(_isOverlapping);
        }
    }

    private void OnDestroy()
    {
        OnOverlapChange.RemoveAllListeners();
    }

    private void Update()
    {
        if (_isOverlapping)
            return;

        for (int i = 0; i < _boundingPoints.Length; i++)
        {
            if (!HitGround(i))
            {
                _isOnGround = false;
                OnOverlapChange.Invoke(true);
                return;
            }
        }

        if (_isOnGround)
            return;

        _isOnGround = true;
        OnOverlapChange.Invoke(false);
    }

    private bool HitGround(int boundingIndex)
    {
        Ray ray = new Ray(_boundingPoints[boundingIndex].position + (Vector3.up * 0.5f), Vector3.down);

        if (Physics.Raycast(ray, 1f))
            return true;

        return false;
    }

    public bool CanPlace()
    {
        if (_isOnGround && !_isOverlapping)
            return true;
        else return false;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _boundingPoints.Length; i++)
        {
            Gizmos.DrawLine(_boundingPoints[i].position, _boundingPoints[i].position + Vector3.down);
        }
    }
}
