using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask itemMask;
    [SerializeField] float dist;
    private void Update()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, dist, itemMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
    }
}
