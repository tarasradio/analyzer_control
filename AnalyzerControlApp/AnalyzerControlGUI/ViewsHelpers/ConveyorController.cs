using AnalyzerControlGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnalyzerControlGUI.ViewsHelpers
{
    public class ConveyorController
    {
        public double TubeDiameter { set; get; }

        public ObservableCollection<ConveyorCell> Cells = new ObservableCollection<ConveyorCell>();

        private List<Point> _coords;
        private List<int> _cellsCoordsIndexes;
        private Canvas _canvas;
        private double _pathResolution;
        private double _scale;
        private int _offset = 1;

        public ConveyorController(Canvas canvas, double pathResolution, double scale, ObservableCollection<ConveyorCell> cells) 
        {
            _coords = new List<Point>();
            _cellsCoordsIndexes = new List<int>();
            _canvas = canvas;
            _pathResolution = pathResolution;
            _scale = scale;
            Cells = cells;

            calcConveyorPath();
            calcCellDiameter();
            calcCellsCoordsIndexes();

            drawCells();
        }

        public void ConveyorLoopStep(object sender, EventArgs e)
        {
            for (int i = 0; i < Cells.Count; i++) {

                Ellipse ellipse = (Ellipse)_canvas.Children[i];
                ellipse.Fill = GetFillBrush(Cells[i]);

                int nextPointIndex = (_cellsCoordsIndexes[i] + _offset) % _coords.Count;
                Canvas.SetLeft(_canvas.Children[i], _coords[nextPointIndex].X);
                Canvas.SetTop(_canvas.Children[i], _coords[nextPointIndex].Y);
            }

            _offset += 50;

            if (_offset > _coords.Count) {
                _offset = 0;
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
            Brush fillBrush = Brushes.LightGray;

            ConveyorCell cell = Cells[num];
            fillBrush = GetFillBrush(cell);

            Ellipse Ellipse = new Ellipse
            {
                Tag = num,
                Width = diameter,
                Height = diameter,
                Fill = fillBrush,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            Canvas.SetLeft(Ellipse, point.X - diameter / 2);
            Canvas.SetTop(Ellipse, point.Y - diameter / 2);
            return Ellipse;
        }

        private static Brush GetFillBrush(ConveyorCell cell)
        {
            Brush brush = Brushes.LightGray;
            switch (cell.State)
            {
                case ConveyorCellState.Empty:
                    brush = Brushes.LightGray;
                    break;
                case ConveyorCellState.Processed:
                    brush = Brushes.LightGreen;
                    break;
                case ConveyorCellState.Processing:
                    brush = Brushes.Khaki;
                    break;
                case ConveyorCellState.Error:
                    brush = Brushes.LightPink;
                    break;
                default:
                    break;
            }

            return brush;
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

        private void calcCellDiameter()
        {
            double path_length = 0;

            for (int i = 1; i < _coords.Count; i++) {
                path_length += GraphMath.PointLenth(_coords[i - 1], _coords[i]);
            }

            TubeDiameter = path_length / Cells.Count;
        }
        
        public void calcCellsCoordsIndexes()
        {
            int length = _coords.Count;
            int step = (int)Math.Floor((double)length / Cells.Count);

            for (int i = step; i < length; i += step) {
                _cellsCoordsIndexes.Add(i);
            }
        }

        private void drawCells()
        {
            for (int i = 0; i < Cells.Count; i++) {
                _canvas.Children.Add(getEllipse(_coords[_cellsCoordsIndexes[i]], i, TubeDiameter));
            }
        }
    }
}
