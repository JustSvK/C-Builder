using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace C_Builder
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new C_Builder.Login());
        }
        static public bool setSetting(string Key, string Value)
		{
			Configuration ConfigFile =
				ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			bool KeyExists = false;

			foreach (string strKey in ConfigFile.AppSettings.Settings.AllKeys)
			{
				if (strKey == Key)
				{
					KeyExists = true;
					ConfigFile.AppSettings.Settings[Key].Value = Value;
					break;
				}
			}
			if (!KeyExists)
			{
				ConfigFile.AppSettings.Settings.Add(Key, Value);
			}
			ConfigFile.Save(ConfigurationSaveMode.Modified);
			ConfigurationManager.RefreshSection("appSettings");
			return true;
		}
	}
}
