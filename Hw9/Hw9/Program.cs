using System.Reflection;
using System.Text;

namespace Hw9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? s = ObjectToString(new TestClass(32, "string", 0.5m, ['U', 't', 'i', 'n']));
            Console.WriteLine(s);
            object? o = StringToObject(s);
            Console.WriteLine(o.GetType());
        }
        static string ObjectToString(object obj)
        {
            StringBuilder sb = new();
            Type type = obj.GetType();
            sb.Append(type.Assembly.FullName + ":");
            sb.Append(type.Name + "|");
            List<PropertyInfo> properties = [.. type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)];
            properties.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            foreach (var prop in properties)
            {
                Console.WriteLine(prop.Name);
                var value = prop.GetValue(obj);
                sb.Append((prop.Name) + ":");
                if (prop.PropertyType == typeof(char[]))
                {
                    sb.Append(new String(value as char[]) + "|");
                }
                else
                    sb.Append(value + "|");
            }
            return sb.ToString();
        }

        static object StringToObject(string s)
        {
            List<string> values = [.. s.Split("|")];
            values.Remove("");
            string[] classAssemblyAndName = values[0].Split(':');

            object? obj = Activator.CreateInstance(classAssemblyAndName[0], "Hw9.TestClass")?.Unwrap();

            if (values.Count > 1 && obj is not null)
            {
                var type = obj.GetType();
                Console.WriteLine(type);
                for (int i = 1; i < values.Count; i++)
                {
                    var nameAndValue = values[i].Split(':');
                    var pi = type.GetProperty(nameAndValue[0]);
                    Console.WriteLine($"{nameAndValue[0]}:{nameAndValue[1]}");
                    if (pi == null)
                        continue;
                    if (pi.PropertyType == typeof(int))
                    {
                        pi.SetValue(obj, int.Parse(nameAndValue[1]));
                    }
                    if (pi.PropertyType == typeof(string))
                    {
                        pi.SetValue(obj, nameAndValue[1]);
                    }
                    if (pi.PropertyType == typeof(decimal))
                    {
                        pi.SetValue(obj, decimal.Parse(nameAndValue[1]));
                    }
                    if (pi.PropertyType == typeof(char[]))
                    {
                        pi.SetValue(obj, nameAndValue[1].ToCharArray());
                    }
                }
            }
            return obj;
        }
    }
}
