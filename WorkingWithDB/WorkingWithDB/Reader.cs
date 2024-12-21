using DnsClient.Protocol;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace WorkingWithDB
{
    enum ReadResult
    {
        Succesfull,
        Menu,
        End,
        Error
    }
    class Reader
    {
        public static bool Try<T>(out T result)
        {
            ReadResult readLineResult;
            do
            {
                readLineResult = Reader.ReadWhile(out result);
                if (readLineResult == ReadResult.Succesfull)
                {
                    return true;
                }
                Console.WriteLine($"Invalid Value {typeof(T)}");
            }
            while (readLineResult == ReadResult.Error);
            return false; //ReadResult.End
        }
        public static void TryWhile<T>(out T result)
        {
            var readLineResult = Reader.ReadWhile(out result);
            while (readLineResult != ReadResult.Succesfull)
            {
                Console.WriteLine($"Invalid Value {typeof(T)}");
                readLineResult = Reader.ReadWhile(out result);
            }
        }

        public static (T result, ReadResult readResult) GetValue<T>()
        {
            TryWhile(out T result);
            return (result, ReadResult.Succesfull);
        }


        public static ReadResult ReadWhile<T>(out T result)
        {
            string? s = Console.ReadLine();
            switch (s)
            {
                case null:
                    result = default;
                    return ReadResult.Error;
                case "end":
                    result = default;
                    return ReadResult.End;
                case "menu":
                    result = default;
                    return ReadResult.Menu;
                default:
                    try
                    {
                        result = TryConvertResult(s);
                        return ReadResult.Succesfull;
                    }
                    catch
                    {
                        goto case null;
                    }
            }

            static T TryConvertResult(string s)
            {
                if (Compare.IsCompare(s))
                {
                    return (T)Convert.ChangeType(new Compare(s), typeof(T));
                }
                else
                {
                    return (T)Convert.ChangeType(s, typeof(T));
                }

            }
        }


        public static (T? result, ReadResult readResult) ReadTuple<T>()
        {
            string? s = Console.ReadLine();
            switch (s)
            {
                case null:
                    return (default, ReadResult.Error);
                case "end":
                    return (default, ReadResult.End);
                case "menu":
                    return (default, ReadResult.Menu);
                default:
                    try
                    {
                        return (TryConvertResult(s), ReadResult.Succesfull);
                    }
                    catch
                    {
                        goto case null;
                    }
            }

            static T TryConvertResult(string s)
            {
                if (Compare.IsCompare(s))
                {
                    return (T)Convert.ChangeType(new Compare(s), typeof(T));
                }
                return (T)Convert.ChangeType(s, typeof(T));
            }
        }

        public static bool Try<T>(out T result, Func<T,bool> question)
        {
            ReadResult readLineResult;
            do
            {
                readLineResult = Reader.ReadWhile(out result);
                if (readLineResult == ReadResult.Succesfull && question(result))
                {
                    return true;
                }
                Console.WriteLine("Invalid Value");
            }
            while (readLineResult == ReadResult.Error || !question(result));
            return false; //ReadResult.End
        }
    }
}