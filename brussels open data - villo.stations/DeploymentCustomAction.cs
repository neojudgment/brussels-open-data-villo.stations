// •————————————————————————————————————————————————————————————————————————————————————————————————————————————•
// |                                                                                                            |
// |    brussels open data - villo.stations is a proof of concept (PoC). <https://github.com/neojudgment/>      |
// |                                                                                                            |
// |    villo.stations, location of the Villo! stations in the Brussels Capital Region with indication of       |
// |    the availability (bikes, bike stands) in real time.                                                     |
// |                                                                                                            |
// |    brussels open data - villo.stations uses Microsoft WindowsAPICodePack and GMap.NET to                   |
// |    demonstrate how to retrieve data from Brussels open data Store.                                         |
// |                                                                                                            |
// |    brussels open data - villo.stations uses Elysium library that implements Modern UI for                  |
// |    Windows Presentation Foundation.                                                                        |
// |                                                                                                            |
// |    This program is under Microsoft Reciprocal License (Ms-RL)                                              |
// |                                                                                                            |
// |    This program is distributed in the hope that it will be useful but WITHOUT ANY WARRANTY;                |
// |    without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.               |
// |                                                                                                            |
// |    You should have received a copy of the Microsoft Reciprocal License (Ms-RL)                             |
// |    along with this program.  If not, see <http://opensource.org/licenses/MS-RL>.                           |
// |                                                                                                            |
// |    Copyright © Pascal Hubert - Brussels, Belgium 2017. <mailto:pascal.hubert@outlook.com>                  |
// •————————————————————————————————————————————————————————————————————————————————————————————————————————————•

using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using static System.Environment;

namespace OpenData
{
    [RunInstaller(true)]
    public partial class DeploymentCustomAction : Installer
    {
        public DeploymentCustomAction()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            // Création des répertoires
            string path = GetFolderPath(SpecialFolder.ApplicationData) + "\\brussels open data - villo.stations";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (Directory.Exists(path))
            {
                Directory.CreateDirectory(path + "\\log");
                Directory.CreateDirectory(path + "\\json");
                Directory.CreateDirectory(path + "\\profiles");
            }

            string path1 = GetFolderPath(SpecialFolder.CommonApplicationData) + "\\brussels open data - villo.stations";
            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }

            if (Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1 + "\\cache\\");
            }

            // Optisation ngen
            string runtimeStr = RuntimeEnvironment.GetRuntimeDirectory();
            string ngenStr = Path.Combine(runtimeStr, "ngen.exe");

            Process process = new Process();
            process.StartInfo.FileName = ngenStr;

            string assemblyPath = Context.Parameters["assemblypath"];

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.Arguments = "install \"" + assemblyPath + "\"";

            process.Start();
            process.WaitForExit();
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            // Suppression des données résiduelles et des répertoires
            string pathAppData = GetFolderPath(SpecialFolder.ApplicationData) + "\\brussels open data - villo.stations";
            string pathProgramData = GetFolderPath(SpecialFolder.CommonApplicationData) + "\\brussels open data - villo.stations";

            if (pathAppData != null && Directory.Exists(pathAppData))
            {
                Directory.Delete(pathAppData, recursive: true);
            }

            if (pathProgramData != null && Directory.Exists(pathProgramData))
            {
                Directory.Delete(pathProgramData, recursive: true);
            }
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            // Inscription du certificat Root Ascertia
            string ApplicationPath = Context.Parameters["AssemblyPath"];

            int indexOfSteam = ApplicationPath.IndexOf("villo.stations.exe");
            if (indexOfSteam >= 0)
                ApplicationPath = ApplicationPath.Remove(indexOfSteam);

            string CertificateRoot = ApplicationPath + "AscertiaRootCA.der";

            ProcessStartInfo process = new ProcessStartInfo();
            process.FileName = ApplicationPath + "certmgr.exe";
            process.WorkingDirectory = ApplicationPath;
            process.UseShellExecute = false;
            process.CreateNoWindow = true;
            process.Verb = "runas";
            process.Arguments = "-add -all -c AscertiaRootCA.der -s -r localMachine root";
            Process rootProc = Process.Start(process);
            rootProc.WaitForExit();
            rootProc.Dispose();

            if (File.Exists(ApplicationPath + "certmgr.exe"))
            {
                File.Delete(ApplicationPath + "certmgr.exe");
            }

            if (File.Exists(ApplicationPath + "AscertiaRootCA.der"))
            {
                File.Delete(ApplicationPath + "AscertiaRootCA.der");
            }
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
    }
}