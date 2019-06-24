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
    public partial class GameOfLife : Form
    {
        //Note: A progress bar has been added to give the user some feedback when clicking the screen.
        //      Running the debug build causes the program to hang when calculating the new phase, so this progress bar's development can be seen.
        //      Running the release build calculates the next phase almost instantaneously, rendering the bar useless.

        const int SECTION_SIZE = 12; // 1 box is 12x12 pixels minimum, line border 1 pixel which leaves 10x10 to be filled in. This can be scaled to multiples of 12 for bigger or smaller games.
        const int GRID_OFFSET = 5*SECTION_SIZE; // create blank space around the board for the life to move into
        
        Bitmap bitmap;
        bool[,] lifeGrid;
        int gridSizeX, gridSizeY;

        public GameOfLife()
        {
            if (SECTION_SIZE<12 || SECTION_SIZE > 360 || SECTION_SIZE%12 != 0 )
            {
                MessageBox.Show("Section Size was set to an invalid size.");
                Application.Exit();
            }

            InitializeComponent();
            
            gridSizeX = pictureBox1.Width / SECTION_SIZE;
            gridSizeY = pictureBox1.Height / SECTION_SIZE;
            lifeGrid = new bool[gridSizeX, gridSizeY];
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            toolStripProgressBar1.Maximum = 720 / SECTION_SIZE + gridSizeX; // 1 step for each main drawing loop, 1 step for each calculateNextPhase loop
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bitmap, 0, 0);
        }


        private void GameOfLife_Load(object sender, EventArgs e)
        {

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
            for (int i = 0 + GRID_OFFSET; i < 720 - GRID_OFFSET; i += SECTION_SIZE)
            {
                for (int j = 0 + GRID_OFFSET; j < 720 - GRID_OFFSET; j += SECTION_SIZE)
                {
                    if (r.Next() % 2 == 0)
                    {
                        lifeGrid[i/ SECTION_SIZE, j/ SECTION_SIZE] = true;
                        g.DrawRectangle(Pens.ForestGreen, i + 1, j + 1, SECTION_SIZE - 3, SECTION_SIZE - 3);
                        g.FillRectangle(Brushes.ForestGreen, i + 1, j + 1, SECTION_SIZE - 3, SECTION_SIZE - 3);
                    }
                    else
                    {
                        lifeGrid[i/ SECTION_SIZE, j/ SECTION_SIZE] = false;
                    }
                }
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Visible = false;
            calculateNextPhase();
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Black);


            for (int i = 0; i < 720; i += SECTION_SIZE)
            {
                g.DrawLine(Pens.DimGray, i, 0, i, 720);
                g.DrawLine(Pens.DimGray, i + SECTION_SIZE - 1, 0, i + SECTION_SIZE - 1, 720);
                g.DrawLine(Pens.DimGray, 0, i, 720, i);
                g.DrawLine(Pens.DimGray, 0, i + SECTION_SIZE - 1, 720, i + SECTION_SIZE - 1);

                for (int j = 0; j < 720; j += SECTION_SIZE)
                {
                    if (lifeGrid[i/ SECTION_SIZE, j/ SECTION_SIZE])
                    {
                        
                        g.DrawRectangle(Pens.ForestGreen, i + 1, j + 1, SECTION_SIZE - 3, SECTION_SIZE - 3);
                        g.FillRectangle(Brushes.ForestGreen, i + 1, j + 1, SECTION_SIZE - 3, SECTION_SIZE - 3);
                    }
                }
                toolStripProgressBar1.PerformStep();
            }
            pictureBox1.Invalidate();
            //toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Visible = true;
        }

        private void calculateNextPhase()
        {
            bool currentBoxState = false;
            int boxNeighbours = 0;
            bool[,] newGrid = new bool[gridSizeX, gridSizeY];
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j ++) // for each block on the grid, lifeGrid[i,j]
                {
                    boxNeighbours = 0;
                    for (int di = -1; di<=1; di++)
                    {
                        for (int dj = -1; dj <= 1; dj++)
                        {
                            try
                            {
                                if (di == 0 && dj == 0)
                                {
                                    currentBoxState = lifeGrid[i, j];
                                }
                                else if (lifeGrid[i + di, j + dj])
                                {
                                    boxNeighbours++;
                                }
                            }
                            catch (IndexOutOfRangeException) { }
                        }
                    }

                    switch (boxNeighbours)
                    {
                        // if box has 0-1, or 4-8 neighbours, it is always dead next turn
                        // if box has 2 neighbours, essentially no change to its current state
                        // if box has 3 neighbours, it is always alive next turn
                        case 0:
                        case 1: 
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8: newGrid[i, j] = false; break;

                        case 2: newGrid[i, j] = lifeGrid[i, j]; break;

                        case 3: newGrid[i, j] = true; break;
                    }
                }
                toolStripProgressBar1.PerformStep();
            }
            lifeGrid = newGrid;
        }
    }
}
