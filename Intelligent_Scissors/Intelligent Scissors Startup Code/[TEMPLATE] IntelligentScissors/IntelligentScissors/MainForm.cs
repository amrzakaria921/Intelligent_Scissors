using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IntelligentScissors
{

    public partial class MainForm : Form
    {
        RGBPixel[,] ImageMatrix; // 0(1)
        List<int> parent_list; // 0(1)
        List<Point> Anchors; // 0(1)
        List<Point> SelectedPoints; // 0(1)
        List<Point> CurrentPath; // 0(1)
        RGBPixel[,] CropedImage; // 0(1)
        Boundary ImageBoundry; // 0(1)
        int lastclick = -1, FirstClick = -1; // 0(1)

        public MainForm() // 0(1)
        {
            InitializeComponent();
            Anchors = new List<Point>(); // 0(1)
            SelectedPoints = new List<Point>(); // 0(1)
        }
        void reset() // O(N)
        {
            CurrentPath = null; // 0(1)
            Anchors.Clear(); // O(N)
            SelectedPoints.Clear(); // O(N)
            lastclick = -1; // 0(1)
            FirstClick = -1; // 0(1)
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox);
                reset();
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }
        private void DrawPath(object sender, PaintEventArgs e)
        {
            Point AnchorSize = new Point(5, 5);  // SHAPE OF ANCHOR POINT 
            if (ImageMatrix != null)
            {
                foreach (Point v in Anchors)
                {
                    e.Graphics.FillEllipse(Brushes.Yellow, new Rectangle(new Point(v.X - AnchorSize.X / 2, v.Y - AnchorSize.Y / 2),new Size(AnchorSize)));
                }
                if (CurrentPath != null)
                {
                    if (CurrentPath.Count > 1)
                    {
                        e.Graphics.DrawLines(new Pen(Color.Aqua, 1), CurrentPath.ToArray());
                    }
                }
                if (SelectedPoints != null && SelectedPoints.Count > 5)
                {
                    e.Graphics.DrawLines(new Pen(Color.Orange, 1), SelectedPoints.ToArray());
                }
            }
        }
        private void Mouse_Movement(object sender, MouseEventArgs e) // O(E log(V))
        {
            if (ImageMatrix != null) // O(E log(V))
            {
                var mouseNode = Functions.twoDtoOneD(e.X, e.Y, ImageOperations.GetWidth(ImageMatrix)); // 0(1)
                if (FirstClick != -1) // O(E log(V))
                {
                    if (Functions.CheckBoundry(mouseNode, ImageBoundry, ImageOperations.GetWidth(ImageMatrix))) // O(V)
                    {
                        int T_MouseLocation = Functions.TransferPixels(mouseNode, ImageBoundry,
                            ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(CropedImage)); // 0(1)
                        List<Point> Path = new List<Point>(); // 0(1)
                        Path = ShortestPath.Inverting_Path(parent_list, T_MouseLocation, ImageOperations.GetWidth(CropedImage)); // O(V)
                        List<Point> Curpath = Functions.TransferPixels(Path, ImageBoundry); // 0(1)
                        CurrentPath = Curpath; // 0(1)
                    }
                    else // O(E log(V))
                    {
                        CurrentPath = ShortestPath.Create_ShortestPath(lastclick, mouseNode, ImageMatrix); // O(E log(V))
                    }
                }
            }
            pictureBox.Invalidate();
        }
        private void Mouse_Click(object sender, MouseEventArgs e) // O(EV)
        {
            if (pictureBox.Image != null) // O(EV)
            {
                var clicked_pixel = Functions.twoDtoOneD(e.X, e.Y, ImageOperations.GetWidth(ImageMatrix)); // 0(1)
                //save frist clicked anchor
                if (lastclick == -1) // 0(1) 
                    FirstClick = clicked_pixel; // 0(1)
                //save next clicks
                else // O(E)
                {
                    int i = 0; // 0(1)
                    while ( i < CurrentPath.Count) // O(E)
                    {
                        SelectedPoints.Add(CurrentPath[i]); // 0(1)
                        i++; // 0(1)
                    }
                }
                lastclick = clicked_pixel; // 0(1)
                Anchors.Add(e.Location); // 0(1)
                ImageBoundry = new Boundary(); // 0(1)
                ImageBoundry = Functions.Image_Boundary(clicked_pixel,ImageOperations.GetWidth(ImageMatrix) - 1, ImageOperations.GetHeight(ImageMatrix) - 1); // 0(1)
                //make a square segment
                CropedImage = Functions.CropedImageFrame(ImageMatrix, ImageBoundry); // O(V^2)
                // currsrc in segment
                int newsrc = Functions.TransferPixels(clicked_pixel, ImageBoundry, ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(CropedImage)); // 0(1)
                parent_list = ShortestPath.Dijkstra(newsrc,0, CropedImage); // O(EV)
            }
        }
        private void Crop_Click(object sender, EventArgs e) // O(E^2 + V)
        {
            if (lastclick != FirstClick) // O(E ^ 2 + V)
            {
                if (Functions.CheckBoundry(FirstClick, ImageBoundry, ImageOperations.GetWidth(ImageMatrix))) // 0(1)
                {
                    int T_MouseLocation = Functions.TransferPixels(FirstClick, ImageBoundry, ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(CropedImage)); // ?(1)
                    List<Point> Path = new List<Point>(); // 0(1)
                    Path = ShortestPath.Inverting_Path(parent_list, T_MouseLocation, ImageOperations.GetWidth(CropedImage)); // O(V)
                    CurrentPath = Functions.TransferPixels(Path, ImageBoundry); // 0(1)
                }
                else // O(EV)
                {
                    CurrentPath = ShortestPath.Create_ShortestPath(lastclick, FirstClick, ImageMatrix); // O(EV)
                }
                int i = 0; // 0(1)
                while ( i < CurrentPath.Count) // O(E)
                {
                    SelectedPoints.Add(CurrentPath[i]); // 0(1)
                    i++; // 0(1)
                }
                RGBPixel[,] selected_image = CropingImageFunctions.Croped_Image(SelectedPoints, ImageMatrix); // O(E^2 + V)
                CropedImage Croped = new CropedImage(selected_image); // 0(1)
                Croped.Show(); // 0(1)
                reset(); // O(N)
            }
        }
        private void Clear_Click(object sender, EventArgs e) // O(N)
        {
            reset(); // O(N)
        }
    }  
}


