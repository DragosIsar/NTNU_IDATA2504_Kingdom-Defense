using System;
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
        towerDamageText.text = $"Damage: {tower.damage}";
        towerRangeText.text = $"Range: {tower.range}";
        towerAttackRateText.text = $"Attack Rate: {string.Format(tower.attackRate % 1 == 0 ? "{0:0}" : "{0:0.00}", tower.attackRate)}/s";
        sellButton.GetComponentInChildren<TMP_Text>().text = $"Sell (+{tower.sellValue})";
        upgradeButton.GetComponentInChildren<TMP_Text>().text = $"Upgrade (-{tower.upgradeCost})";
        
        upgradeButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();
        
        upgradeButton.interactable = tower.towerTier < tower.settings.maxTier;
        
        sellButton.onClick.AddListener(() =>
        {
            LevelManager.Instance.SellTower(tower);
            gameObject.SetActive(false);
            GameManager.Player.SetPlayerState(PlayerState.None);
        });
        
        upgradeButton.onClick.AddListener(() =>
        {
            if (LevelManager.Instance.TryUpgradeTower(tower))
            {
                SetTower(tower);
                tower.ShowRangeIndicator(true);
            }
        });
    }
}
