using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour
{
    private static LevelUpManager instance;

    [Header("Upgrade Data")]
    [SerializeField] private UpgradeData[] upgrades;

    [Header("Level Up Settings")]
    [SerializeField] private int numberOfChoices = 3;

    private PlayerExperience playerExperience;
    private PlayerController playerController;
    private PlayerHealth playerHealth;

    private Canvas canvas;
    private GameObject panel;
    private Text titleText;

    private Button[] upgradeButtons;
    private Text[] upgradeButtonTexts;

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

        upgrades = Resources.LoadAll<UpgradeData>("Upgrades");

        Debug.Log($"Loaded UpgradeData: {upgrades.Length}");

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

        PlayerExperience newExperience =
            player.GetComponent<PlayerExperience>();

        PlayerController newController =
            player.GetComponent<PlayerController>();

        PlayerHealth newHealth =
            player.GetComponent<PlayerHealth>();

        if (newExperience == null ||
            newController == null ||
            newHealth == null)
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

        titleText.text = $"Level {level} reached";

        GenerateRandomChoices();

        panel.SetActive(true);
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

    private void GenerateRandomChoices()
    {
        if (upgrades == null || upgrades.Length == 0)
        {
            Debug.LogWarning(
                "No UpgradeData found in Resources/Upgrades."
            );

            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                upgradeButtons[i].gameObject.SetActive(false);
            }

            return;
        }

        List<UpgradeData> availableUpgrades =
            new List<UpgradeData>(upgrades);

        int choiceCount = Mathf.Min(
            numberOfChoices,
            upgradeButtons.Length,
            availableUpgrades.Count
        );

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i >= choiceCount)
            {
                upgradeButtons[i].gameObject.SetActive(false);
                continue;
            }

            UpgradeData selectedUpgrade =
                GetRandomWeightedUpgrade(
                    availableUpgrades
                 );

            if (selectedUpgrade == null)
            {
                continue;
            }

            availableUpgrades.Remove(
                selectedUpgrade
            );

            float rolledValue =
                selectedUpgrade.RollValue();

            SetUpgradeButton(
                i,
                selectedUpgrade,
                rolledValue
            );
        }
    }

    private void SetUpgradeButton(
        int index,
        UpgradeData upgrade,
        float rolledValue
    )
    {
        upgradeButtons[index].gameObject.SetActive(true);

        string valueText =
            FormatUpgradeValue(
                upgrade.UpgradeType,
                rolledValue
            );

        string buttonText =
            $"{upgrade.UpgradeName}\n" +
            $"{upgrade.Description}\n" +
            $"{valueText}";

        upgradeButtonTexts[index].text = buttonText;

        upgradeButtons[index].onClick.RemoveAllListeners();

        upgradeButtons[index].onClick.AddListener(() =>
        {
            ApplyUpgrade(
                upgrade,
                rolledValue
            );
        });
    }

    private string FormatUpgradeValue(
        UpgradeType upgradeType,
        float value
    )
    {
        switch (upgradeType)
        {
            case UpgradeType.MoveSpeed:
            case UpgradeType.AttackDamage:
            case UpgradeType.MaxHealth:
                return $"+{value:0.##}";

            default:
                return $"Value: {value:0.##}";
        }
    }

    private void ApplyUpgrade(
        UpgradeData upgrade,
        float rolledValue
    )
    {
        if (upgrade == null)
        {
            return;
        }

        Debug.Log(
            $"Upgrade selected: {upgrade.UpgradeName} " +
            $"(+{rolledValue:0.##})"
        );

        switch (upgrade.UpgradeType)
        {
            case UpgradeType.MoveSpeed:

                if (playerController != null)
                {
                    playerController.IncreaseMoveSpeed(
                        rolledValue
                    );
                }

                break;

            case UpgradeType.AttackDamage:

                if (playerController != null)
                {
                    playerController.IncreaseAttackDamage(
                        Mathf.RoundToInt(rolledValue)
                    );
                }

                break;

            case UpgradeType.MaxHealth:

                if (playerHealth != null)
                {
                    playerHealth.IncreaseMaxHealth(
                        Mathf.RoundToInt(rolledValue)
                    );
                }

                break;

            case UpgradeType.NewWeapon:

                Debug.Log(
                    $"New weapon upgrade selected: " +
                    $"{upgrade.UpgradeName}"
                );

                break;

            case UpgradeType.WeaponUpgrade:

                Debug.Log(
                    $"Weapon upgrade selected: " +
                    $"{upgrade.UpgradeName}"
                );

                break;

            case UpgradeType.NewPart:

                Debug.Log(
                    $"New part upgrade selected: " +
                    $"{upgrade.UpgradeName}"
                );

                break;

            case UpgradeType.PartUpgrade:

                Debug.Log(
                    $"Part upgrade selected: " +
                    $"{upgrade.UpgradeName}"
                );

                break;

            default:

                Debug.LogWarning(
                    $"Unknown upgrade type: " +
                    $"{upgrade.UpgradeType}"
                );

                break;
        }

        HideLevelUpUI();
    }

    private void CreateUI()
    {
        GameObject canvasObject =
            new GameObject("LevelUpCanvas");

        canvasObject.transform.SetParent(
            transform,
            false
        );

        canvas =
            canvasObject.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler =
            canvasObject.AddComponent<CanvasScaler>();

        scaler.uiScaleMode =
            CanvasScaler.ScaleMode.ScaleWithScreenSize;

        canvasObject.AddComponent<GraphicRaycaster>();

        panel =
            CreatePanel(canvas.transform);

        titleText =
            CreateText(
                panel.transform,
                "Choose one upgrade",
                34,
                new Vector2(0f, 150f),
                new Vector2(560f, 60f)
            );

        upgradeButtons =
            new Button[numberOfChoices];

        upgradeButtonTexts =
            new Text[numberOfChoices];

        for (int i = 0; i < numberOfChoices; i++)
        {
            float yPosition =
                40f - (i * 90f);

            Button button =
                CreateButton(
                    panel.transform,
                    $"Upgrade {i + 1}",
                    new Vector2(0f, yPosition),
                    null
                );

            upgradeButtons[i] = button;

            Text buttonText =
                button.GetComponentInChildren<Text>();

            upgradeButtonTexts[i] =
                buttonText;
        }

        panel.SetActive(false);
    }

    private GameObject CreatePanel(
        Transform parent
    )
    {
        GameObject panelObject =
            new GameObject("LevelUpPanel");

        panelObject.transform.SetParent(
            parent,
            false
        );

        Image image =
            panelObject.AddComponent<Image>();

        image.color =
            new Color(
                0f,
                0f,
                0f,
                0.72f
            );

        RectTransform rectTransform =
            panelObject.GetComponent<RectTransform>();

        rectTransform.anchorMin =
            Vector2.zero;

        rectTransform.anchorMax =
            Vector2.one;

        rectTransform.offsetMin =
            Vector2.zero;

        rectTransform.offsetMax =
            Vector2.zero;

        return panelObject;
    }

    private Text CreateText(
        Transform parent,
        string content,
        int fontSize,
        Vector2 anchoredPosition,
        Vector2 size
    )
    {
        GameObject textObject =
            new GameObject("LevelUpText");

        textObject.transform.SetParent(
            parent,
            false
        );

        Text text =
            textObject.AddComponent<Text>();

        text.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        text.text =
            content;

        text.fontSize =
            fontSize;

        text.alignment =
            TextAnchor.MiddleCenter;

        text.color =
            Color.white;

        RectTransform rectTransform =
            textObject.GetComponent<RectTransform>();

        rectTransform.sizeDelta =
            size;

        rectTransform.anchoredPosition =
            anchoredPosition;

        return text;
    }

    private Button CreateButton(
        Transform parent,
        string label,
        Vector2 anchoredPosition,
        Action onClick
    )
    {
        GameObject buttonObject =
            new GameObject(label);

        buttonObject.transform.SetParent(
            parent,
            false
        );

        Image image =
            buttonObject.AddComponent<Image>();

        image.color =
            new Color(
                0.18f,
                0.18f,
                0.18f,
                0.95f
            );

        Button button =
            buttonObject.AddComponent<Button>();

        if (onClick != null)
        {
            button.onClick.AddListener(
                () => onClick()
            );
        }

        RectTransform rectTransform =
            buttonObject.GetComponent<RectTransform>();

        rectTransform.sizeDelta =
            new Vector2(
                360f,
                70f
            );

        rectTransform.anchoredPosition =
            anchoredPosition;

        GameObject labelObject =
            new GameObject("Text");

        labelObject.transform.SetParent(
            buttonObject.transform,
            false
        );

        Text text =
            labelObject.AddComponent<Text>();

        text.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf"
            );

        text.text =
            label;

        text.fontSize =
            20;

        text.alignment =
            TextAnchor.MiddleCenter;

        text.color =
            Color.white;

        RectTransform labelRect =
            labelObject.GetComponent<RectTransform>();

        labelRect.anchorMin =
            Vector2.zero;

        labelRect.anchorMax =
            Vector2.one;

        labelRect.offsetMin =
            Vector2.zero;

        labelRect.offsetMax =
            Vector2.zero;

        return button;
    }

    private void EnsureEventSystem()
    {
        if (FindFirstObjectByType<EventSystem>() != null)
        {
            return;
        }

        GameObject eventSystemObject =
            new GameObject("EventSystem");

        eventSystemObject.AddComponent<EventSystem>();

        eventSystemObject.AddComponent<StandaloneInputModule>();

        DontDestroyOnLoad(
            eventSystemObject
        );
    }
    private UpgradeData GetRandomWeightedUpgrade(
    List<UpgradeData> availableUpgrades)
    {
        if (availableUpgrades == null ||
            availableUpgrades.Count == 0)
        {
            return null;
        }

        float totalWeight = 0f;

        foreach (UpgradeData upgrade in availableUpgrades)
        {
            if (upgrade == null)
            {
                continue;
            }

            totalWeight += Mathf.Max(0f, upgrade.Weight);
        }

        if (totalWeight <= 0f)
        {
            return availableUpgrades[
                UnityEngine.Random.Range(
                    0,
                    availableUpgrades.Count
                )
            ];
        }

        float randomValue =
            UnityEngine.Random.Range(0f, totalWeight);

        float currentWeight = 0f;

        foreach (UpgradeData upgrade in availableUpgrades)
        {
            if (upgrade == null)
            {
                continue;
            }

            currentWeight += Mathf.Max(
                0f,
                upgrade.Weight
            );

            if (randomValue <= currentWeight)
            {
                return upgrade;
            }
        }

        return availableUpgrades[
            availableUpgrades.Count - 1
        ];
    }
}