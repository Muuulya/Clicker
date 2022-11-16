using Clicker;
using UnityEngine;
using UnityEngine.UI;

public class HeaderPanel : MonoBehaviour
{
    [SerializeField] private Text _moneyText;
    [SerializeField] private Earnings _earnings;
    
    void Start()
    {
        _moneyText.text = "0";
    }

    void Update()
    {
        _moneyText.text = _earnings.Money.ToString();
    }
}
