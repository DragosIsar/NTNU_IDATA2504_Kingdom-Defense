using TMPro;
using UnityEngine;

public class DisplayGlobalSpeed : MonoBehaviour
{
    private TMP_Text _text;

    private void Start()
    {
        _text = GetComponent<TMP_Text>();
        _text.text = $"x{Time.timeScale}";
    }

    private void Update()
    {
        _text.text = $"x{Time.timeScale}";
    }
}
