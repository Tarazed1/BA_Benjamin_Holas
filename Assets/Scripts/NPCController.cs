using UnityEngine;

public class NPCController : MonoBehaviour
{
    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        EventManager.instance.SubscribeToEvent("Event1", HandleEvent1);
        EventManager.instance.SubscribeToEvent("Event2", HandleEvent2);
        // Subscribe to more events as needed
    }

    protected virtual void HandleEvent1()
    {
        // Handle Event1 for NPCs
        Debug.Log(name + " received Event1.");
    }

    protected virtual void HandleEvent2()
    {
        // Handle Event2 for NPCs
        Debug.Log(name + " received Event2.");
    }
    // Handle more events as needed

    protected virtual void OnDestroy()
    {
        EventManager.instance.UnsubscribeFromEvent("Event1", HandleEvent1);
        EventManager.instance.UnsubscribeFromEvent("Event2", HandleEvent2);
        // Unsubscribe from more events as needed
    }
}