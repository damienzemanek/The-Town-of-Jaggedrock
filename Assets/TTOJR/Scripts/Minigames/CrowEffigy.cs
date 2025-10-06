using UnityEngine;

public class CrowEffigy : MonoBehaviour
{
    CursedRoom room;

    private void Awake()
    {
        room = transform.parent.GetComponentInChildren<CursedRoom>() ??
            throw new System.Exception($"Crow Effigy: No Cursed Room Found");
    }

    public void DestroyEffigy()
    {
        room.Uncurse();
    }
}
