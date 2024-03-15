using Cinemachine;
using Insection;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ProtagonistController : MonoBehaviour
{
    private const string PATH_POINT_TAG = "Path_Point";
    private const string PATH_CHOICE_TAG = "Path_Choice";
    private const string PATH_ANIMATION_TAG = "Path_Animation";

    public bool useDevSpeed = true;
    public float moveSpeed = 3f; // Adjust the bee's movement speed as needed
    public float turnSpeed = 5f;
    public float stoppingDistance = 0.1f; // Adjust the distance to stop near the destination
    public AnimationClip idleAnimation; // Reference to the idle animation
    public Rigidbody rb;
    public GameObject gameObjectTree;
    public PlayableDirector timeline;
    public StoryTree storyTree;
    [Header("UI")]
    public DecisionBoard decisionBoard;
    public GameObject infoTextField;
    public EndScreen endScreen;
    public CameraShake cameraShake;
    public CinemachineVirtualCamera virtualCamera;

    private Vector3 currentDestination;
    private bool isMoving = false;
    private Animator animator;
    private Transform currentDestinationPoint;
    private int destinationCount = 0;

    private float devSpeed = 100f;

    private void Awake()
    {
        if(cameraShake ==  null) cameraShake = GetComponent<CameraShake>();
        storyTree = new StoryTree();
        if(useDevSpeed)
        {
            stoppingDistance = 2f;
        }
    }

    void Start()
    {
        storyTree.InitTree();
        animator = GetComponent<Animator>();
        StartCoroutine(ShowInfo("Willkommen zu dieser Reise durch die Augen einer Biene."));
        //EventManager.instance.SubscribeToEvent("Event1", HandleEvent1);
        //EventManager.instance.SubscribeToEvent("Event2", HandleEvent2);
        // Subscribe to more events as needed
    }

    //void HandleEvent1()
    //{
    //    // Handle Event1
    //    Debug.Log("Received Event1.");
    //}

    //void HandleEvent2()
    //{
    //    // Handle Event2
    //    Debug.Log("Received Event2.");
    //}
    //// Handle more events as needed

    //private void OnDestroy()
    //{
    //    EventManager.instance.UnsubscribeFromEvent("Event1", HandleEvent1);
    //    EventManager.instance.UnsubscribeFromEvent("Event2", HandleEvent2);
    //    // Unsubscribe from more events as needed
    //}

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
        Debug.Log(currentDestinationPoint.name + "   " + destinationCount);
        destinationCount = 0;
        SetDestination(gameObjectTree.transform.GetChild(0).GetChild(destinationCount));
    }

    #endregion

    public void SetDestination(Transform destination)
    {
        if(destination != this.transform) moveSpeed = destination.GetComponent<NodeValueContainer>().speed;
        else moveSpeed = 100;

        virtualCamera.LookAt = destination;

        if(useDevSpeed)
        {
            moveSpeed = devSpeed;
        }

        currentDestination = destination.position;
        isMoving = true;
        if (idleAnimation != null)
        {
            animator.Play(idleAnimation.name); // Replace "Default" with the name of your movement animation
        }
    }

    private IEnumerator OfferDecicion()
    {
        int numberDecisions = 0;
        int numberPoints = 0;
        int alternateDecision = 0;

        string[] descriptions = new string[4] { "", "", "", "" };
        Transform alternateRoute = null;

        if(currentDestinationPoint.childCount == 0)
        {
            Debug.Log("Reached a Leaf.");
            yield break;
        }

        //look for Choices in the Tree
        foreach (Transform t in currentDestinationPoint)
        {
            if (t.tag == PATH_CHOICE_TAG)
            {
                Debug.Log("Found a Choice.");

                NodeValueContainer nodeValueContainer = t.gameObject.GetComponent<NodeValueContainer>();

                if (nodeValueContainer != null)
                {
                    if(nodeValueContainer.choiceDescription == null)
                    {
                        Debug.LogWarning("No String on this NodeValueContainer.");
                        nodeValueContainer.choiceDescription = "empty choice";
                    }
                    descriptions[numberDecisions] = nodeValueContainer.choiceDescription;
                    if (nodeValueContainer.choiceRedirect)
                    {
                        Debug.Log("Found a alternate Route.");
                        alternateDecision = numberDecisions;
                        alternateRoute = nodeValueContainer.choiceRedirect.transform;
                    }
                    numberDecisions++;
                } else
                {
                    Debug.LogError("No NodeValueContainer on this Choice.");
                }
            }
            else
                numberPoints++;

            Debug.Log(numberPoints + "  " + numberDecisions);
        }

        decisionBoard.InitDecision(numberDecisions, descriptions[0], descriptions[1], descriptions[2], descriptions[3]);

        while(!decisionBoard.AwaitDecision()) yield return null;

        Debug.Log("Decision was made. " + alternateDecision + "  " + decisionBoard.GetDecision());
        decisionBoard.UnInitDecision();
        Transform nextPoint;
        nextPoint = currentDestinationPoint.GetChild(numberPoints + decisionBoard.GetDecision());

        if(nextPoint != null && nextPoint.GetComponent<NodeValueContainer>().choiceRedirect != null)
        {
            Debug.Log("Redirecting.");
            nextPoint = nextPoint.GetComponent<NodeValueContainer>().choiceRedirect.transform;
        }

        if (nextPoint.tag == PATH_POINT_TAG) Debug.LogError("Returned a Point as Choice.");

        currentDestinationPoint = nextPoint;
        //storyTree.MakeChoice(decisionBoard.GetDecision());
        destinationCount = 0;
        if (!string.IsNullOrEmpty(currentDestinationPoint.GetChild(0).GetComponent<NodeValueContainer>().optionalInfo))
        {
            StartCoroutine(ShowInfo(currentDestinationPoint.GetChild(0).GetComponent<NodeValueContainer>().optionalInfo));
        }
        SetDestination(currentDestinationPoint.GetChild(0));
    }

    private IEnumerator AnimateProtagonist(NodeValueContainer nvc)
    {
        float duration = nvc.animDuration;
        animator.Play(nvc.anim.name);
        if(nvc.lookAt) virtualCamera.LookAt = nvc.lookAt;

        Debug.Log($"Playing Animation {nvc.anim.name} for {duration} seconds.");

        while(duration > 0f)
        {
            duration -= Time.deltaTime;

            yield return null;
        }

        Debug.Log("Restting look at.");
        Quaternion oldRot = virtualCamera.transform.rotation;
        virtualCamera.LookAt = null;
        transform.rotation = oldRot;
        SetDestination(transform);
    }

    private IEnumerator ShowInfo(string infoText)
    {
        infoTextField.SetActive(true);
        int size = infoText.Length;
        float duration = size / 15;

        Debug.Log("Showing info for " + duration + " seconds.");
        infoTextField.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = infoText;

        while (duration > 0f)
        {
            duration -= Time.deltaTime;
            yield return null;
        }

        infoTextField.SetActive(false);
    }

    private void MoveToDestination()
    {
        Vector3 direction = currentDestination - transform.position;

        if (direction.magnitude <= stoppingDistance)
        {
            cameraShake.Reset();

            if(destinationCount +1 == currentDestinationPoint.childCount && currentDestinationPoint.GetChild(destinationCount).childCount == 0)
            {
                StartCoroutine(Restart());
            } else
            {
                if(!string.IsNullOrEmpty(currentDestinationPoint.GetChild(destinationCount + 1).GetComponent<NodeValueContainer>().optionalInfo))
                {
                    StartCoroutine(ShowInfo(currentDestinationPoint.GetChild(destinationCount + 1).GetComponent<NodeValueContainer>().optionalInfo));
                } 
                // Reached the destination
                // Look for a new Point in the Tree, if there is no more point look for choices
                if (currentDestinationPoint.GetChild(destinationCount + 1).tag == PATH_POINT_TAG)
                {
                    destinationCount++;
                    SetDestination(currentDestinationPoint.GetChild(destinationCount));
                }
                else if (currentDestinationPoint.GetChild(destinationCount + 1).tag == PATH_ANIMATION_TAG)
                {
                    Debug.Log("Found Animation.");

                    isMoving = false;
                    rb.velocity = Vector3.zero; // Stop the rigidbody's velocity

                    StartCoroutine(AnimateProtagonist(currentDestinationPoint.GetChild(destinationCount + 1).GetComponent<NodeValueContainer>()));
                    destinationCount++;
                }
                else
                {
                    Quaternion oldRot = virtualCamera.transform.rotation;
                    virtualCamera.LookAt = null;
                    transform.rotation = oldRot;

                    StartCoroutine(OfferDecicion());

                    isMoving = false;
                    if (idleAnimation != null)
                    {
                        animator.Play(idleAnimation.name);
                    }
                    rb.velocity = Vector3.zero; // Stop the rigidbody's velocity
                }
            }
        }
        else
        {
            // Move towards the destination using velocity
            rb.velocity = direction.normalized * moveSpeed;

            cameraShake.MoveCamera(rb.velocity.magnitude);

            // Smoothly rotate towards the destination
            //Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
        }
    }

    private IEnumerator Restart()
    {
        endScreen.gameObject.SetActive(true);

        while (!endScreen.end && !endScreen.restart)
        {
            yield return null;
        }

        if(endScreen.restart)
        {
            Debug.Log("Restarting Game.");
            timeline.Stop();
            timeline.time = 0;
            destinationCount = 0;

            timeline.Play();
            endScreen.ResetEndScreen();
        } else if(endScreen.end)
        {
            Application.Quit();
        }
    }
}