using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;

namespace standardApplication
{
    public partial class UserControlRelay : UserControl
    {
        public UserControlRelay()
        {
            InitializeComponent();
        }

        private void UserControlRelay_Load(object sender, EventArgs e)
        {
            if (Gloab.Config == null)
            {
                return;
            }
            comboBoxEdit7.Properties.Items.Clear();
            comboBoxEdit7.Properties.Items.AddRange(Gloab.Config.RelayModelA.Select(c => c.Key).ToArray());
        }

        private int number { get; set; }

        public void SetRelayToControl(RelayEntity relay)
        {
            number = relay.Number;
            labelControl17.Text = "继电器：" + relay.Number + "模式";
            comboBoxEdit7.Text = relay.RelayModel.Name;
            spinEdit10.Value = relay.RelayMatchChannel;
            spinEdit4.Value = relay.RelayInterval;
            spinEdit9.Value = relay.RelayActionTimeSpan;
        }

        public RelayEntity GetRelayFromControl()
        {
            RelayEntity relay = new RelayEntity()
            {
                Number = number,
                RelayActionTimeSpan = (ushort)spinEdit9.Value,
                RelayInterval = (ushort)spinEdit4.Value,
                RelayMatchChannel = (short)spinEdit10.Value,
                RelayModel = new FieldValue() { Name = comboBoxEdit7.Text, Value = Gloab.Config.RelayModel.First(c => c.Key == comboBoxEdit7.Text).Value }
            };
            return relay;
        }
    }
}
