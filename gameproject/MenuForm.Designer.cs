namespace gameproject {
    partial class MenuForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            StartButton = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)StartButton).BeginInit();
            SuspendLayout();
            // 
            // StartButton
            // 
            StartButton.Anchor = AnchorStyles.None;
            StartButton.Image = Properties.Resources.StartButton;
            StartButton.Location = new Point(320, 233);
            StartButton.Name = "StartButton";
            StartButton.Size = new Size(171, 64);
            StartButton.SizeMode = PictureBoxSizeMode.StretchImage;
            StartButton.TabIndex = 0;
            StartButton.TabStop = false;
            StartButton.Click += StartButton_Click;
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(800, 450);
            Controls.Add(StartButton);
            Name = "MenuForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UltimateDelusion";
            ((System.ComponentModel.ISupportInitialize)StartButton).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox StartButton;
    }
}