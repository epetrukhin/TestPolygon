using System.Windows.Forms;

namespace WinForms
{
    internal sealed partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var frm = new Form1();

            frm.Show(this);

            frm.Owner = null;

            var owner = frm.Owner;
            var parent = frm.Parent;
            var parentForm = frm.ParentForm;
        }
    }
}