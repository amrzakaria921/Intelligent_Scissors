using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace IntelligentScissors
{
    public class Arc // θ(1)
    {
        public int src, dest; // θ(1)
        public double Cost; // θ(1)
        public Arc(int src, int dest, double Cost) // θ(1)
        {
            this.src = src; // θ(1)
            this.dest = dest; // θ(1)
            this.Cost = Cost; // θ(1)
        }
    }
    public static class Functions
    {
        public static RGBPixel[,] CropedImageFrame(RGBPixel[,] ImageMatrix, Boundary border) // O(V^2)
        {
            int W = border.X_max - border.X_min; // θ(1)
            int H = border.Y_max - border.Y_min; // θ(1)

            RGBPixel[,] CropedImage = new RGBPixel[H + 1, W + 1]; // θ(1)

            int i = 0; // θ(1)
            while (i <= H) // O(V^2)
            {
                int j = 0; // θ(1)
                while (j <= W) // O(V)
                {
                    CropedImage[i, j] = ImageMatrix[border.Y_min + i, border.X_min + j]; // θ(1)
                    j++; // θ(1)
                }
                i++; // θ(1)
            }
            return CropedImage; // θ(1)
        }
        public static bool CheckBoundry(int Target, Boundary border, int Width) // θ(1)
        {
            Vector2D TwoD = oneDtoTwoD(Target, Width); // θ(1)
            bool CheckX = false, CheckY = false; // θ(1)

            if (TwoD.X >= border.X_min && TwoD.X < border.X_max) // θ(1)
                CheckX = true; // θ(1)
            if (TwoD.Y >= border.Y_min && TwoD.Y < border.Y_max) // θ(1)
                CheckY = true; // θ(1)

            return CheckX && CheckY; // θ(1)
        }
        public static List<Point> TransferPixels(List<Point> Path, Boundary boundry) // O(V)
        {
            int i = 0; // θ(1)
            while (i < Path.Count) // O(V)
            {
                Point P = Path[i]; // θ(1)
                P.X = Path[i].X + boundry.X_min; // θ(1)
                P.Y = Path[i].Y + boundry.Y_min; // θ(1)
                Path[i] = P; // θ(1)
                i++; // θ(1)
            }
            return Path;// θ(1)
        }
        public static int TransferPixels(int node_number, Boundary bondry, int Main, int Segment) // θ(1)
        {
            Vector2D TwoD = oneDtoTwoD(node_number, Main); // θ(1)
            TwoD.X = TwoD.X - bondry.X_min; // θ(1)
            TwoD.Y = TwoD.Y - bondry.Y_min; // θ(1)
            int OneD = twoDtoOneD((int)TwoD.X, (int)TwoD.Y, Segment); // θ(1)
            return OneD; // θ(1)
        }
        public static int twoDtoOneD(int X, int Y, int W) // θ(1)
        {
            int temp = (X) + (Y * W); // θ(1)
            return temp; // θ(1)
        }
        public static Vector2D oneDtoTwoD(int Index, int width) // θ(1)
        {
            double x = Index % width; // θ(1)
            double y = Index / width; // θ(1)
            Vector2D temp = new Vector2D(x, y); // θ(1)
            return temp; // θ(1)
        }
        public static Boundary Image_Boundary(int Src, int Width, int Height) // θ(1)
        {
            Vector2D TwoD = Functions.oneDtoTwoD(Src, Width + 1); // θ(1)
            Boundary boundry = new Boundary(); // θ(1)
            int max_dist = 150; // θ(1)
            if (TwoD.X > max_dist) // θ(1)
                boundry.X_min = (int)TwoD.X - max_dist; // θ(1)
            else // θ(1)
                boundry.X_min = 0; // θ(1)

            if (Width - TwoD.X > max_dist) // θ(1)
                boundry.X_max = (int)TwoD.X + max_dist; // θ(1)
            else // θ(1)
                boundry.X_max = Width; // θ(1)

            if (TwoD.Y > max_dist) // θ(1)
                boundry.Y_min = (int)TwoD.Y - max_dist; // θ(1)
            else // θ(1)
                boundry.Y_min = 0; // θ(1)

            if (Height - TwoD.Y > max_dist) // θ(1)
                boundry.Y_max = (int)TwoD.Y + max_dist; // θ(1)
            else // θ(1)
                boundry.Y_max = Height; // θ(1)
            return boundry; // θ(1)
        }
        public static List<Arc> GetSibling(int Pixel, RGBPixel[,] ImageMatrix) // θ(1) 
        {
            List<Arc> Sibling = new List<Arc>(); // θ(1)
            int H = ImageOperations.GetHeight(ImageMatrix); // θ(1)
            int W = ImageOperations.GetWidth(ImageMatrix); // θ(1)

            Vector2D TwoD = oneDtoTwoD(Pixel, W); // θ(1)
            int X = (int)TwoD.X; // θ(1)
            int Y = (int)TwoD.Y; // θ(1)
            var G = ImageOperations.CalculatePixelEnergies(X, Y, ImageMatrix);
            if (X < W - 1) // θ(1)
            {
                if (G.X != 0) // θ(1)
                    Sibling.Add(new Arc(Pixel, twoDtoOneD(X + 1, Y, W), 1 / (G.X))); // θ(1)
                else // θ(1)
                    Sibling.Add(new Arc(Pixel, twoDtoOneD(X + 1, Y, W), 10000000000000000)); // θ(1)
            }
            if (Y < H - 1) // θ(1)
            {
                if (G.Y != 0) // θ(1)
                    Sibling.Add(new Arc(Pixel, twoDtoOneD(X, Y + 1, W), 1 / (G.Y))); // θ(1)
                else // θ(1)
                    Sibling.Add(new Arc(Pixel, twoDtoOneD(X, Y + 1, W), 10000000000000000)); // θ(1)
            }
            if (Y > 0) // θ(1)
            {
                if (G.Y != 0) // θ(1)
                    Sibling.Add(new Arc(Pixel, twoDtoOneD(X, Y - 1, W), 1 / (G.Y))); // θ(1)
                else // θ(1)
                    Sibling.Add(new Arc(Pixel, twoDtoOneD(X, Y - 1, W), 10000000000000000)); // θ(1)
            }
            if (X > 0) // θ(1)
            {
                if (G.X != 0) // θ(1) 
                    Sibling.Add(new Arc(Pixel, twoDtoOneD(X - 1, Y, W), 1 / (G.X))); // θ(1)
                else // θ(1)
                    Sibling.Add(new Arc(Pixel, twoDtoOneD(X - 1, Y, W), 10000000000000000)); // θ(1)
            }
            return Sibling; // θ(1)
        }
    }
}
