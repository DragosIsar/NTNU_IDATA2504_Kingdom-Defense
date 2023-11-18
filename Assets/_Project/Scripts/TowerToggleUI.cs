using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerToggleUI : MonoBehaviour
{
    [SerializeField] private Image image;
    
    private void Awake()
    {
        image ??= GetComponentInChildren<Image>();
    }
    
    public void SetIcon(Sprite icon)
    {
        image.sprite = icon;
    }
}
