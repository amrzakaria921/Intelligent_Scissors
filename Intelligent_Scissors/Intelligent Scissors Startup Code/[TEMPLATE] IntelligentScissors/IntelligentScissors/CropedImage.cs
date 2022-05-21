using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
namespace IntelligentScissors
{
    public partial class CropedImage : Form
    {
        public CropedImage()
        {
            InitializeComponent();
        }
        public CropedImage(RGBPixel[,] cropedImage)
        {
            InitializeComponent();
            ImageOperations.DisplayImage(cropedImage, pictureBox1);
        }

    }
}

