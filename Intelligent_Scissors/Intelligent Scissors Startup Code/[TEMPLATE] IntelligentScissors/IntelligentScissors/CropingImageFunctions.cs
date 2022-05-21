using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IntelligentScissors
{
    public struct Boundary // θ(1)
    {
        public int X_min; // θ(1)
        public int X_max; // θ(1)
        public int Y_min; // θ(1)
        public int Y_max; // θ(1)
    }
    public static class CropingImageFunctions
    {
        private static RGBPixel[,] CropedImage; // θ(1)
        public static RGBPixel[,] Croped_Image(List<Point> MainSelection, RGBPixel[,] ImageMatrix) // O(E^2 + V)
        {
            Boundary border = Border_Limits(MainSelection); // Boundary of the main selection
            CropedImage = Functions.CropedImageFrame(ImageMatrix, border); // get croped image 
            int counter = MainSelection.Count; // θ(1)
            int i = 0; // θ(1)
            while (i < counter) // O(V)
            {
                int X = MainSelection[i].X - border.X_min; // θ(1)
                int Y = MainSelection[i].Y - border.Y_min; // θ(1)
                CropedImage[Y, X].visited = true; // θ(1)
                i++; // θ(1)
            }
            filtering_Image(ImageOperations.GetWidth(CropedImage) - 1, ImageOperations.GetHeight(CropedImage) - 1); // O(E^2)
            return CropedImage; // θ(1)
        }
        private static void filtering_Image(int Width , int Height) // O(E^2)
        {
            int i = 0; // θ(1)
            while (i <= Width) // O(E^2)
            {
                if (!CropedImage[0, i].visited) // O(E)
                    DFS(new Vector2D(i, 0)); // O(E)
                i++; // θ(1)
            }
            i = 0; // θ(1)
            while (i <= Width) // O(E^2)
            {
                if (!CropedImage[Height, i].visited) // O(E)
                    DFS(new Vector2D(i, Height)); // O(E)
                i++; // θ(1)
            }
            i = 0; // θ(1)
            while (i <= Height) // O(E^2)
            {
                if (!CropedImage[i, 0].visited) // O(E)
                    DFS(new Vector2D(0, i)); // O(E)
                i++; // θ(1)
            }
            i = 0; // θ(1)
            while (i <= Height) // O(E^2)
            {
                if (!CropedImage[i, Width].visited) // O(E)
                    DFS(new Vector2D(Width, i)); // O(E)
                i++; // θ(1)
            }
        }
        private static void DFS(Vector2D SelectedPixel) // O(E)
        {
            Queue<Vector2D> queue = new Queue<Vector2D>(); // θ(1)
            queue.Enqueue(SelectedPixel); // θ(1)
            while (queue.Count > 0) // O(E)
            {
                Vector2D Cpixel = queue.Dequeue(); // θ(1)
                bool FoundX = (Cpixel.X >= 0 && Cpixel.X < ImageOperations.GetWidth(CropedImage)); // θ(1)
                bool FoundY = (Cpixel.Y >= 0 && Cpixel.Y < ImageOperations.GetHeight(CropedImage)); // θ(1)

                if (FoundX && FoundY && !CropedImage[(int)Cpixel.Y, (int)Cpixel.X].visited) // θ(1)
                {
                    //make these pixels white
                    CropedImage[(int)Cpixel.Y, (int)Cpixel.X].visited = true; // θ(1)
                    CropedImage[(int)Cpixel.Y, (int)Cpixel.X].red = 255; // θ(1)
                    CropedImage[(int)Cpixel.Y, (int)Cpixel.X].green = 255; // θ(1)
                    CropedImage[(int)Cpixel.Y, (int)Cpixel.X].blue = 255; // θ(1)

                    //push arounded pixels
                    queue.Enqueue(new Vector2D(Cpixel.X, Cpixel.Y + 1)); // θ(1)
                    queue.Enqueue(new Vector2D(Cpixel.X, Cpixel.Y - 1)); // θ(1)
                    queue.Enqueue(new Vector2D(Cpixel.X + 1, Cpixel.Y)); // θ(1)
                    queue.Enqueue(new Vector2D(Cpixel.X - 1, Cpixel.Y)); // θ(1)
                }
            }
        }
        private static Boundary Border_Limits(List<Point> selected_points)  // O(V)
        {
            Boundary border; // θ(1)
            border.X_max = border.Y_max = -999999999; // θ(1)
            border.X_min = border.Y_min =  999999999; // θ(1)
            int counter = selected_points.Count; // θ(1)
            int i = 0; // θ(1)
            while ( i < counter) // O(V)
            {
                if (selected_points[i].X > border.X_max) // θ(1)
                    border.X_max = selected_points[i].X; // θ(1)

                if (selected_points[i].X < border.X_min) // θ(1)
                    border.X_min = selected_points[i].X; // θ(1)

                if (selected_points[i].Y > border.Y_max) // θ(1)
                    border.Y_max = selected_points[i].Y; // θ(1)

                if (selected_points[i].Y < border.Y_min) // θ(1)
                    border.Y_min = selected_points[i].Y; // θ(1)
                i++; // θ(1)
            }
            return border; // θ(1)
        }
    }
}
