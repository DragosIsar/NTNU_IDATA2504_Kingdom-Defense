using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static GameManager;
using Random = UnityEngine.Random;
using LRS;

public class LevelManager : Singleton<LevelManager>
{
    public event Action<int> OnCurrencyChanged;
    public Action<int> onHealthChanged;
    
    [SerializeField] private Level level;
    [SerializeField] public WaveSpawner waveSpawner;

    private int _baseHealth;

    public int inLevelCurrency;
    private int _levelScore = -1;

    private List<Tower> _towers;
    private Tower _towerToPlace;

    public LayerMask antiTowerPlacementLayerMask;
    public LayerMask placementLayerMask;
    
    private bool _gameEnded;
    
    private Tower _currentGhostTower;
    private Tower _selectedTower;

    protected override void Awake()
    {
        base.Awake();
        useDontDestroyOnLoad = false;
        _baseHealth = level.maxBaseHealth;
        inLevelCurrency = level.startingCurrency;
        waveSpawner.SetWaves(level.waves);
    }
    
    public void DamageBase(int damage)
    {
        _baseHealth -= damage;
        if (_baseHealth <= 0)
        {
            _baseHealth = 0;
            GameOver();
        }
        onHealthChanged?.Invoke(_baseHealth);
    }

    public void CollectCurrency(int amount)
    {
        inLevelCurrency += amount;
        OnCurrencyChanged?.Invoke(inLevelCurrency);
    }
    
    public void SpendCurrency(int amount)
    {
        inLevelCurrency -= amount;
        OnCurrencyChanged?.Invoke(inLevelCurrency);
    }

    private void GameOver()
    {
        if (_gameEnded) return;
        _gameEnded = true;
        PauseGame();
        GameManager.Player.SetPlayerState(PlayerState.None);
        GameManager.HUD.ShowGameOverScreen();
    }

    public void GameWin()
    {
        if (_gameEnded) return;
        _gameEnded = true;
        
        if (level.levelToUnlock != null)
        {
            UnlockLevel(level.levelToUnlock);
        }

        PauseGame();
        GameManager.Player.SetPlayerState(PlayerState.None);
        CalculateScore();
        GrantCurrencyBasedOnScore();
        GameManager.HUD.ShowGameWinScreen();
    }
    
    public void GameDone()
    {
        if (_gameEnded) return;
        _gameEnded = true;
        PauseGame();
        GameManager.Player.SetPlayerState(PlayerState.None);
        CalculateScore();
        GrantCurrencyBasedOnScore();
        GameManager.HUD.ShowGameDoneScreen();
    }

    private void GrantCurrencyBasedOnScore()
    {
        int currency = level.globalCurrencyToUnlock * _levelScore / 100;
        if (currency > level.globalCurrencyGained)
        {
            currency -= level.globalCurrencyGained;
        }
        AddGlobalCurrency(currency);
        level.globalCurrencyGained += currency;
        GameManager.Instance.SaveLevelData();
    }

    private void CalculateScore()
    {
        _levelScore = _baseHealth / level.maxBaseHealth * 100;
    }

    public bool TryPlaceTower(Vector3 pos)
    {
        if (inLevelCurrency < _towerToPlace.settings.placementCost)
        {
            SetStatusText("Not enough currency!");
            return false;
        }

        if (!IsTowerLocationValid(pos, _towerToPlace) ||
            pos == Vector3.zero)
        {
            SetStatusText("Not enough space!");
            return false;
        }
        
        Instantiate(_towerToPlace, pos, Quaternion.identity);
        SpendCurrency(_towerToPlace.settings.placementCost);
        return true;
    }

    private bool IsTowerLocationValid(Vector3 pos, Tower tower)
    {
        if (Physics.CheckSphere(pos, tower.settings.placeRadius, antiTowerPlacementLayerMask))
        {
            return false;
        }
        
        if (Physics.Raycast(pos + Vector3.up, Vector3.down, 3f, placementLayerMask))
        {
            return true;
        }

        return false;

    }

    public void ShowGhostTower(Transform parent)
    {
        Vector3 pos = parent.position;
        
        if (_currentGhostTower != _towerToPlace)
        {
            if (_currentGhostTower)
            {
                Destroy(_currentGhostTower.gameObject);
            }
            _currentGhostTower = Instantiate(_towerToPlace, pos, Quaternion.identity, parent);
            _currentGhostTower.GetComponentsInChildren<Collider>().ToList().ForEach(c => c.enabled = false);
        }
        else
        {
            _currentGhostTower.transform.position = pos;
        }
        
        _currentGhostTower.SetGhostMaterial(IsTowerLocationValid(pos, _towerToPlace));
        _currentGhostTower.ShowRangeIndicator(true);
    }
    
    public void HideGhostTower()
    {
        if (_currentGhostTower)
        {
            Destroy(_currentGhostTower.gameObject);
        }
    }

    public void SellTower(Tower tower)
    {
        CollectCurrency(tower.settings.sellValue);
        Destroy(tower.gameObject);
    }
    
    public int GetBaseHealth()
    {
        return _baseHealth;
    }

    public void SetTowerToPlace(Tower tower)
    {
        _towerToPlace = tower;
        GameManager.Player.SetPlayerState(PlayerState.TowerPlacement);
    }
    
    public static void SetStatusText(string text, float duration = 2f)
    {
        GameManager.HUD.SetStatusText(text, duration);
    }

    public void ReloadLevel()
    {
        LoadLevel(level);
    }

    public void LoadMainMenu()
    {
        GameManager.LoadMainMenu();
    }

    public float GetBaseMaxHealth()
    {
        return level.maxBaseHealth;
    }

    public string GetLevelScore()
    {
        if (_levelScore == -1)
        {
            CalculateScore();
        }
        
        return _levelScore.ToString("F0");
    }

    public void LoadNextLevel()
    {
        if (level.levelToUnlock != null)
        {
            LoadLevel(level.levelToUnlock);
        }
        else
        {
            GameManager.LoadMainMenu();
        }
    }

    public bool TrySelectTower(Vector3 pos, out Tower tower)
    {
        tower = null;
        Collider[] hits = Physics.OverlapSphere(pos, 5f);
        
        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out tower))
            {
                DeselectTower();
                _selectedTower = tower;
                _selectedTower.ShowRangeIndicator(true);
                GameManager.HUD.ShowTowerDetails(tower);
                return true;
            }
        }

        return false;
    }
    
    public void DeselectTower()
    {
        GameManager.HUD.HideTowerDetails();
        if (_selectedTower)
        {
            _selectedTower.ShowRangeIndicator(false);
            _selectedTower = null;
        }
    }
    
    public bool TryUpgradeTower(Tower tower = null)
    {
        if (tower == null)
        {
            tower = _selectedTower;
        }
        
        if (inLevelCurrency < tower.settings.upgradeCost)
        {
            SetStatusText("Not enough currency!");
            return false;
        }

        if (tower.towerTier >= tower.settings.maxTier)
        {
            SetStatusText("Max tier reached!");
            return false;
        }
        
        tower.Upgrade();
        SpendCurrency(tower.settings.upgradeCost);
        return true;
    }

    public Level GetLevel()        
    {
        return level;
    }
}