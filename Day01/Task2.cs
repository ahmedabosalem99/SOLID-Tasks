using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    public interface IAreaCalculable
    {
        double Area();
    }

    public interface IVolumeCalculable : IAreaCalculable
    {
        double Volume();
    }

    public class Rectangle : IAreaCalculable
    {
        public Rectangle()
        {
        }

        public double Height { get; set; }
        public double Width { get; set; }

        public double Area() => Height * Width;
    }

    public class Circle : IAreaCalculable
    {
        public double Radius { get; set; }

        public double Area() => Radius* Radius * Math.PI;

    }

    public class AreaCalculator
    {

        public double TotalArea(IEnumerable<IAreaCalculable> shapes) => shapes.Sum(s => s.Area());
        
    }

    public class Square : IAreaCalculable
    {
        public double SideLength { get;  set; }

        public double Area() => SideLength * SideLength;

    }

    public class Triangle : IAreaCalculable
    {
        public double Base { get; set; }
        public double Height { get; set; }

        public double Area() => (Base * Height) / 2;
    }

    public class Cube : IVolumeCalculable
    {
        public double SideLength { get; set; }

        public double Area() => 6 * SideLength * SideLength;
        public double Volume() => SideLength * SideLength * SideLength;
    }

}
