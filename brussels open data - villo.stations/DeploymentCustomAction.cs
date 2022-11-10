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
// |    Copyright © Pascal Hubert - Brussels, Belgium 2022. <mailto:pascal.hubert@outlook.com>                  |
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
            string RuntimePath = RuntimeEnvironment.GetRuntimeDirectory();
            string NgenPath = Path.Combine(RuntimePath, "ngen.exe");
            string assemblyPath = Context.Parameters["assemblypath"];

            ProcessStartInfo process = new ProcessStartInfo()
            {
                FileName = NgenPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                Arguments = "install \"" + assemblyPath + "\""
            };

            Process rootProc = Process.Start(process);
            rootProc.WaitForExit();
            rootProc.Dispose();
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            // Suppression des données résiduelles et des répertoires
            string AppDataPath = GetFolderPath(SpecialFolder.ApplicationData) + "\\brussels open data - villo.stations";
            string ProgramDataPath = GetFolderPath(SpecialFolder.CommonApplicationData) + "\\brussels open data - villo.stations";

            if (AppDataPath != null && Directory.Exists(AppDataPath))
            {
                Directory.Delete(AppDataPath, recursive: true);
            }

            if (ProgramDataPath != null && Directory.Exists(ProgramDataPath))
            {
                Directory.Delete(ProgramDataPath, recursive: true);
            }
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            // Inscription du certificat Root
            string ApplicationPath = Context.Parameters["AssemblyPath"];
            int indexOfSteam = ApplicationPath.IndexOf("villo.stations.exe");

            if (indexOfSteam >= 0)
            {
                ApplicationPath = ApplicationPath.Remove(indexOfSteam);
            }

            ProcessStartInfo process = new ProcessStartInfo()
            {
                FileName = ApplicationPath + "certmgr.exe",
                WorkingDirectory = ApplicationPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                Verb = "runas",
                Arguments = "-add -all -c AscertiaRootCA.der -s -r localMachine root"
            };

            Process rootProc = Process.Start(process);
            rootProc.WaitForExit();
            rootProc.Dispose();

            // Suppression des fichiers obsolètes
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