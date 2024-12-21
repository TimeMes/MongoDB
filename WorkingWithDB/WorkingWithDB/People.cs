using MongoDB.Bson.Serialization.Attributes;

namespace WorkingWithDB
{
    [BsonIgnoreExtraElements]
    public class People:IPeople
    {
        public string Name {  get; set; }
        public int Age { get; set; }
        public int Balance { get; set; }
        public People() 
        {
            Name = "";
            Age = 0;
            Balance = 0;
        }
        public override string ToString() =>
            $"{Name} is {Age} years old and has {Balance}$";
    }

}
