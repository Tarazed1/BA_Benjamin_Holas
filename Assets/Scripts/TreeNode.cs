using System.Collections.Generic;

public class TreeNode
{
    public string eventDescription; // The event description or narrative content for this node
    public TreeNode parent;
    public List<TreeNode> children = new List<TreeNode>();

    public TreeNode(string eventDescription)
    {
        this.eventDescription = eventDescription;
        EventManager.instance.SubscribeToEvent(eventDescription, null); // Creates an entry in the Dictionary
    }

    // Method to add a child to this node
    public void AddChild(TreeNode childNode)
    {
        children.Add(childNode);
        childNode.parent = this;
    }

    // Method to check if this node is a leaf node (has no children)
    public bool IsLeafNode()
    {
        return children.Count == 0;
    }

    public bool IsRootNode()
    { 
        return parent == null;
    }
}