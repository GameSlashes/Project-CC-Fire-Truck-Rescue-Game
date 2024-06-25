using UnityEngine;
public class controllerDetection : MonoBehaviour
{
    public string controllerName;
    public GameObject controller;
    //public vParachuteController _parachuteController;
    //public LuxyCars myNewCar;
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

    public GameObject obj1;

    public void getInBtn()
    {
        //if (FindObjectOfType<Handler>())
        //{
        //    PlayerPrefs.SetInt("adShowMore", 5);
        //    FindObjectOfType<Handler>().ShowInterstitialAd();
        //    PlayerPrefs.SetInt("loadInterstitialAD", 5);
        //}

        //if (FindObjectOfType<TimerScriptAD>())
        //{
        //    FindObjectOfType<TimerScriptAD>().checkInterstitial();
        //}
        if (controllerName == "Collectable")
        {
            GameManager.instance.uiElements.fadeManager.SetActive(true);
            if (controller)
            {
                GameManager.instance.setController(GameManager.instance.allControllers[1]);
                controller.transform.parent.gameObject.SetActive(false);
                Vector3 spawnPosition = controller.transform.position;
                Quaternion spawnRotation = controller.transform.rotation; // Get the rotation of the controller
                obj1 = Instantiate(controller.gameObject.GetComponent<controllerCollision>().carEngineEnabled.gameObject.GetComponent<carEngineEnable>().rccCarEngine.gameObject, spawnPosition, spawnRotation);
                GameManager.instance.rccCamera.GetComponent<RCCP_Camera>().cameraTarget.playerVehicle = obj1.GetComponent<RCCP_CarController>();
                RCCP_SceneManager.Instance.activePlayerVehicle = obj1.GetComponent<RCCP_CarController>();
            }
            coroutineManager.instance.getIn("Collectable");
        }
    }

    public void getOutBtn()
    {
        PlayerPrefs.SetInt("adShowMore", 5);
        if (controllerName == "Collectable")
        {
            if (obj1)
            {
                obj1.GetComponent<Rigidbody>().drag = 3f;
                Invoke("getOut", 1f);
            }
        }
    }
    public void getOut()
    {
        //if (FindObjectOfType<Handler>())
        //{
        //    FindObjectOfType<Handler>().ShowInterstitialAd();
        //    PlayerPrefs.SetInt("loadInterstitialAD", 5);
        //}

        //if (FindObjectOfType<TimerScriptAD>())
        //{
        //    FindObjectOfType<TimerScriptAD>().checkInterstitial();
        //}
        if (controllerName == "Collectable")
        {
            if (obj1)
            {
                Vector3 spawnPosition = obj1.transform.position;
                Quaternion spawnRotation = obj1.transform.rotation;
                controller.transform.parent.gameObject.transform.position = spawnPosition;
                controller.transform.parent.gameObject.transform.rotation = spawnRotation;
                controller.transform.parent.gameObject.SetActive(true);
	            Destroy(obj1);
	            //obj1.SetActive(false);
            }
            GameManager.instance.setController(GameManager.instance.allControllers[0]);
            coroutineManager.instance.getOut("Collectable");
        }
    }
}
