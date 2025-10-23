using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class Spawner : MonoBehaviour
{
    public bool single;
    public bool multiple;
    [ShowIf("single")] public GameObject prefab;
    [ShowIf("multiple")] public GameObject[] prefabs;
    public Transform location;

    public void Spawn()
    {
        if (single) SingleSpawn();
        if (multiple) MultiSpawn();
    }

    void SingleSpawn()
    {
        Instantiate(
        prefab,
        location.position,
        Quaternion.identity,
        null
        );
    }

    void MultiSpawn()
    {
        prefabs.ForEach(p => Instantiate(
            p,
            location.position,
            Quaternion.identity,
            null
            ));
    }
}
