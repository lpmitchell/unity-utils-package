#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace LpmGames.Utils.Core
{
    public static class LpmUtilsEditor
    {
    
        #region Update package button
    
        private static AddRequest _currentAddRequest;
      
        [MenuItem("Window/Update LPM Utils Package")]
        private static void UpdateUtilities()
        {
            _currentAddRequest = Client.Add("https://github.com/lpmitchell/unity-utils-package.git");
            EditorApplication.update += CheckInstallProgress;
        }

        private static void CheckInstallProgress()
        {
            if (!_currentAddRequest.IsCompleted) return;
        
            if (_currentAddRequest.Status == StatusCode.Success)
            {
                UnityEngine.Debug.Log($"<color=#FF0><b>[LPM Utils]</b></color> Upgraded to version <color=#0F0><b>{_currentAddRequest.Result.version}</b></color>");
            }
            else if (_currentAddRequest.Status >= StatusCode.Failure)
            {
                UnityEngine.Debug.LogWarning($"<color=#FF0><b>[LPM Utils]</b></color> Failed install with error: {_currentAddRequest.Error.message}");
            }
        
            // ReSharper disable once DelegateSubtraction
            EditorApplication.update -= CheckInstallProgress;
            _currentAddRequest = null;
        }
    
        #endregion
    }
}

#endif
