using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace gameproject {
    public partial class MenuForm : Form
    {
        
        private WindowsMediaPlayer bgmPlayer;
        private string tempFilePath;

        public MenuForm()
        {
            InitializeComponent();

            tempFilePath = Path.Combine(Path.GetTempPath(), "Blackjack_theme.wav");
            File.WriteAllBytes(tempFilePath, Properties.Resources.Blackjack_theme); 

            
            bgmPlayer = new WindowsMediaPlayer
            {
                URL = tempFilePath 
            };

            // Set looping mode
            bgmPlayer.settings.setMode("loop", true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
            bgmPlayer.controls.play();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            
            bgmPlayer.controls.stop();

            this.Hide();
            var gameForm = new GameForm();
            gameForm.Closed += (s, args) => this.Close();
            gameForm.Show();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);

            // Clean up the player
            bgmPlayer.controls.stop();
            bgmPlayer.close();
        }
    }
}
