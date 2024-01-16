using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Insection
{
    public class StoryTree
    {
        private TreeNode root;
        private List<TreeNode> tree;

        public StoryTree()
        { 
            tree = new List<TreeNode>();
            BuildTree();
        }

        private void BuildTree()
        {
            TreeNode tempNode = null;

            //root
            root = new TreeNode(null, EventNames.EVENT_START, null);
            tree.Add(root);

            //first choice
            tree.Add(new TreeNode(root, EventNames.EVENT_WATCH_WORKERS,
                new List<TreeNode.TreeWay> { TreeNode.TreeWay.left }));

            tree.Add(new TreeNode(root, EventNames.EVENT_JOIN_MEETING,
                new List<TreeNode.TreeWay> { TreeNode.TreeWay.right }));
            //--------------

                //left - second choice
                tempNode = FindNode(new List<TreeNode.TreeWay>() { TreeNode.TreeWay.left });

                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_FAR_FLOWERS_LESS_KNOWLODGE,
                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.left, TreeNode.TreeWay.left }));

                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_MIDDLE_FLOWERS_LESS_KNOWLEDGE,
                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.left, TreeNode.TreeWay.right }));
                //--------------

                //right - second choice
                tempNode = FindNode(new List<TreeNode.TreeWay>() { TreeNode.TreeWay.right });

                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_CLOSE_FLOWERS, 
                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.right, TreeNode.TreeWay.left }));

                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_MIDDLE_FLOWERS,
                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.right, TreeNode.TreeWay.middle }));

                tree.Add(new TreeNode(tempNode, EventNames.EVENT_GO_TO_FAR_FLOWERS, 
                    new List<TreeNode.TreeWay> { TreeNode.TreeWay.right, TreeNode.TreeWay.right }));
                //---------------

            //left-left - third choice
            tempNode = FindNode(new List<TreeNode.TreeWay>() { TreeNode.TreeWay.left, TreeNode.TreeWay.left });

            tree.Add(new TreeNode(tempNode, EventNames.EVENT_BREAK,
                new List<TreeNode.TreeWay>() { TreeNode.TreeWay.left, TreeNode.TreeWay.left, TreeNode.TreeWay.left }));

            tree.Add(new TreeNode(tempNode, EventNames.EVENT_CONTINUE, 
                new List<TreeNode.TreeWay>() { TreeNode.TreeWay.left, TreeNode.TreeWay.left, TreeNode.TreeWay.right }));
            //---------------
        }

        private TreeNode FindNode(List<TreeNode.TreeWay> way)
        {
            foreach (TreeNode node in tree)
            {
                if(node.treePath == way) return node;
            }
            Debug.LogWarning("Could not find the node to the given path.");
            return null;
        }
    }
}
