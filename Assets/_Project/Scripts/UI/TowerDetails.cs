using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerDetails : MonoBehaviour
{
    [SerializeField] private TMP_Text towerNameText;
    [SerializeField] private TMP_Text towerTierText;
    [SerializeField] private TMP_Text towerDamageText;
    [SerializeField] private TMP_Text towerRangeText;
    [SerializeField] private TMP_Text towerAttackRateText;
    
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button sellButton;
    
    public void SetTower(Tower tower)
    {
        towerNameText.text = tower.settings.towerName;
        towerTierText.text = $"Tier {tower.towerTier}/{tower.settings.maxTier}";
        towerDamageText.text = $"Damage - {tower.settings.damage}";
        towerRangeText.text = $"Range - {tower.settings.range}";
        towerAttackRateText.text = $"Attack Rate - {tower.settings.attackRate}";
        
        upgradeButton.interactable = tower.towerTier < tower.settings.maxTier;
        
        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(() =>
        {
            LevelManager.Instance.SellTower(tower);
            gameObject.SetActive(false);
            GameManager.Player.SetPlayerState(PlayerState.None);
        });
        
        upgradeButton.onClick.AddListener(() =>
        {
            tower.Upgrade();
            SetTower(tower);
        });
    }
}
