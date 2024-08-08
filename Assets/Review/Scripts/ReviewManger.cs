using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.Review;

public class ReviewManger : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;
    int count;
    // Start is called before the first frame update
    void Start()
    {
        //AppOpenAdManager.isInterstialAdPresent = true;
            StartCoroutine(RequestReviews());
        
    }

    // Update is called once per frame

    IEnumerator RequestReviews()
    {
        _reviewManager = new ReviewManager();
        // Request a ReviewInfo Object
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
      //  Debug.LogError("Responce_requestFlowOperation "+requestFlowOperation.GetResult().ToString());
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
           // firebasecall.Instance.Event("RateUs_Error_"+requestFlowOperation.Error.ToString());
            //Debug.LogError("Error_At_requestFlowOperation "+requestFlowOperation.Error.ToString());
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
    
        _playReviewInfo = requestFlowOperation.GetResult();
      
        //Lunch the InappReview
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
      //  Debug.LogError("Responce_launchFlowOperation "+launchFlowOperation.GetResult().ToString());
      //  Debug.LogError("Responce_launchFlowOperation "+launchFlowOperation.IsDone.ToString());
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
          //  firebasecall.Instance.Event("RateUs_Error_Lunch_"+launchFlowOperation.Error.ToString());
          //  Debug.LogError("Error_At_launchFlowOperation "+launchFlowOperation.Error.ToString());
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }
}
