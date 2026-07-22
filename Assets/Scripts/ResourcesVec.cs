using System;
using System.Text;
using UnityEngine;

[Serializable]
public class ResourcesVec
{
    public int wood;
    public int stone;
    public int gold;
    public int ppl;

    public ResourcesVec(int wood, int stone, int gold, int ppl)
    {
        this.wood = wood;
        this.stone = stone;
        this.gold = gold;
        this.ppl = ppl;
    }

    public static bool operator !=(ResourcesVec l, ResourcesVec r)
    {
        return l.wood != r.wood
            || l.stone != r.stone
            || l.gold != r.gold
            || l.ppl != r.ppl;
    }

    public static bool operator ==(ResourcesVec l, ResourcesVec r)
    {
        return l.wood == r.wood
            && l.stone == r.stone
            && l.gold == r.gold
            && l.ppl == r.ppl;
    }

    public static bool operator >=(ResourcesVec l, ResourcesVec r)
    {
        return l.wood >= r.wood
            && l.stone >= r.stone
            && l.gold >= r.gold
            && l.ppl >= r.ppl;
    }

    public static bool operator <=(ResourcesVec l, ResourcesVec r)
    {
        return l.wood <= r.wood
            && l.stone <= r.stone
            && l.gold <= r.gold
            && l.ppl <= r.ppl;
    }


    public static bool operator <(ResourcesVec l, ResourcesVec r)
    {
        return l.wood < r.wood
            && l.stone < r.stone
            && l.gold < r.gold
            && l.ppl < r.ppl;
    }


    public static bool operator >(ResourcesVec l, ResourcesVec r)
    {
        return l.wood > r.wood
            && l.stone > r.stone
            && l.gold > r.gold
            && l.ppl > r.ppl;
    }

    public string ToStringTMP()
    {
        StringBuilder sb = new();
        if (wood != 0)
        {
            sb.Append(wood);
            sb.Append("<sprite name=\"icon_wood\">");
        }
        if (stone != 0)
        {
            if (sb.Length > 0)
            {
                sb.Append(" ");
            }
            sb.Append(stone);
            sb.Append("<sprite name=\"icon_stone\">");
        }
        if (gold != 0)
        {
            if (sb.Length > 0)
            {
                sb.Append(" ");
            }
            sb.Append(gold);
            sb.Append("<sprite name=\"icon_steel\">");
        }
        if (ppl != 0)
        {
            if (sb.Length > 0)
            {
                sb.Append(" ");
            }
            sb.Append(ppl);
            sb.Append("<sprite name=\"icon_ppl\">");
        }
        return sb.ToString();
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        if (wood != 0)
        {
            sb.Append(wood);
            sb.Append(" W");
        }
        if (stone != 0)
        {
            sb.Append(stone);
            sb.Append(" S");
        }
        if (gold != 0)
        {
            sb.Append(gold);
            sb.Append(" G");
        }
        if (ppl != 0)
        {
            sb.Append(ppl);
            sb.Append(" P");
        }
        return sb.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return this == (ResourcesVec)obj;
    }

    public override int GetHashCode()
    {
        return 7 * (wood + 7 * (stone + 7 * (gold + 7 * ppl)));
    }

    public static ResourcesVec Zero()
    {
        return new(0, 0, 0, 0);
    }

    public ResourcesVec AddWood(int wood)
    {
        this.wood += wood;
        return this;
    }

    public ResourcesVec AddStone(int stone)
    {
        this.stone += stone;
        return this;
    }

    public ResourcesVec AddGold(int gold)
    {
        this.gold += gold;
        return this;
    }

    public ResourcesVec AddPpl(int ppl)
    {
        this.ppl += ppl;
        return this;
    }
}