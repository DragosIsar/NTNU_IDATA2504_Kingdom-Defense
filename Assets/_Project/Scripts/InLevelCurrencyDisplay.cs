using TMPro;
using UnityEngine;

public class InLevelCurrencyDisplay : MonoBehaviour
{
    private TMP_Text _text;
    
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }
    
    private void OnEnable()
    {
        LevelManager.Instance.OnCurrencyChanged += OnCurrencyChangedHandler;
    }
    
    private void OnDisable()
    {
        LevelManager.Instance.OnCurrencyChanged -= OnCurrencyChangedHandler;
    }
    
    private void OnCurrencyChangedHandler(int currency)
    {
        _text.text = currency.ToString();
    }
}
