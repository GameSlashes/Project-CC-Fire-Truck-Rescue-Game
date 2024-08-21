using UnityEngine;
public class controllerDetection : MonoBehaviour
{
    public string controllerName;
    public GameObject controller;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            controllerName = "Collectable";
            controller = other.gameObject;
            other.gameObject.GetComponent<controllerCollision>().carEngineEnabled.SetActive(true);
            GameManager.instance.uiElements.carEnterBtn.SetActive(true);
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            GameManager.instance.uiElements.carEnterBtn.SetActive(false);
            other.gameObject.GetComponent<controllerCollision>().carEngineEnabled.SetActive(false);
        }
    }

    //public GameObject obj1;

    public void getInBtn()
    {
        if (FindObjectOfType<Handler>())
        {
            PlayerPrefs.SetInt("adShowMore", 5);
            FindObjectOfType<Handler>().showWaitInterstitial();
            PlayerPrefs.SetInt("InterstitialAdLoadDelay", 5);
        }
        if (SoundManager.instance != null)
        {
            SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
        }
        if (FindObjectOfType<TimerScriptAD>())
        {
            FindObjectOfType<TimerScriptAD>().checkInterstitial();
        }
        if (controllerName == "Collectable")
        {
            GameManager.instance.uiElements.fadeManager.SetActive(true);
            if (controller)
            {
                controller.GetComponentInParent<Rigidbody>().drag = 0.01f;
                controller.GetComponentInParent<Rigidbody>().isKinematic = false;
                GameManager.instance.setController(GameManager.instance.allControllers[1]);
                GameManager.instance.rccCamera.GetComponent<RCCP_Camera>().cameraTarget.playerVehicle = controller.GetComponentInParent<RCCP_CarController>();
                RCCP_SceneManager.Instance.activePlayerVehicle = controller.GetComponentInParent<RCCP_CarController>();
                MissionManager.Instance.GameElements.mapLine.GetComponent<MapLine>().startPoint = controller;
                MissionManager.Instance.TurnSirenOn();
            }
            coroutineManager.instance.getIn("Collectable");
        }
    }

    public void getOutBtn()
    {
        PlayerPrefs.SetInt("adShowMore", 5);
        if (controllerName == "Collectable")
        {
            if (FindObjectOfType<Handler>())
            {
                PlayerPrefs.SetInt("adShowMore", 5);
                FindObjectOfType<Handler>().showWaitInterstitial();
                PlayerPrefs.SetInt("InterstitialAdLoadDelay", 5);
            }

            if (FindObjectOfType<TimerScriptAD>())
            {
                FindObjectOfType<TimerScriptAD>().checkInterstitial();
            }
            controller.GetComponentInParent<Rigidbody>().drag = 5f;
            controller.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            controller.GetComponentInParent<Rigidbody>().angularVelocity = Vector3.zero;

            Invoke("getOut", 1f);
        }
    }
    public void getOut()
    {
        if (controllerName == "Collectable")
        {
            MissionManager.Instance.TurnSirenOff();

            controller.GetComponentInParent<Rigidbody>().drag = 5f;
            controller.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
            controller.GetComponentInParent<Rigidbody>().angularVelocity = Vector3.zero;

            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlayButtonClickSound(SoundManager.instance.buttonClickSound);
            }
            MissionManager.Instance.GameElements.mapLine.GetComponent<MapLine>().startPoint = gameObject;
            GameManager.instance.setController(GameManager.instance.allControllers[0]);
            coroutineManager.instance.getOut("Collectable");
        }
    }
}
