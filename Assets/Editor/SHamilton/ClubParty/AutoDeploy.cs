using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace SHamilton.ClubParty {
    public static class AutoDeploy {
        [PostProcessBuild(100)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
            if (target != BuildTarget.WebGL) return;
            
            // Check if Deploy config files are present before asking this
            var assets = Application.dataPath; // Project Folder/Assets
            var deployDir = Path.Join(assets, "Deploy");
            var deployConfigPath = Path.Join(deployDir, "deploy-config.txt");
            if (!File.Exists(deployConfigPath)) {
                Debug.LogWarning("No deploy.py config files found! " +
                                 "You should run 'python deploy.py' in the Deploy folder " +
                                 "to go through the first-run setup.");
                return;
            }
            
            var shouldDeploy = EditorUtility.DisplayDialog(
                "Deploy WebGL Build?",
                "Do you want to deploy this WebGL build to the server?",
                "Yes",
                "No"
            );
            if (shouldDeploy) {
                // TODO: Maybe try a separate thread? The editor appears to hang when yes is clicked.
                // TODO: Test this with Windows
                var deployScript = Path.Join(deployDir, "deploy.py");
                var process = new Process();
                process.StartInfo = new ProcessStartInfo("python", deployScript) {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                };
                process.Start();
                var output = process.StandardOutput.ReadToEnd();
                Debug.Log("deploy.py output:\n"+output);
            }
        }
    }
}
