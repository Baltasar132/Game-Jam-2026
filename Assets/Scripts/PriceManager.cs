using UnityEngine;


/// <summary>
/// Trying to make a singleton in unity and trying to not die in the process and trying to make it thread safe-ish
/// 
/// It will hold its own static reference.
/// 
/// Ah, it manages the prices of things (upgrades, entities, other... stuff)
/// </summary>
public class PriceManager : MonoBehaviour
{
    public static PriceManager INSTANCE;

    void Awake()
    {
        INSTANCE = this;
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
}
