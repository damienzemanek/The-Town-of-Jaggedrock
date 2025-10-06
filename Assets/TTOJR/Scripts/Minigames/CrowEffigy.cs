using UnityEngine;

public class CrowEffigy : MonoBehaviour
{
    CursedRoom room;
    CallbackDetector cbDetector;

    private void Awake()
    {
        room = transform.parent.GetComponentInChildren<CursedRoom>() ??
            throw new System.Exception($"Crow Effigy: No Cursed Room Found");
        cbDetector = GetComponent<CallbackDetector>() ?? throw new System.Exception
            ("Crow Effigy: No Callback Detector Found");

        AssignEffigyUseCallback();
    }

    public void DestroyEffigy()
    {
        print($"Crow Effigy: Destroying Effigy in room {room.name}");
        room.Uncurse();
        Destroy(gameObject, 0.1f);
    }

    void AssignEffigyUseCallback()
    {
        cbDetector.useCallback.AddListener(() => DestroyEffigy());
    }
}
