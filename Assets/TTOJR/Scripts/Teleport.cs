using System.Collections;
using System.ComponentModel;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Teleport : MonoBehaviour
{
    bool teleporting;
    public Transform tpLoc;
    [Sirenix.OdinInspector.ReadOnly] public GameObject objToTeleport;
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
        bool foundTpLocOnNavMesh = NavMeshUtility.NearestLocOnNavMesh(tpLoc.position, 5f, out Vector3 tpLocOnNavMesh);
        if(objToTeleport.gameObject.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            if (foundTpLocOnNavMesh) agent.Warp(tpLocOnNavMesh);
            else
            {
                agent.enabled = false;
                objToTeleport.transform.position = tpLoc.position;
                agent.enabled = true;
            }
        }
        else
            objToTeleport.transform.position = foundTpLocOnNavMesh ? tpLocOnNavMesh : tpLoc.position;

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
