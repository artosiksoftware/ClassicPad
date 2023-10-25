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

using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using Блокнот.Properties;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.VisualBasic.Devices;

namespace Блокнот
{
    public partial class forma : Form
    {
        public forma()
        {
            InitializeComponent();
            openFileDialog1.Multiselect = false;
            richTextBox1.AllowDrop = true;
            fontDialog1.MaxSize = 92;
            fontDialog1.MinSize = 8;
            saveFileDialog1.AddExtension = true;
            richTextBox1.Font = Settings.Default.richfont;
            fontDialog1.Font = Settings.Default.richfont;
            this.Opacity = Settings.Default.form;
            richTextBox2 = new RichTextBox();
        }

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr,
ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        public static bool UseImmersiveDarkMode(Form handle, bool enabled)
        {
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                int useImmersiveDarkMode = enabled ? 1 : 0;
                return DwmSetWindowAttribute(handle.Handle, (int)attribute,
                    ref useImmersiveDarkMode, sizeof(int)) == 0;
            }

            return false;
        }

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 &&
                Environment.OSVersion.Version.Build >= build;
        }

        const string UxTHEME = "UxTheme.dll";

        [DllImport(UxTHEME, SetLastError = true, CharSet = CharSet.Unicode,
            ExactSpelling = true)]
        public static extern int SetWindowTheme(
             IntPtr hWnd,
             string pszSubAppName,
             string pszSubIdList
            );

        private void GetAboutBox()
        {
            if (this.richTextBox1.Text == "free software")
            {
                MessageBox.Show("The Free Software is like Sex");
            }
            else
            {
                Form2 f2 = new Form2();
                if (this.richTextBox1.BackColor == Color.FromArgb(42, 42, 42) || this.richTextBox1.BackColor == Color.Black)
                {
                    UseImmersiveDarkMode(f2, true);
                    f2.BackColor = this.richTextBox1.BackColor;
                    f2.BackColor = this.richTextBox1.BackColor;
                    f2.label1.BackColor = f2.BackColor;
                    f2.label2.BackColor = f2.BackColor;
                    f2.label3.BackColor = f2.BackColor;
                    f2.label4.BackColor = f2.BackColor;
                    f2.label1.ForeColor = Color.White;
                    f2.label2.ForeColor = Color.White;
                    f2.label3.ForeColor = Color.White;
                    f2.label4.ForeColor = Color.White;
                    f2.panel1.BackColor = f2.BackColor;
                    SetWindowTheme(f2.button1.Handle, "DarkMode_Explorer", null);
                }
                if (this.richTextBox1.BackColor == Color.White)
                {
                    UseImmersiveDarkMode(f2 , false);
                    f2.BackColor = this.richTextBox1.BackColor;
                    f2.BackColor = this.richTextBox1.BackColor;
                    f2.label1.BackColor = f2.BackColor;
                    f2.label2.BackColor = f2.BackColor;
                    f2.label3.BackColor = f2.BackColor;
                    f2.label4.BackColor = f2.BackColor;
                    f2.label1.ForeColor = Color.Black;
                    f2.label2.ForeColor = Color.Black;
                    f2.label3.ForeColor = Color.Black;
                    f2.label4.ForeColor = Color.Black;
                }
                f2.ShowDialog(this);
            }
        }
        public static void ShowOpenWithDialog(string path)
        {
            var args = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shell32.dll");
            args += ",OpenAs_RunDLL " + path;
            Process.Start("rundll32.exe", args);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;
        public static bool ShowFileProperties(string Filename)
        {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = Marshal.SizeOf(info);
            info.lpVerb = "properties";
            info.lpFile = Filename;
            info.nShow = SW_SHOW;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            return ShellExecuteEx(ref info);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool SetMenuItemInfo(IntPtr hMenu, int uItem, bool fByPosition, MENUITEMINFO lpmii);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MENUINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MENUINFO));
            public uint fMask = 0x0; //MIM_STYLE
            public int dwStyle = 0x4000000; //MNS_CHECKORBMP
            public uint cyMax;
            public IntPtr hbrBack;
            public int dwContextHelpID = 0;
            public IntPtr dwMenuData = IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MENUITEMINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MENUITEMINFO));
            public int fMask = 0x00000080; //MIIM_BITMAP = 0x00000080
            public int fType = 128;
            public int fState = 0;
            public int wID = 0;
            public IntPtr hSubMenu = IntPtr.Zero;
            public IntPtr hbmpChecked = IntPtr.Zero;
            public IntPtr hbmpUnchecked = IntPtr.Zero;
            public IntPtr dwItemData = IntPtr.Zero;
            public IntPtr dwTypeData = IntPtr.Zero;
            public int cch = 0;
            public IntPtr hbmpItem = IntPtr.Zero;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetMenuInfo(IntPtr hMenu, MENUINFO lpcmi);

        [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern int SetPreferredAppMode(int preferredAppMode);

        [DllImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern void FlushMenuThemes();


        public void forma_Load(object sender, EventArgs e)
        {
            bool isdarktheme = Settings.Default.ThemeDark;
            if (isdarktheme)
            {
                NativeMethods.UseImmersiveDarkMode(Handle, true);
                NativeMethods.SetPreferredAppMode(2);
                NativeMethods.FlushMenuThemes();
            }
            else
            {
                NativeMethods.UseImmersiveDarkMode(Handle, false);
                NativeMethods.SetPreferredAppMode(0);
                NativeMethods.FlushMenuThemes();
            }
            Bitmap rtficon = new Bitmap(Properties.Resources._static);
            Bitmap bingicon = new Bitmap(Properties.Resources._1);
            Bitmap wiki = new Bitmap(Properties.Resources._2);
            Bitmap google = new Bitmap(Properties.Resources._3);
            Bitmap duckduckgo = new Bitmap(Properties.Resources._4);
            MENUITEMINFO menues = new MENUITEMINFO();
            menues.hbmpItem = rtficon.GetHbitmap();
            MENUITEMINFO bing = new MENUITEMINFO();
            bing.hbmpItem = bingicon.GetHbitmap();
            MENUITEMINFO wikipediaicon = new MENUITEMINFO();
            wikipediaicon.hbmpItem = wiki.GetHbitmap();
            MENUITEMINFO googleicon = new MENUITEMINFO();
            googleicon.hbmpItem = google.GetHbitmap();
            MENUITEMINFO duckduckicon = new MENUITEMINFO();
            duckduckicon.hbmpItem = duckduckgo.GetHbitmap();
            SetMenuItemInfo(menuItem14.Handle, 0, true, bing);
            SetMenuItemInfo(menuItem51.Handle, 0, true, menues);
            SetMenuItemInfo(menuItem14.Handle, 1, true, wikipediaicon);
            SetMenuItemInfo(menuItem14.Handle, 2, true, googleicon);
            SetMenuItemInfo(menuItem14.Handle, 3, true, duckduckicon);
            statusBar1.Visible = Settings.Default.panel;
            menuItem71.Checked = Settings.Default.menuitemcheck;
            toolStrip1.BackColor = Settings.Default.toolstripcolor;
            richTextBox1.ContextMenu = contextMenu1;
            richTextBox1.AutoWordSelection = Settings.Default.autowordselection;
            menuItem28.Visible = Settings.Default.visi;
            menuItem12.Visible = Settings.Default.vi;
            menuItem33.Visible = Settings.Default.vis;
            richTextBox1.BackColor = Settings.Default.back;
            if (richTextBox1.BackColor == Color.Black)
            {
                richTextBox1.ForeColor = Color.White;
            }
            if (richTextBox1.BackColor == Color.FromArgb(42, 42, 42))
            {
                richTextBox1.ForeColor = Color.White;
            }
            this.Size = Settings.Default.siz;
            richTextBox1.AutoWordSelection = false;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                statusBar1.Visible = Settings.Default.panel;
                menuItem71.Checked = Settings.Default.menuitemcheck;
                toolStrip1.BackColor = Settings.Default.toolstripcolor;
                richTextBox1.ContextMenu = contextMenu1;
                richTextBox1.AutoWordSelection = Settings.Default.autowordselection;
                menuItem28.Visible = Settings.Default.visi;
                menuItem12.Visible = Settings.Default.vi;
                menuItem33.Visible = Settings.Default.vis;
                richTextBox1.BackColor = Settings.Default.back;
                menuItem78.Enabled = true;
                string filename1 = Environment.GetCommandLineArgs()[1];
                string fil = File.ReadAllText(filename1);
                richTextBox1.Text = fil;
                this.Text = Path.GetFileName(filename1) + " - ClassicPad";
                openFileDialog1.FileName = filename1;
                richTextBox1.Modified = false;
                menuItem13.Enabled = false;
                menuItem32.Enabled = true;
                menuItem65.Enabled = true;
                menuItem58.Enabled = true;
                menuItem82.Enabled = true;
                menuItem84.Enabled = true;
                menuItem78.Enabled = true;
                menuItem13.Enabled = false;
                menuItem7.Enabled = true;
                if (richTextBox1.Text == ".LOG")
                {
                    richTextBox1.Text = ".LOG" + "\n\n" + DateTime.Now;
                }
            }
            richTextBox1.DragDrop += new DragEventHandler(richTextBox1_DragDrop);
            this.richTextBox1.ShortcutsEnabled = true;
        }


        public void PrintPageH(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox1.Text, richTextBox1.Font, Brushes.Black, 0, 0);
        }
        private void forma_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Settings.Default.menuitemcheck = menuItem71.Checked;
            Settings.Default.toolstripcolor = toolStrip1.BackColor;
            Settings.Default.border = richTextBox1.BorderStyle;
            Settings.Default.autowordselection = richTextBox1.AutoWordSelection;
            Settings.Default.vi = menuItem12.Visible;
            Settings.Default.vis = menuItem33.Visible;
            Settings.Default.visi = menuItem28.Visible & menuItem34.Visible;
            Settings.Default.form = this.Opacity;
            Settings.Default.back = richTextBox1.BackColor;
            Settings.Default.siz = this.Size;
            Settings.Default.Save();
            if (richTextBox1.Modified == true && openFileDialog1.FileName.Length == 0)
            {
                DialogResult dialogResult = MessageBox.Show("File was modified. Do you want to exit?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    richTextBox1.Modified = false;
                    Application.Exit();
                }
                if (dialogResult == DialogResult.No)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return;
                    string file = saveFileDialog1.FileName;
                    File.WriteAllText(file, richTextBox1.Text);
                }
                if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
            if (richTextBox1.Modified == true && openFileDialog1.FileName.Length != 0)
            {
                DialogResult dialogResult = MessageBox.Show("File was modified. Do you want to exit without saving file?", "Save File", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return;
                    string file = saveFileDialog1.FileName;
                    File.WriteAllText(file, richTextBox1.Text);
                }
                if (dialogResult == DialogResult.No)
                {
                    richTextBox1.Modified = false;
                    Application.Exit();
                }
                if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void fontDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void приближениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float zoom = richTextBox1.ZoomFactor;
            richTextBox1.ZoomFactor = zoom + 1;
        }

        private void отдалениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float zoom = richTextBox1.ZoomFactor;
            richTextBox1.ZoomFactor = zoom - 1;
        }

        private void forma_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menuItem49_Click(object sender, EventArgs e)
        {
            GetAboutBox();
            
        }

        private void menuItem46_Click(object sender, EventArgs e)
        {

        }

        private void menuItem37_Click(object sender, EventArgs e)
        {
            float zoom = richTextBox1.ZoomFactor;
            richTextBox1.ZoomFactor = zoom + 1;
        }

        private void menuItem38_Click(object sender, EventArgs e)
        {
            float zoom = richTextBox1.ZoomFactor;
            try
            {
                richTextBox1.ZoomFactor = zoom - 1;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void menuItem24_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            richTextBox1.Font = fontDialog1.Font;
            richTextBox1.Modified = false;
            Settings.Default.f = fontDialog1.Font;
            Settings.Default.Save();

            if (richTextBox1.Text != "")
            {
                richTextBox1.Modified = true;
            }
        }

        private void menuItem26_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                richTextBox1.SelectedText = DateTime.Now.ToString();
            }
            else
            {
                richTextBox1.Text += DateTime.Now.ToString();
                richTextBox1.Modified = true;
            }
        }

        private void menuItem28_Click(object sender, EventArgs e)
        {
            int major = Environment.OSVersion.Version.Major;
            int minor = Environment.OSVersion.Version.Minor;
            if (major == 10 && minor == 0)
            {
                Process.Start("ms-settings:dateandtime");
            }
            if (major == 6 && minor == 2 || major == 6 && minor == 1)
            {
                Process.Start("timedate.cpl");
            }
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {
            richTextBox2.Undo();
            menuItem80.Enabled = true;
            menuItem13.Enabled = false;
        }

        private void menuItem15_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                Clipboard.SetText(richTextBox1.SelectedText);
            }
        }

        private void menuItem16_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                richTextBox1.SelectedText = Clipboard.GetText();
            }
            else
            {
                richTextBox1.Text += Clipboard.GetText();
                richTextBox1.Modified = true;
                menuItem13.Enabled = false;
            }
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
            int StartPosDel = richTextBox1.SelectionStart;
            int LenSelection = richTextBox1.SelectionLength;
            richTextBox1.Text = richTextBox1.Text.Remove(StartPosDel, LenSelection);
            if (richTextBox1.Text != "")
            {
                richTextBox1.Modified = true;
            }
            else
            {
                richTextBox1.Modified = false;
            }
        }

        private void menuItem18_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.Focus();
        }

        private void menuItem19_Click(object sender, EventArgs e)
        {

        }

        private void menuItem2_Click_1(object sender, EventArgs e)
        {
            if (richTextBox1.Modified == false)
            {
                richTextBox1.Text = "";
                richTextBox1.Modified = false;
                this.Text = "Новый1 - ClassicPad";
                openFileDialog1.FileName = "";
                menuItem32.Enabled = false;
                menuItem58.Enabled = false;
                menuItem65.Enabled = false;
                menuItem7.Enabled = false;
                menuItem82.Enabled = false;
                menuItem84.Enabled = false;
            }

            if (richTextBox1.Modified == true)
            {
                DialogResult dil = MessageBox.Show("Вы хотите сохранить файл после изменений?", "ClassicPad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dil == DialogResult.Yes)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return;
                    string filer = saveFileDialog1.FileName;
                    File.WriteAllText(filer, richTextBox1.Text);
                    richTextBox1.Modified = false;
                }
                if (dil == DialogResult.No)
                {
                    richTextBox1.Text = "";
                    richTextBox1.Modified = false;
                    this.Text = "Новый1 - ClassicPad";
                    openFileDialog1.FileName = "";
                    menuItem32.Enabled = false;
                }
                if (dil == DialogResult.Cancel)
                {

                }
            }
        }

        public void GetOpenFileName()
        {

            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string file = openFileDialog1.FileName;
                string path = Path.GetExtension(file);
                if (path == ".cs")
                {
                    statusBarPanel5.Text = "C# Source File";
                }
                if (path == ".iss")
                {
                    statusBarPanel5.Text = "Inno Setup Script";
                }
                if (path == ".txt")
                {
                    statusBarPanel5.Text = "Text File";
                }
                if (path == ".cpp" || path == ".cxx" || path == ".h")
                {
                    statusBarPanel5.Text = "C++ Source File";
                }
                if (path == ".vbs")
                {
                    statusBarPanel5.Text = "VBScript File";
                }
                if (path == ".bash" || path == ".bsh")
                {
                    statusBarPanel5.Text = "Unix Script File";
                }
                if (path == ".java")
                {
                    statusBarPanel5.Text = "Java Source File";
                }
                if (path == ".xml")
                {
                    statusBarPanel5.Text = "XML File";
                }
                string filetext = File.ReadAllText(file);
                menuItem82.Enabled = true;
                menuItem84.Enabled = true;
                richTextBox1.Modified = false;
                richTextBox1.Text = filetext;
                this.Text = "\"" + openFileDialog1.FileName + "\"" + " - ClassicPad";
                menuItem13.Enabled = false;
                menuItem32.Enabled = true;
                menuItem65.Enabled = true;
                menuItem77.Enabled = true;
                menuItem78.Enabled = true;
                menuItem7.Enabled = true;
                menuItem58.Enabled = true;
            }
            catch (System.UnauthorizedAccessException)
            {
                DialogResult res = MessageBox.Show("Отказано в доступе " + "\n\n" + openFileDialog1.FileName, "ClassicPad", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                if (res == DialogResult.OK)
                {
                    menuItem58.Enabled = true;
                }
            }
            catch (IOException)
            {
                DialogResult dialogResult = MessageBox.Show("Невозможно загрузить файл " + openFileDialog1.SafeFileName + ", поскольку он выполняется другим процессом. Повторите попытку позже", "ClassicPad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.OK)
                {
                    richTextBox1.Text = "";
                    openFileDialog1.FileName = "";
                    this.Text = "Новый1 - ClassicPad";
                    menuItem32.Enabled = false;
                    menuItem65.Enabled = false;
                    menuItem77.Enabled = false;
                    menuItem78.Enabled = false;
                    menuItem82.Enabled = false;
                    menuItem84.Enabled = false;
                    menuItem7.Enabled = false;
                }
            }
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            GetOpenFileName();
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            string file = openFileDialog1.FileName;
            if (file.Length > 1)
            {
                try
                {
                    File.WriteAllText(file, richTextBox1.Text);
                    richTextBox1.Modified = false;
                    if (openFileDialog1.FileName.Length == 0)
                    {
                        Text = "Новый1 - ClassicPad";
                    }
                    if (openFileDialog1.FileName.Length != 0)
                    {
                        Text = "\"" + openFileDialog1.FileName + "\"" + " - ClassicPad";
                        menuItem4.Enabled = false;
                    }
                }
                catch (ArgumentException)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return;
                    string filen = saveFileDialog1.FileName;
                    File.WriteAllText(filen, richTextBox1.Text);
                    richTextBox1.Modified = false;
                    if (openFileDialog1.FileName.Length != 0)
                    {
                        menuItem4.Enabled = false;
                    }
                }
                catch (System.UnauthorizedAccessException)
                {
                    MessageBox.Show("Отказано в доступе " + "\n\n" + file, "ClassicPad", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            if (openFileDialog1.FileName == "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string filen = saveFileDialog1.FileName;
                File.WriteAllText(filen, richTextBox1.Text);
                richTextBox1.Modified = false;
                if (openFileDialog1.FileName.Length != 0)
                {
                    menuItem4.Enabled = false;
                }
            }


            if (args.Length > 1)
            {
                string filename1 = Environment.GetCommandLineArgs()[1];
                try
                {
                    File.WriteAllText(filename1, richTextBox1.Text);
                    richTextBox1.Modified = false;
                    if (openFileDialog1.FileName.Length != 0)
                    {
                        menuItem4.Enabled = false;
                    }
                }
                catch (System.UnauthorizedAccessException)
                {
                    MessageBox.Show("Отказано в доступе " + "\n\n" + Path.GetFullPath(filename1), "ClassicPad", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string file = saveFileDialog1.FileName;
            File.WriteAllText(file, richTextBox1.Text);
            richTextBox1.Modified = false;
            if (openFileDialog1.FileName.Length != 0)
            {
                Text = "\"" + openFileDialog1.FileName + "\"" + " - ClassicPad";
                menuItem4.Enabled = false;
            }
        }

        private void menuItem6_Click(object sender, EventArgs e)
        {
            PrintDocument pDoc = new PrintDocument();
            pDoc.PrintPage += PrintPageH;
            PrintDialog pDial = new PrintDialog();
            pDial.Document = pDoc;
            pDoc.DocumentName = "новый документ1";
            pDial.Document = pDoc;
            pDial.AllowSelection = true;
            pDial.AllowSomePages = true;
            pDial.UseEXDialog = true;
            pDoc.DefaultPageSettings = pageSetupDialog1.PageSettings;
            if (pDial.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pDial.Document.Print();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Не удалось распечатать 'новый документ1'", "Ошибка печати", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void menuItem34_Click_1(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                richTextBox1.SelectedText = Clipboard.GetText();
            }
            else
            {
                richTextBox1.Text += Clipboard.GetText();
                richTextBox1.Modified = true;
            }
        }

        private void menuItem35_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.Copy();
            }
        }

        private void menuItem29_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.Focus();
        }

        private void richTextBox1_SelectionChanged_1(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText == "")
            {
                menuItem14.Enabled = false;
                menuItem15.Enabled = false;
                menuItem17.Enabled = false;
                menuItem25.Enabled = false;
                menuItem35.Enabled = false;
            }
            else
            {
                menuItem14.Enabled = true;
                menuItem15.Enabled = true;
                menuItem17.Enabled = true;
                menuItem25.Enabled = true;
                menuItem35.Enabled = true;
            }
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            {
                if (richTextBox1.Text == "")
                {
                    richTextBox1.Modified = false;
                }
            }
            {
                if (richTextBox1.Text == "")
                {
                    richTextBox1.Modified = false;
                }
            }
            if (richTextBox1.SelectedText == "")
            {
                menuItem14.Enabled = false;
            }
            else
            {
                menuItem14.Enabled = true;
            }
            if (richTextBox1.Text == "")
            {
                menuItem53.Enabled = false;
                menuItem13.Enabled = false;
            }
            else
            {
                menuItem53.Enabled = true;
                menuItem13.Enabled = true;
            }
        }

        private void menuItem48_Click_1(object sender, EventArgs e)
        {

        }

        private void menuItem25_Click(object sender, EventArgs e)
        {
            int StartPosDel = richTextBox1.SelectionStart;
            int LenSelection = richTextBox1.SelectionLength;
            richTextBox1.Text = richTextBox1.Text.Remove(StartPosDel, LenSelection);
            if (richTextBox1.Text != "")
            {
                richTextBox1.Modified = true;
            }
            else
            {
                richTextBox1.Modified = false;
            }
        }

        private void menuItem52_Click(object sender, EventArgs e)
        {

        }

        private void menuItem53_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                richTextBox1.Undo();
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void создатьToolStripButton_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Modified == false)
            {
                richTextBox1.Text = "";
                richTextBox1.Modified = false;
                this.Text = "ClassicPad";
            }

            if (richTextBox1.Modified == true)
            {
                DialogResult dil = MessageBox.Show("Вы хотите сохранить файл после изменений?", "ClassicPad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dil == DialogResult.Yes)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return;
                    string filer = saveFileDialog1.FileName;
                    File.WriteAllText(filer, richTextBox1.Text);
                    richTextBox1.Modified = false;
                }
                if (dil == DialogResult.No)
                {
                    richTextBox1.Text = "";
                    richTextBox1.Modified = false;
                    this.Text = "Classic";
                }
                if (dil == DialogResult.Cancel)
                {

                }
            }
        }

        private void справкаToolStripButton_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void вырезатьToolStripButton_Click(object sender, EventArgs e)
        {
            int StartPosDel = richTextBox1.SelectionStart;
            int LenSelection = richTextBox1.SelectionLength;
            richTextBox1.Text = richTextBox1.Text.Remove(StartPosDel, LenSelection);
            if (richTextBox1.Text != "")
            {
                richTextBox1.Modified = true;
            }
            else
            {
                richTextBox1.Modified = false;
            }
        }

        private void menuItem30_Click(object sender, EventArgs e)
        {

            string opf = openFileDialog1.FileName;
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                string file = Environment.GetCommandLineArgs()[1];
                Process.Start(@"C:\Windows\System32\notepad.exe", file);
            }
            else if (openFileDialog1.FileNames.Length == 0)
            {
                MessageBox.Show("Данный файл не существует!", "Открытие...", MessageBoxButtons.OK);
            }
            else
            {
                Process.Start(@"C:\Windows\System32\notepad.exe", opf);
            }
        }

        private void richTextBox1_TextChanged_2(object sender, EventArgs e)
        {
            {
                if (richTextBox1.Text == "")
                {
                    richTextBox1.Modified = false;
                }
            }
            if (richTextBox1.SelectedText == "")
            {
                menuItem14.Enabled = false;
            }
            else
            {
                menuItem14.Enabled = true;
            }
            if (richTextBox1.Text == "")
            {
                menuItem53.Enabled = false;
            }
            else
            {
                menuItem53.Enabled = true;
                menuItem13.Enabled = true;
            }
            if (richTextBox1.Modified == true)
            {
                menuItem4.Enabled = true;
            }
            String s = richTextBox1.Text.Length.ToString();
            statusBarPanel2.Text = "     Символов: " + s;
            statusBarPanel1.Text = "Строк: " + richTextBox1.Lines.Length.ToString();
        }

        private void richTextBox1_SelectionChanged_2(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText == "")
            {
                menuItem14.Enabled = false;
                menuItem15.Enabled = false;
                menuItem17.Enabled = false;
                menuItem25.Enabled = false;
                menuItem35.Enabled = false;
                menuItem64.Enabled = false;
            }
            else
            {
                menuItem14.Enabled = true;
                menuItem15.Enabled = true;
                menuItem17.Enabled = true;
                menuItem25.Enabled = true;
                menuItem35.Enabled = true;
                menuItem64.Enabled = true;
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void menuItem32_Click_2(object sender, EventArgs e)
        {
            FileInfo fileInfo = new FileInfo(openFileDialog1.FileName);
            string s = fileInfo.CreationTime.ToShortDateString() + "," + fileInfo.LastWriteTime.ToShortTimeString();
            string d = fileInfo.LastAccessTime.ToShortDateString() + "," + fileInfo.LastAccessTime.ToShortTimeString();
            string f = fileInfo.Name;
            string a = fileInfo.Length.ToString();
            string b = fileInfo.LastWriteTime.ToShortDateString() + "," + fileInfo.LastWriteTime.ToShortTimeString();
            string text = richTextBox1.Text.Length.ToString();
            MessageBox.Show("Path: " + fileInfo + "\n" + "File Name: " + f + "\n" + "Creation Date: " + s + "\n" + "Opened: " + d + "\n" + "Last Change: " + b + "\n" + "Size: " + a + " Б" + "\n" + "Lenght: " + text, "File Information", MessageBoxButtons.OK);
        }

        private void menuItem55_Click(object sender, EventArgs e)
        {

        }

        private void menuItem61_Click(object sender, EventArgs e)
        {

        }

        private void menuItem62_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (a != null)
                {
                    string s = a.GetValue(0).ToString();
                    this.Activate();
                    string y = File.ReadAllText(s);
                    richTextBox1.Text = y;
                    string drop = Path.GetFullPath(s);
                    this.Text = "\"" + openFileDialog1.FileName + "\"" + " - ClassicPad";
                    openFileDialog1.FileName = s;
                    this.Cursor = Cursors.Default;
                    menuItem32.Enabled = true;
                    menuItem4.Enabled = true;
                    menuItem65.Enabled = true;
                    menuItem58.Enabled = true;
                    menuItem82.Enabled = true;
                    menuItem84.Enabled = true;
                    menuItem78.Enabled = true;
                    menuItem13.Enabled = false;
                    menuItem7.Enabled = true;
                }
                string st = openFileDialog1.FileName;
                string path = Path.GetExtension(st);
                if (path == ".cs")
                {
                    statusBarPanel5.Text = "C# Source File";
                }
                if (path == ".iss")
                {
                    statusBarPanel5.Text = "Inno Setup Script";
                }
                if (path == ".txt")
                {
                    statusBarPanel5.Text = "Текстовый файл";
                }
                if (path == ".cpp" || path == ".cxx" || path == ".h")
                {
                    statusBarPanel5.Text = "C++ Source File";
                }
                if (path == ".vbs")
                {
                    statusBarPanel5.Text = "VBScript File";
                }
                if (path == ".bash" || path == ".bsh")
                {
                    statusBarPanel5.Text = "Unix Script File";
                }
                if (path == ".java")
                {
                    statusBarPanel5.Text = "Java Source File";
                }
                if (path == ".xml")
                {
                    statusBarPanel5.Text = "XML File";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка: данный объект не поддерживается для перетаскивания!", "Перетаскивание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            richTextBox1.Modified = false;
            if (richTextBox1.Modified == false && openFileDialog1.FileName.Length != 0)
            {
                Text = "\"" + openFileDialog1.FileName + "\"" + " - ClassicPad";
            }
        }

        private void menuItem55_Click_1(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string file = saveFileDialog1.FileName;
            File.WriteAllText(file, richTextBox1.Text);
            richTextBox1.Modified = false;
        }

        private void forma_FormClosed(object sender, FormClosedEventArgs e)
        {

        }


        private void menuItem58_Click_1(object sender, EventArgs e)
        {
            FileInfo g = new FileInfo(openFileDialog1.FileName);
            DialogResult res = MessageBox.Show("Файл" + " \"" + openFileDialog1.FileName + "\"" + "\n" + "Будет полностью удален." + "\n" + "Продолжить?", "ClassicPad", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                g.Delete();
                richTextBox1.Text = "";
                this.Text = "Новый1 - ClassicPad";
                openFileDialog1.FileName = "";
                menuItem58.Enabled = false;
                menuItem65.Enabled = false;
                menuItem82.Enabled = false;
                menuItem84.Enabled = false;
                menuItem7.Enabled = false;
                menuItem32.Enabled = false;
            }
            if (res == DialogResult.Cancel)
            {
                MessageBox.Show("Операция была отменена пользователем", "Отмена", MessageBoxButtons.OK);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void menuItem42_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.FileName = "https://www.donationalerts.com/r/artosik_pc";
            p.StartInfo = startInfo;
            p.Start();
        }

        private void menuItem60_Click(object sender, EventArgs e)
        {
            richTextBox1.ZoomFactor = 1;
        }


        private void menuItem27_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                richTextBox1.SelectedText = DateTime.Now.ToString();
            }
            else
            {
                richTextBox1.Text += DateTime.Now.ToString();
                richTextBox1.Modified = true;
            }
        }

        private void menuItem61_Click_1(object sender, EventArgs e)
        {
            GetSettingsDialog();
        }

        private void GetSettingsDialog()
        {
            new Form3(this).ShowDialog();
        }

        private void menuItem46_Click_1(object sender, EventArgs e)
        {
            int StartPosDel = richTextBox1.SelectionStart;
            int LenSelection = richTextBox1.SelectionLength;
            richTextBox1.Text = richTextBox1.Text.Remove(StartPosDel, LenSelection);
            if (richTextBox1.Text != "")
            {
                richTextBox1.Modified = true;
            }
            else
            {
                richTextBox1.Modified = false;
            }
        }

        private void menuItem64_Click(object sender, EventArgs e)
        {
            int StartPosDel = richTextBox1.SelectionStart;
            int LenSelection = richTextBox1.SelectionLength;
            richTextBox1.Text = richTextBox1.Text.Remove(StartPosDel, LenSelection);
            if (richTextBox1.Text != "")
            {
                richTextBox1.Modified = true;
            }
            else
            {
                richTextBox1.Modified = false;
            }
        }

        private void menuItem65_Click(object sender, EventArgs e)
        {
            string file = openFileDialog1.FileName;
            ShowFileProperties(file);
        }

        private void menuItem72_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string savefiledialog = saveFileDialog1.FileName;
        }

        private void menuItem73_Click(object sender, EventArgs e)
        {

        }

        private void menuItem75_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string savefiledialog = saveFileDialog1.FileName;
        }

        private void menuItem77_Click(object sender, EventArgs e)
        {
            DefaultBrowser browser = new DefaultBrowser();
            if (browser.IsInitSuccess)
            {
                if (browser.StartUrl(openFileDialog1.FileName))
                {

                }
                else
                {

                }
            }
        }

        private void menuItem78_Click(object sender, EventArgs e)
        {
            DefaultBrowser browser = new DefaultBrowser();
            if (browser.IsInitSuccess)
            {
                if (browser.StartUrl(openFileDialog1.FileName))
                {

                }
                else
                {

                }
            }
        }

        private void menuItem80_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
            menuItem80.Enabled = false;
        }
        private void menuItem82_Click(object sender, EventArgs e)
        {
            Interaction.Shell("explorer /select," + openFileDialog1.FileName, AppWinStyle.NormalFocus);
        }

        private void menuItem84_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.FileName.Length != 0)
            {
                Process.Start(openFileDialog1.FileName);
            }
            else
            {
                MessageBox.Show("Данный файл не существует!");
            }
        }

        private void richTextBox1_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            statusBarPanel3.Text = "Масштаб: " + richTextBox1.ZoomFactor.ToString();
        }

        private void menuItem7_Click(object sender, EventArgs e)
        {
            ShowOpenWithDialog(openFileDialog1.FileName);
        }

        private void menuItem53_Select(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Arrow;
        }

        private void contextMenu1_Popup(object sender, EventArgs e)
        {

        }

        private void menuItem19_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Создание нового файла: Ctrl + N" + "\n" + "Открытие файла: Ctrl + O" + "\n" + "Сохранение файла: Ctrl + S" + "\n" + "Сохранение нового файла: Ctrl + Shift + S" + "\n" + "Печать файла: Ctrl + P" + "\n" + "Открытие окна " + "\"" + "Шрифт" + "\"" + ": Ctrl + F" + "\n" + "Добавление даты и времени в текстовое поле: F5" + "\n" + "Открытие окна изменения даты и времени: F8" + "\n" + "Увеличение масштаба: Ctrl + 1" + "\n" + "Отдаление масштаба: Ctrl + 2" + "\n" + "Восстановление масштаба по умолчанию: Shift + F3", "Справка");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void menuItem56_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedRtf != "")
            {
                richTextBox1.SelectedRtf = Clipboard.GetText(TextDataFormat.Rtf);
            }
            else
            {
                richTextBox1.Rtf += Clipboard.GetText(TextDataFormat.Rtf);
                richTextBox1.Modified = true;
                menuItem13.Enabled = false;
            }
            richTextBox1.Rtf = Clipboard.GetText(TextDataFormat.Rtf);
        }

        private void menuItem67_Click_1(object sender, EventArgs e)
        {
            Process.Start("https://www.bing.com/search?q=" + richTextBox1.SelectedText + "&form=QBLH&sp=-1&lq=0&pq=" + richTextBox1.SelectedText + "&sc=10-5&qs=n&sk=&cvid=F11B33849AC94607AB7E5E9182F367C8&ghsh=0&ghacc=0&ghpl=");
        }

        private void menuItem68_Click(object sender, EventArgs e)
        {
            Process.Start("https://en.wikipedia.org/wiki/" + richTextBox1.SelectedText);
        }

        private void menuItem69_Click_1(object sender, EventArgs e)
        {
            Process.Start("https://yandex.com/search/?text=" + richTextBox1.SelectedText);
        }

        private void menuItem62_Click_1(object sender, EventArgs e)
        {
            Process.Start("https://www.google.com/search?q=" + richTextBox1.SelectedText + "&rlz=1C1GTPM_enRU1041RU1041&oq=test&aqs=chrome..69i57.497j0j7&sourceid=chrome&ie=UTF-8");
        }

        private void menuItem48_Click(object sender, EventArgs e)
        {
            Process.Start("https://duckduckgo.com/?q=" + richTextBox1.SelectedText + "&t=h_&ia=web");
        } 

        private void создатьToolStripButton_Click_1(object sender, EventArgs e)
        {
            if (richTextBox1.Modified == false)
            {
                richTextBox1.Text = "";
                richTextBox1.Modified = false;
                this.Text = "Новый1 - ClassicPad";
                openFileDialog1.FileName = "";
                menuItem32.Enabled = false;
                menuItem58.Enabled = false;
                menuItem65.Enabled = false;
                menuItem7.Enabled = false;
                menuItem82.Enabled = false;
                menuItem84.Enabled = false;
            }

            if (richTextBox1.Modified == true)
            {
                DialogResult dil = MessageBox.Show("Вы хотите сохранить файл после изменений?", "ClassicPad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dil == DialogResult.Yes)
                {
                    if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                        return;
                    string filer = saveFileDialog1.FileName;
                    File.WriteAllText(filer, richTextBox1.Text);
                    richTextBox1.Modified = false;
                }
                if (dil == DialogResult.No)
                {
                    richTextBox1.Text = "";
                    richTextBox1.Modified = false;
                    this.Text = "Новый1 - ClassicPad";
                    openFileDialog1.FileName = "";
                    menuItem32.Enabled = false;
                }
                if (dil == DialogResult.Cancel)
                {

                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            float zoom = richTextBox1.ZoomFactor;
            richTextBox1.ZoomFactor = zoom + 1;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            float zoom = richTextBox1.ZoomFactor;
            try
            {
                richTextBox1.ZoomFactor = zoom - 1;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Нельзя выбрать коэффицент масштабирования меньше единицы!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void открытьToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    return;
                string file = openFileDialog1.FileName;
                string filetext = File.ReadAllText(file);
                richTextBox1.Text = filetext;
                menuItem82.Enabled = true;
                menuItem84.Enabled = true;
                richTextBox1.Modified = false;
                this.Text = "\"" + openFileDialog1.FileName + "\"" + " - ClassicPad";
                menuItem13.Enabled = false;
                menuItem32.Enabled = true;
                menuItem65.Enabled = true;
                menuItem77.Enabled = true;
                menuItem78.Enabled = true;
                menuItem7.Enabled = true;
                string encoding = string.Empty;
                Stream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
                using (StreamReader sr = new StreamReader(fs, true))
                    encoding = sr.CurrentEncoding.EncodingName;
                menuItem58.Enabled = true;
            }
            catch (System.UnauthorizedAccessException)
            {
                DialogResult res = MessageBox.Show("Отказано в доступе " + "\n\n" + openFileDialog1.FileName, "ClassicPad", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                if (res == DialogResult.OK)
                {
                    openFileDialog1.FileName = "";
                    richTextBox1.Text = "";
                    this.Text = "Новый1 - ClassicPad";
                    menuItem32.Enabled = false;
                    menuItem65.Enabled = false;
                    menuItem77.Enabled = false;
                    menuItem78.Enabled = false;
                    menuItem82.Enabled = false;
                    menuItem84.Enabled = false;
                    menuItem7.Enabled = false;
                }
            }
            catch (IOException)
            {
                DialogResult dialogResult = MessageBox.Show("Невозможно загрузить файл " + openFileDialog1.SafeFileName + ", поскольку он выполняется другим процессом. Повторите попытку позже", "ClassicPad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (dialogResult == DialogResult.OK)
                {
                    richTextBox1.Text = "";
                    openFileDialog1.FileName = "";
                    this.Text = "Новый1 - ClassicPad";
                    menuItem32.Enabled = false;
                    menuItem65.Enabled = false;
                    menuItem77.Enabled = false;
                    menuItem78.Enabled = false;
                    menuItem82.Enabled = false;
                    menuItem84.Enabled = false;
                    menuItem7.Enabled = false;
                }
            }
        }

        private void сохранитьToolStripButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string file = saveFileDialog1.FileName;
            File.WriteAllText(file, richTextBox1.Text);
            richTextBox1.Modified = false;
        }

        private void печатьToolStripButton_Click(object sender, EventArgs e)
        {
            PrintDocument pDoc = new PrintDocument();
            pDoc.PrintPage += PrintPageH;
            PrintDialog pDial = new PrintDialog();
            pDial.Document = pDoc;
            pDoc.DocumentName = "новый документ1";
            pDial.Document = pDoc;
            pDial.AllowSelection = true;
            pDial.AllowSomePages = true;
            pDoc.DefaultPageSettings = pageSetupDialog1.PageSettings;
            if (pDial.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pDial.Document.Print();
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Не удалось распечатать 'новый документ1'", "Ошибка печати", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void вырезатьToolStripButton_Click_1(object sender, EventArgs e)
        {
            int StartPosDel = richTextBox1.SelectionStart;
            int LenSelection = richTextBox1.SelectionLength;
            richTextBox1.Text = richTextBox1.Text.Remove(StartPosDel, LenSelection);
            if (richTextBox1.Text != "")
            {
                richTextBox1.Modified = true;
            }
            else
            {
                richTextBox1.Modified = false;
            }
        }

        private void копироватьToolStripButton_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
            {
                Clipboard.SetText(richTextBox1.SelectedText);
            }
        }

        private void вставкаToolStripButton_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                richTextBox1.SelectedText = Clipboard.GetText();
            }
            else
            {
                richTextBox1.Text += Clipboard.GetText();
                richTextBox1.Modified = true;
                menuItem13.Enabled = false;
            }
        }

        private void undo_ToolStripButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
            menuItem80.Enabled = true;
            menuItem13.Enabled = false;
        }

        private void redo_toolStripButton1_Click(object sender, EventArgs e)
        {
            richTextBox1.Redo();
            menuItem80.Enabled = false;
        }

        private void вставкаToolStripButton_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void вставкаToolStripButton_MouseLeave(object sender, EventArgs e)
        {

        }

        private void создатьToolStripButton_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void menuItem71_Click(object sender, EventArgs e)
        {
            
                this.menuItem71.Checked = !this.menuItem71.Checked;
                if (menuItem71.Checked == true)
                {
                    statusBar1.Visible = true;
                }
            if (menuItem71.Checked == false)
            {
                statusBar1.Visible = false;
            }
        }

        private void menuItem39_Click(object sender, EventArgs e)
        {
            forma form1 = new forma();
            form1.Show();
        }

        private void richTextBox2_ContentsResized_1(object sender, ContentsResizedEventArgs e)
        {
            statusBarPanel3.Text = "Масштаб: " + richTextBox1.ZoomFactor.ToString();
        }

        private void richTextBox2_SelectionChanged_1(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText == "")
            {
                menuItem14.Enabled = false;
                menuItem15.Enabled = false;
                menuItem17.Enabled = false;
                menuItem25.Enabled = false;
                menuItem35.Enabled = false;
                menuItem64.Enabled = false;
                menuItem46.Enabled = false;
            }
            else
            {
                menuItem14.Enabled = true;
                menuItem15.Enabled = true;
                menuItem17.Enabled = true;
                menuItem25.Enabled = true;
                menuItem35.Enabled = true;
                menuItem64.Enabled = true;
                menuItem46.Enabled = true;
            }
        }

        private void richTextBox2_TextChanged_1(object sender, EventArgs e) 
        {
            if (richTextBox1.Text != "" && openFileDialog1.FileName.Length == 0)
            {
                richTextBox1.Modified = true;
            }
            if (richTextBox1.Modified == true)
            {
                Text = "*New1 - ClassicPad";
            }
            if (richTextBox1.Text == "" && openFileDialog1.FileName.Length == 0)
            {
                richTextBox1.Modified = false;
            }
            if (richTextBox1.Modified == false && openFileDialog1.FileName.Length == 0)
            {
                Text = "New1 - ClassicPad";
            }
            if (richTextBox1.Text != "" && openFileDialog1.FileName.Length != 0)
            {
                richTextBox1.Modified = true;
            }
            if (richTextBox1.Modified == true &&  openFileDialog1.FileName.Length != 0)
            {
                Text = "* " + "\"" + openFileDialog1.FileName + "\"" + " - ClassicPad";
            }
            if (richTextBox1.Text == "" && openFileDialog1.FileName.Length != 0)
            {
                richTextBox1.Modified = false;
            }
            if (richTextBox1.Modified == false && openFileDialog1.FileName.Length != 0)
            {
                Text = "\"" + openFileDialog1.FileName + "\"" + " - ClassicPad";
            }
            {
            }
            if (richTextBox1.SelectedText == "")
            {
                menuItem14.Enabled = false;
            }
            else
            {
                menuItem14.Enabled = true;
            }
            if (richTextBox1.Text == "")
            {
                menuItem53.Enabled = false;
            }
            else
            {
                menuItem53.Enabled = true;
                menuItem13.Enabled = true;
            }
            if (richTextBox1.Modified == true)
            {
                menuItem4.Enabled = true;
            }
            String s = richTextBox1.Text.Length.ToString();
            statusBarPanel2.Text = "Lenght: " + s;
            statusBarPanel1.Text = "    Lines: " + richTextBox1.Lines.Length.ToString();
        }


        private void menuItem49_Popup(object sender, EventArgs e)
        {
            
        }

        private void menuItem7_Popup(object sender, EventArgs e)
        {
            
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        public RichTextBox richTextBox2;
    }
}






























