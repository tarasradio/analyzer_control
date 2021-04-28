using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnalyzerControlGUI.ViewsHelpers
{
    public class ConveyorHelper
    {
        public double TubeDiameter { set; get; }

        private List<Point> _coords;
        private List<int> _tubesCoordsIndexes;
        private Canvas _canvas;
        private double _pathResolution;
        private double _scale;
        private uint _tubesNum;
        private int offset = 1;

        public ConveyorHelper(Canvas canvas, double pathResolution, double scale, uint tubesNum)
        {
            _coords = new List<Point>();
            _tubesCoordsIndexes = new List<int>();
            _canvas = canvas;
            _pathResolution = pathResolution;
            _scale = scale;
            _tubesNum = tubesNum;

            calcConveyorPath();
            calcTubeDiameter();
            calcTubesCoordsIndexes();
            drawTubes();
        }

        public void TubeLoopStep(object sender, EventArgs e)
        {
            for (int i = 0; i < _tubesNum; i++) {
                int nextPointIndex = (_tubesCoordsIndexes[i] + offset) % _coords.Count;
                Canvas.SetLeft(_canvas.Children[i], _coords[nextPointIndex].X);
                Canvas.SetTop(_canvas.Children[i], _coords[nextPointIndex].Y);
            }

            offset += 50;

            if (offset > _coords.Count) {
                offset = 0;
            }
        }

        private void calcConveyorPath()
        {
            List<Point> topPart = GraphMath.CalcArcPoint(
                150, 500,
                45, 225,
                50, _pathResolution, _scale);

            List<Point> leftPart = GraphMath.CalcArcPoint(
                -43.0672, 304.1683,
                -45, 45,
                225, _pathResolution, _scale);

            leftPart.Reverse();

            List<Point> bottomPart = GraphMath.CalcArcPoint(
                150, 108.3365,
                135, 315,
                50, _pathResolution, _scale);

            List<Point> rightPart = GraphMath.CalcArcPoint(
                -43.0672, 304.1683,
                -45, 45,
                325, _pathResolution, _scale);

            _coords = _coords.Concat(topPart).ToList();
            _coords = _coords.Concat(leftPart).ToList();
            _coords = _coords.Concat(bottomPart).ToList();
            _coords = _coords.Concat(rightPart).ToList();
            _coords.Add(_coords[0]);
        }

        private Ellipse getEllipse(Point point, int num, double diameter)
        {
            Ellipse Ellipse = new Ellipse
            {
                Tag = num,
                Width = diameter,
                Height = diameter,
                Fill = Brushes.LightGray,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(Ellipse, point.X - diameter / 2);
            Canvas.SetTop(Ellipse, point.Y - diameter / 2);
            return Ellipse;
        }

        public void DrawPath()
        {
            for (int i = 0; i < _coords.Count - 1; i++) {
                Line myLine = new Line
                {
                    Stroke = Brushes.Black,

                    X1 = _coords[i].X,
                    Y1 = _coords[i].Y,
                    X2 = _coords[i + 1].X,
                    Y2 = _coords[i + 1].Y,

                    StrokeThickness = 1
                };

                _canvas.Children.Add(myLine);
            }
        }

        private void calcTubeDiameter()
        {
            double path_length = 0;

            for (int i = 1; i < _coords.Count; i++) {
                path_length += GraphMath.PointLenth(_coords[i - 1], _coords[i]);
            }

            TubeDiameter = path_length / _tubesNum;
        }

        private void drawTubes()
        {
            for (int i = 0; i < _tubesNum; i++) {
                _canvas.Children.Add(getEllipse(_coords[_tubesCoordsIndexes[i]], i, TubeDiameter));
            }
        }

        public void calcTubesCoordsIndexes()
        {
            int length = _coords.Count;
            int step = (int)Math.Floor((double)length / _tubesNum);

            for (int i = step; i < length; i += step) {
                _tubesCoordsIndexes.Add(i);
            }
        }
    }
}
