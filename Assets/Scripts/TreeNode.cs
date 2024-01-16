using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Insection
{
    public class TreeNode
    {
        public enum TreeWay
        {
            left, 
            middleleft,
            middle,
            middleright,
            right
        }

        public List<TreeNode> childNodes { get; private set; }
        public TreeNode parentNode { get; private set; }
        public List<TreeWay> treePath { get; private set; }
        public string nodeEvent { get; private set; }

        public TreeNode(TreeNode patent, string nodeEvent, List<TreeWay> treePath)
        {
            childNodes = new List<TreeNode>();
            this.parentNode = patent;
            this.nodeEvent = nodeEvent;
            this.treePath = treePath;
            patent.AddChild(this);
        }

        public void AddChild(TreeNode child)
        {
            if(child != null && !childNodes.Contains(child))
                childNodes.Add(child);
        }
    }
}