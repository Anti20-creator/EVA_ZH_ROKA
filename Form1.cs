using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EVA_ZH.Model;

namespace EVA_ZH
{
    public partial class Form1 : Form
    {
        private GameModel _model;
        private Button[,] _buttonGrid;
        Timer _timer;
        public Form1()
        {
            InitializeComponent();
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Tick += Tick;

            _model = new GameModel();
            _model.generateTable += generateTable;
            _model.refreshTable += refreshTable;
            _model.gameOver += gameOver;
            _buttonGrid = new Button[0,0];
            gameStart1.Click += StartGameClick;
        }

        private void gameOver(object sender, int e)
        {
            _timer.Stop();
            MessageBox.Show("A játéknak vége." + Environment.NewLine + "Pontjaid száma: " + e.ToString());
        }

        private void Tick(object sender, EventArgs e)
        {
            _model.advanceTime();
        }

        public void generateTable(object sender, int e)
        {
            foreach (var btn in Controls.OfType<Button>().ToList())
            {
                Controls.Remove(btn);
            }

            if (_buttonGrid != null) Array.Clear(_buttonGrid, 0, _buttonGrid.Length);
            _buttonGrid = new Button[e, e];

            for (int i = 0; i < _model.Size; ++i)
            {
                for (int j = 0; j < _model.Size; ++j)
                {
                    _buttonGrid[i, j] = new Button();
                    _buttonGrid[i, j].Location = new Point(5 + 50 * j, 35 + 50 * i);
                    _buttonGrid[i, j].Size = new Size(50, 50);
                    _buttonGrid[i, j].Enabled = false;
                    _buttonGrid[i, j].TabIndex = 100 + i * _model.Size + j;
                    _buttonGrid[i, j].FlatStyle = FlatStyle.Flat; // lapított stípus

                    Controls.Add(_buttonGrid[i, j]);
                }
            }
        }

        private void refreshTable(object sender, int e)
        {
            for (int i = 0; i < e; ++i)
            {
                for (int j = 0; j < e; ++j)
                {
                    switch (_model.getMapElem(i, j))
                    {
                        case 0:
                            _buttonGrid[i, j].BackColor = Color.White;
                            break;

                        case 1:
                            _buttonGrid[i, j].BackColor = Color.Black;
                            break;

                        case 2:
                            _buttonGrid[i, j].BackColor = Color.Yellow;
                            break;
                        
                        case 3:
                            _buttonGrid[i, j].BackColor = Color.Green;
                            break;
                    }
                }
            }
            healthText.Text = _model.getHealth().ToString();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!_timer.Enabled) return;

            //if (!_model.getCanMove()) return;
            if (e.KeyCode == Keys.Left)
            {
                _model.movePlayer(0, -1);
            }
            if (e.KeyCode == Keys.Right)
            {
                _model.movePlayer(0, 1);
                //MessageBox.Show("Right");
            }
            if (e.KeyCode == Keys.Up)
            {
                _model.movePlayer(-1, 0);
                //MessageBox.Show("Up");
            }
            if (e.KeyCode == Keys.Down)
            {
                _model.movePlayer(1, 0);
                //MessageBox.Show("Down");
            }
        }

        public void StartGameClick(object sender, EventArgs e)
        {
            _model.newGame(6);
            _timer.Start();
            //refreshTable(this, 6);
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
