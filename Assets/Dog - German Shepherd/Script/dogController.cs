using Invector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogController : MonoBehaviour
{
    public Transform player;
    public Animator playerAnim;
    public float distanceFromPlayer = 2f;
    public float forwardDistance = 2f;
    public float upwardDistance = 1f;
    public float walkSpeedThreshold = 1f;
    public float followSpeed = 5f;
    public float fetchSpeed = 10f;
    public float returnSpeed = 5f;
    public Animator dogAnimator;
    private GameObject ball;
    public GameObject ballPrefab;
    private bool isFollowingPlayer = true;
    private bool isFetchingBall = false;

    private bool nextBall;
    public AudioSource dogSound;

    public bool adShow;


    private void OnEnable()
    {
        isFollowingPlayer = true;
        isFetchingBall = false;
        nextBall = false;

        if (ball)
            Destroy(ball);
    }


    private void FixedUpdate()
    {
        if (player != null)
        {
            if (isFollowingPlayer)
            {
                FollowPlayer();
            }
            else if (isFetchingBall)
            {
                FetchBall();
            }
            else
            {
                ReturnToPlayer();
            }
        }
    }

    private void FollowPlayer()
    {
        // Calculate the target position for the dog
        Vector3 targetPosition = player.position + player.forward * forwardDistance + player.right * distanceFromPlayer + player.up * upwardDistance;

        // Smoothly move the dog towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Rotate the dog to match the player's rotation
        transform.rotation = player.rotation;

        // Determine if the player is walking, running, or idle
        float playerSpeed = player.GetComponent<Rigidbody>().velocity.magnitude;

        // Set animation parameter based on player movement speed
        if (playerSpeed >= 0.1f && playerSpeed < walkSpeedThreshold) // Player is walking
        {
            dogAnimator.SetBool("IsRunning", false);
            dogAnimator.SetBool("IsWalking", true);
        }
        else if (playerSpeed > walkSpeedThreshold) // Player is running
        {
            dogAnimator.SetBool("IsRunning", true);
            dogAnimator.SetBool("IsWalking", false);
        }
        else // Player is standing idle
        {
            dogAnimator.SetBool("IsRunning", false);
            dogAnimator.SetBool("IsWalking", false);
        }
    }

    private void FetchBall()
    {
        if (ball != null)
        {
            // Move towards the ball
            Vector3 targetPosition = ball.transform.position + Vector3.up * upwardDistance;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fetchSpeed * Time.deltaTime);

            dogAnimator.SetBool("IsRunning", true);
            dogAnimator.SetBool("IsWalking", false);

            // Check if the dog has reached the ball
            if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
            {
                // Dog has reached the ball, collect it and start returning to the player
                isFetchingBall = false;
                Destroy(ball);
                isFollowingPlayer = false;
            }
        }
    }

    private void ReturnToPlayer()
    {
        // Move towards the player
        Vector3 targetPosition = player.position + player.forward * forwardDistance + Vector3.up * upwardDistance;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, returnSpeed * Time.deltaTime);

        // Rotate the dog to face the player
        transform.LookAt(player.position);

        dogAnimator.SetBool("IsRunning", true);
        dogAnimator.SetBool("IsWalking", false);

        // Check if the dog has reached the player
        float distanceToPlayer = Vector3.Distance(transform.position, targetPosition);
        if (distanceToPlayer <= 0.1f)
        {
            // Reset flags and positions
            isFetchingBall = false;
            isFollowingPlayer = true;
            nextBall = false;
            adShow = false;
            Firebase.Analytics.FirebaseAnalytics.LogEvent("Dog_Collect_Ball");
        }
    }

    public void ballThrowing()
    {
        if (nextBall == false)
        {
            if (ball == null)
                StartCoroutine(ThrowBall());
        }

        if (adShow == false)
        {
            if (FindObjectOfType<Handler>())
            {
                FindObjectOfType<Handler>().showWaitInterstitial();
                PlayerPrefs.SetInt("loadInterstitialAD", 5);
            }

            if (FindObjectOfType<TimerScriptAD>())
                FindObjectOfType<TimerScriptAD>().checkInterstitial();

            adShow = true;
        }
    }

    private IEnumerator ThrowBall()
    {
        playerAnim.SetTrigger("BallThrow");
        nextBall = true;
        yield return new WaitForSeconds(0.5f);
        dogSound.Play();
        yield return new WaitForSeconds(0.15f);
        Vector3 throwDirection = player.forward; // Calculate the direction in which the ball should be thrown based on the player's orientation
        // Define the upward offset for spawning the ball
        Vector3 spawnOffset = Vector3.up * 1f; // You can adjust the value as needed
        // Instantiate the ball prefab and apply force in the calculated direction
        ball = Instantiate(ballPrefab, player.transform.position + throwDirection * 2f + spawnOffset, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.AddForce(throwDirection * 10f, ForceMode.Impulse);
        // Set flags to indicate the dog is fetching the ball
        isFetchingBall = true;
        isFollowingPlayer = false;

    }
}
