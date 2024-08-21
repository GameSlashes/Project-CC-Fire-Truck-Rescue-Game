using UnityEngine;

public class Players : MonoBehaviour
{
    public static Players instance;
    public GameObject[] players;
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        ActivePlayer();
    }
    void ActivePlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (i == SavepPlayerData.instance.finalPlayer)
            {
                players[i].SetActive(true);
            }
            else
            {
                Destroy(players[i]);
            }
        }
    }
}

