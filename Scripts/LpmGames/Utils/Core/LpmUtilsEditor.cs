#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace LpmGames.Utils.Core
{
    public static class LpmUtilsEditor
    {
        
        #region Package Updater

        private class PackageUpdater
        {
            private readonly string _url;
            private AddRequest _addRequest;
            public bool Done { get; private set; }

            public PackageUpdater(string url)
            {
                _url = url;
            }

            public void Update()
            {
                _addRequest = Client.Add(_url);
                EditorApplication.update += CheckInstallProgress;
                
                UnityEngine.Debug.Log($"<color=#FF0><b>[LPM Utils]</b></color> Updating {_url}");
            }

            private void CheckInstallProgress()
            {
                if (!_addRequest.IsCompleted) return;
                
                // ReSharper disable once DelegateSubtraction
                EditorApplication.update -= CheckInstallProgress;
                Done = true;
        
                if (_addRequest.Status == StatusCode.Success)
                {
                    UnityEngine.Debug.Log($"<color=#FF0><b>[LPM Utils]</b></color> Upgraded <b>{_addRequest.Result.name}</b> to version <color=#0F0><b>{_addRequest.Result.version}</b></color>");
                }
                else if (_addRequest.Status >= StatusCode.Failure)
                {
                    UnityEngine.Debug.LogWarning($"<color=#FF0><b>[LPM Utils]</b></color> Failed to install <b>{_url}</b>  with error: {_addRequest.Error.message}");
                }

                _addRequest = null;
            }
        }
        #endregion
    
        #region Update package button

        private static ListRequest _currentListRequest;
        private static PackageUpdater _currentUpdater;
        private static readonly Queue<PackageUpdater> _packageUpdateQueue = new Queue<PackageUpdater>();
        private static bool _updateHooked;
      
        [MenuItem("Window/Update LPM Utils Package")]
        private static void UpdateUtilities()
        {
            if (_currentUpdater != null && !_currentUpdater.Done)
            {
                UnityEngine.Debug.Log($"<color=#FF0><b>[LPM Utils]</b></color> Cannot currently update: an update is already in progress");
                return;
            }
            
            EnqueueUpdate("https://github.com/lpmitchell/unity-utils-package.git");
        }

        private static void EnqueueUpdate(string url)
        {
            _packageUpdateQueue.Enqueue(new PackageUpdater(url));
            if (!_updateHooked)
            {
                _updateHooked = true;
                EditorApplication.update += CheckUpdateQueue;
            }
        }

        private static void CheckUpdateQueue()
        {
            if (_currentUpdater == null || _currentUpdater.Done)
            {
                if (_packageUpdateQueue.Count == 0)
                {
                    _currentUpdater = null;
                    _updateHooked = false;
                    // ReSharper disable once DelegateSubtraction
                    EditorApplication.update -= CheckUpdateQueue;
                    UnityEngine.Debug.Log($"<color=#FF0><b>[LPM Utils]</b></color> All done!");
                }
                else
                {
                    _currentUpdater = _packageUpdateQueue.Dequeue();
                    _currentUpdater.Update();
                }
            }
        }

        [MenuItem("Window/Update All Git Packages")]
        private static void UpdateAllUtilities()
        {
            if (_currentUpdater != null && !_currentUpdater.Done)
            {
                UnityEngine.Debug.Log($"<color=#FF0><b>[LPM Utils]</b></color> Cannot currently update: an update is already in progress");
                return;
            }
            
            UnityEngine.Debug.Log($"<color=#FF0><b>[LPM Utils]</b></color> Checking packages...");
            _currentListRequest = Client.List(true);
            EditorApplication.update += CheckListProgress;
        }

        private static void CheckListProgress()
        {
            if (!_currentListRequest.IsCompleted) return;
            // ReSharper disable once DelegateSubtraction
            EditorApplication.update -= CheckListProgress;
            
            if (_currentListRequest.Status != StatusCode.Success)
            {
                UnityEngine.Debug.LogWarning($"<color=#FF0><b>[LPM Utils]</b></color> Failed list packages with error: {_currentListRequest.Error.message}");
                _currentListRequest = null;
                return;
            }

            var packages = _currentListRequest.Result;
            _currentListRequest = null;
            
            foreach (var package in packages)
            {
                if (package.source != PackageSource.Git) continue;
                var gitUrl = package.packageId.Substring(package.packageId.IndexOf('@') + 1);
                EnqueueUpdate(gitUrl);
            }
        }

        #endregion
    }
}

#endif
