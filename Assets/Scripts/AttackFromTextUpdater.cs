using TMPro;
using UnityEngine;

public class AttackFromTextUpdater : MonoBehaviour
{

    [HideInInspector] public TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.SetText("Attack from: " + WaveManager.CurrentWave.Direction);
    }
}
