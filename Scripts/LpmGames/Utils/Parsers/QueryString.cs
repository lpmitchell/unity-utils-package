using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once CheckNamespace
namespace LpmGames.Utils.Parsers
{
    public class QueryString
    {
        public static Dictionary<string, string> Parse(string queryString)
        {
            if(queryString.StartsWith("?")) queryString = queryString.Substring(1);
            
            var parsed = new Dictionary<string, string>();
            
            var parts = queryString.Split('&');
            foreach (var part in parts)
            {
                var equalsPosition = part.IndexOf("=", StringComparison.Ordinal);
                if (equalsPosition == -1)
                {
                    var boolKey = UrlDecode(part);
                    parsed[boolKey] = "1";
                }
                else
                {
                    var key = UrlDecode(part.Substring(0, equalsPosition));
                    var value = UrlDecode(part.Substring(equalsPosition + 1));
                    parsed[key] = value;
                }
            }
            return parsed;
        }
        
        #region From Mono HttpUtility.cs
        
        // Self-contained url decode method..
        
        public static string UrlDecode (string str)
        {
            if (null == str) 
                return null;

            if (str.IndexOf ('%') == -1 && str.IndexOf ('+') == -1)
                return str;
			
            var e = Encoding.UTF8;

            long len = str.Length;
            var bytes = new List <byte> ();
            int xchar;
            char ch;
			
            for (var i = 0; i < len; i++) {
                ch = str [i];
                if (ch == '%' && i + 2 < len && str [i + 1] != '%') {
                    if (str [i + 1] == 'u' && i + 5 < len) {
                        // unicode hex sequence
                        xchar = GetChar (str, i + 2, 4);
                        if (xchar != -1) {
                            WriteCharBytes (bytes, (char)xchar, e);
                            i += 5;
                        } else
                            WriteCharBytes (bytes, '%', e);
                    } else if ((xchar = GetChar (str, i + 1, 2)) != -1) {
                        WriteCharBytes (bytes, (char)xchar, e);
                        i += 2;
                    } else {
                        WriteCharBytes (bytes, '%', e);
                    }
                    continue;
                }

                if (ch == '+')
                    WriteCharBytes (bytes, ' ', e);
                else
                    WriteCharBytes (bytes, ch, e);
            }
			
            var buf = bytes.ToArray ();
            return e.GetString (buf);
        }
        
        static void WriteCharBytes (IList buf, char ch, Encoding e)
        {
            if (ch > 255) {
                foreach (byte b in e.GetBytes (new[] { ch }))
                    buf.Add (b);
            } else
                buf.Add ((byte)ch);
        }

        
        static int GetChar (string str, int offset, int length)
        {
            var val = 0;
            var end = length + offset;
            for (var i = offset; i < end; i++) {
                var c = str [i];
                if (c > 127)
                    return -1;

                var current = GetInt ((byte) c);
                if (current == -1)
                    return -1;
                val = (val << 4) + current;
            }

            return val;
        }
        
        static int GetInt (byte b)
        {
            var c = (char) b;
            if (c >= '0' && c <= '9')
                return c - '0';

            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;

            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;

            return -1;
        }
        #endregion

    }
}