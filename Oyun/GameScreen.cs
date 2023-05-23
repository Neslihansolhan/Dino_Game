using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Oyun
{
    public partial class Dino : Form
    {
        bool jump = false;
        int jumpCounter = 0;
        int jumping = 0;
        int score = 0;
        bool gameStart = false;
        int rightCounter = 0;
        int position;
        Random random = new Random();
        int dinoTop = 448;
        int picsSpeed = 10;

        public Dino()
        {
            InitializeComponent();
            GameResetSettings();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            rightCounter++;
            lblScore.Text = "Score : " + score;
            picDinoStart.Top += jumping;

            if (picDinoStart.Visible && !jump && picDinoStart.Top == 448)
            {
                picDinoStart.Visible = false;
                picDinoLeft.Visible = true;
            }

            if (picDinoLeft.Visible)
            {
                picDinoRight.Visible = true;
                picDinoLeft.Visible = false;
            }

            if (picDinoRight.Visible && rightCounter==2)
            {
                picDinoLeft.Visible = true;
                picDinoRight.Visible = false;
                rightCounter = 0;
            }

            if (rightCounter > 3)
                rightCounter = 0;


            if (jump)
            {
                picDinoLeft.Visible = false;
                picDinoRight.Visible = false;
                picDinoStart.Visible = true;

                jumping = -15;
            }
            else if(!jump && gameStart && picDinoStart.Top<448)
            {
                jumping = 15;
                //picDinoStart.Top = 448;
            }


            if (picDinoStart.Top > 448 && !jump)
            {
                picDinoStart.Top = 448;
                jumping = 0;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && ((string)x.Tag == "cactus" || (string)x.Tag == "rocks" || (string)x.Tag == "bird"))
                {
                    if (x.Tag != "rocks")
                    {
                        Random rnd = new Random();
                        rnd.Next(0, 1);
                        if (rnd.ToString() == "0")
                            x.Visible = false;
                        else
                            x.Visible = true;
                    }
                    if(x.Visible)
                        x.Left -= picsSpeed;

                    if (x.Left < -20)
                    {
                        x.Left = this.ClientSize.Width + random.Next(200, 500) + (x.Width * 15);
                        
                        score++;
                    }
                    
                    if (x.Tag != "rocks" && (picDinoStart.Bounds.IntersectsWith(x.Bounds) || (picDinoLeft.Visible && picDinoLeft.Bounds.IntersectsWith(x.Bounds)) || (picDinoRight.Visible && picDinoRight.Bounds.IntersectsWith(x.Bounds))))
                    {
                        gameTimer.Stop();
                        picDinoStart.Visible = false;
                        picDinoLeft.Visible = false;
                        picDinoRight.Visible = false;
                        picDinoDown.Visible = false;
                        picDinoDead.Visible = true;
                        lblScore.Text = " Press Space to restart the game!";
                        gameStart = false;
                    }
                    
                }
            }

            if (score > 5)
                picsSpeed = 15;
            if (score > 10)
                picsSpeed = 20;
        }

        private void GameResetSettings()
        {
            picDinoStart.Visible = true;
            picDinoStart.Top = 448;
            score = 0;
            picsSpeed = 10;
            lblScore.Text = "Score : " + score;

            foreach (Control x in this.Controls)
            {
                string bame = x.Name;
                if (x is PictureBox && ((string)x.Tag == "cactus" || (string)x.Tag == "rocks" || (string)x.Tag == "bird"))
                {
                    if ((string)x.Tag == "cactus" || (string)x.Tag == "rocks" || (string)x.Tag == "bird")
                    {
                        position = this.ClientSize.Width + random.Next(200, 500) + (x.Width * 10);

                        x.Left = position;
                    }
                    else if ((string)x.Tag == "dino" && x.Name != "picDinoStart")
                    {
                        x.Visible = false;
                    }
                }
            }
        }

        private void Dino_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !gameStart)
            {
                if (picDinoDead.Visible)
                {
                    picDinoDead.Visible = false;
                    picDinoStart.Visible = true;
                    GameResetSettings();
                }
                gameStart = true;
                gameTimer.Start();
            }

            if (e.KeyCode == Keys.Down)
            {
                picDinoLeft.Visible = false;
                picDinoRight.Visible = false;
                picDinoDown.Visible = true;
            }

            if (e.KeyCode == Keys.Up)
            {
                jump = true;
            }
        }

        private void Dino_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up && jump)
            {
                jump = false;
                jumpCounter = 0;
            }

            if (e.KeyCode == Keys.Down)
            {
                picDinoLeft.Visible = true;
                picDinoDown.Visible = false;
            }
        }
        
    }
}
