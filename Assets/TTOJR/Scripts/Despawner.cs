using System.Collections.Generic;
using System.Linq;
using DependencyInjection;
using Sirenix.Utilities;
using UnityEngine;

[DefaultExecutionOrder(300)]
[RequireComponent(typeof(TimeCycle))]
public class Despawner : MonoBehaviour, IDependencyProvider
{
    [Provide] Despawner Provide() => this;
    [Inject] TimeCycle time;

    [SerializeField] List<GameObject> spawnedNPCs;
    [SerializeField] List<GameObject> disabledNPCs;

    private void Awake()
    {
        spawnedNPCs = new List<GameObject>();
        disabledNPCs = new List<GameObject>();
    }

    private void OnEnable()
    {
        AssignDespawnEvents();
    }

    private void OnDisable()
    {
        RemoveDespawnEvents();
    }

    public void SaveToBeDespawned(GameObject go)
    {
        if (!go) return;
        spawnedNPCs.Add(go);
        disabledNPCs.Remove(go);
    }

    public void DisableAllNPCS()
    {
        foreach (GameObject spawnedNPC in spawnedNPCs.ToArray())
        {
            if (!spawnedNPC) continue;
            spawnedNPC.SetActive(false);
            disabledNPCs.Add(spawnedNPC);
            spawnedNPCs.Remove(spawnedNPC);
        }

    }

    public bool TryGetFromPool(GameObject prefab, out GameObject match)
    {
        this.Log("1");
        match = null;
        if (!prefab) return false;
        this.Log("2");

        if (disabledNPCs.Count <= 0) return false;
        string lookingForName = prefab.TryGet<Dialuage>().personName;

        this.Log($"3, looking for name {lookingForName}");

        match = disabledNPCs.FirstOrDefault(npc => 
            npc != null && 
            npc.name.StartsWith(lookingForName));

        if (!match) return false;
        this.Log("4");

        this.Log($"Pool Get found match {match} using name {lookingForName}");

        disabledNPCs.Remove(match);
        spawnedNPCs.Add(match);
        return true;
    }

    void AssignDespawnEvents()
    {
        time.OnDayStart.AddListener(StartNewDayOrNight);
        time.OnNightStart.AddListener(StartNewDayOrNight);
    }

    void RemoveDespawnEvents()
    {
        time.OnDayStart?.RemoveAllListeners();
        time.OnNightStart?.RemoveAllListeners();
    }

    void StartNewDayOrNight()
    {
        DisableAllNPCS();
    }
    
}
