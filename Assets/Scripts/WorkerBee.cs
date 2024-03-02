using UnityEngine;

public class WorkerBee : NPCController
{
    // Add specific behavior or properties for WorkerBee here
    protected override void Start()
    {
        base.Start();
        // Additional initialization for WorkerBee
    }

    protected override void HandleEvent1()
    {
        // Customized handling of Event1 for WorkerBee
        Debug.Log(name + " is a worker bee and received Event1.");
    }

    // You can override or add more event handling methods as needed
}