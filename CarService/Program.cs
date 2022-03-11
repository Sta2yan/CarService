using System;
using System.Collections.Generic;

namespace CarService
{
    class Progam
    {
        static void Main(string[] args)
        {
            CarService carService = new CarService();
            bool isOpen = true;

            while (isOpen)
            {
                Console.WriteLine($"{(int)MenuCommands.ServeCar}. {MenuCommands.ServeCar}" +
                                  $"\n{(int)MenuCommands.Exit}. {MenuCommands.Exit}");
                Console.SetCursorPosition(0, 20);
                Console.WriteLine($"Ваш баланс: {carService.Money}");
                Console.SetCursorPosition(0, 2);

                MenuCommands userInput = (MenuCommands)GetNumber(Console.ReadLine());

                switch (userInput)
                {
                    case MenuCommands.ServeCar:
                        carService.ServeCar(new Car());
                        break;
                    case MenuCommands.Exit:
                        isOpen = false;
                        break;
                }

                Console.ReadKey(true);
                Console.Clear();
            }

            Console.WriteLine("Выход ...");
        }

        public static int GetNumber(string numberText)
        {
            int number;

            while (int.TryParse(numberText, out number) == false)
            {
                Console.WriteLine("Повторите попытку:");
                numberText = Console.ReadLine();
            }

            return number;
        }
    }

    enum MenuCommands
    {
        ServeCar = 1,
        Exit
    }

    enum DetailType
    {
        Wheel,
        Door,
        Headlight,
        Hood,
        Glass
    }

    interface IShowInfo
    {
        public void ShowInfo();
    }

    class CarService : IShowInfo
    {
        private List<StackDetail> _stackDetails = new List<StackDetail>();
        private Random _random = new Random();
        private int _priceWork = 500;
        private int _fine = 500;

        public int Money { get; private set; }

        public CarService(List<StackDetail> stackDetails = null)
        {
            if (stackDetails == null)
                SetDefaultListStackDetails();
        }

        public void ServeCar(Car car)
        {
            Console.Clear();
            car.ShowInfo();
            Console.WriteLine("\nВыберите деталь для починки" +
                              "\n\nВаши детали:");
            ShowInfo();
            Console.WriteLine("\n\nИли введите 0, чтобы отказаться от починки транспорта");
            int index = GetIndex();

            if (index != -1)
            {
                FixCar(car, index);
            }
            else
            {
                PayFine();
            }
        }

        public void ShowInfo()
        {
            for (int i = 0; i < _stackDetails.Count; i++)
            {
                Console.WriteLine(i + 1 + ".");
                _stackDetails[i].ShowInfo();
                Console.WriteLine("-----------------");
            }
        }

        private void FixCar(Car car, int indexDetail)
        {
            int fixMoney = 0;
            
            if (car.TypeBreaking == _stackDetails[indexDetail].Type && _stackDetails[indexDetail].Count > 0)
            {
                Console.Clear();
                fixMoney = _stackDetails[indexDetail].Price + _priceWork;
                Console.WriteLine("Вы выбрали нужную деталь:");
                _stackDetails[indexDetail].ShowInfo();
                Console.WriteLine($"\nОплата за работу - {fixMoney}");
                Console.ReadKey(true);
                Console.WriteLine($"Вы починили машину" +
                              $"\nДоход: +{fixMoney}");
                _stackDetails[indexDetail].PutDetail();
                Money += fixMoney;
            }
            else
            {
                PayFine();
            }
        }

        private void PayFine()
        {
            Console.Clear();
            Console.WriteLine($"Вы не смогли починить машину" +
                              $"\nШтраф за невыполненную работу - {_fine}");
            Money -= _fine;
        }

        private int GetIndex()
        {
            int index = 0;

            do
            {
                index = Progam.GetNumber(Console.ReadLine()) - 1;
            } while (index < -1 || index >= _stackDetails.Count);

            return index;
        }

        private void SetDefaultListStackDetails()
        {
            int maximumDetailType = 5;

            for (int i = 0; i < maximumDetailType; i++)
            {
                _stackDetails.Add(new StackDetail((DetailType)i));
            }
        }
    }

    class Car : IShowInfo
    {
        private Random _random = new Random();
        private int _maximumTypeBreaking = 5;

        public DetailType TypeBreaking { get; private set; }

        public Car()
        {
            TypeBreaking = (DetailType)_random.Next(0, _maximumTypeBreaking);
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Поломка в машине - {TypeBreaking}");
        }
    }

    class StackDetail : IShowInfo
    {
        private Random _random = new Random();
        private List<Detail> _details = new List<Detail>();

        public DetailType Type { get; private set; }
        public int Count => _details.Count;
        public int Price => _details[0].Price;

        public StackDetail(DetailType detailType, List<Detail> details = null)
        {
            Type = detailType;

            if (details == null)
                SetDefaultListDetails();
        }

        public void PutDetail()
        {
            if (Count > 0)
                _details.RemoveAt(0);
        }

        public void ShowInfo()
        {
            if (_details.Count > 0)
            {
                Console.WriteLine($"Кол-во деталей - {_details.Count}");
                _details[0].ShowInfo();
            }
            else
            {
                Console.WriteLine("Детали кончились");
            }
        }

        private void SetDefaultListDetails()
        {
            int maximumPrice = 3000;
            int minimumPrice = 1000;
            int maximumDetail = 17;
            int minimumDetail = 7;

            for (int i = 0; i < _random.Next(minimumDetail, maximumDetail); i++)
            {
                _details.Add(new Detail(Type, _random.Next(minimumPrice, maximumPrice)));
            }
        }
    }

    class Detail : IShowInfo
    {
        public DetailType Type { get; private set; }
        public int Price { get; private set; }

        public Detail(DetailType type, int price)
        {
            Type = type;
            Price = price;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Деталь - {Type} | Цена - {Price}");
        }
    }
}