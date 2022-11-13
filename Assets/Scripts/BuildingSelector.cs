using UnityEngine;

public class BuildingSelector : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController = null;

    public void SelectBuilding(BuildingSO buildingSO)
    {
        if (_playerController)
        {
            _playerController.SelectBuilding(buildingSO);
        }
    }
}
