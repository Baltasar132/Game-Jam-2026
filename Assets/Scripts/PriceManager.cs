using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Trying to make a singleton in unity and trying to not die in the process and trying to make it thread safe-ish
/// 
/// It will hold its own static reference.
/// 
/// Ah, it manages the prices of things (upgrades, entities, other... stuff)
/// </summary>
/// wow como se nota que no sabía nada cuando empecé el proyecto... XD
public class PriceManager : MonoBehaviour
{
    public static PriceManager INSTANCE;

    [SerializeField] private float workerRange = 10f;
    [SerializeField] private List<BuildingCostData> rawBuildingData;
    private Dictionary<BuildingType, ResourcesVec> buildingCosts = new();
    private Dictionary<BuildingType, ResourcesVec> risingBuildingCosts = new();
    private Dictionary<BuildingType, int> buildings = new();

    public static float WorkerRange => INSTANCE.workerRange;
    public static Dictionary<BuildingType, int> Buildings => INSTANCE.buildings;
    public static Dictionary<BuildingType, ResourcesVec> BuildingCosts => INSTANCE.buildingCosts;
    public static Dictionary<BuildingType, ResourcesVec> RisingBuildingCosts => INSTANCE.risingBuildingCosts;
    public static ResourcesVec GetBuildingCost(BuildingType type) => BuildingCosts[type] + (RisingBuildingCosts[type] * Buildings[type]);
    public static void AddBuilding(BuildingType type) => Buildings[type] += 1;

    void Awake()
    {
        INSTANCE = this;
        foreach (var data in rawBuildingData)
        {
            BuildingCosts[data.type] = data.cost;
            RisingBuildingCosts[data.type] = data.risingCost;
            Buildings[data.type] = data.count;
        }
    }

    public static ResourcesVec getWoodLevelPrice()
    {
        return ResourcesVec.Zero().AddWood(ResourceManager.GetLevel(ResType.Wood) * 2);
    }
    public static ResourcesVec getStoneLevelPrice()
    {
        return ResourcesVec.Zero().AddStone(ResourceManager.GetLevel(ResType.Stone) * 2);
    }
    public static ResourcesVec getWoodWorkerPrice()
    {
        return ResourcesVec.Zero().AddWood((ResourceManager.GetWorkers(ResType.Wood) + 1) * 10);
    }
    public static ResourcesVec getStoneWorkerPrice()
    {
        return ResourcesVec.Zero().AddStone((ResourceManager.GetWorkers(ResType.Wood) + 1) * 10);
    }

    public static bool CanExpend(ResourcesVec res)
    {
        return ResourceManager.GetRes() >= res;
    }
}

[Serializable]
public struct BuildingCostData
{
    public BuildingType type;
    public ResourcesVec cost;
    public ResourcesVec risingCost;
    public int count;
}