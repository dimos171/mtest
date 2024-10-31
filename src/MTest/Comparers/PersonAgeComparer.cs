public class PersonAgeComparer : Comparer<Person>
{
    public override int Compare(Person x, Person y)
    {
        return x.Age.CompareTo(y.Age);
    }
}