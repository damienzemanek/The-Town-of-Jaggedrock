using System.Collections;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    bool teleporting;
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
        if (teleporting) return;
        teleporting = true;
        print("Teleport: Attempting TP");
        if (objToTeleport == null) SetObjectToTeleportFromDetector();

        if (objToTeleport.TryGetComponent<FadeScreen>(out FadeScreen fade))
            FadeTeleport(fade);
        else
            TpImplementation();
    }

    public void FadeTeleport(FadeScreen fade) => fade.FadeInAndOutCallback(TpImplementation);
    void TpImplementation()
    {
        objToTeleport.transform.position = tpLoc.position;
        teleporting = false;
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
