using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Drawing;
using System.Diagnostics;
using System.ComponentModel;
using System.Reflection;
using DIcon;

namespace Блокнот
{
    public partial class OSInfo : Form
    {
        public OSInfo()
        {
            InitializeComponent();
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

        private void Form4_Load(object sender, EventArgs e)
        {
            int hSysMenu = SystemMenu.GetSystemMenu(this.Handle, false);
            SystemMenu.DeleteMenu(hSysMenu, 3, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 0, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 2, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 1, 1024);
            SystemMenu.DeleteMenu(hSysMenu, 1, 1024);
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(pictureBox1, "Редактор реестра");
            Icon icon = ExtractIconClass.GetIconFromExeDll(0, "regedit.exe");
            Icon iconforform = ExtractIconClass.GetSysIconFromDll(15, "shell32.dll");
            this.ShowIcon = true;
            this.Icon= iconforform;
            pictureBox1.Image = icon.ToBitmap();
            try
            {
                string path = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors", "Hand", null).ToString();
                var hand = LoadCustomCursor(path);
                label1.Cursor = hand;
                pictureBox1.Cursor = hand;
            }
            catch(Win32Exception)
            {
                label1.Cursor = Cursors.Hand;
                pictureBox1.Cursor = Cursors.Hand;
            }
            listView1.ContextMenu = contextMenu1;
            RegistryKey registry = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform");
            RegistryKey productid = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\DefaultProductKey2");
            string windows = productid.GetValue("ProductId").ToString();
            string[]windowsid = new string[] {"ID продукта", windows};
            string licensekey = registry.GetValue("BackupProductKeyDefault").ToString();
            string[] license = new string[] { "Ключ продукта по умолчанию", licensekey};
            string[] computername = new string[] {"Имя компьютера", Environment.MachineName.ToString() };
            string[] netbios = new string[] { "Имя NetBIOS", Environment.MachineName.ToString() };
            string[] username = new string[] {"Имя учетной записи", Environment.UserName.ToString() };
            ListViewItem item = listView1.Items.Add(new ListViewItem(computername));
            ListViewItem item1 = listView1.Items.Add(new ListViewItem(username));
            ListViewItem item2 = listView1.Items.Add(new ListViewItem(netbios));
            ListViewItem item3 = listView1.Items.Add(new ListViewItem(license));
            ListViewItem item4 = listView1.Items.Add(new ListViewItem(windowsid));
            listView1.SmallImageList = imageList1;
            item.ImageIndex= 0;
            item.ImageKey = "info.png";
            item1.ImageIndex= 0;
            item1.ImageKey = "info.png";
            item2.ImageIndex = 0;
            item2.ImageKey = "info.png";
            item3.ImageIndex = 0;
            item3.ImageKey = "info.png";
            item4.ImageIndex = 0;
            item4.ImageKey = "info.png";
        }

        [DllImport("shell32.dll", EntryPoint = "#261", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void GetUserTilePath(
    string username,
    UInt32 whatever, // 0x80000000
    StringBuilder picpath, int maxLength
);

        public static string GetUserTilePath(string username)
        {
            var sb = new StringBuilder(1000);
            GetUserTilePath(username, 0x80000000, sb, sb.Capacity);
            return sb.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            RegistryKey registry = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform");
            RegistryKey productid = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\DefaultProductKey2");
            string windows = productid.GetValue("ProductId").ToString();
            string[] windowsid = new string[] { "ID продукта", windows };
            string licensekey = registry.GetValue("BackupProductKeyDefault").ToString();
            string[] license = new string[] { "Ключ продукта по умолчанию", licensekey };
            string[] computername = new string[] { "Имя компьютера", Environment.MachineName.ToString() };
            string[] netbios = new string[] { "Имя NetBIOS", Environment.MachineName.ToString() };
            string[] username = new string[] { "Имя учетной записи", Environment.UserName.ToString() };
            ListViewItem item = listView1.Items.Add(new ListViewItem(computername));
            ListViewItem item1 = listView1.Items.Add(new ListViewItem(username));
            ListViewItem item2 = listView1.Items.Add(new ListViewItem(netbios));
            ListViewItem item3 = listView1.Items.Add(new ListViewItem(license));
            ListViewItem item4 = listView1.Items.Add(new ListViewItem(windowsid));
            listView1.SmallImageList = imageList1;
            item.ImageIndex = 0;
            item.ImageKey = "info.png";
            item1.ImageIndex = 0;
            item1.ImageKey = "info.png";
            item2.ImageIndex = 0;
            item2.ImageKey = "info.png";
            item3.ImageIndex = 0;
            item3.ImageKey = "info.png";
            item4.ImageIndex = 0;
            item4.ImageKey = "info.png";
        }

        private void label1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("regedit.exe");
            }
            catch (Win32Exception)
            {
                MessageBox.Show("Операция была отменена пользователем", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
