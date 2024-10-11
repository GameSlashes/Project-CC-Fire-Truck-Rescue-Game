using UnityEngine;

public class CoinsCollection : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SaveData.instance.Coins += 10;
            //GameController.instance.Game_Elements.totalCoinsCollect++;
        }
    }
}
