using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI INSTANCE { get; private set; }

    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private RectTransform backgroundRect;
    [SerializeField] private Vector2 offset = new(15f, -15f);

    private RectTransform rectTransform;

    private void Awake()
    {
        if (INSTANCE == null) INSTANCE = this;
        else Destroy(gameObject);

        rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    private void Update()
    {
        // mover más tarde
        // Vector2 mousePos = Mouse.current.position.ReadValue();
        // rectTransform.position = mousePos + offset;
    }

    public void Show(string text)
    {
        gameObject.SetActive(true);
        tooltipText.text = text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRect);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}