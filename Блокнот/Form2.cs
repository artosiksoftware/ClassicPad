/*
 *  ClassicPad is a Open-Source project for OS Windows, written in C#
 *  Copyright (C) 2023 Artyom Cheganov
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *
 */

using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Блокнот
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            int hSysMenu = SystemMenu.GetSystemMenu(this.Handle, false);
            SystemMenu.DeleteMenu(hSysMenu, 3, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 0, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 2, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 1, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 1, 1024);
        }

        private static Cursor LoadCustomCursor(string path)
        {
            IntPtr hCurs = LoadCursorFromFile(path);
            if (hCurs == IntPtr.Zero) throw new Win32Exception();
            var curs = new Cursor(hCurs);
            // Note: force the cursor to own the handle so it gets released properly
            var fi = typeof(Cursor).GetField("ownHandle", BindingFlags.NonPublic | BindingFlags.Instance);
            fi.SetValue(curs, true);
            return curs;
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadCursorFromFile(string path);


        private void Form2_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(label6, "https://sourceforge.net/projects/artostypenote/");
            button1.Focus();
            AcceptButton = button1;
            try
            {
                string path = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors", "Hand", null).ToString();
                var hand = LoadCustomCursor(path);
                label5.Cursor = hand;
                label6.Cursor = hand;
            }
            catch (Win32Exception)
            {
                label6.Cursor = Cursors.Hand;
                label5.Cursor = Cursors.Hand;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            Process.Start("https://sourceforge.net/projects/artostypenote/");
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.gnu.org/licenses/gpl-3.0.txt");
        }

        private void label6_MouseMove(object sender, MouseEventArgs e)
        {
            this.label6.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void label6_MouseLeave(object sender, EventArgs e)
        {
            this.label6.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void label5_MouseMove(object sender, MouseEventArgs e)
        {
            this.label5.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            this.label5.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void label7_Click(object sender, EventArgs e)
        {
            OSInfo form4= new OSInfo();
            form4.ShowDialog();
        }

        private void label9_Click(object sender, EventArgs e)
        {


        }
    }
}
