using UnityEngine.UI;
using UnityEngine;

[System.Serializable]
public class playerStats
{
    public Material[] playerMat;
}

public class Customization : MonoBehaviour
{
    [Header("PlayerColors")]
    public Image defaultColor; 
    public Color[] allColors;
    public GameObject[] UI_ColorsObj;

    [Header("PlayerTyres")]
    public GameObject[] UI_TyresObj;

    [Header("PlayerSpoilers")]
    public GameObject[] UI_SpoilersObj;

    [Header("PlayerDecals")]
    public GameObject[] UI_DecalsObj;

    [Header("Players")]
    public playerStats[] Players;

    [Header("UI_Elements")]
    public GameObject insufficentCash;
    public GameObject successfullyPurchased;
    public GameObject buyColorsBtn;
    public GameObject buyTyresBtn;
    public GameObject buySpoilerBtn;
    public GameObject buyDecalsBtn;
    public GameObject colorBar;
    public GameObject tyreBar;

    [Header("Camera Positions")]
    public Transform cameraColorPosition;
    public Transform cameraTyrePosition;
    public Transform cameraSpoilerPosition;
    public Transform cameraDecalsPosition;

    bool colorPosition;
    bool tyrePosition;
    bool spoilerPosition;
    bool decalsPosition;

    int activeColorID = 0;

    public void OnEnable()
    {
        colors();
        defaultColor.color = FindObjectOfType<PlayerCustomization>().defaultColor;
        allColors[0] = FindObjectOfType<PlayerCustomization>().defaultColor;
        buyColorsBtn.SetActive(false);
        buyTyresBtn.SetActive(false);
        isColorPurchased();
        isTyrePurchased();
        isSpoilerPurchased();
        isDecalPurchased();
        onStart();
    }

    #region Color Setting

    public void isColorPurchased()
    {
        for (int i = 1; i < UI_ColorsObj.Length; i++)
        {
            if (PlayerPrefs.GetInt("colorPurchased" + PlayerCustomization.instance.playerID + i) == 2)
            {
                UI_ColorsObj[i].transform.GetChild(0).GetComponent<Text>().text = "Purchased";
            }
            else
            {
                UI_ColorsObj[i].transform.GetChild(0).GetComponent<Text>().text = "Price:100";
            }
        }
    }

    public void updateColor(int colorID)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }

        if (colorID != 0)
        {
            if (PlayerPrefs.GetInt("colorPurchased" + PlayerCustomization.instance.playerID + colorID) != 2)
            {
                buyColorsBtn.SetActive(true);
            }
            else
            {
                buyColorsBtn.SetActive(false);
                PlayerPrefs.SetInt("PlayerColor" + PlayerCustomization.instance.playerID + Players[PlayerSelection.instance.current], colorID);
            }
        }
        else
        {
            buyColorsBtn.SetActive(false);
            PlayerPrefs.SetInt("PlayerColor" + PlayerCustomization.instance.playerID + Players[PlayerSelection.instance.current], colorID);
        }

        for (int i = 0; i < Players[PlayerSelection.instance.current].playerMat.Length; i++)
        {
            Players[PlayerSelection.instance.current].playerMat[i].color = allColors[colorID];
        }

        activeColorID = colorID;
    }

    public void buyColor()
    {
        if (SaveData.instance.Coins >= 100)
        {
            for (int i = 0; i < Players[PlayerSelection.instance.current].playerMat.Length; i++)
            {
                Players[PlayerSelection.instance.current].playerMat[i].color = allColors[activeColorID];
                PlayerPrefs.SetInt("PlayerColor" + PlayerCustomization.instance.playerID + Players[PlayerSelection.instance.current], activeColorID);
            }
            SaveData.instance.Coins -= 100;
            PlayerPrefs.SetInt("colorPurchased" + PlayerCustomization.instance.playerID + activeColorID, 2);
            buyColorsBtn.SetActive(false);
            successfullyPurchased.SetActive(true);
            isColorPurchased();
        }
        else
        {
            insufficentCash.SetActive(true);
        }
    }

    public void colors()
    {
        colorPosition = true;
        tyrePosition = false;
        spoilerPosition = false;
        decalsPosition = false;
        buyTyresBtn.SetActive(false);
        buyColorsBtn.SetActive(false);
        buySpoilerBtn.SetActive(false);
        buyDecalsBtn.SetActive(false);
    }

    #endregion

    #region Tyre Setting

    public void isTyrePurchased()
    {
        for (int i = 1; i < UI_TyresObj.Length; i++)
        {
            if (PlayerPrefs.GetInt("tyrePurchased" + PlayerCustomization.instance.playerID + i) == 2)
            {
                UI_TyresObj[i].transform.GetChild(0).GetComponent<Text>().text = "Purchased";
            }
            else
            {
                UI_TyresObj[i].transform.GetChild(0).GetComponent<Text>().text = "Price:100";
            }
        }
    }

    public void updateTyres(int tyresID)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }


        PlayerCustomization.instance.activeTyresID = tyresID;
        PlayerCustomization.instance.updateTyres();

        if (tyresID != 0)
        {
            if (PlayerPrefs.GetInt("tyrePurchased" + PlayerCustomization.instance.playerID + tyresID) != 2)
            {
                buyTyresBtn.SetActive(true);
            }
            else
            {
                buyTyresBtn.SetActive(false);
                PlayerPrefs.SetInt("PlayerTyres" + PlayerCustomization.instance.playerID, tyresID);

            }
        }
        else
        {
            buyTyresBtn.SetActive(false);
            PlayerPrefs.SetInt("PlayerTyres" + PlayerCustomization.instance.playerID, tyresID);
        }

        
    }

    public void buyTyres()
    {
        if (SaveData.instance.Coins >= 100)
        {
            PlayerCustomization.instance.buyTyres();
            SaveData.instance.Coins -= 100;
            buyTyresBtn.SetActive(false);
            successfullyPurchased.SetActive(true);
            isTyrePurchased();
        }
        else
        {
            insufficentCash.SetActive(true);
        }
    }

    public void tyres()
    {
        colorPosition = false;
        tyrePosition = true;
        spoilerPosition = false;
        decalsPosition = false;
        buyTyresBtn.SetActive(false);
        buyColorsBtn.SetActive(false);
        buySpoilerBtn.SetActive(false);
        buyDecalsBtn.SetActive(false);
    }

    #endregion

    #region Camera Setting

    public void Update()
    {
        if (colorPosition)
            updateCameraPosition(cameraColorPosition);

        if(tyrePosition)
            updateCameraPosition(cameraTyrePosition);

        if(spoilerPosition)
            updateCameraPosition(cameraSpoilerPosition);

    }

    public void updateCameraPosition(Transform position)
    {
        PlayerSelection.instance.mainCamera.transform.position = Vector3.Lerp(PlayerSelection.instance.mainCamera.transform.position, position.position, Time.timeScale * 0.5f);
        PlayerSelection.instance.mainCamera.transform.rotation = Quaternion.Lerp(PlayerSelection.instance.mainCamera.transform.rotation, position.rotation, Time.timeScale * 0.5f);
    }

    #endregion

    #region Spoiler Setting

    public void isSpoilerPurchased()
    {
        for (int i = 1; i < UI_SpoilersObj.Length; i++)
        {
            if (PlayerPrefs.GetInt("spoilerPurchased" + PlayerCustomization.instance.playerID + i) == 2)
            {

                UI_SpoilersObj[i].transform.GetChild(0).GetComponent<Text>().text = "Purchased";
            }
            else
            {
                UI_SpoilersObj[i].transform.GetChild(0).GetComponent<Text>().text = "Price:100";
            }
        }
    }

    public void updateSpoiler(int SpoilerID)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }


        PlayerCustomization.instance.activeSpoilerID = SpoilerID;
        PlayerCustomization.instance.updateSpoilers();

        if (SpoilerID != 0)
        {
            if (PlayerPrefs.GetInt("spoilerPurchased" + PlayerCustomization.instance.playerID + SpoilerID) != 2)
            {
                buySpoilerBtn.SetActive(true);
            }
            else
            {
                buySpoilerBtn.SetActive(false);
                PlayerPrefs.SetInt("PlayerSpoilers" + PlayerCustomization.instance.playerID, SpoilerID);

            }
        }
        else
        {
            buySpoilerBtn.SetActive(false);
            PlayerPrefs.SetInt("PlayerSpoilers" + PlayerCustomization.instance.playerID, SpoilerID);
        }

    }

    public void buySpoiler()
    {
        if (SaveData.instance.Coins >= 100)
        {
            PlayerCustomization.instance.buySpoilers();
            SaveData.instance.Coins -= 100;
            buySpoilerBtn.SetActive(false);
            successfullyPurchased.SetActive(true);
            isSpoilerPurchased();
        }
        else
        {
            insufficentCash.SetActive(true);
        }
    }

    public void Spoilers()
    {
        colorPosition = false;
        tyrePosition = false;
        spoilerPosition = true;
        decalsPosition = false;
        buyTyresBtn.SetActive(false);
        buyColorsBtn.SetActive(false);
        buySpoilerBtn.SetActive(false);
        buyDecalsBtn.SetActive(false);
    }

    #endregion

    #region Decal Setting

    public void isDecalPurchased()
    {
        for (int i = 1; i < UI_DecalsObj.Length; i++)
        {
            if (PlayerPrefs.GetInt("decalsPurchased" + PlayerCustomization.instance.playerID + i) == 2)
            {

                UI_DecalsObj[i].transform.GetChild(0).GetComponent<Text>().text = "Purchased";
            }
            else
            {
                UI_DecalsObj[i].transform.GetChild(0).GetComponent<Text>().text = "Price:100";
            }
        }
    }

    public void updateDecals(int DecalsID)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }


        PlayerCustomization.instance.activeDecalsID = DecalsID;
        PlayerCustomization.instance.updateDecals();

        if (DecalsID != 0)
        {
            if (PlayerPrefs.GetInt("decalsPurchased" + PlayerCustomization.instance.playerID + DecalsID) != 2)
            {
                buyDecalsBtn.SetActive(true);
            }
            else
            {
                buyDecalsBtn.SetActive(false);
                PlayerPrefs.SetInt("PlayerDecals" + PlayerCustomization.instance.playerID, DecalsID);

            }
        }
        else
        {
            buyDecalsBtn.SetActive(false);
            PlayerPrefs.SetInt("PlayerDecals" + PlayerCustomization.instance.playerID, DecalsID);
        }
    }

    public void buyDecals()
    {
        if (SaveData.instance.Coins >= 100)
        {
            PlayerCustomization.instance.buyDecals();
            SaveData.instance.Coins -= 100;
            buyDecalsBtn.SetActive(false);
            successfullyPurchased.SetActive(true);
            isDecalPurchased();
        }
        else
        {
            insufficentCash.SetActive(true);
        }
    }

    public void Decals()
    {
        colorPosition = false;
        tyrePosition = false;
        spoilerPosition = false;
        decalsPosition = true;
        buyTyresBtn.SetActive(false);
        buyColorsBtn.SetActive(false);
        buySpoilerBtn.SetActive(false);
        buyDecalsBtn.SetActive(false);
    }

    public void engines()
    {
        colorPosition = false;
        tyrePosition = false;
        spoilerPosition = true;
        buyTyresBtn.SetActive(false);
        buyColorsBtn.SetActive(false);
    }

    #endregion


    public void backBtn()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }


        for (int i = 0; i < Players[PlayerSelection.instance.current].playerMat.Length; i++)
        {
            Players[PlayerSelection.instance.current].playerMat[i].color = allColors[PlayerPrefs.GetInt("PlayerColor" + PlayerCustomization.instance.playerID + Players[PlayerSelection.instance.current], 0)];
        }

        PlayerSelection.instance.backtoPlayerSelection.Invoke();
        PlayerCustomization.instance.checkTyres();
        PlayerCustomization.instance.checkSpoilers();
        PlayerCustomization.instance.checkDecals();
    }

    public void onStart()
    {
        colorBar.SetActive(true);
        tyreBar.SetActive(false);
    }

    public void OnDisable()
    {
        PlayerSelection.instance.defaultPosition = true;
    }

    public void playBtnSound()
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }
    }

}
