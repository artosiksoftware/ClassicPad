using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System
{
    public class SystemMenu
    {
        /// <summary>
        /// Определение флагов пункта меню
        /// </summary>
        public enum ItemFlags
        { // The item ...
            /// <summary>
            /// Определяет, что на пункте меню не должна быть установлена галочка
            /// </summary>
            mfUnchecked = 0x00000000,    // ... is not checked
            /// <summary>
            /// Определяет, что пункт меню должен являться строкой
            /// </summary>
            mfString = 0x00000000,    // ... contains a string as label
            /// <summary>
            /// Определяет, что пункт меню отключен
            /// </summary>
            mfDisabled = 0x00000002,    // ... is disabled
            /// <summary>
            /// Определяет, что пункт меню отключен
            /// </summary>
            mfGrayed = 0x00000001,    // ... is grayed
            /// <summary>
            /// Определяет, что на пункте меню должна быть установлена галочка
            /// </summary>
            mfChecked = 0x00000008,    // ... is checked
            /// <summary>
            /// Определяет, что пункт меню должен быть всплывающим
            /// </summary>
            mfPopup = 0x00000010,    // ... Is a popup menu. Pass the
            mfBarBreak = 0x00000020,    // ... is a bar break
            mfBreak = 0x00000040,    // ... is a break
            mfByPosition = 0x00000400,    // ... is identified by the position
            mfByCommand = 0x00000000,    // ... is identified by its ID
            /// <summary>
            /// Определяет, что пункт меню должен быть разделителем
            /// </summary>
            mfSeparator = 0x00000800     // ... is a seperator (String and
                                         //     ID parameters are ignored).
        }

        public enum WindowMessages
        {
            wmSysCommand = 0x0112
        }
        // I havn't found any other solution than using plain old
        // WinAPI to get what I want.
        // If you need further information on these functions, their
        // parameters, and their meanings, you should look them up in
        // the MSDN.

        // All parameters in the [DllImport] should be self explanatory.
        // NOTICE: Use never stdcall as a calling convention, since Winapi
        // is used.
        // If the underlying structure changes, your program might cause
        // errors that are hard to find.

        // First, we need the GetSystemMenu() function.
        // This function does not have an Unicode counterpart
        [DllImport("USER32", EntryPoint = "GetSystemMenu", SetLastError = true,
                   CharSet = CharSet.Unicode, ExactSpelling = true,
                   CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr apiGetSystemMenu(IntPtr WindowHandle,
                                                      int bReset);

        // And we need the AppendMenu() function. Since .NET uses Unicode,
        // we pick the unicode solution.
        [DllImport("USER32", EntryPoint = "AppendMenuW", SetLastError = true,
                   CharSet = CharSet.Unicode, ExactSpelling = true,
                   CallingConvention = CallingConvention.Winapi)]
        private static extern int apiAppendMenu(IntPtr MenuHandle, int Flags,
                                                 int NewID, String Item);

        // And we also may need the InsertMenu() function.
        [DllImport("USER32", EntryPoint = "InsertMenuW", SetLastError = true,
                   CharSet = CharSet.Unicode, ExactSpelling = true,
                   CallingConvention = CallingConvention.Winapi)]

        private static extern int apiInsertMenu(IntPtr hMenu, int Position,
                                                  int Flags, int NewId,
                                                  String Item);
        [DllImport("user32.dll")]
        public static extern bool DeleteMenu(int hMenu, int uPosition, int uFlags);

        private IntPtr m_SysMenu = IntPtr.Zero;    // Handle to the System Menu

        [DllImport("user32.dll")]
        public static extern int GetSystemMenu(IntPtr hWnd, bool bRevert);

        public SystemMenu()
        {
        }

        // Insert a separator at the given position index starting at zero.
        public bool InsertSeparator(int Pos)
        {
            return (InsertMenu(Pos, ItemFlags.mfSeparator |
                                ItemFlags.mfByPosition, 0, ""));
        }

        // Simplified InsertMenu(), that assumes that Pos is a relative
        // position index starting at zero
        public bool InsertMenu(int Pos, int ID, String Item)
        {
            return (InsertMenu(Pos, ItemFlags.mfByPosition |
                                ItemFlags.mfString, ID, Item));
        }

        // Insert a menu at the given position. The value of the position
        // depends on the value of Flags. See the article for a detailed
        // description.
        public bool InsertMenu(int Pos, ItemFlags Flags, int ID, String Item)
        {
            return (apiInsertMenu(m_SysMenu, Pos, (Int32)Flags, ID, Item) == 0);
        }

        // Appends a seperator
        /// <summary>
        /// Вставляет в системное меню формы в конец разделитель 
        /// </summary>
        public bool AppendSeparator()
        {
            return AppendMenu(0, "", ItemFlags.mfSeparator);
        }

        // This uses the ItemFlags.mfString as default value
        /// <summary>
        /// Вставляет в системное меню формы в конец пункт меню
        /// </summary>
        public bool AppendMenu(int ID, String Item)
        {
            return AppendMenu(ID, Item, ItemFlags.mfString);
        }
        // Superseded function.
        public bool AppendMenu(int ID, String Item, ItemFlags Flags)
        {
            return (apiAppendMenu(m_SysMenu, (int)Flags, ID, Item) == 0);
        }

        // Retrieves a new object from a Form object
        /// <summary>
        /// Получает дескриптор формы Windows Forms (HWIND) для получения доступа к системному меню
        /// </summary>
        public static SystemMenu FromForm(Form Frm)
        {
            SystemMenu cSysMenu = new SystemMenu();

            cSysMenu.m_SysMenu = apiGetSystemMenu(Frm.Handle, 0);
            if (cSysMenu.m_SysMenu == IntPtr.Zero)
            { // Throw an exception on failure
                return null;
            }

            return cSysMenu;
        }

        // Reset's the window menu to it's default
        /// <summary>
        /// Возвращает форму Windows Forms с дескриптором
        /// </summary>
        public static void ResetSystemMenu(Form Frm)
        {
            apiGetSystemMenu(Frm.Handle, 1);
        }

        // Checks if an ID for a new system menu item is OK or not
        public static bool VerifyItemID(int ID)
        {
            return (bool)(ID < 0xF000 && ID > 0);
        }
    }
}
