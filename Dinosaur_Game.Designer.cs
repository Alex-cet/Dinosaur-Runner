namespace Dinosaur_Game
{
    partial class Dinosaur_Game
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dinosaur = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dinosaur)).BeginInit();
            this.SuspendLayout();
            // 
            // dinosaur
            // 
            this.dinosaur.BackColor = System.Drawing.Color.Transparent;
            this.dinosaur.Image = global::Dinosaur_T_Rex_Game.Properties.Resources.dinosaur;
            this.dinosaur.Location = new System.Drawing.Point(24, 212);
            this.dinosaur.Name = "dinosaur";
            this.dinosaur.Size = new System.Drawing.Size(50, 50);
            this.dinosaur.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.dinosaur.TabIndex = 0;
            this.dinosaur.TabStop = false;
            // 
            // Dinosaur_Game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dinosaur);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "Dinosaur_Game";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dinosaur Game";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Dinosaur_Game_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dinosaur)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox dinosaur;
    }
}

