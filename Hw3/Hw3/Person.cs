using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw3
{
    public class Person
    {
        public Person? Mother { get; set; }
        public Person? Father { get; set; }
        public List<Person> Childs { get; set; }
        public string Name { get; set; } = "no name";
        public string Surname { get; set; } = "no surname";
        public string Sex { get; set; } = "no sex";
        public DateTime DateBirth { get; set; } = DateTime.Now;
        public Person? Spouse { get; set; }
        public Person()
        {
            Mother = null;
            Father = null;
            Childs = [];
            Spouse = null;
        }

        public Person(string name, string surname, string sex, DateTime dateBirth, List<Person> childs, Person? Mother, Person? Father, Person? Spouse)
        {
            Name = name;
            Surname = surname;
            Sex = sex;
            DateBirth = dateBirth;
            Childs = childs;
            this.Mother = Mother;
            this.Father = Father;
            this.Spouse = Spouse;
        }
        public void PrintChilds()
        {
            foreach (var item in Childs)
            {
                if (item.Sex.Equals("Woman"))
                    Console.WriteLine($"Dought: {item}");
                else Console.WriteLine($"Son: {item}");
            }
        }
        public void PrintParents()
        {
            Console.WriteLine($"Father: {Father}\nMather: {Mother}");
        }
        public void PrintGrandfathers()
        {
            Console.WriteLine($"Grandfathers: {Father?.Father}\n{Mother?.Father}");
        }
        public void PrintGrandmothers()
        {
            Console.WriteLine($"Grandmothers: {Father?.Mother}\n{Mother?.Mother}");
        }

        public void PrintFammily(int indent = 5)
        {

            Console.WriteLine($"{new string('=', indent)} {"|"}{ToString()}");
            foreach (var item in Childs)
            {
                item.PrintFammily(indent + 15);
            }
        }
        public string GetSpouse()
        {
            string text;
            if (Spouse is null)
                text = "no spouse";
            else text = $"{Spouse?.Name} {Spouse?.Surname}";
            return text;
        }
        public override string ToString()
        {
            string text = GetSpouse();
            return $"{Name} {Surname}, sex: {Sex}, date of birth: {DateBirth}, spouse: {text}";
        }
    }
}
