using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace LpmGames.Utils.Debug
{
    public static class Log
    {
        
        #if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void LocalBridge(string method, string argJson);
        #endif
        
        public static readonly Queue<string> LogHistory = new Queue<string>(500);

        public static string PathToTag(string path, int lineNumber)
        {
            var tag = path.Replace('\\', '/');
            return Path.GetFileNameWithoutExtension(tag) + ":" + lineNumber;
        }
        
        [Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
        public static void Message(string message, object data = null, [CallerFilePath] string tag = "Unknown", [CallerLineNumber] int lineNumber = 0)
        {
            tag = PathToTag(tag, lineNumber);
            Write($"[{tag}] " + message, data);
        }
        
        [Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
        public static void Warning(string message, object data = null, [CallerFilePath] string tag = "Unknown", [CallerLineNumber] int lineNumber = 0)
        {
            tag = PathToTag(tag, lineNumber);
            WriteWarn($"[{tag}] " + message, data);
        }
        
        [Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
        public static void Error(string message, object data = null, [CallerFilePath] string tag = "Unknown", [CallerLineNumber] int lineNumber = 0)
        {
            tag = PathToTag(tag, lineNumber);
            WriteError($"[{tag}] " + message, data);
        }

        [Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
        public static void AssertMessage(bool assert, string message, object data = null, [CallerFilePath] string tag = "Unknown", [CallerLineNumber] int lineNumber = 0)
        {
            if (assert) return;
            tag = PathToTag(tag, lineNumber);
            Write($"[{tag}.Assert] " + message, data);
        }
        
        [Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
        public static void Assert(bool assert, string message, object data = null, [CallerFilePath] string tag = "Unknown", [CallerLineNumber] int lineNumber = 0)
        {
            if (assert) return;
            tag = PathToTag(tag, lineNumber);
            WriteWarn($"[{tag}.Assert]" + message, data);
        }
        
        [Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
        public static void AssertError(bool assert, string message, object data = null, [CallerFilePath] string tag = "Unknown", [CallerLineNumber] int lineNumber = 0)
        {
            if (assert) return;
            tag = PathToTag(tag, lineNumber);
            WriteError($"[{tag}.Assert] " + message, data);
        }

        private static string SerializeDataObject(object data)
        {
            if (data == null) return "";
            return JsonUtility.ToJson(data, true);
        }
        
        [Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
        private static void Write(string message, object data)
        {
            var messageExtra = SerializeDataObject(data);
            if (LogHistory.Count > 400) LogHistory.Dequeue();
            LogHistory.Enqueue($"L {Time.time:N4}: {message}{messageExtra}");
            
#if UNITY_WEBGL && !UNITY_EDITOR
            BrowserDispatch(new LogMessage
            {
                Level = "Debug",
                Message = message,
                Time = Time.time,
                Data = messageExtra
            });
#else
            UnityEngine.Debug.Log(message + messageExtra);
#endif
        }
        
        [Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
        private static void WriteWarn(string message, object data)
        {
            var messageExtra = SerializeDataObject(data);
            LogHistory.Enqueue($"W {Time.time:N4}: {message}{messageExtra}");
            if (LogHistory.Count > 499) LogHistory.Dequeue();
            
#if UNITY_WEBGL && !UNITY_EDITOR
            BrowserDispatch(new LogMessage
            {
                Level = "Warning",
                Message = message,
                Time = Time.time,
                Data = messageExtra
            });
#else
            UnityEngine.Debug.LogWarning(message + messageExtra);
#endif
        }
        
        [Conditional("UNITY_WEBGL"), Conditional("UNITY_EDITOR")]
        private static void WriteError(string message, object data)
        {
            var messageExtra = SerializeDataObject(data);
            LogHistory.Enqueue($"E {Time.time:N4}: {message}{messageExtra}");
            if (LogHistory.Count > 499) LogHistory.Dequeue();
            
#if UNITY_WEBGL && !UNITY_EDITOR
            BrowserDispatch(new LogMessage
            {
                Level = "Error",
                Message = message,
                Time = Time.time,
                Data = messageExtra
            });
#else
            UnityEngine.Debug.LogError(message + messageExtra);
#endif
        }


#if UNITY_WEBGL && !UNITY_EDITOR
        
        [System.Serializable]        
        private struct LogMessage
        {
            public string Level;
            public string Message;
            public float Time;
            public string Data;
        }
        
        [System.Serializable]
        private struct EventCall
        {
            public string Tool;
            public string Event;
            public string Data;
        }
        
        private static void BrowserDispatch(LogMessage message)
        {
            LocalBridge("ToolCallback", JsonUtility.ToJson(new EventCall
            {
                Tool = "Console",
                Event = "Message",
                Data = JsonUtility.ToJson(message)
            }));
        }
#endif
    }
}