using System;
using TMPro;
using UnityEngine;

public class UpgradeTextThingy : MonoBehaviour
{
    [SerializeField] private Upgrade upgrade;
    [HideInInspector] public TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        switch (upgrade)
        {
            case Upgrade.StoneUpgrade:
                text.SetText("<sprite name=\"icon_stone\"> Upgrade (" + PriceManager.getStoneLevelPrice().ToStringTMP() + ")");
                break;
            case Upgrade.WoodUpgrade:
                text.SetText("<sprite name=\"icon_wood\"> Upgrade (" + PriceManager.getWoodLevelPrice().ToStringTMP() + ")");
                break;
            case Upgrade.StoneWorker:
                text.SetText("<sprite name=\"icon_stone\"> Worker(" + PriceManager.getStoneWorkerPrice().ToStringTMP() + ")");
                break;
            case Upgrade.WoodWorker:
                text.SetText("<sprite name=\"icon_wood\"> Worker(" + PriceManager.getWoodWorkerPrice().ToStringTMP() + ")");
                break;
        }
    }
}

[Serializable]
public enum Upgrade
{
    StoneUpgrade, WoodUpgrade, StoneWorker, WoodWorker
}
