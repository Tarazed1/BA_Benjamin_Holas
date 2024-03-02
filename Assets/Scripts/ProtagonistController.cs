using Insection;
using System.Collections;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;

public class ProtagonistController : MonoBehaviour
{
    private const string PATH_POINT_TAG = "Path_Point";
    private const string PATH_CHOICE_TAG = "Path_Choice";

    public float moveSpeed = 3f; // Adjust the bee's movement speed as needed
    public float stoppingDistance = 0.1f; // Adjust the distance to stop near the destination
    public Animation idleAnimation; // Reference to the idle animation
    public Rigidbody rb;
    public GameObject gameObjectTree;
    public PlayableDirector timeline;
    public StoryTree storyTree;
    public DecisionBoard decisionBoard;

    private Vector3 currentDestination;
    private bool isMoving = false;
    private Animator animator;
    private Transform currentDestinationPoint;
    private int destinationCount = 0;

    private void Awake()
    {
        storyTree = new StoryTree();
    }

    void Start()
    {
        storyTree.InitTree();
        animator = GetComponent<Animator>();
        EventManager.instance.SubscribeToEvent("Event1", HandleEvent1);
        EventManager.instance.SubscribeToEvent("Event2", HandleEvent2);
        // Subscribe to more events as needed
    }

    void HandleEvent1()
    {
        // Handle Event1
        Debug.Log("Received Event1.");
    }

    void HandleEvent2()
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
        if(Input.GetKeyDown(KeyCode.KeypadEnter)) timeline.Play();


        if (isMoving)
        {
            MoveToDestination();
        }
    }

    #region Signal Funcitons

    public void InitGame()
    {
        currentDestinationPoint = gameObjectTree.transform.GetChild(0);
        SetDestination(gameObjectTree.transform.GetChild(0).GetChild(destinationCount));
    }

    #endregion

    public void SetDestination(Transform destination)
    {
        moveSpeed = destination.GetComponent<NodeValueContainer>().speed;
        currentDestination = destination.position;
        isMoving = true;
        if (idleAnimation != null)
        {
            animator.Play("Default"); // Replace "Default" with the name of your movement animation
        }
    }

    private IEnumerator OfferDecicion()
    {
        int numberDecisions = 0;
        int numberPoints = 0;
        int alternateDecision = 0;

        string[] descriptions = new string[4] { "", "", "", "" };
        Transform alternateRoute = null;
        //look for Choices in the Tree
        foreach (Transform t in currentDestinationPoint)
        {
            if (t.tag == PATH_CHOICE_TAG)
            {
                NodeValueContainer nodeValueContainer = t.gameObject.GetComponent<NodeValueContainer>();
                descriptions[numberDecisions] = nodeValueContainer.choiceDescription;
                if (nodeValueContainer.choiceRedirect)
                {
                    alternateDecision = numberDecisions;
                    alternateRoute = nodeValueContainer.choiceRedirect.transform;
                }
                numberDecisions++;
            }
            else
                numberPoints++;

            Debug.Log(numberPoints + "  " + numberDecisions);
        }

        decisionBoard.InitDecision(numberDecisions, descriptions[0], descriptions[1], descriptions[2], descriptions[3]);
        while(!decisionBoard.AwaitDecision()) yield return null;

        Debug.Log("Decision was made");
        decisionBoard.UnInitDecision();
        Transform nextPoint = null;
        if (decisionBoard.GetDecision() == alternateDecision && alternateRoute != null)
        {
            nextPoint = alternateRoute.transform;
        }
        else
        {
            nextPoint = currentDestinationPoint.GetChild(numberPoints + decisionBoard.GetDecision());
        }
        if (nextPoint.tag == PATH_POINT_TAG) Debug.LogError("Returned a Point as Choice.");

        currentDestinationPoint = nextPoint;
        //storyTree.MakeChoice(decisionBoard.GetDecision());
        destinationCount = 0;
        SetDestination(currentDestinationPoint.GetChild(0));
    }

    private void MoveToDestination()
    {
        Vector3 direction = currentDestination - transform.position;

        if (direction.magnitude <= stoppingDistance)
        {
            // Reached the destination
            // Look for a new Point in the Tree, if there is no more point look for choices
            if (currentDestinationPoint.GetChild(destinationCount + 1).tag == PATH_POINT_TAG)
            {
                destinationCount++;               
                SetDestination(currentDestinationPoint.GetChild(destinationCount));
            }
            else
            {
                StartCoroutine(OfferDecicion());

                isMoving = false;
                if (idleAnimation != null)
                {
                    animator.Play(idleAnimation.name);
                }
                rb.velocity = Vector3.zero; // Stop the rigidbody's velocity
            }
        }
        else
        {
            // Move towards the destination using velocity
            rb.velocity = direction.normalized * moveSpeed;
            transform.LookAt(currentDestination);
        }
    }
}