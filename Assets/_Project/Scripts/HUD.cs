using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("TowerPlacement")]
    [SerializeField] private Toggle towerPlacementTogglePrefab;
    [SerializeField] private RectTransform towerPlacementPanel;
    
    private List<Toggle> _towerPlacementToggles = new();
    private ToggleGroup _toggleGroup;
    
    private void Awake()
    {
        _toggleGroup = towerPlacementPanel.GetComponent<ToggleGroup>();
    }
    
    private void Start()
    {
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
            toggle.GetComponent<TowerToggleUI>().SetIcon(tower.settings.icon);
            _towerPlacementToggles.Add(toggle);
        });
    }
    
    public void SwitchOffAllToggles()
    {
        _towerPlacementToggles.ForEach(toggle => toggle.isOn = false);
    }
}
