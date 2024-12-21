using MongoDB.Bson;

namespace WorkingWithDB
{
    public class Compare: IConvertibleToBsonDocument
    {
        public string Comparer { get; set; }
        public int Result { get; set; }

        public Compare()
        {
            Comparer = "";
            Result = 0;
        }
        public Compare(string tryCompare)
        {
            if(TryCompare(tryCompare, out string comparer, out int result))
            {
                Comparer = comparer;
                Result = result;
            }
            else
            {
                throw new Exception();
            }
        }

        public static bool IsCompare(string? s)
        {
            return TryCompare(s, out _, out _);
        }
        static bool TryCompare(string? s, out string comparer, out int result)
        {
            comparer = "$";
            int Skip = 1;
            if (s is null || s.Length < 2)
            {
                result = default;
                return false;
            }
            if (s[0] == '>')
            {
                comparer += "gt";
            }
            else if (s[0] == '<')
            {
                comparer += "lt";
            }
            else
            {
                result = default;
                return false;
            }
            if (s[1] == '=')
            {
                comparer += "e";
                Skip = 2;
            }
            s = string.Join("", s.Skip(Skip));
            return int.TryParse(s, out result);
        }
        public BsonDocument ToBsonDocument()
        { 
          return new BsonDocument(Comparer, Result);
        }
    }
}
