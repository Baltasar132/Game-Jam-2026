using TMPro;
using UnityEngine;

public class GlobalTextUpdater : MonoBehaviour
{
    [HideInInspector] public TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        text.SetText(ResourceManager.GetRes().ToStringTMP());
    }
}
