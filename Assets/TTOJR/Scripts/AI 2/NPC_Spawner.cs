using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Sirenix.OdinInspector;

public class NPC_Spawner : MonoBehaviour
{
    public bool onlySpawnFirstResident = false;
    [ShowIf("onlySpawnFirstResident")] public float spawnCount;
    public Transform spawnPoint;
    public NPC_Area spawnArea;
    public List<GameObject> spawnResidentPoolForCycle;
    public float delayBetweenSpawns;
    [ReadOnly] public int currentSpawnedResidents; //cant be bigger than the pool

    private void Start()
    {
        if (spawnResidentPoolForCycle == null || spawnResidentPoolForCycle.Count <= 0)
            Debug.LogError("NPC_Spawner: Residents not set, put residents to spawn");

        if (spawnArea == null) throw new System.Exception("NPC_Spawner: spawn area not set");
        StartCoroutine(SpawningCycle());
    }

    IEnumerator SpawningCycle()
    {
        if (onlySpawnFirstResident)
        {
            while(currentSpawnedResidents < spawnCount)
            {
                if (spawnResidentPoolForCycle[0] == null)
                    throw new System.Exception("NPC_Spawner: Set the first NPC_Movement in the pool");

                Spawn(NPC_Movement: spawnResidentPoolForCycle[0]);
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
                Spawn(NPC_Movement: spawnResidentPoolForCycle[currentSpawnedResidents]);
                yield return new WaitForSeconds(delayBetweenSpawns);
                currentSpawnedResidents++;
            }
        }
    }

    void Spawn(GameObject NPC_Movement)
    {
        NPC_Movement newResident = Instantiate(NPC_Movement,
            spawnPoint.transform.position,
            Quaternion.identity,
            null
            ).gameObject.GetComponent<NPC_Movement>();

        newResident.SpawnAtLocation(spawnArea);
    }

}
