namespace gameproject
{
    partial class GameForm
    {
        private System.ComponentModel.IContainer components = null;

        private ProgressBar healthBar;
        private ProgressBar mpBar;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            healthBar = new ProgressBar();
            mpBar = new ProgressBar();
            lblHP = new Label();
            lblMP = new Label();
            SuspendLayout();
            // 
            // healthBar
            // 
            healthBar.ForeColor = Color.Red;
            healthBar.Location = new Point(10, 55);
            healthBar.Name = "healthBar";
            healthBar.Size = new Size(200, 20);
            healthBar.TabIndex = 0;
            healthBar.Value = 100;
            // 
            // mpBar
            // 
            mpBar.BackColor = SystemColors.ActiveCaption;
            mpBar.ForeColor = Color.RoyalBlue;
            mpBar.Location = new Point(10, 98);
            mpBar.Name = "mpBar";
            mpBar.Size = new Size(200, 20);
            mpBar.TabIndex = 1;
            mpBar.Value = 50;
            // 
            // lblHP
            // 
            lblHP.AutoSize = true;
            lblHP.BackColor = Color.Transparent;
            lblHP.ForeColor = SystemColors.Control;
            lblHP.Location = new Point(12, 32);
            lblHP.Name = "lblHP";
            lblHP.Size = new Size(28, 20);
            lblHP.TabIndex = 2;
            lblHP.Text = "HP";
            // 
            // lblMP
            // 
            lblMP.AutoSize = true;
            lblMP.BackColor = Color.Transparent;
            lblMP.ForeColor = Color.White;
            lblMP.Location = new Point(11, 76);
            lblMP.Name = "lblMP";
            lblMP.Size = new Size(30, 20);
            lblMP.TabIndex = 3;
            lblMP.Text = "MP";
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblMP);
            Controls.Add(lblHP);
            Controls.Add(healthBar);
            Controls.Add(mpBar);
            Name = "GameForm";
            Text = "GameForm";
            KeyDown += formKeyDown;
            MouseMove += formMouseMove;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblHP;
        private Label lblMP;
    }
}
