using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormPlane01
{
    class PossiblePlane // Класс возможной плоскости
    {
        // Хранит значения суммы отклонений всех точек от плоскости, а также значения коэффициентов плоскости
        public double sumP = 0.0, A = 0.0, B = 0.0, C = 0.0, D = 0.0;
        public PossiblePlane(double sum, double a, double b, double c, double d)
        {
            sumP = sum;
            A = a;
            B = b;
            C = c;
            D = d;
        }
    }
}
