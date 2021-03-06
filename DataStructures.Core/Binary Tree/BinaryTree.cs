﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace DataStructures.Core.BinaryTree
{
    /// <summary>
    /// Represents a Binary Tree
    /// </summary>
    /// <typeparam name="T">The type of data to be stored.</typeparam>
    public class BinaryTree<T> : IEnumerable<T>
        where T : IComparable<T>
    {

        private BinaryTreeNode<T> _head;
        private int _count;

        #region Add
        /// <summary>
        /// Adds a new value to the binary tree.
        /// </summary>
        /// <param name="value">The value to be added.</param>
        public void Add(T value)
        {
            //CASE 1: The tree is empty - allocate the head to the value.
            if (_head == null)
            {
                _head = new BinaryTreeNode<T>(value);
            }
            //CASE 2: The tree is not empty so find the right location to insert the value.
            else
            {
                AddTo(_head, value);
            }

            //Update the current number of nodes
            _count++;
        }

        /// <summary>
        /// Recursive Add algorithm
        /// </summary>
        /// <param name="node">The root node of the sub tree.</param>
        /// <param name="value">The value to be inserted.</param>
        private void AddTo(BinaryTreeNode<T> node, T value)
        {
            //CASE 1: The value is less than the current node value
            if (value.CompareTo(node.Value) < 0)
            {
                //There is no left child of current node, so add it.
                if (node.Left == null)
                {
                    node.Left = new BinaryTreeNode<T>(value);
                }
                //There is a left child of current node, so go to that child and repeat the procedure.
                else
                {
                    AddTo(node.Left, value);
                }
            }
            //CASE 2: The value is greater than or equal to the current value.
            else
            {
                if (node.Right == null)
                {
                    node.Right = new BinaryTreeNode<T>(value);
                }
                else
                {
                    AddTo(node.Right, value);
                }
            }
        }
        #endregion

        /// <summary>
        /// Checks if the specified value exists in the binary tree or not.
        /// </summary>
        /// <param name="value">The value to be checked.</param>
        /// <returns>True if value exists otherwise false.</returns>
        public bool Contains(T value)
        {
            BinaryTreeNode<T> parent;
            return FindWithParent(value, out parent) != null;
        }

        /// <summary>
        /// Finds and returns the first node containing the specified value. If the value is not found
        /// then it returns null. It also returns the parent of the found node (or null) which is used in Remove.
        /// </summary>
        /// <param name="value">The value to be searched.</param>
        /// <param name="parent">The parent of the found node.</param>
        /// <returns>The found node (or null).</returns>
        private BinaryTreeNode<T> FindWithParent(T value, out BinaryTreeNode<T> parent)
        {
            BinaryTreeNode<T> current = _head;
            parent = null;

            while (current != null)
            {
                int result = current.CompareTo(value);
                if (result > 0)
                {
                    //If value is less than current, go Left.
                    parent = current;
                    current = current.Left;
                }
                else if (result < 0)
                {
                    //If value is greater than current, go Right.
                    parent = current;
                    current = current.Right;
                }
                else
                {
                    //We have a match.
                    break;
                }
            }

            return current;
        }

        #region Remove
        /// <summary>
        /// Removes the first node with the given value.
        /// </summary>
        /// <param name="value">The value to be removed.</param>
        /// <returns>True if the node is removed otherwise false.</returns>
        public bool Remove(T value)
        {
            BinaryTreeNode<T> current, parent;

            current = FindWithParent(value, out parent);

            if (current == null)
            {
                return false;
            }

            _count--;

            //CASE 1: The current as no right child
            if (current.Right == null)
            {
                if (parent == null)
                {
                    _head = current.Left;
                }
                else
                {
                    int result = parent.CompareTo(value);

                    if (result > 0)
                    {
                        parent.Left = current.Left;
                    }
                    else if (result < 0)
                    {
                        parent.Right = current.Left;
                    }
                }
            }
            //CASE 2: If current's right child has no left child, then current's right child replaces current.
            else if (current.Right.Left == null)
            {
                //The new parent should point to the left sub tree of current.
                current.Right.Left = current.Left;

                if(parent == null)
                {
                    _head = current.Right;
                }
                else
                {
                    int result = parent.CompareTo(value);

                    if (result > 0)
                    {
                        parent.Left = current.Right;
                    }
                    else if (result < 0)
                    {
                        parent.Right = current.Right;
                    }
                }
            }
            //CASE 3: If current's right child has a left child, replace current with current's right child's
            //left most child
            else
            {
                BinaryTreeNode<T> leftMost = current.Right.Left;
                BinaryTreeNode<T> leftMostParent = current.Right;

                while (leftMost.Left != null)
                {
                    leftMostParent = leftMost;
                    leftMost = leftMost.Left;
                }

                leftMostParent.Left = leftMost.Right;

                leftMost.Left = current.Left;
                leftMost.Right = current.Right;

                if (parent == null)
                {
                    _head = leftMost;
                }
                else
                {
                    int result = parent.CompareTo(value);

                    if (result > 0)
                    {
                        parent.Left = leftMost;
                    }
                    else if(result < 0)
                    {
                        parent.Right = leftMost;
                    }
                }
            }

            return true;
        }
        #endregion

        #region Traversal
        public enum TraversalType
        {
            PreOrder,
            PostOrder,
            InOrder
        }

        public void Traverse(Action<T> action, TraversalType traversalType)
        {
            Traverse(action, traversalType, _head);
        }

        public void Traverse(Action<T> action, TraversalType traversalType, BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                if (traversalType == TraversalType.PreOrder)
                {
                    action(node.Value);
                    Traverse(action, traversalType, node.Left);
                    Traverse(action, traversalType, node.Right);
                }
                else if (traversalType == TraversalType.PostOrder)
                {
                    Traverse(action, traversalType, node.Left);
                    Traverse(action, traversalType, node.Right);
                    action(node.Value);
                }
                else
                {
                    Traverse(action, traversalType, node.Left);
                    action(node.Value);
                    Traverse(action, traversalType, node.Right);
                }
            }
        }

        /// <summary>
        /// Enumerates the values contained in the binary tree in in-order traversal order.
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<T> InOrderTraversal()
        {
            if (_head != null)
            {
                Stack<BinaryTreeNode<T>> stack = new Stack<BinaryTreeNode<T>>();

                BinaryTreeNode<T> current = _head;

                bool goLeftNext = true;

                stack.Push(current);

                while (current != null)
                {
                    if (goLeftNext)
                    {
                        while (current.Left != null)
                        {
                            stack.Push(current);
                            current = current.Left;
                        }
                    }

                    yield return current.Value;

                    if (current.Right != null)
                    {
                        current = current.Right;

                        goLeftNext = true;
                    }
                    else
                    {
                        current = stack.Pop();
                        goLeftNext = false;
                    }
                }
            }

        }
        #endregion       

        /// <summary>
        /// Removes all the nodes from the tree
        /// </summary>
        public void Clear()
        {
            _count = 0;
            _head = null;
        }


        /// <summary>
        /// The number of items currently in the tree/
        /// </summary>
        public int Count
        {
            get { return _count; }
        }



        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
