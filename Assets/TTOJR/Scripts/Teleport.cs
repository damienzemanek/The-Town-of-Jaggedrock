using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform tpLoc;
    public GameObject objToTeleport;
    Detector detector;

    private void Awake()
    {
        detector = GetComponent<Detector>();
    }

    public void DoTeleport(GameObject GO)
    {
        objToTeleport = GO;
        objToTeleport.transform.position = tpLoc.position;
    }

    public void DoTeleport()
    {
        print("Teleport: Attempting TP");
        if (objToTeleport == null) Debug.LogError(message: $"TP: No Object found to teleport");
        objToTeleport.transform.position = tpLoc.position;
    }

    public void SetObjectToTeleport(GameObject GO)
    {
        objToTeleport = GO;
    }
    public void SetObjectToTeleportFromDetector()
    {
        if (detector.colliderObject == null)
            Debug.LogError("Teleport: Cannot assign obj to teleport, its null from detector");
        objToTeleport = detector.colliderObject;
    }
}
