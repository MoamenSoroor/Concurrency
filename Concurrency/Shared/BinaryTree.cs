using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency.Shared
{

    public class Node<T> where T : IComparable<T>
    {
        public static Node<T> Create(T value)
        {
            return new Node<T>() { Value = value };
        } 
        public T Value { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }
    }
    public class BinaryTree<T> where T : IComparable<T>
    {
        private  Node<T> root;

        public Node<T> Root => root;
        public void AddNode(T value)
        {
            if (value == null) throw new ArgumentNullException("Added value Can't be Null");
            Node<T> newNode = Node<T>.Create(value);

            if (root == null)
            {
                root = newNode;
                return;
            }

            AddNodeHelper(root, newNode);
        }

        private void AddNodeHelper(Node<T> currentNode, Node<T> newNode)
        {
            

            if(currentNode.Value.CompareTo(newNode.Value) > 0 )
            {
                if (currentNode.Left == null)
                    currentNode.Left = newNode;
                else
                    AddNodeHelper(currentNode.Left,newNode);
            }
            
            if (currentNode.Value.CompareTo(newNode.Value) <= 0)
            {
                if (currentNode.Right == null)
                    currentNode.Right = newNode;
                else
                    AddNodeHelper(currentNode.Right, newNode);
            }

        }



        public void Traverse(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException("action can't be null");
            if (root == null) return;
            TraverseHelper(root, action);
        }

        private void TraverseHelper(Node<T> current, Action<T> action)
        {
            if (current == null) return;

            action(current.Value);
            TraverseHelper(current.Left, action);
            TraverseHelper(current.Right, action);
        }


        public void TraverseInParallel(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException("action can't be null");
            if (root == null) return;

            Task traversing = Task.Factory.StartNew(() => TraverseInParallelHelper(root, action), 
                CancellationToken.None, TaskCreationOptions.AttachedToParent, TaskScheduler.Default);
            traversing.Wait();
        }

        private void TraverseInParallelHelper(Node<T> current, Action<T> action)
        {
            action(current.Value);

            if (current.Left != null)
                Task.Factory.StartNew(()=> TraverseInParallelHelper(current.Left, action), 
                    CancellationToken.None,TaskCreationOptions.AttachedToParent, TaskScheduler.Default);
            


            if (current.Right != null)
                Task.Factory.StartNew(() => TraverseInParallelHelper(current.Right, action),
                    CancellationToken.None, TaskCreationOptions.AttachedToParent, TaskScheduler.Default);
        }



    }


    public static class BinaryTreeTester
    {
        public static void Test()
        {
            BinaryTree<long> numbersTree = new BinaryTree<long>();
            numbersTree.AddNode(50);
            numbersTree.AddNode(40);
            numbersTree.AddNode(60);
            numbersTree.AddNode(30);
            numbersTree.AddNode(35);
            numbersTree.AddNode(25);
            numbersTree.AddNode(55);
            numbersTree.AddNode(70);
            numbersTree.AddNode(80);
            numbersTree.AddNode(75);

            numbersTree.Traverse((value) => Console.WriteLine(value));
        }
    }

}
