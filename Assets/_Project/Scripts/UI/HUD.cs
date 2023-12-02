using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using LRS;

public class HUD : MonoBehaviour
{
    [Header("TowerPlacement")]
    [SerializeField] private Toggle towerPlacementTogglePrefab;
    [SerializeField] private RectTransform towerPlacementPanel;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text globalCurrencyText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TowerDetails towerDetails;
    
    private List<Toggle> _towerPlacementToggles = new();
    private ToggleGroup _toggleGroup;
    
    private Coroutine _statusTextCoroutine;
    
    private bool _showTime = true;
    
    private void Awake()
    {
        _toggleGroup = towerPlacementPanel.GetComponent<ToggleGroup>();
    }
    
    private void Start()
    {
        CreateTowerPlacementToggles();
    }

    private void Update()
    {
        if (_showTime) statusText.text = LevelManager.Instance.GetCurrentLevelTimeFormatted();
    }
    
    private void CreateTowerPlacementToggles()
    {
        GameManager.Instance.towers
            .Where(tower => tower.settings.isUnlocked)
            .ToList()
            .ForEach(tower =>
            {
                Toggle toggle = Instantiate(towerPlacementTogglePrefab, towerPlacementPanel);
                toggle.group = _toggleGroup;
                toggle.isOn = false;
                toggle.onValueChanged.AddListener(isOn =>
                {
                    if (isOn)
                    {
                        LevelManager.Instance.SetTowerToPlace(tower);
                    }
                    else
                    {
                        LevelManager.Instance.HideGhostTower();
                    }
                });
                TowerToggleUI towerToggleUi = toggle.GetComponent<TowerToggleUI>();
                towerToggleUi.SetIcon(tower.settings.icon);
                towerToggleUi.SetName(tower.settings.towerName);
                towerToggleUi.SetCost(tower.settings.placementCost);
                _towerPlacementToggles.Add(toggle);
            });
    }
    
    public void SwitchOffAllToggles()
    {
        _towerPlacementToggles.ForEach(toggle => toggle.isOn = false);
    }
    
    public void SetStatusText(string text, float duration)
    {
        if (_statusTextCoroutine != null)
        {
            StopCoroutine(_statusTextCoroutine);
        }
        
        _statusTextCoroutine = StartCoroutine(SetStatusTextCoroutine(text, duration));
    }
    
    private IEnumerator SetStatusTextCoroutine(string text, float duration)
    {
        _showTime = false;
        statusText.text = text;
        yield return new WaitForSeconds(duration);
        _showTime = true;
    }
    
    public void ShowGameOverScreen()
    {
        gameOverPanel.SetActive(true);
    }
    
    public void ShowGameWinScreen()
    {
        scoreText.text = $"Score: {LevelManager.Instance.GetLevelScore()}";
        globalCurrencyText.text = $"Total Global Currency: {GameManager.GlobalCurrency}";
        winPanel.SetActive(true);
    }

    public void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        GameManager.SetPauseGame(pauseMenu.activeSelf);
    }

    public void ShowTowerDetails(Tower tower)
    {
        towerDetails.gameObject.SetActive(true);
        towerDetails.SetTower(tower);
    }
    
    public void HideTowerDetails()
    {
        towerDetails.gameObject.SetActive(false);
    }
}
