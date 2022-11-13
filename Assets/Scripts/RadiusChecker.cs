using UnityEngine;

public class RadiusChecker : MonoBehaviour
{
    public int Radius { set => _radius = value; }
    private int _radius;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
