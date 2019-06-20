using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        const int SECTION_SIZE = 12;
        Bitmap bitmap;

        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Graphics g = e.Graphics;
            //g.InterpolationMode = InterpolationMode.NearestNeighbor;


            //for(int i = 0; i < 720; i += SECTION_SIZE)
            //{
            //    g.DrawLine(Pens.DimGray, i, 0, i, 720);
            //    g.DrawLine(Pens.DimGray, i + SECTION_SIZE - 1, 0, i + SECTION_SIZE - 1, 720);
            //    g.DrawLine(Pens.DimGray, 0, i, 720, i);
            //    g.DrawLine(Pens.DimGray, 0, i + SECTION_SIZE - 1, 720, i + SECTION_SIZE - 1);

            //}
            //Random r =new Random();
            //for (int i = 0; i < 720; i += SECTION_SIZE)
            //{
            //    for (int j = 0; j < 720; j += SECTION_SIZE)
            //    {
            //        if (r.Next() % 2 == 0)
            //        {
            //            g.DrawRectangle(Pens.ForestGreen, i + 1, j + 1, SECTION_SIZE - 3, SECTION_SIZE - 3);
            //            g.FillRectangle(Brushes.ForestGreen, i + 1, j + 1, SECTION_SIZE - 3, SECTION_SIZE - 3);
            //        }
            //    }
            //}

            e.Graphics.DrawImage(bitmap, 0, 0);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Black);

            for (int i = 0; i < 720; i += SECTION_SIZE)
            {
                g.DrawLine(Pens.DimGray, i, 0, i, 720);
                g.DrawLine(Pens.DimGray, i + SECTION_SIZE - 1, 0, i + SECTION_SIZE - 1, 720);
                g.DrawLine(Pens.DimGray, 0, i, 720, i);
                g.DrawLine(Pens.DimGray, 0, i + SECTION_SIZE - 1, 720, i + SECTION_SIZE - 1);

            }
            Random r = new Random();
            for (int i = 0; i < 720; i += SECTION_SIZE)
            {
                for (int j = 0; j < 720; j += SECTION_SIZE)
                {
                    if (r.Next() % 2 == 0)
                    {
                        g.DrawRectangle(Pens.ForestGreen, i + 1, j + 1, SECTION_SIZE - 3, SECTION_SIZE - 3);
                        g.FillRectangle(Brushes.ForestGreen, i + 1, j + 1, SECTION_SIZE - 3, SECTION_SIZE - 3);
                    }
                }
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form1_Load(this, null);
        }
    }
}
