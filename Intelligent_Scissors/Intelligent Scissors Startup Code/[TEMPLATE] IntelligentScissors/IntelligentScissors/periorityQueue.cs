using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelligentScissors
{
    public class PeriorityQueue // O(log (V))
    {
        private List<Arc> H = new List<Arc>();  // θ(1)
        private int LeftChild(int Node) // θ(1)
        {
            return Node * 2 + 1; // θ(1)
        }
        private int RightChild(int Node) // θ(1)
        {
            return Node * 2 + 2; // θ(1)
        }
        private int Parent(int Node) // θ(1)
        {
            return (Node - 1) / 2; // θ(1)
        }
        private void ShiftUp(int Node) // θ(1)
        {
            if (Node == 0 || H[Node].Cost >= H[Parent(Node)].Cost) // θ(1)
                return; // θ(1)
            Arc temp = H[Parent(Node)]; // θ(1)
            H[Parent(Node)] = H[Node]; // θ(1)
            H[Node] = temp; // θ(1)
            ShiftUp(Parent(Node)); // θ(1)
        }
        private void ShiftDown(int Node) // θ(1)
        {

            if (LeftChild(Node) >= H.Count
                || (LeftChild(Node) < H.Count && RightChild(Node) >= H.Count && H[LeftChild(Node)].Cost >= H[Node].Cost)
                || (LeftChild(Node) < H.Count && RightChild(Node) < H.Count && H[LeftChild(Node)].Cost >= H[Node].Cost &&
                H[RightChild(Node)].Cost >= H[Node].Cost)) // θ(1)
                return; // θ(1)
            if (RightChild(Node) < H.Count && H[RightChild(Node)].Cost <= H[LeftChild(Node)].Cost) // θ(1)
            {
                Arc temp = H[RightChild(Node)]; // θ(1)
                H[RightChild(Node)] = H[Node]; // θ(1)
                H[Node] = temp; // θ(1)
                ShiftDown(RightChild(Node)); // θ(1)
            }
            else
            {
                Arc temp = H[LeftChild(Node)]; // θ(1)
                H[LeftChild(Node)] = H[Node]; // θ(1)
                H[Node] = temp; // θ(1)
                ShiftDown(LeftChild(Node)); // θ(1)
            }
        }
        public void Push(Arc Node) // O(log (V))
        {
            H.Add(Node); // θ(1)
            ShiftUp(H.Count - 1); // θ(1)
        }
        public Arc Pop() // O(log (V))
        {
            Arc temp = H[0]; // θ(1)
            H[0] = H[H.Count - 1]; // θ(1)
            H.RemoveAt(H.Count - 1); // O(log (V))
            ShiftDown(0); // θ(1)
            return temp; // θ(1)
        }
        public bool IsEmpty() // θ(1)
        {
            if (H.Count == 0) // θ(1)
                return true; // θ(1)
            return false; // θ(1)
        }
        public Arc Top() // θ(1)
        {
            return H[0]; // θ(1)
        }
    }
}
