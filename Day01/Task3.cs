using Day1;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    public abstract class ShapeBase
    {
        public double dimensionOne;

        public abstract void SetDimensionOne(double value);

    }

    public class Rectangle : ShapeBase
    {
        public double dimensionTwo;

        public override void SetDimensionOne(double value)
        {
            dimensionOne = value;
        }
        public  void SetDimensionTwo(double value)
        {
            dimensionTwo = value;
        }
    }

    public class Square : ShapeBase
    {
        public override void SetDimensionOne(double value)
        {
            dimensionOne = value;
        }
    }

}


