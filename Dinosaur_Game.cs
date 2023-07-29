using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Text;
using System.IO;
using System.Reflection;

namespace Dinosaur_Game
{
    public partial class Dinosaur_Game : Form
    {
        private Boolean stopJumpThread = false;
        private Boolean gameThread = false;
        private PictureBox background;
        private Label startGame;
        private Label score;
        private Thread spaceKeyHandler;
        private Thread generateCacti;
        private Thread scoreTimer;
        private int threadStart = 0;
        private int i = 0;
        private int cactusHeight;

        public Dinosaur_Game() {
            InitializeComponent();
            displayStartGameMessage();
            generateBackground();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximumSize = new Size(800, 500);
            this.MinimumSize = new Size(800, 500);
            this.WindowState = FormWindowState.Normal;
            this.KeyPreview = true;
            dinosaur.Location = new Point(10, 220);
        }

        private void displayStartGameMessage() {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("D:\\Ce ar trbui sa pastrez\\GitHub Projects\\Dinosaur T-Rex Game\\Font\\PressStart2P_Regular.ttf");
            startGame = new Label();
            startGame.Size = new Size(200, 40);
            startGame.TextAlign = ContentAlignment.MiddleCenter;
            startGame.Font = new Font(pfc.Families[0], 10);
            startGame.Text = "Press space key to start the game";
            startGame.Location = new Point(280, 50);
            startGame.Visible = true;
            this.Controls.Add(startGame);
        }

        private void generateBackground()
        {
            background = new PictureBox();
            background.SizeMode = PictureBoxSizeMode.StretchImage;
            background.Size = new Size(800, 147);
            background.Image = Dinosaur_T_Rex_Game.Properties.Resources.background2;
            background.Location = new Point(0, 182);
            background.Visible = false;
            this.Controls.Add(background);
        }

        private void displayScore() {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("D:\\Ce ar trbui sa pastrez\\GitHub Projects\\Dinosaur T-Rex Game\\Font\\PressStart2P_Regular.ttf");
            score = new Label();
            score.Size = new Size(200, 40);
            score.TextAlign = ContentAlignment.MiddleCenter;
            score.Font = new Font(pfc.Families[0], 10);
            score.Text = "Score: 0";
            score.Location = new Point(280, 50);
            score.Visible = true;
            this.Controls.Add(score);
        }


        private void Dinosaur_Game_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Space) {
                if (threadStart == 0) {
                    displayScore();
                    scoreTimer = new Thread(() => {
                        while (!gameThread) {
                            Thread.Sleep(1000);
                            this.Invoke(new Action(() =>
                            {
                                score.Text = "Score: " + ++i;
                            }));
                        }
                    });
                    scoreTimer.Start();

                    generateCacti = new Thread(generateCactus);
                    generateCacti.Start();

                    threadStart = 1;
                }
                startGame.Visible = false;
                background.Visible = true;

                spaceKeyHandler = new Thread(dinosaurJump);
                spaceKeyHandler.Start();
            }
            stopJumpThread = false;
        }


        private void dinosaurJump() {
            int flag = 0;
            while (!stopJumpThread) {
                this.Invoke(new Action(() => {
                    if (dinosaur.Location.Y >= 108 && flag == 0) {
                        dinosaur.Top -= 4;
                    }
                    else if (dinosaur.Location.Y <= 220) {
                        dinosaur.Top += 4;
                        flag = 1;
                    }
                    else {
                        stopJumpThread = true;
                    }
                }));
                Thread.Sleep(1);
            }
        }

        private void generateCactus() {
            Random rand = new Random();
            while (!gameThread) {
                Thread.Sleep(rand.Next(900, 3000));

                cactusHeight = 20 + rand.Next(1, 30);

                this.Invoke(new Action(() => {
                    PictureBox cactus = new PictureBox();
                    cactus.Image = Dinosaur_T_Rex_Game.Properties.Resources.cactuss;
                    cactus.BackColor = Color.Transparent;
                    cactus.Size = new Size(30, cactusHeight);
                    cactus.SizeMode = PictureBoxSizeMode.StretchImage;
                    cactus.BackColor = Color.Transparent;
                    cactus.Location = new Point(850, 265 - cactusHeight);
                    cactus.Visible = true;
                    this.Controls.Add(cactus);
                    cactus.BringToFront();

                    Thread cactusThread = new Thread(() => {   
                        moveCactus(cactus);
                    });
                    cactusThread.Start();
                })); 
            }
        }

        private void moveCactus(PictureBox cactus) {
            while (!gameThread) {
                Thread.Sleep(1);
                this.Invoke(new Action(() => { 
                    if (cactus.Location.X >= -15) {
                        cactus.Left -= 2;
                        checkCollision(cactus);
                    }
                    else {
                        cactus.Dispose();
                    }
                }));
            }
        }

        private void checkCollision(PictureBox cactus) {
            if (dinosaur.Location.X + 40 >= cactus.Location.X && dinosaur.Location.Y > (cactus.Location.Y - cactusHeight)) {
                gameThread = true;
                stopJumpThread = true;
                MessageBox.Show("Game over! Your score: " + i, "EndGame", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
    }
}
