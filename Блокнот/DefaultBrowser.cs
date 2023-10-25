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
using System.Diagnostics;
using System.IO;

class DefaultBrowser
{
    private object regValue;

    /// <summary>
    /// ID браузера
    /// </summary>
    public string ProgID { get; private set; }

    /// <summary>
    /// Путь, используемый для запуска браузера
    /// </summary>
    public string BrowserPath { get; private set; }

    /// <summary>
    /// Местонахождение exe-файла браузера
    /// </summary>
    public string BrowserExePath { get; private set; }

    /// <summary>
    /// Результат первоначальной инициализации
    /// </summary>
    public bool IsInitSuccess { get { return !string.IsNullOrEmpty(BrowserPath); } }

    public DefaultBrowser()
    {
        InitBrowserId();
        IntiBrowserPath();
        InitBrowserExePath();
    }

    /// <summary>
    /// Определение ID браузера по-умолчанию
    /// </summary>
    private void InitBrowserId()
    {
        using (RegistryKey userDefBrowserKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
        {
            if (userDefBrowserKey != null && (regValue = userDefBrowserKey.GetValue("Progid")) != null)
            {
                ProgID = regValue as string;
            }
        }
    }

    /// <summary>
    /// Определения пути для запуска браузера
    /// В зависимости от настроек ОС, он может хранится в 3 ключах:
    ///     1. HKEY_CURRENT_USER\SOFTWARE\Classes\ + ProgID + \shell\open\command
    ///     2. HKEY_LOCAL_MACHINE\SOFTWARE\Classes\ + ProgID + \shell\open\command
    ///     3. HKEY_CLASSES_ROOT\ + ProgID + "\shell\open\command
    /// </summary>
    private void IntiBrowserPath()
    {
        // Попытаемся получить из CurrentUser путь к браузеру по-умолчанию, используя ID
        using (RegistryKey defBrowserKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Classes" + ProgID + @"\shell\open\command"))
        {
            if (defBrowserKey != null && (regValue = defBrowserKey.GetValue(null)) != null)
                BrowserPath = regValue as string;
        }

        // Попытаемся получить из LocalMachine путь к браузеру по-умолчанию, используя ID
        if (BrowserPath == null)
        {
            using (RegistryKey defBrowserKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Classes" + ProgID + @"\shell\open\command"))
            {
                if (defBrowserKey != null && (regValue = defBrowserKey.GetValue(null)) != null)
                    BrowserPath = regValue as string;
            }
        }

        // Попытаемся получить из ClassesRoot путь к браузеру по-умолчанию, используя ID
        if (BrowserPath == null)
        {
            using (RegistryKey defBrowserKey = Registry.ClassesRoot.OpenSubKey(ProgID + @"\shell\open\command"))
            {
                if (defBrowserKey != null && (regValue = defBrowserKey.GetValue(null)) != null)
                    BrowserPath = regValue as string;
            }
        }
    }

    /// <summary>
    /// Разные браузеры имеют разные пути запуска, кто-то использует "%1" для параметра, кто-то нет
    /// </summary>
    private void InitBrowserExePath()
    {
        if (!IsInitSuccess) return;

        if (BrowserPath[0] == '"')
            BrowserExePath = BrowserPath.Substring(0, BrowserPath.Substring(1).IndexOf('"') + 2).Trim();
        else
            BrowserExePath = BrowserPath.Substring(0, BrowserPath.IndexOf(" ", StringComparison.Ordinal)).Trim();
    }

    /// <summary>
    /// Открытие файла в браузере
    /// </summary>
    /// <param name="url">Интернет адрес или путь к файлу на компьютере</param>
    /// <returns>Результат выполнения запроса</returns>
    public bool StartUrl(string url)
    {
        if (!IsInitSuccess)
            return false;

        if (!new FileInfo(BrowserExePath.Trim('"')).Exists)
            return false;

        var BrowserArgs = BrowserPath.Substring(BrowserExePath.Length).TrimStart();

        /// У некоторых браузеров есть параметр "%1", некоторые нет
        if (BrowserArgs.Contains("%1"))
        {
            // Некоторые браузеры окружают кавычками этот параметр, некоторые нет
            if (BrowserArgs.Contains("% 1"))
                BrowserArgs = BrowserArgs.Replace(" % 1", url);
            else
                BrowserArgs = BrowserArgs.Replace("%1", url);
        }
        else
            BrowserArgs = url;

        Process.Start(BrowserExePath, BrowserArgs);
        return true;
    }
}