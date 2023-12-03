using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Image levelImage;
    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private TMP_Text levelCurrencyText;
    
    public void SetLevel(Level level)
    {
        levelImage.sprite = level.thumbnail;
        levelNameText.text = level.name;
        levelCurrencyText.text = $"{level.globalCurrencyGained} / {level.globalCurrencyToUnlock}";
    }
}
