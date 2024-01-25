using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolTipUI : MonoBehaviour
{
    public static ToolTipUI Instance;

    [SerializeField] private RectTransform canvasRect;
    private RectTransform backgroundRect;
    private TextMeshProUGUI textMeshPro;
    private TooltipTimer timer;

    private RectTransform uiRect;

    void Awake()
    {
        Instance = this;

        uiRect = GetComponent<RectTransform>();
        textMeshPro = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        backgroundRect = transform.Find("Background").GetComponent<RectTransform>();

        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        HandleFollowMouse();

        if (timer != null)
        {
            timer.timer -= Time.deltaTime;
            if (timer.timer <= 0)
            {
                Hide();
                timer = null;
            }
        }
    }

    void HandleFollowMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRect.localScale.x;

        if (anchoredPosition.x + backgroundRect.rect.width > canvasRect.rect.width)
        {
            anchoredPosition.x = canvasRect.rect.width - backgroundRect.rect.width;
        }
        if (anchoredPosition.y + backgroundRect.rect.height > canvasRect.rect.height)
        {
            anchoredPosition.y = canvasRect.rect.height - backgroundRect.rect.height;
        }


        uiRect.anchoredPosition = anchoredPosition;
    }

    void SetText(string tip)
    {
        textMeshPro.text = tip;
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        backgroundRect.sizeDelta = textSize + new Vector2(10, 10);
    }

    public void Show(string tip, TooltipTimer timer = null)
    {
        SetText(tip);
        this.timer = timer;
        this.gameObject.SetActive(true);
        HandleFollowMouse();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public class TooltipTimer
    {
        public float timer;
    }
}
