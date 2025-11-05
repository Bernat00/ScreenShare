using System;
using System.Threading;
using System.Windows.Forms;
using LibVLCSharp.Shared;


namespace LibVLCSharp.WinForms.Sample
{
    public partial class Form1 : Form
    {
        Menu menu;
        Shareing shareing;
        WatchShare watchShare;

        public Form1()
        {
            InitializeComponent();

            menu = new Menu();
            Controls.Add(menu);
            menu.ShareBTN_Clicked += ShareingEvent;
            menu.WatchBTN_Clicked += WatchShareEvent;
        }

        void ShareingEvent(object s, EventArgs e)
        {
            shareing = new Shareing();
            Controls.Add(shareing);
        }
        void WatchShareEvent(object s, EventArgs e)
        {
            watchShare = new WatchShare();
            Controls.Add(watchShare);
            menu.Hide();
            watchShare.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            watchShare?.Dispose();
            shareing?.Dispose();
        }
    }
}
