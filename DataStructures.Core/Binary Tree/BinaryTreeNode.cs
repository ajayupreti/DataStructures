﻿using System;

namespace DataStructures.Core.BinaryTree
{
    /// <summary>
    /// Represents a node of a binary tree. Contains a value, a pointer to left node
    /// and a pointer to right node.
    /// </summary>
    /// <typeparam name="TNode">The type of data to be stored</typeparam>
    public class BinaryTreeNode<TNode> : IComparable<TNode>
        where TNode : IComparable<TNode>
    {
        public BinaryTreeNode(TNode value)
        {
            Value = value;
        }

        public BinaryTreeNode<TNode> Left { get; set; }
        public BinaryTreeNode<TNode> Right { get; set; }
        public TNode Value { get; private set; }

        /// <summary>
        /// Compares the current node to the provided value.
        /// </summary>
        /// <param name="other">The node to be compared</param>
        /// <returns>1 if the instance value is greater than the provided value, -1 if less or 0 if equal.</returns>
        public int CompareTo(TNode other)
        {
            return Value.CompareTo(other);
        }
    }
}
