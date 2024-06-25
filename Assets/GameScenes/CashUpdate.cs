using UnityEngine;
using UnityEngine.UI;

public class CashUpdate : MonoBehaviour
{
    public Text CashText;
    public Text GemsText;

    public bool cash;
    public bool gems;

    private SaveData database;

    public void Awake()
    {
        database = SaveData.instance;
    }

    private void OnEnable()
    {
        if(cash)
        {
            SaveData.allCashUpdate += updateCash;
        }
        else if(gems)
        {
            SaveData.allGemsUpdate += updateCash;
        }
    }

    private void OnDisable()
    {
        if(cash)
        {
            SaveData.allCashUpdate -= updateCash;
        }
        else if(gems)
        {
            SaveData.allGemsUpdate -= updateCash;
        }
        
    }

    public void Start()
    {
       updateCash();
    }

    public void updateCash()
    {
        if(cash)
        {
            CashText.text = database.Coins.ToString();
        }
        else if(gems)
        {
            GemsText.text = database.Gems.ToString();
        }
    }

    
}
