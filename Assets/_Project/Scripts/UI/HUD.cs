using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("TowerPlacement")]
    [SerializeField] private Toggle towerPlacementTogglePrefab;
    [SerializeField] private RectTransform towerPlacementPanel;
    [SerializeField] private TMP_Text statusText;
    
    private List<Toggle> _towerPlacementToggles = new();
    private ToggleGroup _toggleGroup;
    
    private Coroutine _statusTextCoroutine;
    
    private void Awake()
    {
        _toggleGroup = towerPlacementPanel.GetComponent<ToggleGroup>();
    }
    
    private void Start()
    {
        statusText.text = "";
        CreateTowerPlacementToggles();
    }
    
    private void CreateTowerPlacementToggles()
    {
        GameManager.Instance.towers.ForEach(tower =>
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
            });
            TowerToggleUI towerToggleUi = toggle.GetComponent<TowerToggleUI>();
            towerToggleUi.SetIcon(tower.settings.icon);
            towerToggleUi.SetName(tower.settings.towerName);
            towerToggleUi.SetCost(tower.settings.cost);
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
        statusText.text = text;
        yield return new WaitForSeconds(duration);
        statusText.text = "";
    }
}
