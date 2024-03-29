﻿using System;
using System.Collections.Generic;
using System.Windows;

namespace AnalyzerControlGUI.ViewsHelpers
{
    public static class GraphMath
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="beginAngleDeg"></param>
        /// <param name="endAngleDeg"></param>
        /// <param name="radius"></param>
        /// <param name="stepLen"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static List<Point> CalcArcPoint(
            double x, 
            double y, 
            double beginAngleDeg, 
            double endAngleDeg, 
            double radius,
            double stepLen, 
            double scale)
        {
            var arc = new List<Point>();
            double x_offset = 75;
            double y_offset = -35;

            double beginAngleRad = beginAngleDeg * Math.PI / 180;
            double endAngleRad = endAngleDeg * Math.PI / 180;
            double deltaAngle = stepLen / radius;

            for (var angle = beginAngleRad; angle < endAngleRad; angle += deltaAngle)
            {
                var point = new Point
                {
                    X = (x + x_offset + Math.Cos(angle) * radius) * scale,
                    Y = (y + y_offset + Math.Sin(angle) * radius) * scale
                };
                arc.Add(point);
            }
            return arc;
        }

        public static List<Point> CalcVerticalLinePoint(
            Point begin,
            Point end,
            double stepLen)
        {
            var line = new List<Point>();

            /*
            if (Math.Abs(begin.X - end.X) < stepLen)
            {
                throw new ArgumentException("Points ain't placed on vectical line");
            }
            */

            if (begin.Y < end.Y)
            {
                for (var y = begin.Y; y <= end.Y; y+=stepLen)
                {
                    var point = new Point();
                    point.X = begin.X;
                    point.Y = y;
                    line.Add(point);
                }
            } else if (begin.Y > end.Y)
            {
                for (var y = begin.Y; y >= end.Y; y -= stepLen)
                {
                    var point = new Point
                    {
                        X = begin.X,
                        Y = y
                    };
                    line.Add(point);
                }
            }
            if (line[line.Count - 1] != end)
            {
                line.Add(end);
            }

            return line;
        }

        public static double PointLenth(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow((point2.X - point1.X), 2) + Math.Pow((point2.Y - point1.Y), 2));
        }
    }
}
