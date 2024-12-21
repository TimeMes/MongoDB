using WorkingWithDatabase;
using WorkingWithDB;
bool whiletrue = true;
IRepository<People> repository = new Repository<People>();
IPeopleService peopleService = new PeopleService(repository);


Console.WriteLine("Welcome to DB");
Console.WriteLine("Enter one of options - search/new/all/rand/count");
while (whiletrue)
{
    if (Reader.Try(out string? switcher))
    {
        switch (switcher)
        {
            case "new":
                peopleService.CreateNewPeople();
                break;
            case "search":
                peopleService.Search();
                break;
            case "all":
                peopleService.WriteAllPeopleAsync();
                break;
            case "edit":
                peopleService.PeopleEdit();
                break;
            case "rand":
                peopleService.CreateNewRandomPeople();
                break;
            case "count":
                Console.WriteLine(peopleService.PeopleCount);
                break;
            default:
                Console.Clear();
                Console.WriteLine("Welcome to DB");
                Console.WriteLine("Enter one of options - search/new/nameSearch/all/rand/count/age");
                break;
        }
    }
}