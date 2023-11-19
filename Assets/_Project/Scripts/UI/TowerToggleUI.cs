using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerToggleUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;
    
    private void Awake()
    {
        image ??= GetComponentInChildren<Image>();
    }
    
    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
    }
    
    public void SetName(string name)
    {
        nameText.text = name;
    }
    
    public void SetCost(int cost)
    {
        costText.text = cost.ToString();
    }
}
