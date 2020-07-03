using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormPlane01
{
    class mathPlanes
    {
        // Функция для генерации случайных значений 
        static public string generateCoordinates()
        {
            Random rand = new Random();
            double p = rand.Next(1, 51);
            p /= 100; // Случайный выбор значения числа p
            int randSize = rand.Next(1, 5);
            int N = 0;
            switch (randSize) // Случайный выбор значения числа N
            {
                case 1: N = rand.Next(3, 101); break;
                case 2: N = rand.Next(3, 501); break;
                case 3: N = rand.Next(3, 1001); break;
                case 4: N = rand.Next(3, 25001); break;
            }
            //int N = rand.Next(3, 400);
            double[] x0 = new double[3]; // Массивы для координат первых трех точек
            double[] y0 = new double[3];
            double[] z0 = new double[3];
            int[] x = new int[N]; // Массивы для координат всех точек
            int[] y = new int[N];
            int[] z = new int[N];
            string s = Convert.ToString(p) + "\n" + Convert.ToString(N) + "\n"; // Строка ввода

            for (int i = 0; i < 3; i++) // Цикл для получения случайных координат первых трех точек
            {
                x0[i] = rand.Next(-100, 101); // Случайный выбор из промежутка от -100 до 100 для координаты х
                x[i] = (int)x0[i]; // Добавляем этой координаты в массив со всеми точками
                y0[i] = rand.Next(-100, 101);
                y[i] = (int)y0[i];
                z0[i] = rand.Next(-100, 101);
                z[i] = (int)z0[i];
                s += Convert.ToString(x[i]) + "\t" + Convert.ToString(y[i]) + "\t" + Convert.ToString(z[i]) + "\n"; // Добавляем этих координат в строку ввода
            }

            var plane = Plane(x0, y0, z0); // Нахождение уравнения плоскости (коэффициентов A, B, C, D), проходящей через первые три точки

            for (int i = 3; i < N; i++) // Цикл для нахождения координат оставшихся точек
            {
                x[i] = rand.Next(-100, 101);
                int x1 = x[i];
                y[i] = rand.Next(-100, 101);
                int y1 = y[i];
                z[i] = rand.Next(-100, 101);
                int z1 = z[i];
                double distance = distanceToPlane(x[i], y[i], z[i], plane); // Нахождение расстояния от точки до плоскости
                if ((double)i <= (double)N / 1.25) // Для 80% случаев подбираем приемлемое расстояние (р)
                {
                    if (distance <= p) // Случай, когда точка принадлежит нашей плоскости
                    {
                        s += Convert.ToString(x[i]) + "\t" + Convert.ToString(y[i]) + "\t" + Convert.ToString(z[i]) + "\n";
                        continue;
                    }
                    else // Математические алгоритмы для нахождения одной из координат точки для ее принадлежности плоскости
                    {
                        double p0 = rand.Next(1, (int)(p * 100) + 1); // Выбор случайного расстояния со значениями от 0 до р
                        p0 /= 100;
                        // Нахождение z при неизменных x и y
                        z[i] = (int)Math.Round(((p0 * Math.Sqrt(plane[0] * plane[0] + plane[1] * plane[1] + plane[2] * plane[2])) - plane[0] * x[i] - plane[1] * y[i] - plane[3]) / plane[2]);
                        if (z[i] < -100 || z[i] > 100)
                        {
                            // Аналогичная операция при другом раскрытии модульных скобок
                            z[i] = (int)Math.Round((plane[0] * x[i] + plane[1] * y[i] + plane[3] - (p * Math.Sqrt(plane[0] * plane[0] + plane[1] * plane[1] + plane[2] * plane[2]))) / plane[2]);
                            if (z[i] < -100 || z[i] > 100)
                            {
                                z[i] = z1;
                                // Нахождение y при неизменных x и z
                                y[i] = (int)Math.Round(((p0 * Math.Sqrt(plane[0] * plane[0] + plane[1] * plane[1] + plane[2] * plane[2])) - plane[0] * x[i] - plane[2] * z[i] - plane[3]) / plane[1]);
                                if (y[i] < -100 || y[i] > 100)
                                {
                                    // Аналогичная операция при другом раскрытии модульных скобок
                                    y[i] = (int)Math.Round((plane[0] * x[i] + plane[2] * z[i] + plane[3] - (p * Math.Sqrt(plane[0] * plane[0] + plane[1] * plane[1] + plane[2] * plane[2]))) / plane[1]);
                                    if (y[i] < -100 || y[i] > 100)
                                    {
                                        y[i] = y1;
                                        // Нахождение x при неизменных y и z
                                        x[i] = (int)Math.Round(((p0 * Math.Sqrt(plane[0] * plane[0] + plane[1] * plane[1] + plane[2] * plane[2])) - plane[1] * y[i] - plane[2] * z[i] - plane[3]) / plane[0]);
                                        if (x[i] < -100 || x[i] > 100)
                                        {
                                            // Аналогичная операция при другом раскрытии модульных скобок
                                            x[i] = (int)Math.Round((plane[1] * y[i] + plane[2] * z[i] + plane[3] - (p * Math.Sqrt(plane[0] * plane[0] + plane[1] * plane[1] + plane[2] * plane[2]))) / plane[0]);
                                            if (x[i] < -100 || x[i] > 100)
                                            {
                                                // Если не удается найти приемлемую координату, то ставим пороговое значение x (то есть, точка не принадлежит нашей плоскости)
                                                if (x[i] < -100)
                                                    x[i] = -100;
                                                else if (x[i] > 100)
                                                    x[i] = 100;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                s += Convert.ToString(x[i]) + "\t" + Convert.ToString(y[i]) + "\t" + Convert.ToString(z[i]) + "\n"; // Добавляем данные координаты на ввод
            }
            return s;
        }
        // Функция вычисления правильной плоскости, которой принадлежит большинство точек
        static public double[] CalculateMainPlane(double p, double[] x, double[] y, double[] z)
        {
            double[] answer = new double[4]; // Массив, хранящий ответ
            List<PossiblePlane> planes = new List<PossiblePlane>(); // Список объектов класса возможных плоскостей
        Again_calcul:
            for (int i = 0; i < x.Length; i++)
            {
                double[] xLocal = new double[3]; // Создаём локальные массивы для хранения 3-х случайных точек
                double[] yLocal = new double[3];
                double[] zLocal = new double[3];
                List<int> prevIndex = new List<int>(); // Список для хранения предыдущих индексов

                Random rnd = new Random();
                for (int j = 0; j < 3; j++) // Цикл для выбора случайных точек
                {
                Again: // Метка перехода
                    int index = rnd.Next(0, x.Length); // Выбираем случайно точку из всех возможных
                    for (int k = 0; k < prevIndex.Count; k++)
                        if (index == prevIndex[k]) // Проверяем, не была ли эта точка уже выбрана
                            goto Again; // Если была выбрана, то снова выбираем
                    xLocal[j] = x[index]; // Заносим эту точку в локальные массивы
                    yLocal[j] = y[index];
                    zLocal[j] = z[index];
                    prevIndex.Add(index); // Заносим индекс в список предыдущих индексов
                }
                var plane = Plane(xLocal, yLocal, zLocal); // Находим уравнение плоскости по этим случайным 3-м точкам
                int countPointsOnThePlane = 0; // Количество точек удовлетворяющих данной плоскости
                double sumP = 0.0; // Сумма расстояний до точек от этой плоскости
                for (int j = 0; j < x.Length; j++) // Находим расстояние до каждой точки от нашей плоскости
                {
                    double distance = distanceToPlane(x[j], y[j], z[j], plane); // Находим расстояние
                    if (distance <= p) // Если это расстояние удовлетворяет условию плоскости
                        countPointsOnThePlane++; // То увеличиваем количество удовлетворяющих точек
                    sumP += distance; // И прибавляем это расстояние к сумме расстояний
                }
                if ((double)countPointsOnThePlane >= (double)(x.Length / 2.0)) // Если количество точек, принадлежащих полу больше половины
                    planes.Add(new PossiblePlane(sumP, plane[0], plane[1], plane[2], plane[3])); // То создаём возможную плоскость
            }
            if (planes.Count > 0) // Если имеются подходящие плоскости, то выберем наиболее подходящую по минимальной сумме расстояний
            {
                double[] sumArray = new double[planes.Count]; // вспомогательный массив для записи сумм расстояний у каждой плоскости
                for (int i = 0; i < planes.Count; i++)
                    sumArray[i] = planes[i].sumP; // Заносим все суммы в вспомогательный массив
                Array.Sort(sumArray); // Сортируем его
                double minSum = sumArray[0]; // Минимальная сумма = в начале отсортированного массива
                for (int i = 0; i < planes.Count; i++) // Перебираем все возможные плоскости
                {
                    if (minSum == planes[i].sumP) // Если сумма данной плоскости минимальна
                    {
                        // То выводим коэффициенты плоскости
                        answer[0] = planes[i].A;
                        answer[1] = planes[i].B;
                        answer[2] = planes[i].C;
                        answer[3] = planes[i].D;
                        break; // И выходим их цикла
                    }
                }
            }
            else // Если же возможных плоскостей не нашлось(такое может быть из-за ошибок рандома - выбирались одинаковые сеты 3-х точек),
                goto Again_calcul; // То произведём вычисления заново
            return answer;
        }
        // Функция нахождения расстояния до плоскости
        // На вход получает x,y,z точки и значения коэффициентов плоскости
        static public double distanceToPlane(double x, double y, double z, double[] plane)
        {
            double distance = Math.Abs(plane[0] * x + plane[1] * y + plane[2] * z + plane[3]) / Math.Sqrt(Math.Pow(plane[0], 2) + Math.Pow(plane[1], 2) + Math.Pow(plane[2], 2));
            return distance;
        }
        // Функция определения коэффициентов уравнения плоскости
        // Также приводит коэффициенты к нормальному виду
        static public double[] Plane(double[] x, double[] y, double[] z)
        {
            double[] plane = new double[4]; // Массив, в который записываются значения коэффициентов
            plane[0] = ((y[1] - y[0]) * (z[2] - z[0]) - (z[1] - z[0]) * (y[2] - y[0])); // находим коэф A
            plane[1] = (-1.0) * ((x[1] - x[0]) * (z[2] - z[0]) - (z[1] - z[0]) * (x[2] - x[0])); // B
            plane[2] = ((x[1] - x[0]) * (y[2] - y[0]) - (y[1] - y[0]) * (x[2] - x[0])); // C
            plane[3] = (-x[0] * plane[0] - y[0] * plane[1] - z[0] * plane[2]); // D
            // Для приведения к нормальному виду поделим все элементы массива на минимальное по модулю значение (но не 0)
            double max = 0.0;
            double[] helpArr = new double[4]; // Вспомогательный массив
            for (int i = 0; i < 4; i++)
            {
                helpArr[i] = Math.Abs(plane[i]); // Заносим в массив модули всех элементов
            }
            Array.Sort(helpArr); // Сортируем вспомогательный массив
            for (int i = 3; i >= 0; i--)
            {
                if (helpArr[i] != 0)
                {
                    max = helpArr[i]; // Находим максимальный элемент, неравный 0
                    break;
                }
            }
            for (int i = 0; i < plane.Length; i++)
            {
                plane[i] /= max; // Делим все элементы на максимальное значение по модулю
            }
            return plane; // Выводим массив
        }
    }
}
