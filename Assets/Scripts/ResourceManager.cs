using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager INSTANCE;

    private static readonly Collider[] hitBuffer = new Collider[16];
    public ResourcesVec resourcesVec = ResourcesVec.Zero();
    public List<int> resLevels = new List<int>();
    public List<int> resWorkers = new List<int>();

    void Awake()
    {
        INSTANCE = this;
    }

    void Start()
    {
        for (int i = 0; i < (int)ResType.Ppl; i++)
        {
            resLevels.Add(1);
            resWorkers.Add(0);
        }
    }


    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    //methods ty c#
    public static List<int> GetEmptyRes()
    {
        List<int> res = new();
        for (int i = 0; i < (int)ResType.Ppl; i++)
        {
            res.Add(0);
        }
        return res;
    }

    public static ResourcesVec GetRes()
    {
        return INSTANCE.resourcesVec;
    }

    public static void AddWood(int wood)
    {
        INSTANCE.resourcesVec.wood += wood;
    }

    public static void AddStone(int stone)
    {
        INSTANCE.resourcesVec.stone += stone;
    }

    public static void AddGold(int gold)
    {
        INSTANCE.resourcesVec.gold += gold;
    }

    public static void AddPpl(int ppl)
    {
        INSTANCE.resourcesVec.ppl += ppl;
    }

    public static void SubWood(int wood)
    {
        INSTANCE.resourcesVec.wood -= wood;
    }

    public static void SubStone(int stone)
    {
        INSTANCE.resourcesVec.stone -= stone;
    }

    public static void SubGold(int gold)
    {
        INSTANCE.resourcesVec.gold -= gold;
    }

    public static void SubPpl(int ppl)
    {
        INSTANCE.resourcesVec.ppl -= ppl;
    }

    public static void SubRes(ResourcesVec res)
    {
        SubWood(res.wood);
        SubStone(res.stone);
        SubGold(res.gold);
        SubPpl(res.ppl);
    }


    public static int GetLevel(ResType type)
    {
        return INSTANCE.resLevels[(int)type];
    }

    public static int GetWorkers(ResType type)
    {
        return INSTANCE.resWorkers[(int)type];
    }

    public static void AddLevel(ResType type)
    {
        INSTANCE.resLevels[(int)type] += 1;
    }

    public static void AddWorkers(ResType type)
    {
        INSTANCE.resWorkers[(int)type] += 1;
    }

    public static void ReduceWood(Vector3 where, float range)
    {
        int count = Physics.OverlapSphereNonAlloc(where, range, hitBuffer, 1 << 10);

        WoodSource nearest = null;
        float minDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < count; i++)
        {
            if (hitBuffer[i].TryGetComponent<WoodSource>(out var woodSource))
            {
                float sqrDist = (hitBuffer[i].transform.parent.position - where).sqrMagnitude;

                if (sqrDist < minDistanceSqr)
                {
                    minDistanceSqr = sqrDist;
                    nearest = woodSource;
                }
            }
        }

        if (nearest != null)
        {
            nearest.quantity -= 100;
        }
        {
            print("No se encontró árbol en " + where + " con " + Builds.StoneAmount() + " árboles");
        }
    }

    public static void ReduceStone(Vector3 where, float range)
    {
        int count = Physics.OverlapSphereNonAlloc(where, range, hitBuffer, 1 << 9);

        StoneSource nearest = null;
        float minDistanceSqr = Mathf.Infinity;

        for (int i = 0; i < count; i++)
        {
            if (hitBuffer[i].TryGetComponent<StoneSource>(out var stoneSource))
            {
                float sqrDist = (hitBuffer[i].transform.position - where).sqrMagnitude;

                if (sqrDist < minDistanceSqr)
                {
                    minDistanceSqr = sqrDist;
                    nearest = stoneSource;
                }
            }
        }

        if (nearest != null)
        {
            nearest.quantity -= 1000;
        }
        else
        {
            print("No se encontró piedra en " + where + " con " + Builds.StoneAmount() + " piedras");
        }
    }
}
