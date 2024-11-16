using static System.Console;
using static System.Math;

namespace Local
{
    class Program
    {
        class Box_office
        {
            // класс, описывающий одну кассу
            public long quan;  // количество клиентов, обслуженных на кассе
            public double tim_rel;  // время, когда касса освободиться
            public double speed_ser;  // скорость обслуживания 1 клиента
        
            public Box_office() 
            {
                quan = 0;
                tim_rel = 0;
                speed_ser = S;
            }
        }

        /* сделать 0 конфигурацию, от которой мы отталкивались и которую мы оптимизируем
         * 
         * 1 конфигурация. Обычный день в магазине
         * А = 0.5
         * N = 2
         * S = 1,2 (для всех касс одинаковая)
         * 
         * 2 конфигурация. Правильное распределение кассиров
         * А = 0.5
         * N = 3
         * S1 = 0.7
         * S2 = 1
         * S3 = 1.3
         * 
         * 3 конфигурация. Новый год
         * А = 2
         * N = 4
         * S1 = 0.8
         * S2 = 1.1
         * S3 = 1.4
         * S4 = 1.7
         */

        static double A = 100;  // плотность потока клиентов (клиентов/минуту)
        static int N = 2;  // количество касс в магазине
        static double S = 1;  // время обслуживая 1 клиента (минуты)
        static double T = 1440;  // время работы магазина за день (минуты)
        static Box_office[] arr = new Box_office[N + 1];  // массив информации о кассах
        static int replay = 5000;  // количество повторов 1 дня
        static double time = 0;  // текущее время

        // поля для случайной величины
        static Random rnd = new Random();
        static int степень = 8;  // сколько знаков, после запятой будет
        static int support_1 = (int)Pow(10, степень) + 1;
        static double support_2 = Pow(10, степень);
        static double support_3 = -1/A;

        public static double Rand_var()
        {
            // случайная величина с экспоненциальный законом распределения 
            return support_3 * Log(rnd.Next(0, support_1) / support_2);
        }

        static void Main()
        {
            bool info = false;  // пошаговый просмотр дня для отладки

            for (int k = 0; k < arr.Length; k++)
                arr[k] = new Box_office();

            //arr[0].speed_ser = 0.7;
            //arr[1].speed_ser = 1;
            //arr[2].speed_ser = 1.3;


            //arr[0].speed_ser = 0.8;
            //arr[1].speed_ser = 1.1;
            //arr[2].speed_ser = 1.4;
            //arr[3].speed_ser = 1.7;

            arr[N].speed_ser = 0;  // последняя касса обслуживает мгновенно. 

            for (int i = 0; i < replay; i++)
            {
                time += Rand_var();  // первый клиент клиент
                while (time < T)
                {
                    if (info)
                        WriteLine($"Время, когда пришёл следующий клиент {time}");
                    for (int j = 0; j < arr.Length; j++)
                    {
                        if (arr[j].tim_rel <= time)  // нашли свободную кассу
                        {
                            arr[j].tim_rel = time + arr[j].speed_ser;
                            arr[j].quan++;
                            j += arr.Length;
                        }
                    }
                    time += Rand_var();  // cледующий клиент
                    if (info)
                    {
                        for (int j = 0; j < arr.Length; j++)
                            Write($"{arr[j].quan} - {arr[j].tim_rel}   ");
                        WriteLine();
                        ReadKey();
                    }

                }
                time = 0;
                for (int j = 0; j < arr.Length; j++)
                    arr[j].tim_rel = 0;
            }
            double summa = 0;
            for (int j = 0; j < arr.Length; j++)
            {
                Write($"{(double)arr[j].quan / replay, 11} ");
                summa += arr[j].quan;
            }
            Write("  сред. кол. клиентов, обслуж. кассами\n");
            for (int j = 0; j < arr.Length; j++)
                Write($"{Round((arr[j].quan/summa)*100, 3), 11} ");
            Write("  %\n");
        }
    }
}