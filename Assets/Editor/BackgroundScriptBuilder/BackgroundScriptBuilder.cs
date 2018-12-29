/* Project Home: https://github.com/luketimothyjones/unity-background-script-builder/
 * License: Mozilla Public License 2.0
 * 
 * NOTE: I kindly ask that you do not publish this script (or a pluginized version of it) to the Unity Asset Store;
 * that's a right that I am explicitly reserving despite the fairly permissive license.
 * 
 * In the spirit of open source, if you make awesome changes, please make a pull request. You'll get credit for them
 * in this comment block (if you so desire).
 *
 * =====
 * Contributors:
 *    Luke Pflibsen-Jones :: Project author | GH: luketimothyjones
 *  
 */

using System;
using System.IO;
using UnityEngine;
using UnityEditor;


namespace BackgroundScriptBuilder
{

    public class BackgroundScriptBuilderWindow : EditorWindow
    {
        /* This Unity editor script listens for changes to .cs files in tthe provided folder and triggers
         * a script rebuild when they do. It is meant to slightly improve quality of life for developers,
         * as the build process starts as soon as the file is saved rather than when Unity gains focus.
         *
         * USAGE:
         * Place this script in Assets/Editor/
         * 
         * In the Unity menu bar, go to Window > Asset Management > Background Script Builder and place
         * the window that opens somewhere in your editor.
         * 
         * Due to the way that Unity handles GUI elements (which is usually a good thing), the window
         * must exist somewhere in the editor for this to work (eg, it has to exist as a tab somewhere
         * in your layout, but the tab doesn't need to be open).
         */

        ScriptChangeWatcher scriptChangeWatcher;

        bool doBackgroundBuilds = true;
        string scriptFolderPath = "Scripts/";
        
        bool hasInitialized = false;

        [MenuItem("Window/Asset Management/Background Script Builder")]
        static void Init()
        {
            BackgroundScriptBuilderWindow window = (BackgroundScriptBuilderWindow)GetWindow(typeof(BackgroundScriptBuilderWindow));
            window.Show();
        }

        void OnGUI()
        {
            // This conditional is true when the window is first opened and every time scripts are rebuilt
            if (doBackgroundBuilds && scriptChangeWatcher == default(ScriptChangeWatcher)) {
                if (doBackgroundBuilds) {
                    InitializeWatcher();
                }
            }

            EditorGUI.BeginChangeCheck();
            doBackgroundBuilds = EditorGUILayout.BeginToggleGroup("Enable Background Building", doBackgroundBuilds);

            if (EditorGUI.EndChangeCheck()) {
                if (doBackgroundBuilds) {
                    InitializeWatcher();

                } else {
                    if (scriptChangeWatcher != default(ScriptChangeWatcher)) {
                        scriptChangeWatcher.Destroy();
                        hasInitialized = false;
                    }
                }
            }

            EditorGUI.BeginChangeCheck();
            scriptFolderPath = EditorGUILayout.TextField("Script Folder", scriptFolderPath);

            if (EditorGUI.EndChangeCheck()) {
                if (doBackgroundBuilds) {
                    InitializeWatcher();
                }
            }

            EditorGUILayout.EndToggleGroup();
        }

        void InitializeWatcher()
        {
            /* Handle parsing of script folder path and manage watcher state */

            if (scriptChangeWatcher != default(ScriptChangeWatcher)) {
                scriptChangeWatcher.Destroy();
            }

            if (scriptFolderPath != "") {
                if (scriptFolderPath.StartsWith("/")) {
                    scriptFolderPath = scriptFolderPath.Substring(1);
                }

                if (!scriptFolderPath.EndsWith("/")) {
                    scriptFolderPath += "/";
                }

                bool assetsPrepended = false;
                if (!scriptFolderPath.StartsWith("Assets")) {
                    assetsPrepended = true;
                    scriptFolderPath = "Assets" + (scriptFolderPath.StartsWith("/") ? "" : "/") + scriptFolderPath;
                }

                if ((scriptFolderPath != "Assets/" || !assetsPrepended) && Directory.Exists(scriptFolderPath)) {
                    scriptChangeWatcher = new ScriptChangeWatcher();

                    if (scriptChangeWatcher.Initialize(scriptFolderPath)) {
                        if (!hasInitialized) {
                            Debug.Log("Now watching " + scriptFolderPath + " and its subdirectories");
                        }
                        hasInitialized = true;
                    }
                }
            }
        }
    }

    public class ScriptChangeWatcher
    {
        FileSystemWatcher watcher;

        public bool Initialize(string scriptFolder)
        {
            /* Adds a watcher to the given folder that calls OnChanged whenever
             * a .cs file is modified */

            try {
                watcher = new FileSystemWatcher(scriptFolder, "*.cs") {
                    NotifyFilter = NotifyFilters.LastWrite,
                    IncludeSubdirectories = true
                };

                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.EnableRaisingEvents = true;

                return true;

            } catch (System.UnauthorizedAccessException) {
                Debug.LogWarning("Editor does not have permission to access folder '" + scriptFolder + "'");
                watcher.Dispose();

                return false;
            }
        }

        public void Destroy()
        {
            if (watcher != default(FileSystemWatcher)) {
                watcher.Dispose();
            }
        }

        public static void ScriptReloadTask()
        {
            /* This is bound to EditorApplication.update whenever a script file is changed.
             * Doing so is necessary because AssetDatebase.Refresh() does nothing if it is not
             * executed in the main thread, and this script runs in the GUI thread. */
              
            try {
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

                EditorApplication.update += () => {
                    EditorApplication.update -= ScriptReloadTask;
                };

            } catch (Exception ex) {
                Debug.LogError(ex.Message);
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            EditorApplication.update += ScriptReloadTask;
        }
    }
}