using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace LpmGames.Utils.Extensions
{
    public static class Encoding
    {
        public static string JavascriptSerialize(this object arg)
        {
            if (arg == null) return "null";
        
            switch (arg)
            {
                case string argString:                   return argString.JavascriptEncode(true);
                case int argInt:                         return argInt.ToString();
                case uint argUint:                       return argUint.ToString();
                case short argShort:                     return argShort.ToString();
                case ushort argUshort:                   return argUshort.ToString();
                case float argFloat:                     return $"{argFloat}";
                case double argDouble:                   return $"{argDouble}";
                case bool argBool:                       return argBool ? "true" : "false";
                case long argLong:                       return argLong.ToString();
                case ulong argUlong:                     return argUlong.ToString();
                case IDictionary iDict:                  return iDict.JsonEncode();
                case IEnumerable iEnumerable:            return iEnumerable.JsonEncode();
                default:                                 return JsonUtility.ToJson(arg, Application.isEditor);
            }
        }

        public static string JsonEncode(this IDictionary dictionary)
        {
            if (dictionary == null) return "{}";
            
            var variablesData = new StringBuilder();
            variablesData.Append("{");
            var count = dictionary.Count;
            var keyList = new object[count];
            var valueList = new object[count];
            
            var i = 0;
            foreach (var key in dictionary.Keys) keyList[i++] = key;
            
            i = 0;
            foreach (var value in dictionary.Values) valueList[i++] = value;
            
            for (i = 0; i < count; i++)
            {
                if(i > 0) variablesData.Append(",");
                variablesData.Append(keyList[i].JavascriptSerialize());
                variablesData.Append(":");
                variablesData.Append(valueList[i].JavascriptSerialize());
            }
            variablesData.Append("}");
            return variablesData.ToString();
        }
        
        public static string JsonEncode(this IEnumerable list)
        {
            if (list == null) return "[]";
            
            var variablesData = new StringBuilder();
            variablesData.Append("[");
            var first = true;
            foreach(var item in list)
            {
                if (!first) variablesData.Append(",");
                else first = false;
                variablesData.Append(item.JavascriptSerialize());
            }
            variablesData.Append("]");
            return variablesData.ToString();
        }
        
        // From mono HttpUtility.cs JavaScriptStringEncode
        public static string JavascriptEncode (this string value, bool addDoubleQuotes)
        {
            if (string.IsNullOrEmpty (value))
                return addDoubleQuotes ? "\"\"" : string.Empty;

            var len = value.Length;
            var needEncode = false;
            char c;
            for (var i = 0; i < len; i++) {
                c = value [i];

                if (c >= 0 && c <= 31 || c == 34 || c == 39 || c == 60 || c == 62 || c == 92) {
                    needEncode = true;
                    break;
                }
            }

            if (!needEncode)
                return addDoubleQuotes ? "\"" + value + "\"" : value;

            var sb = new StringBuilder ();
            if (addDoubleQuotes)
                sb.Append ('"');

            for (var i = 0; i < len; i++) {
                c = value [i];
                if (c >= 0 && c <= 7 || c == 11 || c >= 14 && c <= 31 || c == 39 || c == 60 || c == 62)
                    sb.AppendFormat ("\\u{0:x4}", (int)c);
                else switch ((int)c) {
                    case 8:
                        sb.Append ("\\b");
                        break;

                    case 9:
                        sb.Append ("\\t");
                        break;

                    case 10:
                        sb.Append ("\\n");
                        break;

                    case 12:
                        sb.Append ("\\f");
                        break;

                    case 13:
                        sb.Append ("\\r");
                        break;

                    case 34:
                        sb.Append ("\\\"");
                        break;

                    case 92:
                        sb.Append ("\\\\");
                        break;

                    default:
                        sb.Append (c);
                        break;
                }
            }

            if (addDoubleQuotes)
                sb.Append ('"');

            return sb.ToString ();
        }

    }
}