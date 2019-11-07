using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSerialize
{
    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }

    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            List<ListNode> arr = new List<ListNode>();
            ListNode temp = new ListNode();
            temp = Head;

            //запись списка в List<>
            do
            {
                arr.Add(temp);
                temp = temp.Next;
            } while (temp != null);

            //запись в файл
            //запись ведется с разделителем ":", слева данные, справа индексы случайных элементов
            using (StreamWriter w = new StreamWriter(s))
                foreach (ListNode n in arr)
                    w.WriteLine(n.Data.ToString() + ":" + arr.IndexOf(n.Rand).ToString());
        }

        public void Deserialize(FileStream s)
        {
            List<ListNode> arr = new List<ListNode>();
            ListNode temp = new ListNode();
            Count = 0;
            Head = temp;
            string line;

            //чтение файла и запись элементов в List<>
            try
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Equals("")==false)
                        {
                            Count++;
                            temp.Data = line;
                            ListNode next = new ListNode();
                            temp.Next = next;
                            arr.Add(temp);
                            next.Prev = temp;
                            temp = next;
                        }
                    }
                }

                //"хвост" списка
                Tail = temp.Prev;
                Tail.Next = null;

                //запись восстановленных элементов в список с ссылками на случайный элемент
                foreach (ListNode n in arr)
                {
                    n.Rand = arr[Convert.ToInt32(n.Data.Split(':')[1])];
                    n.Data = n.Data.Split(':')[0];
                }
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press Enter to exit.");
                Console.Read();
                Environment.Exit(0);
            }
        }
    }

    class Program
    {
        static Random rand = new Random();

        //для создания следующего элемента списка
        static ListNode AddNode(ListNode prev)
        {
            ListNode result = new ListNode
            {
                Prev = prev,
                Next = null,
                Data = rand.Next(0, 100).ToString()
            };
            prev.Next = result;
            return result;
        }
        //для генерации ссылок на случайные элементы
        static ListNode RandomNode(ListNode _head, int _length)
        {
            int k = rand.Next(0, _length);
            int i = 0;
            ListNode result = _head;
            while (i < k)
            {
                result = result.Next;
                i++;
            }
            return result;
        }

        static void Main(string[] args)
        {
            //кол-во элементов в списке
            int length = 20;

            //первый элемент
            ListNode head = new ListNode();
            ListNode tail = new ListNode();
            ListNode temp = new ListNode();
            temp = head;
            head.Data = rand.Next(0, 1000).ToString(); //записывается случайное число в "голову" списка

            tail = head;

            for (int i = 1; i < length; i++)
            { tail = AddNode(tail); }
 
            //создание ссылки на случайный элемент
            for (int i = 0; i < length; i++)
            {
                temp.Rand = RandomNode(head, length);
                temp = temp.Next;
            }

            ListRand serializeList = new ListRand
            {
                Head = head,
                Tail = tail,
                Count = length
            };

            // получаем поток, куда будем записывать сериализованные данные
            FileStream fs = new FileStream("file.dat", FileMode.OpenOrCreate);
            serializeList.Serialize(fs);   //сериализация

            //десериализация в List<>
            ListRand deserializeList = new ListRand();
            try
            {
                fs = new FileStream("file.dat", FileMode.Open);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Press Enter to exit.");
                Console.Read();
                Environment.Exit(0);
            }
            deserializeList.Deserialize(fs);

            //проверка, корректности сериализации и десериализации
            //для этого сравниваем "хвосты" списков до сериализации и после десериализации
            if (deserializeList.Tail.Data == serializeList.Tail.Data) Console.WriteLine("Данные сериализованы и десериализованы успешно");
            Console.Read();
        }
    }
}
