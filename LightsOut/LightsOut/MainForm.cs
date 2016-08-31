using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightsOut
{
    public partial class MainForm : Form
    {
        private int GridOffset = 50;
        private int GridLength = 250;
        private int NumCells = 3;
        private int CellLength;
        private int moveCount = 0;
        private bool[,] grid;
        private Random rand;

        public MainForm()
        {
            InitializeComponent();

            CellLength = GridLength / NumCells;
            rand = new Random();
            grid = new bool[NumCells, NumCells];

            for (int r = 0; r < NumCells; r++)
            {
                for (int c = 0; c < NumCells; c++)
                {
                    grid[r, c] = true;
                }
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int r = 0; r < NumCells; r++)
            {
                for (int c = 0; c < NumCells; c++)
                {
                    Brush brush;
                    Pen pen;

                    if (grid[r, c])
                    {
                        pen = Pens.Black;
                        brush = Brushes.White;
                    }
                    else
                    {
                        pen = Pens.White;
                        brush = Brushes.Black;
                    }

                    int x = c * CellLength + GridOffset;
                    int y = r * CellLength + GridOffset;

                    g.DrawRectangle(pen, x, y, CellLength, CellLength);
                    g.FillRectangle(brush, (x + 1), (y + 1), (CellLength - 1), (CellLength - 1));
                }
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < GridOffset || e.X > CellLength * NumCells + GridOffset ||
                e.Y < GridOffset || e.Y > CellLength * NumCells + GridOffset)
            {
                return;
            }

            moveCount++;

            int r = (e.Y - GridOffset) / CellLength;
            int c = (e.X - GridOffset) / CellLength;

            for (int i = r - 1; i <= r + 1; i++)
            {
                for (int j = c - 1; j <= c + 1; j++)
                {
                    if (i >= 0 && i < NumCells && j >= 0 && j < NumCells)
                    {
                        grid[i, j] = !grid[i, j];
                    }
                }
            }

            this.Invalidate();

            if (PlayerWon())
            {
                MessageBox.Show(this, "Congratulations! You won in " + moveCount + " moves!", "Lights Out!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                NewGame_Click(sender, e);
            }
        }

        private void NewGame_Click(object sender, EventArgs e)
        {
            for (int r = 0; r < NumCells; r++)
            {
                for (int c = 0; c < NumCells; c++)
                {
                    grid[r, c] = rand.Next(2) == 1;
                }
            }

            moveCount = 0;
            this.Invalidate();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool PlayerWon()
        {
            bool result = true;

            for (int i = 0; i < NumCells; i++)
            {
                for (int j = 0; j < NumCells; j++)
                {
                    if (grid[i, j] == true)
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame_Click(sender, e);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit_Click(sender, e);
        }

        private void x3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = true;
            x4ToolStripMenuItem.Checked = false;
            x5ToolStripMenuItem.Checked = false;
            
            NumCells = 3;
            CellLength = GridLength / NumCells;
            grid = new bool[NumCells, NumCells];
            NewGame_Click(sender, e);
        }

        private void x4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = false;
            x4ToolStripMenuItem.Checked = true;
            x5ToolStripMenuItem.Checked = false;
            
            NumCells = 4;
            CellLength = GridLength / NumCells;
            grid = new bool[NumCells, NumCells];

            NewGame_Click(sender, e);
        }

        private void x5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            x3ToolStripMenuItem.Checked = false;
            x4ToolStripMenuItem.Checked = false;
            x5ToolStripMenuItem.Checked = true;
            
            NumCells = 5;
            CellLength = GridLength / NumCells;
            grid = new bool[NumCells, NumCells];
            NewGame_Click(sender, e);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutForm aboutBox = new AboutForm();
            aboutBox.ShowDialog(this);
        }
    }
}
