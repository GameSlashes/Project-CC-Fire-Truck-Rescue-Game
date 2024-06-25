using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    public static PlayerCustomization instance;

    public int playerID;
    public Color defaultColor;

    public TyresModel[] tyres;
    public GameObject[] spoilers;
    public GameObject[] decals;

    [HideInInspector] public int activeTyresID = 0;
    [HideInInspector] public int activeSpoilerID = 0;
    [HideInInspector] public int activeDecalsID = 0;


    public void OnEnable()
    {
        instance = this;
        checkTyres();
        checkSpoilers();
        checkDecals();
    }

    #region Tyres

    public void checkTyres()
    {
        for (int i = 0; i < tyres.Length; i++)
        {
            for (int j = 0; j < tyres[i].Models.Length; j++)
            {
                tyres[i].Models[j].SetActive(false);
            }
        }

        for (int k = 0; k < tyres.Length; k++)
        {
            tyres[k].Models[PlayerPrefs.GetInt("PlayerTyres" + playerID, 0)].SetActive(true);
        }
    }

    public void updateTyres()
    {
        for (int i = 0; i < tyres.Length; i++)
        {
            for (int j = 0; j < tyres[i].Models.Length; j++)
            {
                tyres[i].Models[j].SetActive(false);
            }
        }

        for (int k = 0; k < tyres.Length; k++)
        {
            tyres[k].Models[activeTyresID].SetActive(true);
        }

    }

    public void buyTyres()
    {
        for (int i = 0; i < tyres.Length; i++)
        {
            for (int j = 0; j < tyres[i].Models.Length; j++)
            {
                tyres[i].Models[j].SetActive(false);
            }
        }

        for (int k = 0; k < tyres.Length; k++)
        {
            tyres[k].Models[activeTyresID].SetActive(true);
        }

        PlayerPrefs.SetInt("PlayerTyres" + playerID, activeTyresID);
        PlayerPrefs.SetInt("tyrePurchased" + playerID + activeTyresID, 2);
    }

    #endregion

    #region Spoilers

    public void updateSpoilers()
    {
        for (int i = 0; i < spoilers.Length; i++)
        {
            spoilers[i].SetActive(false);
        }


        spoilers[activeSpoilerID].SetActive(true);


    }

    public void buySpoilers()
    {
        for (int i = 0; i < spoilers.Length; i++)
        {

            spoilers[i].SetActive(false);

        }


        spoilers[activeSpoilerID].SetActive(true);

        PlayerPrefs.SetInt("PlayerSpoilers" + playerID, activeSpoilerID);
        PlayerPrefs.SetInt("spoilerPurchased" + playerID + activeSpoilerID, 2);
    }

    public void checkSpoilers()
    {
        for (int i = 0; i < spoilers.Length; i++)
        {
            spoilers[i].SetActive(false);
        }


        if (PlayerPrefs.HasKey("PlayerSpoilers" + playerID))
        {
            spoilers[PlayerPrefs.GetInt("PlayerSpoilers" + playerID)].SetActive(true);
        }
    }

    #endregion

    #region Decals
    public void updateDecals()
    {
        for (int i = 0; i < decals.Length; i++)
        {
            decals[i].SetActive(false);
        }

        decals[activeDecalsID].SetActive(true);
    }

    public void buyDecals()
    {
        for (int i = 0; i < decals.Length; i++)
        {
            decals[i].SetActive(false);
        }

        decals[activeDecalsID].SetActive(true);

        PlayerPrefs.SetInt("PlayerDecals" + playerID, activeDecalsID);
        PlayerPrefs.SetInt("decalsPurchased" + playerID + activeDecalsID, 2);
    }

    public void checkDecals()
    {
        for (int i = 0; i < decals.Length; i++)
        {
            decals[i].SetActive(false);
        }

        if (PlayerPrefs.HasKey("PlayerDecals" + playerID))
        {
            decals[PlayerPrefs.GetInt("PlayerDecals" + playerID)].SetActive(true);
        }
    }
    #endregion

}
