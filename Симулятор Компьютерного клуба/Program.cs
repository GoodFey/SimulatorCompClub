

using System;
using System.Collections.Generic;

namespace Симулятор_Компьютерного_клуба
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ComputerClub blueOyster = new ComputerClub(10);
            blueOyster.Work();
        }
    }
    class ComputerClub
    {
        private int _money = 0;
        private List<Computer> _computers = new List<Computer>();
        private Queue<Guest> _guests = new Queue<Guest>();
        Random random = new Random();

        public ComputerClub(int numberOfCopmuters)
        {
            for (int i = 0; i < numberOfCopmuters; i++)
            {
                _computers.Add(new Computer(random));
            }
            CreateNewClients(10);
        }
        public void CreateNewClients(int numberOfGuests)
        {
            for (int j = 0; j < numberOfGuests; j++)
            {
                _guests.Enqueue(new Guest(random, random));
            }
        }
        public void Work()
        {
            while (_guests.Count > 0)
            {
                Guest currentClient = _guests.Dequeue();
                Console.WriteLine($"Баланс компьютерного клуба {_money} руб. Следующий!");
                Console.WriteLine($"\nПоявился новый парень и он хочет посидеть ровно {currentClient.DesiredTime} мин!");
                ShowAllInfo();

                Console.WriteLine("\nВы предлагаете ему компьютер под номером: ");
                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out int currentCompNumber))
                {
                    currentCompNumber -= 1;

                    if (currentCompNumber >= 0 && currentCompNumber < _computers.Count)
                    {
                        if (_computers[currentCompNumber].isTaken)
                        {
                            Console.WriteLine("Этот комп занят.");
                        }
                        else
                        {
                            if (currentClient.CheckSolvency(_computers[currentCompNumber]))
                            {
                                Console.WriteLine("Клиент посчитал бабки и сел за комплютер" + currentCompNumber + 1);
                                _money += currentClient.Pay();
                                _computers[currentCompNumber].BecomeTaken(currentClient);
                            }
                            else
                                Console.WriteLine("У парня не хватает денег!");
                        }
                    }
                    else
                        Console.WriteLine("Реши уже за какой из ИМЕЮЩИХСЯ компьютеров посадить пацана. Кстати он ушел.");
                }
                else
                {
                    CreateNewClients(1);
                    Console.WriteLine("Неверный ввод, попробуйте снова!");
                }


                Console.ReadKey();
                Console.Clear();
                SpendOneMinute();
            }
        }

        private void ShowAllInfo()
        {
            Console.WriteLine("\nСписков всех компутеров: ");
            for (int i = 0; i < _computers.Count; i++)
            {
                Console.Write(i + 1 + " - ");
                _computers[i].ShowInfo();
            }

        }

        private void SpendOneMinute()
        {
            foreach (var item in _computers)
            {
                item.SpendOneMinute();
            }
        }

    }

    class Computer
    {
        public int PricePerMinut { get; private set; }
        private Guest _client;
        public bool isTaken
        {
            get
            {
                return _minutesRemaining > 0;
            }
        }
        private int _minutesRemaining;

        public Computer(Random random)
        {
            PricePerMinut = random.Next(5, 10);
        }

        public void ShowInfo()
        {
            if (isTaken)
                Console.WriteLine($"Компьютер занят. Осталось минут: {_minutesRemaining}");
            else
                Console.WriteLine($"Компьютер свободен. Стоимость за минуту кайфа составляет: {PricePerMinut}");
        }
        public void BecomeTaken(Guest guest)
        {
            _client = guest;
            _minutesRemaining = _client.DesiredTime;
        }
        public void BecomeEmpty()
        {
            _client = null;
        }
        public void SpendOneMinute()
        {
            _minutesRemaining--;
        }
    }

    class Guest
    {
        private int _money;
        private int _needToPay;
        public int DesiredTime { get; private set; }

        public Guest(Random random1, Random random2)
        {
            _money = random1.Next(50, 250);
            DesiredTime = random2.Next(10, 20);
        }

        public bool CheckSolvency(Computer computer)
        {
            _needToPay = DesiredTime * computer.PricePerMinut;
            if (_money >= _needToPay)
            {
                return true;
            }
            else
            {
                _needToPay = 0;
                return false;
            }
        }

        public int Pay()
        {
            _money -= _needToPay;
            return _needToPay;
        }
    }
}
