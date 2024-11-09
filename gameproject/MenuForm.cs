using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gameproject {
    public partial class MenuForm : Form {
        public MenuForm() {
            InitializeComponent();
        }

        private void btnStartGame_Click(object sender, EventArgs e) {
            this.Hide();
            var gameForm = new GameForm();
            gameForm.Closed += (s, args) => this.Close();
            gameForm.Show();
        }
    }
}
