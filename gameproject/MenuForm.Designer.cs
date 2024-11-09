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
        private void InitializeComponent() {
            label1 = new Label();
            btnStartGame = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(163, 152);
            label1.Name = "label1";
            label1.Size = new Size(30, 20);
            label1.TabIndex = 0;
            label1.Text = "ola";
            // 
            // btnStartGame
            // 
            btnStartGame.Location = new Point(216, 228);
            btnStartGame.Name = "btnStartGame";
            btnStartGame.Size = new Size(94, 29);
            btnStartGame.TabIndex = 1;
            btnStartGame.Text = "start game";
            btnStartGame.UseVisualStyleBackColor = true;
            btnStartGame.Click += btnStartGame_Click;
            // 
            // MenuForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnStartGame);
            Controls.Add(label1);
            Name = "MenuForm";
            Text = "MenuForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnStartGame;
    }
}