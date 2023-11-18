using TMPro;
using UnityEngine;

public class InLevelCurrencyDisplay : MonoBehaviour
{
    private TMP_Text _text;
    
    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        OnCurrencyChangedHandler(LevelManager.Instance.inLevelCurrency);
    }
    
    private void OnEnable()
    {
        LevelManager.Instance.OnCurrencyChanged += OnCurrencyChangedHandler;
    }
    
    private void OnCurrencyChangedHandler(int currency)
    {
        _text.text = currency.ToString();
    }
}
