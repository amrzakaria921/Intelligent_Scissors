using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
namespace IntelligentScissors
{

    public static class ShortestPath // O(EV)
    {    
        public static List<Point> Create_ShortestPath(int src, int dest, RGBPixel[,] ImageMatrix) // O(EV)
        {
            List<int> ParentList = Dijkstra(src, dest, ImageMatrix); // O(EV)
            return Inverting_Path(ParentList, dest, ImageOperations.GetWidth(ImageMatrix)); // O(V)
        } 
        public static List<Point> Inverting_Path(List<int> ParentList, int Dest, int matrix_width) // O(E)
        {
            List<Point> ShortestPath = new List<Point>(); // θ(1)
            Stack<int> ReversedPath = new Stack<int>();  // θ(1)
            ReversedPath.Push(Dest); // θ(1)
            int Parent = ParentList[Dest]; // θ(1)
            while (Parent != -1) // θ(E)
            {
                ReversedPath.Push(Parent); // θ(1)
                Parent = ParentList[Parent]; // θ(1)
            }

            while (ReversedPath.Count != 0) // O(E)
            {
                var TwoD = Functions.oneDtoTwoD(ReversedPath.Pop(), matrix_width); // θ(1)
                Point point = new Point((int)TwoD.X, (int)TwoD.Y); // θ(1)
                ShortestPath.Add(point); // θ(1)
            }
            return ShortestPath; // θ(1)
        }
        public static List<int> Dijkstra(int src, int dest, RGBPixel[,] ImageMatrix) // O(EV)
        {
            double infinity = 9999999999999999999; // θ(1)
            int MinValue = -1; // θ(1)

            int W = ImageOperations.GetWidth(ImageMatrix); // θ(1)
            int H = ImageOperations.GetHeight(ImageMatrix); // θ(1)
            int Pixel_number = W * H; // θ(1)

            List<double> Distance = new List<double>(); // θ(1)
            Distance = Enumerable.Repeat(infinity, Pixel_number).ToList(); //O(E)

            List<int> ParentsPath = new List<int>(); // θ(1)
            ParentsPath = Enumerable.Repeat(MinValue, Pixel_number).ToList(); //O(E)

            PeriorityQueue Shortest_Distances = new PeriorityQueue(); // θ(1)
            Shortest_Distances.Push(new Arc(-1, src, 0)); // θ(1)

            if (dest == 0) // O(EV)
            {
                while (!Shortest_Distances.IsEmpty()) // O(EV)
                {
                    Arc CurrentEdge = Shortest_Distances.Top(); // θ(1)
                    Shortest_Distances.Pop(); // O(Log(V))

                    if (CurrentEdge.Cost < Distance[CurrentEdge.dest]) // O(V)
                    {
                        Distance[CurrentEdge.dest] = CurrentEdge.Cost; // θ(1)
                        ParentsPath[CurrentEdge.dest] = CurrentEdge.src; // θ(1)

                        List<Arc> neibours = Functions.GetSibling(CurrentEdge.dest, ImageMatrix);
                        int i = 0; // θ(1)
                        var neiboursNUm = neibours.Count; // θ(1)
                        while (i < neiboursNUm) // O(V)
                        {
                            if (Distance[neibours[i].dest] > Distance[neibours[i].src] + neibours[i].Cost) // θ(1)
                            {
                                neibours[i].Cost += Distance[neibours[i].src]; // θ(1)
                                Shortest_Distances.Push(neibours[i]); // θ(1)
                            }
                            i++; // θ(1)
                        }
                    }
                }
                return ParentsPath; // θ(1)
            }
            else
            {
                while (!Shortest_Distances.IsEmpty()) // O(EV)
                {
                    Arc CurrentEdge = Shortest_Distances.Top(); // θ(1)
                    Shortest_Distances.Pop(); // O(Log(V))

                    if (CurrentEdge.Cost < Distance[CurrentEdge.dest]) // O(V)
                    {
                        ParentsPath[CurrentEdge.dest] = CurrentEdge.src; // θ(1)
                        Distance[CurrentEdge.dest] = CurrentEdge.Cost; // θ(1)
                        if (CurrentEdge.dest == dest) break; // θ(1)

                        List<Arc> neibours = Functions.GetSibling(CurrentEdge.dest, ImageMatrix);
                        int i = 0; // θ(1)
                        while (i < neibours.Count) // O(V)
                        {
                            if (Distance[neibours[i].dest] > Distance[neibours[i].src] + neibours[i].Cost) // θ(1)
                            {
                                neibours[i].Cost += Distance[neibours[i].src]; // θ(1)
                                Shortest_Distances.Push(neibours[i]); // θ(1)
                            }
                            i++; // θ(1)
                        }
                    }
                }
                return ParentsPath; // θ(1)
            }
        }
       
    }
}
