using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw4
{
    public class Bits : IBitNumber
    {
        public long Value { get; private set; }
        private byte SizeOfValueBit { get; set; }

        public Bits(long Value)
        {
            this.Value = Value;
            SizeOfValueBit = sizeof(long) * 8;

        }

        public Bits(int Value)
        {
            this.Value = Value;
            SizeOfValueBit = sizeof(int) * 8;
        }

        public Bits(byte Value)
        {
            this.Value = Value;
            SizeOfValueBit = sizeof(byte) * 8;

        }
        public static implicit operator long(Bits b) => b.Value;
        public static implicit operator Bits(int b) => new(b);
        public static implicit operator Bits(byte b) => new(b);
        public static implicit operator Bits(long b) => new(b);
        public bool GetBit(int index)
        {
            if (index > SizeOfValueBit - 1 || index < 0)
                throw new Exception($"Значение бита должно быть от 0 до {SizeOfValueBit - 1}");
            return ((Value >> index) & 1) == 1;
        }

        public void SetBit(int index, bool bit)
        {
            if (index > SizeOfValueBit - 1 || index < 0) throw new Exception($"Значение бита должно быть от 0 до {SizeOfValueBit - 1}");
            if (bit) Value |= 1 << index;
            else
            {
                long mask = 1 << index;
                mask = 0xfffffffffffffff ^ mask;
                Value &= Value & mask;
            }
        }

        public void PrintBitsNumber()
        {
            string text = string.Empty;
            for (int i = SizeOfValueBit - 1; i >= 0; i--)
            {
                text += GetBit(i) ? 1 : 0;
            }
            Console.WriteLine(text);
        }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
