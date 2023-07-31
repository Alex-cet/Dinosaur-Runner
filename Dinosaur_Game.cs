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
        // Magic numbers
        private readonly int formWidth = 800;
        private readonly int formHeight = 500;
        private readonly int dinosaurXLocation = 10;
        private readonly int dinosaurYLocation = 220;
        private readonly int startGameMessageWidth = 200;
        private readonly int startGameMessageHeight = 40;
        private readonly int startGameXLocation = 280;
        private readonly int startGameYLocation = 50;
        private readonly int backgroundWidh = 800;
        private readonly int backgroundHeight = 147;
        private readonly int backgroundYLocation = 182;
        private readonly int dinosaurJumpLimit = 108;
        private readonly int dinosaurMoveUnit = 4;
        private readonly int cactusMoveUnit = 2;
        private readonly int lowestCactusHeight = 20;
        private readonly int cactusWidth = 30;
        private readonly int cactusOutOfScreenXLocation = -15;

        // Variables used in the functionality
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

        // Constructor
        public Dinosaur_Game() {
            InitializeComponent();
            displayStartGameMessage();
            generateBackground();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximumSize = new Size(formWidth, formHeight);
            this.MinimumSize = new Size(formWidth, formHeight);
            this.WindowState = FormWindowState.Normal;
            this.KeyPreview = true;
            dinosaur.Location = new Point(dinosaurXLocation, dinosaurYLocation);
        }
        
        // Methods
        private void displayStartGameMessage() {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("D:\\Ce ar trbui sa pastrez\\GitHub Projects\\Dinosaur T-Rex Game\\Font\\PressStart2P_Regular.ttf");
            startGame = new Label();
            startGame.Size = new Size(startGameMessageWidth, startGameMessageHeight);
            startGame.TextAlign = ContentAlignment.MiddleCenter;
            startGame.Font = new Font(pfc.Families[0], 10);
            startGame.Text = "Press space key to start the game";
            startGame.Location = new Point(startGameXLocation, startGameYLocation);
            startGame.Visible = true;
            this.Controls.Add(startGame);
        }

        private void generateBackground()
        {
            background = new PictureBox();
            background.SizeMode = PictureBoxSizeMode.StretchImage;
            background.Size = new Size(backgroundWidh, backgroundHeight);
            background.Image = Dinosaur_T_Rex_Game.Properties.Resources.background2;
            background.Location = new Point(0, backgroundYLocation);
            background.Visible = false;
            this.Controls.Add(background);
        }

        private void displayScore() {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile("D:\\Ce ar trbui sa pastrez\\GitHub Projects\\Dinosaur T-Rex Game\\Font\\PressStart2P_Regular.ttf");
            score = new Label();
            score.Size = new Size(startGameMessageWidth, startGameMessageHeight);
            score.TextAlign = ContentAlignment.MiddleCenter;
            score.Font = new Font(pfc.Families[0], 10);
            score.Text = "Score: 0";
            score.Location = new Point(startGameXLocation, startGameYLocation);
            score.Visible = true;
            this.Controls.Add(score);
        }

        
        private void Dinosaur_Game_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Space) {
                if (threadStart == 0) {
                    displayScore();
                    scoreTimer = new Thread(startTimer);
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

        private void startTimer() {
            while (!gameThread)
            {
                Thread.Sleep(1000);
                this.Invoke(new Action(() =>
                {
                    score.Text = "Score: " + ++i;
                }));
            }
        }

        private void dinosaurJump() {
            int flag = 0;
            while (!stopJumpThread) {
                this.Invoke(new Action(() => {
                    if (dinosaur.Location.Y >= dinosaurJumpLimit && flag == 0) {
                        dinosaur.Top -= dinosaurMoveUnit;
                    }
                    else if (dinosaur.Location.Y <= dinosaurYLocation) {
                        dinosaur.Top += dinosaurMoveUnit;
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

                cactusHeight = lowestCactusHeight + rand.Next(1, 30);

                this.Invoke(new Action(() => {
                    PictureBox cactus = new PictureBox();
                    cactus.Image = Dinosaur_T_Rex_Game.Properties.Resources.cactuss;
                    cactus.BackColor = Color.Transparent;
                    cactus.Size = new Size(cactusWidth, cactusHeight);
                    cactus.SizeMode = PictureBoxSizeMode.StretchImage;
                    cactus.BackColor = Color.Transparent;
                    cactus.Location = new Point(backgroundWidh + 50, 265 - cactusHeight);
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
                    if (cactus.Location.X >= cactusOutOfScreenXLocation) {
                        cactus.Left -= cactusMoveUnit;
                        checkCollision(cactus);
                    }
                    else {
                        cactus.Dispose();
                    }
                }));
            }
        }

        private void checkCollision(PictureBox cactus) {
            if (dinosaur.Location.X + startGameMessageHeight >= cactus.Location.X && dinosaur.Location.Y > (cactus.Location.Y - cactusHeight)) {
                gameThread = true;
                stopJumpThread = true;
                background.Enabled = false;
                MessageBox.Show("Game over! Your score: " + i, "EndGame", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
        }
    }
}
