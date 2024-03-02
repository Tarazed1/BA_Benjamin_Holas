using Insection;
using System.Collections.Generic;
using UnityEngine;

public class StoryTree
{
    private TreeNode root; // The root node of the story tree
    private TreeNode currentNode; // Tracks the current node
    private List<TreeNode> children;

    private EventManager eventManager; // Reference to the EventManager

    public StoryTree()
    {
    }

    public void InitTree()
    {
        eventManager = EventManager.instance; // Get the EventManager instance
        if (eventManager == null)
        {
            Debug.LogError("EventManager not found.");
        }

        eventManager.SubscribeToEvent(EventNames.EVENT_END, null);
        BuildTree();
        currentNode = root;
    }

    // ... (Other methods remain the same)

    // Method to handle player's choice and update destination based on chosen node
    public bool MakeChoice(int choiceIndex)
    {
        if(currentNode.IsLeafNode())
        {
            eventManager.TriggerEvent(EventNames.EVENT_END);
            return false;
        }

        if (currentNode != null && choiceIndex >= 0 && choiceIndex < currentNode.children.Count)
        {
            TreeNode chosenNode = currentNode.children[choiceIndex];
            currentNode = chosenNode; // Update the current node to the chosen node

            // Trigger the event based on the chosen node's event description
            if (eventManager != null)
            {
                eventManager.TriggerEvent(chosenNode.eventDescription);
            }
        }
        else
        {
            Debug.LogError("Invalid choice index or current node has no children.");
            return false;
        }
        return true;
    }

    private void BuildTree()
    {
         children = new List<TreeNode>();

        //root
        root = new TreeNode(EventNames.EVENT_START);

            //first Choice
            children.Add(new TreeNode(EventNames.EVENT_WATCH_WORKERS));
                //second Choice
                children.Add(new TreeNode(EventNames.EVENT_GO_TO_FAR_FLOWERS));
                children.Add(new TreeNode(EventNames.EVENT_GO_TO_MIDDLE_FLOWERS));
            
            children.Add(new TreeNode(EventNames.EVENT_JOIN_MEETING)); 
                //second Choice
                children.Add(new TreeNode(EventNames.EVENT_GO_TO_CLOSE_FLOWERS));
                children.Add(new TreeNode(EventNames.EVENT_GO_TO_FAR_FLOWERS));
                children.Add(new TreeNode(EventNames.EVENT_GO_TO_MIDDLE_FLOWERS));
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Insection
//{
//    public class StoryTree
//    {
//        private TreeNode root;
//        private List<TreeNode> tree;

//        public StoryTree()
//        { 
//            tree = new List<TreeNode>();
//            BuildTree();
//        }

//        private void BuildTree()
//        {
//            TreeNode tempNode = null;

//            //root
//            root = new TreeNode(null, EventNames.EVENT_START, null);
//            tree.Add(root);

//            //first choice
//            tree.Add(new TreeNode(root, EventNames.EVENT_WATCH_WORKERS,
//                new List<TreeNode.TreeWay> { TreeNode.TreeWay.left }));

//            tree.Add(new TreeNode(root, EventNames.EVENT_JOIN_MEETING,
//                new List<TreeNode.TreeWay> { TreeNode.TreeWay.right }));
//            //--------------

//                //left - second choice
//                tempNode = FindNode(new List<TreeNode.TreeWay>() { TreeNode.TreeWay.left });

//                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_FAR_FLOWERS_LESS_KNOWLODGE,
//                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.left, TreeNode.TreeWay.left }));

//                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_MIDDLE_FLOWERS_LESS_KNOWLEDGE,
//                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.left, TreeNode.TreeWay.right }));
//                //--------------

//                //right - second choice
//                tempNode = FindNode(new List<TreeNode.TreeWay>() { TreeNode.TreeWay.right });

//                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_CLOSE_FLOWERS, 
//                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.right, TreeNode.TreeWay.left }));

//                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_MIDDLE_FLOWERS,
//                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.right, TreeNode.TreeWay.middle }));

//                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_FAR_FLOWERS, 
//                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.right, TreeNode.TreeWay.right }));
//                //---------------

//            //left-left - third choice
//            tempNode = FindNode(new List<TreeNode.TreeWay>() { TreeNode.TreeWay.left, TreeNode.TreeWay.left });

//            tree.Add(new TreeNode(tempNode, EventNames.EVENT_BREAK,
//                new List<TreeNode.TreeWay>() { TreeNode.TreeWay.left, TreeNode.TreeWay.left, TreeNode.TreeWay.left }));

//            tree.Add(new TreeNode(tempNode, EventNames.EVENT_CONTINUE, 
//                new List<TreeNode.TreeWay>() { TreeNode.TreeWay.left, TreeNode.TreeWay.left, TreeNode.TreeWay.right }));
//            //---------------
//        }

//        private TreeNode FindNode(List<TreeNode.TreeWay> way)
//        {
//            foreach (TreeNode node in tree)
//            {
//                if(node.treePath == way) return node;
//            }
//            Debug.LogWarning("Could not find the node to the given path.");
//            return null;
//        }
//    }
//}
