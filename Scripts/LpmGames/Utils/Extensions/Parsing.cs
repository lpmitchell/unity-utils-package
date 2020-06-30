namespace Extensions
{
    public static class Parsing
    {
        public static bool BoolParse(this string str)
        {
            if (str == "1") return true;
            if (str == "0") return false;
            if (bool.TryParse(str, out var val)) return val;
            return false;
        }
        
    }
}