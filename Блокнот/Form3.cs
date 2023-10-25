using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml.Linq;
using Блокнот.Properties;

namespace Блокнот
{
    public partial class Form3 : Form
    {
        public forma form1;

        private bool dark = false;

        protected override bool ShowWithoutActivation
        {
            get { return true; }
        }
        public Form3(forma Owner)
        {
            form1 = Owner;
            InitializeComponent();
        }


        void Form3_Load(object sender, EventArgs e)
        {
            int hSysMenu = SystemMenu.GetSystemMenu(this.Handle, false);
            SystemMenu.DeleteMenu(hSysMenu, 3, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 0, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 2, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 1, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 1, 1024);
            if (colorDialog1.Color == Color.Black || colorDialog1.Color == Color.FromArgb(42, 42, 42))
            {
                radioButton2.Checked = true;
            }
            if (colorDialog1.Color == Color.White || colorDialog1.Color != Color.Black)
            {
                radioButton1.Checked = true;
            }
            radioButton1.Checked = Settings.Default.radiobutton;
            radioButton2.Checked = Settings.Default.radiobutton2;
            this.fontDialog1.Font = form1.richTextBox1.Font;
            label9.Font = richTextBox1.Font;
            checkBox1.CheckState = Properties.Settings.Default.check;
            checkBox2.CheckState = Properties.Settings.Default.chech1;
            checkBox6.CheckState = Properties.Settings.Default.check2;
            checkBox5.CheckState = Settings.Default.check3;
            checkBox3.CheckState = Settings.Default.check4;
            checkBox7.CheckState = Settings.Default.check6;
            pictureBox1.BackColor = Settings.Default.coldial;
            colorDialog1.Color = Settings.Default.coldial;
            richTextBox1.Text = Settings.Default.fonttext;
            fontDialog1.Font = Settings.Default.fon;
            label9.Font = Settings.Default.labfon;
            textBox1.Text = Settings.Default.zag;
            checkBox8.CheckState = Settings.Default.check8;
            this.richTextBox1.Text = form1.fontDialog1.Font.Name + "; " + form1.fontDialog1.Font.Size;
            fontDialog1.Font = form1.fontDialog1.Font;
            if (NativeMethods.IsWindows10OrGreater() == true)
            {
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;
            }
            else
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                form1.menuItem12.Visible = true;

            }
            else
            {
                form1.menuItem12.Visible = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                form1.menuItem33.Visible = true;

            }
            else
            {
                form1.menuItem33.Visible = false;
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked == false)
            {
                form1.menuItem28.Visible = false;

            }
            else
            {
                form1.menuItem28.Visible = true;
            }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked == true)
            {
                form1.menuItem52.Visible = false;
                form1.menuItem31.Visible = false;
                form1.menuItem32.Visible = false;
                form1.menuItem27.Visible = false;
            }
            else
            {
                form1.menuItem52.Visible = true;
                form1.menuItem31.Visible = true;
                form1.menuItem32.Visible = true;
                form1.menuItem27.Visible = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.check = checkBox1.CheckState;
            Settings.Default.chech1 = checkBox2.CheckState;
            Settings.Default.check2 = checkBox6.CheckState;
            Settings.Default.check3 = checkBox5.CheckState;
            Settings.Default.check4 = checkBox3.CheckState;
            Settings.Default.check6 = checkBox7.CheckState;
            Settings.Default.coldial = richTextBox1.BackColor;
            Settings.Default.coldial = pictureBox1.BackColor;
            Settings.Default.fonttext = richTextBox1.Text;
            Settings.Default.fon = fontDialog1.Font;
            Settings.Default.labfon = label9.Font;
            Settings.Default.zag = textBox1.Text;
            Settings.Default.richfont = form1.richTextBox1.Font;
            Settings.Default.sagolovok = form1.Text;
            Settings.Default.check8 = checkBox8.CheckState;
            Settings.Default.radiobutton = radioButton1.Checked;
            Settings.Default.radiobutton2 = radioButton2.Checked;
            Settings.Default.ThemeDark = dark;
            textBox1.Modified = false;
            richTextBox1.Modified = false;
            Properties.Settings.Default.Save();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                form1.statusBar1.Visible = true;
            }
            else
            {
                form1.statusBar1.Visible = false;
            }
        }

        private void checkBox4_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            radioButton1.Checked = Settings.Default.radiobutton;
            radioButton2.Checked = Settings.Default.radiobutton2;
            this.fontDialog1.Font = form1.richTextBox1.Font;
            label9.Font = richTextBox1.Font;
            checkBox1.CheckState = Properties.Settings.Default.check;
            checkBox2.CheckState = Properties.Settings.Default.chech1;
            checkBox6.CheckState = Properties.Settings.Default.check2;
            checkBox5.CheckState = Settings.Default.check3;
            checkBox3.CheckState = Settings.Default.check4;
            checkBox7.CheckState = Settings.Default.check6;
            pictureBox1.BackColor = Settings.Default.coldial;
            colorDialog1.Color = Settings.Default.coldial;
            richTextBox1.Text = Settings.Default.fonttext;
            fontDialog1.Font = Settings.Default.fon;
            label9.Font = Settings.Default.labfon;
            textBox1.Text = Settings.Default.zag;
            checkBox8.CheckState = Settings.Default.check8;
            this.richTextBox1.Text = form1.fontDialog1.Font.Name + "; " + form1.fontDialog1.Font.Size;
            fontDialog1.Font = form1.fontDialog1.Font;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkBox1.CheckState = Properties.Settings.Default.check;
            checkBox2.CheckState = Properties.Settings.Default.chech1;
            checkBox6.CheckState = Properties.Settings.Default.check2;
            checkBox5.CheckState = Settings.Default.check3;
            checkBox3.CheckState = Settings.Default.check4;
            checkBox7.CheckState = Settings.Default.check6;
            pictureBox1.BackColor = Settings.Default.coldial;
            colorDialog1.Color = Settings.Default.coldial;
            richTextBox1.Text = Settings.Default.fonttext;
            fontDialog1.Font = Settings.Default.fon;
            label9.Font = Settings.Default.labfon;
            textBox1.Text = Settings.Default.zag;
            checkBox8.CheckState = Settings.Default.check8;
            form1.richTextBox1.BackColor = Settings.Default.coldial;
            if (form1.richTextBox1.BackColor == Color.White)
            {
                form1.richTextBox1.ForeColor = Color.Black;
            }
            if (form1.richTextBox1.BackColor == Color.FromArgb(42, 42, 42))
            {
                form1.richTextBox1.ForeColor = Color.White;
            }
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            form1.richTextBox1.Font = this.fontDialog1.Font;
            label9.Font = new Font(this.fontDialog1.Font.Name, 12);
            this.richTextBox1.Text = fontDialog1.Font.Name + ";" + " " + fontDialog1.Font.Size.ToString();
            richTextBox1.Modified = true;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            pictureBox1.BackColor = colorDialog1.Color;
            form1.richTextBox1.BackColor = pictureBox1.BackColor;
            if (colorDialog1.Color == Color.Black)
            {
                form1.richTextBox1.ForeColor = Color.White;
            }
            else
            {
                form1.richTextBox1.ForeColor = Color.Black;
            }
            if (form1.richTextBox1.BackColor == Color.FromArgb(42, 42, 42))
            {
                form1.richTextBox1.ForeColor = Color.White;
            }
            if (colorDialog1.Color == Color.Black || colorDialog1.Color == Color.FromArgb(42, 42, 42))
            {
                radioButton2.Checked = true;
                form1.richTextBox1.BackColor = colorDialog1.Color;
                pictureBox1.BackColor = colorDialog1.Color;
            }
            else if (colorDialog1.Color == Color.White || colorDialog1.Color != Color.Black)
            {
                radioButton1.Checked = true;
                form1.richTextBox1.BackColor = colorDialog1.Color;
                pictureBox1.BackColor = colorDialog1.Color;
            }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked == true)
            {
                form1.ShowIcon = true;
            }
            else
            {
                form1.ShowIcon = false;
            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button6.Enabled = true;
            form1.saveFileDialog1.FileName = textBox1.Text + ".txt";
            if (textBox1.Text == "")
            {
                label12.Visible = true;
            }
            else
            {
                label12.Visible = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = "Новый1";
            button6.Enabled = false;
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void checkBox8_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void checkBox8_CheckedChanged_2(object sender, EventArgs e)
        {
            if (checkBox8.Checked == true)
            {
                form1.richTextBox1.AutoWordSelection = true;
            }
            else
            {
                form1.richTextBox1.AutoWordSelection = false;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            UseLightTheme();
        }


        public void UseLightTheme()
        {
            if (radioButton1.Checked == true)
            {
                NativeMethods.UseImmersiveDarkMode(form1.Handle, false);
                NativeMethods.UseImmersiveDarkMode(Handle, false);
                NativeMethods.SetPreferredAppMode(0);
                NativeMethods.FlushMenuThemes();
                dark = false;
                checkBox1.BackColor = Color.White;
                checkBox1.ForeColor = Color.Black;
                checkBox2.ForeColor = Color.Black;
                checkBox6.ForeColor = Color.Black;
                checkBox5.ForeColor = Color.Black;
                checkBox7.ForeColor = Color.Black;
                checkBox8.ForeColor = Color.Black;
                textBox1.BorderStyle = BorderStyle.Fixed3D;
                richTextBox1.BorderStyle = BorderStyle.Fixed3D;
                form1.BackColor = Color.White;
                form1.toolStrip1.BackColor = Color.White;
                form1.richTextBox1.BackColor = Color.White;
                this.BackColor = Color.White;
                groupBox1.ForeColor = Color.Black;
                label16.ForeColor = Color.Black;
                groupBox2.ForeColor = Color.Black;
                label4.ForeColor = Color.Black;
                groupBox3.ForeColor = Color.Black;
                label8.ForeColor = Color.Black;
                label9.ForeColor = Color.Black;
                label6.ForeColor = Color.Black;
                label10.ForeColor = Color.Black;
                label11.ForeColor = Color.Black;
                richTextBox1.BackColor = Color.White;
                richTextBox1.ForeColor = Color.Black;
                form1.richTextBox1.ForeColor = Color.Black;
                this.pictureBox1.BackColor = Color.White;
                textBox1.BackColor = Color.White;
                textBox1.ForeColor = Color.Black;
                NativeMethods.SetWindowTheme(button2.Handle, null, null);
                NativeMethods.SetWindowTheme(button4.Handle, null, null);
                NativeMethods.SetWindowTheme(button6.Handle, null, null);
                NativeMethods.SetWindowTheme(checkBox1.Handle, null, null);
                NativeMethods.SetWindowTheme(checkBox2.Handle, null, null);
                NativeMethods.SetWindowTheme(checkBox3.Handle, null, null);
                NativeMethods.SetWindowTheme(checkBox5.Handle, null, null);
                NativeMethods.SetWindowTheme(checkBox6.Handle, null, null);
                NativeMethods.SetWindowTheme(checkBox7.Handle, null, null);
                NativeMethods.SetWindowTheme(checkBox8.Handle, null, null);
                NativeMethods.SetWindowTheme(radioButton1.Handle, null, null);
                NativeMethods.SetWindowTheme(radioButton2.Handle, null, null);
            }
        }

        public void UseDarkTheme()
        {
            if (radioButton2.Checked == true)
            {
                NativeMethods.UseImmersiveDarkMode(form1.Handle, true);
                NativeMethods.UseImmersiveDarkMode(Handle, true);
                NativeMethods.SetPreferredAppMode(2);
                NativeMethods.FlushMenuThemes();
                dark = true;
                checkBox1.ForeColor = Color.White;
                checkBox2.ForeColor = Color.White;
                checkBox6.ForeColor = Color.White;
                checkBox5.ForeColor = Color.White;
                checkBox7.ForeColor = Color.White;
                checkBox8.ForeColor = Color.White;
                checkBox1.BackColor = Color.FromArgb(42, 42, 42);
                textBox1.BorderStyle = BorderStyle.FixedSingle;
                richTextBox1.BorderStyle = BorderStyle.FixedSingle;
                form1.BackColor = Color.FromArgb(42, 42, 42);
                form1.richTextBox1.BackColor = Color.FromArgb(42, 42, 42);
                this.BackColor = Color.FromArgb(42, 42, 42);
                groupBox1.ForeColor = Color.White;
                label16.ForeColor = Color.White;
                groupBox2.ForeColor = Color.White;
                label4.ForeColor = Color.White;
                groupBox3.ForeColor = Color.White;
                label8.ForeColor = Color.White;
                label9.ForeColor = Color.White;
                label6.ForeColor = Color.White;
                label10.ForeColor = Color.White;
                label11.ForeColor = Color.White;
                richTextBox1.BackColor = Color.FromArgb(42, 42, 42);
                richTextBox1.ForeColor = Color.White;
                form1.richTextBox1.ForeColor = Color.White;
                pictureBox1.BackColor = Color.FromArgb(42, 42, 42);
                form1.toolStrip1.BackColor = Color.FromArgb(47, 43, 43);
                if (form1.richTextBox1.Text != "")
                {
                    form1.richTextBox1.Modified = true;
                }
                else if (form1.richTextBox1.Text == "")
                {
                    form1.richTextBox1.Modified = false;
                }
                textBox1.BackColor = Color.FromArgb(42, 42, 42);
                textBox1.ForeColor = Color.White;
                NativeMethods.SetWindowTheme(button2.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(button4.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(button6.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(checkBox1.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(checkBox2.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(checkBox3.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(checkBox5.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(checkBox6.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(checkBox7.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(checkBox8.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(radioButton1.Handle, "DarkMode_Explorer", null);
                NativeMethods.SetWindowTheme(radioButton2.Handle, "DarkMode_Explorer", null);
            }
        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            UseDarkTheme();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
