using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform tpLoc;

    public void DoTeleport(GameObject GO)
    {
        GO.transform.position = tpLoc.position;
    }
}
