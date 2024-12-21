using MongoDB.Bson;
using MongoDB.Driver;


namespace WorkingWithDB
{
    public class PeopleService(IRepository<People> repository) : IPeopleService
    {
        private IRepository<People> peopleRepository = repository;
        public int PeopleCount { get => peopleRepository.ReadAll().Result.Count; }



        public async void WriteAllPeopleAsync()
        {
            var cursor = await peopleRepository.ReadAll();
            var peopleList = cursor.ToList();
            foreach (var people in peopleList)
            {
                Console.WriteLine($"{people.Name} is {people.Age} years old and has {people.Balance}$");
            }
        }

        public async void WritePeopleAsync(FilterDefinition<People> filter)
        {
            if (filter == null) return;
            var peopleList = await peopleRepository.ReadByFilter(filter);
            if (peopleList.Count == 0)
            {
                Console.WriteLine("There is no result by this filter");
            }
            foreach (var people in peopleList)
            {
                Console.WriteLine(people.ToString());
            }
        }


        public bool CreateNewPeople()
        {
            People human = new People();

            Console.Write("Name: ");
            bool Try = Reader.Try(out string? nameResult);
            if (!Try) return false;

            Console.Write("Age: ");
            Try = Reader.Try(out int ageResult, x => x > 0 && x < 120);
            if (!Try) return false;

            Console.Write("Balance: ");
            Try = Reader.Try(out int balanceResult);
            if (!Try) return false;

            human.Name = nameResult;
            human.Age = ageResult;
            human.Balance = balanceResult;

            Console.WriteLine($"Save: {human.Name} {human.Age} years old with {human.Balance}$");
            peopleRepository.SaveInDB(human);
            return true;
        }


        public void CreateNewRandomPeople()
        {
            int numberOfPeople = 1;
            Console.Write("Number of people: ");
            if (Reader.Try(out int result, x => x > 0))
            {
                numberOfPeople = result;
            }
            List<People> peopleList = new List<People>();
            for (int p = numberOfPeople; p > 0; p--)
            {
                Random random = new Random();
                var chars = "abcdefghijklmnopqrstuvwxyz";
                var name = chars[random.Next(chars.Length)].ToString().ToUpper();
                for (int j = 0; j < 4; j++)
                {
                    name += chars[random.Next(chars.Length)];
                }
                var age = random.Next(1, 100);
                var balance = random.Next(100, 1000);
                People human = new People() { Name = name, Age = age, Balance = balance };
                peopleList.Add(human);
            }
            peopleRepository.SaveInDB(peopleList.ToArray());
            Console.WriteLine("Randoming is over");
        }

        public void PeopleEdit()
        {
            var peopleList = peopleRepository.ReadByFilter(CreateFilterByType()).Result;
            if (peopleList.Count > 0)
            {
                var human = peopleList.First();
                Console.WriteLine($"{human.Name} is {human.Age} years old and has {human.Balance}$");
                Console.Write("Edit Name||Age||Balance: ");
                string[] peopleKeys = { "Name", "Age", "Balance" };
                if (Reader.Try(out string? key, line => peopleKeys.Any(peopleKey => peopleKey == line)))
                {
                    Console.Write($"Editing {key}:");
                    if (Reader.Try(out BsonValue? value))
                    {
                        BsonElement bsonElement = new(key, value);
                        peopleRepository.ReplaceOne(human.ToBsonDocument(), bsonElement);
                    }
                }
            }

        }


        public void Search()
        {
            var filter = CreateFilterByType();
            WritePeopleAsync(filter);
        }

        public FilterDefinition<People> CreateFilterByType()
        {
            Console.Write("Search by Name/Age/Balance: ");
            string[] peopleKeys = { "Name", "Age", "Balance" };
            Reader.Try(out string key, line => peopleKeys.Any(peopleKey => string.Equals(peopleKey, line, StringComparison.OrdinalIgnoreCase)));
            switch (key.ToLower())
            {
                case "name":
                    Console.Write("Search by FullName(full or skip), or by NameContain(contain)? ");
                    Reader.TryWhile(out string searchType);
                    searchType = searchType.ToLower();
                    Console.Write("Searching by {0}", searchType == "contain" ? "NameContain: " : "FullName: ");
                    Reader.TryWhile(out string strResult);
                    if (searchType == "contain")
                    {
                        var filter = peopleRepository.FilterBuilder(human => human.Name.Contains(strResult));
                        return filter;
                    }
                    return new BsonDocument("Name", strResult);
                case "age":
                    return intKey("Age");
                case "balance":
                    return intKey("Balance");
                default:
                    return new BsonDocument(key, -1);
            }

            static BsonDocument intKey(string key)
            {
                BsonValue value = 0;
                Console.Write($"Search by {key}(skip), or by {key}Compare(compare or 2)? ");
                Reader.TryWhile(out string searchType);
                searchType = searchType.ToLower();
                Console.Write("Searching by {0}", searchType == "compare" ? "{key}Compare: " : "{key}: ");
                if (searchType == "compare" || searchType == "2")
                {
                    Reader.TryWhile(out Compare compareResult);
                    value = compareResult.ToBsonDocument();
                }
                else
                {
                    Reader.TryWhile(out int intResult);
                    value = intResult;
                }
                return new BsonDocument(key, value);
            }

        }
    }
}