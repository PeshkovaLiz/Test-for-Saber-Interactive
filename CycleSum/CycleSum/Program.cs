using System;
using System.Collections.Generic;


namespace CycleSum
{
    class Program
    {
        static void Main()
        {
            /* в данной программе не учитывается вариант, где необходимо 3 итерации сложения,
             т.к. эти числа выходят за пределы допустимых чисел int */

            int n, m;  //n - циклическая сумма, m - верхняя граница чисел
            int s=0;  //сумма одного конкретного числа (исп. в цикле)
            int k = 0;   //переменная-условие счетчика
            int temp = 0; 

            List<int> numbers = new List<int>();

            Console.WriteLine("Введите N (значение циклической суммы)");
            n = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Введите M (верхняя граница чисел)");
            m = Convert.ToInt32(Console.ReadLine());

            numbers.Add(n);   //добавление первого числа, для которого не требуется сложение цифр

            for (int i=10; i<=m; i++)
            {
                s = 0;
                k = i;

                while(k >= 1)
                {
                    s = s + k % 10;
                    k = k / 10;
                }
                if (s > 9)  //если сумма двузначная, и нужна еще одна итерация
                {
                    temp = s;
                    s = 0;
                    while (temp >= 1)
                    {
                        s = s + temp % 10;
                        temp = temp / 10;
                    }
                }
                if (s==n)
                {
                    numbers.Add(i);  
                }

            }
            foreach (int num in numbers)
                Console.WriteLine(num);
            Console.ReadKey();
        }
    }
}