using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace net.ELF.DBTool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void SetShow(string str)
        {
            textBox1.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Select(0, textBox1.Text.Length);
            textBox1.Copy();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1.Instance.ExecuteSql(textBox1.Text);
            MessageBox.Show("执行完成");
            this.Close();
        }

    }
}
