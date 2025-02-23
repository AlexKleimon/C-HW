namespace Hw3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Person grandfather = new()
            {
                Name = "Petr",
                Surname = "Lapeshi",
                Sex = "Man",
                DateBirth = DateTime.Parse("12-03-1950")
            };
            Person grandmother = new()
            {
                Name = "Anna",
                Surname = "Aksenova",
                Sex = "Woman",
                DateBirth = DateTime.Parse("01-12-1955"),
                Spouse = grandfather
            };
            Person mother = new(name: "Katty", surname: "Lapeshi", sex: "Woman",
                            dateBirth: DateTime.Parse("01-02-1967"), childs: [], Mother: grandmother, Father: grandfather, Spouse: null);
            Person brotherMother = new(name: "Victor", surname: "Lapeshi", sex: "Man",
                            dateBirth: DateTime.Parse("01-02-1967"), childs: [], Mother: grandmother, Father: grandfather, Spouse: null);
            Person father = new(name: "Nikola", surname: "Pitrenko", sex: "Man",
                            dateBirth: DateTime.Parse("04-06-1970"), childs: [], Mother: null, Father: null, Spouse: mother);
            Person sisterFather = new(name: "Jeni", surname: "Pitrenko", sex: "Woman",
                            dateBirth: DateTime.Parse("04-06-1973"), childs: [], Mother: null, Father: null, Spouse: brotherMother);
            Person son = new(name: "Grisha", surname: "Pitrenko", sex: "Man",
                            dateBirth: DateTime.Parse("12-09-1990"), childs: [], Mother: mother, Father: father, Spouse: null);
            Person dought = new(name: "Angelina", surname: "Pitrenko", sex: "Woman",
                            dateBirth: DateTime.Parse("02-06-1993"), childs: [], Mother: mother, Father: father, Spouse: null);
            Person son1 = new(name: "Gena", surname: "Lapeshi", sex: "Man",
                            dateBirth: DateTime.Parse("01-02-1994"), childs: [], Mother: sisterFather, Father: brotherMother, Spouse: null);
            Person dought1 = new(name: "Malvina", surname: "Lapeshi", sex: "Woman",
                            dateBirth: DateTime.Parse("01-02-1999"), childs: [], Mother: sisterFather, Father: brotherMother, Spouse: null);
            Person son2 = new(name: "Petya", surname: "Lapeshi", sex: "Man",
                            dateBirth: DateTime.Parse("01-02-2012"), childs: [], Mother: null, Father: son, Spouse: null);
            grandfather.Spouse = grandmother;
            mother.Spouse = father;
            brotherMother.Spouse = sisterFather;
            grandfather.Childs.Add(mother);
            grandfather.Childs.Add(brotherMother);
            grandmother.Childs.Add(mother);
            grandmother.Childs.Add(brotherMother);
            father.Childs.Add(son);
            father.Childs.Add(dought);
            mother.Childs.Add(son);
            mother.Childs.Add(dought);
            brotherMother.Childs.Add(son1);
            brotherMother.Childs.Add(dought1);
            sisterFather.Childs.Add(son1);
            sisterFather.Childs.Add(dought1);
            son.Childs.Add(son2);
            grandfather.PrintFammily();
        }
    }
}
