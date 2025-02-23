namespace Hw4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bits b = new(3);
            Console.WriteLine(b);
            b.PrintBitsNumber();
            b.SetBit(2, true);
            b.SetBit(7, true);
            b.PrintBitsNumber();
            int operand1 = (int)b;
            Bits operand2 = operand1 - 100;
            Console.WriteLine($"operand1 = {operand1}");
            Console.WriteLine($"-100 = operand2 ({operand2}) - operand1 ({operand1})");
        }
    }
}
