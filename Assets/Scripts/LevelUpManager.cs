using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    private static LevelUpManager instance;

    private PlayerExperience playerExperience;
    private PlayerController playerController;
    private PlayerHealth playerHealth;

    private Canvas canvas;
    private GameObject panel;
    private Text titleText;
    private Button moveSpeedButton;
    private Button attackButton;
    private Button healthButton;

    private bool isShowing;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Bootstrap()
    {
        if (instance != null)
        {
            return;
        }

        GameObject managerObject = new GameObject(nameof(LevelUpManager));
        instance = managerObject.AddComponent<LevelUpManager>();
        DontDestroyOnLoad(managerObject);
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        EnsureEventSystem();
        CreateUI();
    }

    private void Start()
    {
        BindPlayer();
    }

    private void Update()
    {
        if (playerExperience == null)
        {
            BindPlayer();
        }
    }

    private void OnDestroy()
    {
        if (playerExperience != null)
        {
            playerExperience.LeveledUp -= HandleLeveledUp;
        }
    }

    private void BindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return;
        }

        PlayerExperience newExperience = player.GetComponent<PlayerExperience>();
        PlayerController newController = player.GetComponent<PlayerController>();
        PlayerHealth newHealth = player.GetComponent<PlayerHealth>();

        if (newExperience == null || newController == null || newHealth == null)
        {
            return;
        }

        if (playerExperience == newExperience)
        {
            return;
        }

        if (playerExperience != null)
        {
            playerExperience.LeveledUp -= HandleLeveledUp;
        }

        playerExperience = newExperience;
        playerController = newController;
        playerHealth = newHealth;
        playerExperience.LeveledUp += HandleLeveledUp;
    }

    private void HandleLeveledUp(int level)
    {
        ShowLevelUpUI(level);
    }

    private void ShowLevelUpUI(int level)
    {
        if (isShowing)
        {
            return;
        }

        isShowing = true;
        Time.timeScale = 0f;

        if (playerController != null)
        {
            playerController.SetInputEnabled(false);
        }

        panel.SetActive(true);
        titleText.text = $"Level {level} reached";
    }

    private void HideLevelUpUI()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
        isShowing = false;

        if (playerController != null)
        {
            playerController.SetInputEnabled(true);
        }
    }

    private void ApplyUpgrade(Action action)
    {
        if (action == null)
        {
            return;
        }

        action.Invoke();
        HideLevelUpUI();
    }

    private void CreateUI()
    {
        GameObject canvasObject = new GameObject("LevelUpCanvas");
        canvasObject.transform.SetParent(transform, false);

        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObject.AddComponent<GraphicRaycaster>();

        panel = CreatePanel(canvas.transform);
        titleText = CreateText(panel.transform, "Choose one upgrade", 34, new Vector2(0f, 120f), new Vector2(560f, 60f));

        moveSpeedButton = CreateButton(panel.transform, "Move Speed + 1", new Vector2(0f, 40f), () =>
        {
            if (playerController != null)
            {
                playerController.IncreaseMoveSpeed(1f);
            }
        });

        attackButton = CreateButton(panel.transform, "Attack Damage + 1", new Vector2(0f, -40f), () =>
        {
            if (playerController != null)
            {
                playerController.IncreaseAttackDamage(1);
            }
        });

        healthButton = CreateButton(panel.transform, "Max HP + 1", new Vector2(0f, -120f), () =>
        {
            if (playerHealth != null)
            {
                playerHealth.IncreaseMaxHealth(1);
            }
        });

        panel.SetActive(false);
    }

    private GameObject CreatePanel(Transform parent)
    {
        GameObject panelObject = new GameObject("LevelUpPanel");
        panelObject.transform.SetParent(parent, false);

        Image image = panelObject.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 0.72f);

        RectTransform rectTransform = panelObject.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;

        return panelObject;
    }

    private Text CreateText(Transform parent, string content, int fontSize, Vector2 anchoredPosition, Vector2 size)
    {
        GameObject textObject = new GameObject("LevelUpText");
        textObject.transform.SetParent(parent, false);

        Text text = textObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = content;
        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        RectTransform rectTransform = textObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = size;
        rectTransform.anchoredPosition = anchoredPosition;

        return text;
    }

    private Button CreateButton(Transform parent, string label, Vector2 anchoredPosition, Action onClick)
    {
        GameObject buttonObject = new GameObject(label);
        buttonObject.transform.SetParent(parent, false);

        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.18f, 0.18f, 0.18f, 0.95f);

        Button button = buttonObject.AddComponent<Button>();
        button.onClick.AddListener(() => ApplyUpgrade(onClick));

        RectTransform rectTransform = buttonObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(320f, 46f);
        rectTransform.anchoredPosition = anchoredPosition;

        GameObject labelObject = new GameObject("Text");
        labelObject.transform.SetParent(buttonObject.transform, false);

        Text text = labelObject.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = label;
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        RectTransform labelRect = labelObject.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        return button;
    }

    private void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();
        DontDestroyOnLoad(eventSystemObject);
    }
}
