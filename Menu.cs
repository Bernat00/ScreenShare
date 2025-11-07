using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibVLCSharp.WinForms.Sample
{
    public partial class Menu : UserControl
    {
        public event EventHandler WatchBTN_Clicked;
        public event EventHandler ShareBTN_Clicked;
        public event EventHandler SelectedAdapterChanged;
        public event EventHandler SelectedEncoderChanged;

        public AdapterInfo SelectedAdapter
        {
            get
            {
                return comboBox1.SelectedItem as AdapterInfo;
            }
        }

        public Encoder SelectedEncoder
        {
            get
            {
                return comboBox2.SelectedItem as Encoder;
            }
        }

        public bool DebugMode
        {
            get { return checkBox1.Checked; }
        }


        public Menu()
        {
            InitializeComponent();

            watchBTN.Click += (s, e) =>
            {
                WatchBTN_Clicked?.Invoke(this, e);
            };

            shareBTN.Click += (s, e) =>
            {
                ShareBTN_Clicked?.Invoke(this, e);
            };


            comboBox1.DataSource = NetUtilsNemLopott.GetEthernetAdapters();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            comboBox2.DataSource = new List<Encoder> { Encoder.Intel, Encoder.Nvidia, Encoder.Universal, Encoder.IntelH265, Encoder.NvidiaH265, Encoder.UniversalH265 };
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;

        }
    }
}
