using Insection;
using UnityEngine;

public class ProtagonistController : MonoBehaviour
{
    public float moveSpeed = 3f; // Adjust the bee's movement speed as needed
    public float stoppingDistance = 0.1f; // Adjust the distance to stop near the destination
    public Animation idleAnimation; // Reference to the idle animation

    private Vector3 currentDestination;
    private bool isMoving = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        EventManager.instance.SubscribeToEvent("Event1", HandleEvent1);
        EventManager.instance.SubscribeToEvent("Event2", HandleEvent2);
        // Subscribe to more events as needed
    }

    private void HandleEvent1()
    {
        // Handle Event1
        Debug.Log("Received Event1.");
    }

    private void HandleEvent2()
    {
        // Handle Event2
        Debug.Log("Received Event2.");
    }
    // Handle more events as needed

    private void OnDestroy()
    {
        EventManager.instance.UnsubscribeFromEvent("Event1", HandleEvent1);
        EventManager.instance.UnsubscribeFromEvent("Event2", HandleEvent2);
        // Unsubscribe from more events as needed
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveToDestination();
        }
    }

    public void SetDestination(Vector3 destination)
    {
        currentDestination = destination;
        isMoving = true;
        if (idleAnimation != null)
        {
            animator.Play("Default"); // Replace "Default" with the name of your movement animation
        }
    }

    private void MoveToDestination()
    {
        Vector3 direction = currentDestination - transform.position;

        if (direction.magnitude <= stoppingDistance)
        {
            // Reached the destination
            isMoving = false;
            if (idleAnimation != null)
            {
                animator.Play(idleAnimation.name);
            }
        }
        else
        {
            // Move towards the destination
            transform.position = Vector3.MoveTowards(transform.position, currentDestination, moveSpeed * Time.deltaTime);
            transform.LookAt(currentDestination);
        }
    }
}