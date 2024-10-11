using Invector;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    public Animator animator; // The character's animator for controlling animations
    public NavMeshAgent agent; // The NavMeshAgent for walking on the NavMesh
    public float health = 100f; // Character's health

    private bool isRunning = false;
    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        Walk();
    }

    private void Update()
    {
        if (!isDead)
        {
            // If health is greater than zero, make the character walk or run on the NavMesh
            if (!isRunning)
            {
                Walk();
            }
        }
    }

    // Method for walking
    void Walk()
    {
        //agent.isStopped = false;
        agent.speed = 2f; // Walking speed
        animator.SetBool("Walk", true); // Play walking animation
        animator.SetBool("Run", false);
    }

    // Method for running
    void Run()
    {
        isRunning = true;
        agent.speed = 6f; // Running speed
        animator.SetBool("Walk", false);
        animator.SetBool("Run", true); // Play running animation
    }

    // Method for when hit by a regular object (normal hit)
    public void Chant()
    {
        animator.SetTrigger("Chant"); // Play chant animation
    }

    // Method for handling collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Normal hit
        {
            Chant();
        }
        else if (collision.gameObject.CompareTag("Player")) // Hit by a car
        {
            TakeDamage(50f); // Reduce health by 50 on car hit
            if (!isDead)
            {
                Fall();
            }
        }
    }

    // Method for falling down
    void Fall()
    {
        animator.SetTrigger("Fall"); // Play fall animation
        agent.isStopped = true; // Stop character movement

        // After falling, get up and run after 3 seconds
        Invoke("GetUpAndRun", 3f);
    }

    // Method for getting up and running
    void GetUpAndRun()
    {
        animator.SetTrigger("GetUp"); // Play get up animation
        Run(); // Run after getting up
    }

    // Method to handle taking damage
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    // Method for dying
    void Die()
    {
        isDead = true;
        animator.SetTrigger("Dead"); // Play dead animation
        agent.isStopped = true; // Stop movement on death
    }
}
