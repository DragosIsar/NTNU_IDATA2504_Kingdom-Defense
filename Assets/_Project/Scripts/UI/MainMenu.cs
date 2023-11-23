using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform levelButtonContainer;
    [SerializeField] private TowerUnlockUI shopItemPrefab;
    [SerializeField] private Transform shopItemContainer;
    [SerializeField] private TMP_Text globalCurrencyText;
    
    private void Start()
    {
        CreateLevelButtons();
        CreateShopItems();
        globalCurrencyText.text = GameManager.GlobalCurrency.ToString();
        GameManager.OnGlobalCurrencyChanged += OnGlobalCurrencyChangedHandler;
    }
    
    private void OnGlobalCurrencyChangedHandler(int globalCurrency)
    {
        globalCurrencyText.text = globalCurrency.ToString();
    }
    
    private void CreateLevelButtons()
    {
        foreach (Level level in GameManager.Instance.levels)
        {
            GameObject levelButton = Instantiate(levelButtonPrefab, levelButtonContainer);
            Button levelButtonComponent = levelButton.GetComponent<Button>();
            levelButtonComponent.onClick.AddListener(() =>
            {
                GameManager.LoadLevel(level);
            });
            
            TMP_Text levelButtonText = levelButton.GetComponentInChildren<TMP_Text>();
            levelButtonText.text = level.name;
            
            levelButtonComponent.interactable = level.isUnlocked;
            
        }
    }

    private void CreateShopItems()
    {
        foreach (Tower tower in GameManager.Instance.towers)
        {
            TowerUnlockUI shopItem = Instantiate(shopItemPrefab, shopItemContainer);
            shopItem.tower = tower;
            shopItem.towerIcon.sprite = tower.settings.icon;
            shopItem.towerName.text = tower.settings.towerName;
            shopItem.towerCost.text = tower.settings.unlockCost.ToString();
            shopItem.button.interactable = !tower.settings.isUnlocked;
            shopItem.button.onClick.AddListener(() =>
            {
                if (GameManager.Instance.UnlockTower(tower))
                {
                    shopItem.button.interactable = false;
                }
            });
        }
    }
}
