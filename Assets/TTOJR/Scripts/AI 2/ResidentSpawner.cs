using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;

public class ResidentSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public ResidentAreas spawnArea;
    public List<GameObject> spawnResidentPoolForCycle;
    public float delayBetweenSpawns;
    [ReadOnly] public int currentSpawnedResidents; //cant be bigger than the pool

    private void Start()
    {
        if (spawnResidentPoolForCycle == null || spawnResidentPoolForCycle.Count <= 0)
            Debug.LogError("ResidentSpawner: Residents not set, put residents to spawn");

        if (spawnArea == null) throw new System.Exception("ResidentSpawner: spawn area not set");
        StartCoroutine(SpawningCycle());
    }

    IEnumerator SpawningCycle()
    {
        while(currentSpawnedResidents < spawnResidentPoolForCycle.Count)
        {
            if (spawnResidentPoolForCycle[currentSpawnedResidents] == null)
                throw new System.IndexOutOfRangeException(tag);
            Spawn(spawnResidentPoolForCycle[currentSpawnedResidents]);
            yield return new WaitForSeconds(delayBetweenSpawns);
            currentSpawnedResidents++;
        }
    }

    void Spawn(GameObject resident)
    {
        Resident newResident = Instantiate(resident,
            spawnPoint.transform.position,
            Quaternion.identity,
            null
            ).gameObject.GetComponent<Resident>();

        newResident.SpawnAtLocation(spawnArea);
    }

}
