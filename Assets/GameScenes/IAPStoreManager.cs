using UnityEngine;

public class IAPStoreManager : MonoBehaviour
{
    public void removeAds()
    {

    }

    public void unlockAllLevels()
    {

    }

    public void unlockAllPlayers()
    {

    }

    public void unlockEverything()
    {

    }

    public void addCoins(int coins)
    {
        SaveData.instance.Coins += coins;
        
    }

    public void addGems(int gems)
    {
        SaveData.instance.Gems += gems;

    }
}
