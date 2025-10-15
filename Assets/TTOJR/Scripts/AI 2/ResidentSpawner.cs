using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;

public class ResidentSpawner : MonoBehaviour
{
    public bool onlySpawnFirstResident = false;
    [ShowIf("onlySpawnFirstResident")] public float spawnCount;
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
        if (onlySpawnFirstResident)
        {
            while(currentSpawnedResidents < spawnCount)
            {
                if (spawnResidentPoolForCycle[0] == null)
                    throw new System.Exception("ResidentSpawner: Set the first resident in the pool");

                Spawn(resident: spawnResidentPoolForCycle[0]);
                currentSpawnedResidents++;
                yield return new WaitForSeconds(delayBetweenSpawns);
            }
        }
        else
        {
            while (currentSpawnedResidents < spawnResidentPoolForCycle.Count)
            {
                if (spawnResidentPoolForCycle[currentSpawnedResidents] == null)
                    throw new System.IndexOutOfRangeException(tag);
                Spawn(resident: spawnResidentPoolForCycle[currentSpawnedResidents]);
                yield return new WaitForSeconds(delayBetweenSpawns);
                currentSpawnedResidents++;
            }
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
