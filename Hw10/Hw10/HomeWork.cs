using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw10
{
    class HomeWork
    {
        public static bool FindWord(string path, string word)
        {
            using (StreamReader sr = new(path))
            {
                while (!sr.EndOfStream)
                {
                    string? tempStr = sr.ReadLine();
                    if (tempStr != null)
                    {
                        if (tempStr.Contains(word))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

        }
        public static void FindFiles(string path, string fileExtension, List<string> listPathFiles)
        {
            listPathFiles.AddRange(Directory.GetFiles(path, fileExtension));

            foreach (var dir in Directory.GetDirectories(path))
            {
                FindFiles(dir, fileExtension, listPathFiles);
            }
        }
    }
}
