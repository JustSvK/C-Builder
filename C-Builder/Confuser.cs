using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C_Builder
{
	class Confuser
	{
        public static void Obfuscate(string file)
        {
            string configpath = Path.GetTempPath() + "configconfuser.crproj";
            string configconfuser = Properties.Resources.Confuser;
            string confuserdirectory = Path.GetTempPath() + "Confuser";
            string basedir = new FileInfo(file).Directory.ToString();

            configconfuser = configconfuser.Replace("%path%", basedir)
                .Replace("%basedir%", basedir)
                .Replace("%stub%", file);

            File.WriteAllText(configpath, configconfuser);
            File.WriteAllBytes(Path.GetTempPath() + "confuser.zip", Properties.Resources.ConfuserEx);

            if (Directory.Exists(confuserdirectory))
            {
                Directory.Delete(confuserdirectory, true);
            }

            Directory.CreateDirectory(confuserdirectory);
            ZipFile.ExtractToDirectory(Path.GetTempPath() + "confuser.zip", confuserdirectory);

            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = confuserdirectory + "\\Confuser.CLI.exe";
            process.UseShellExecute = true;
            process.WindowStyle = ProcessWindowStyle.Hidden;
            process.Arguments = "-n " + configpath;

            Process p = Process.Start(process);
            p.WaitForExit();

            File.Delete(Path.GetTempPath() + "confuser.zip");
            File.Delete(Path.GetTempPath() + "configconfuser.crproj");
            Directory.Delete(confuserdirectory, true);
        }
    }
}
