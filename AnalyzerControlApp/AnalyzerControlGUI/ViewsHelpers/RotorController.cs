using AnalyzerDomain.Models;
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
    public class RotorController
    {
        public double TubeDiameter { set; get; }

        public ObservableCollection<RotorCell> Cells = new ObservableCollection<RotorCell>();

        private List<Point> _coords;
        private List<int> _cellsCoordsIndexes;
        private Canvas _canvas;
        private double _pathResolution;
        private double _scale;
        private int _offset = 1;

        public RotorController(Canvas canvas, double pathResolution, double scale, ObservableCollection<RotorCell> cells) 
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

        public void RotorLoopStep(object sender, EventArgs e)
        {
            for (int i = 0; i < Cells.Count; i++) {

                Ellipse ellipse = (Ellipse)_canvas.Children[i+54];
                ellipse.Fill = GetFillBrush(Cells[i]);

                int nextPointIndex = (_cellsCoordsIndexes[i] + _offset) % _coords.Count;
                Canvas.SetLeft(_canvas.Children[i+54], _coords[nextPointIndex].X);
                Canvas.SetTop(_canvas.Children[i+54], _coords[nextPointIndex].Y);
            }

            _offset += 50;

            if (_offset > _coords.Count) {
                _offset = 0;
            }
        }

        private void calcConveyorPath()
        {
            List<Point> rotor= GraphMath.CalcArcPoint(
                50, 315,
                0, 360,
                100, _pathResolution, _scale);

            _coords = _coords.Concat(rotor).ToList();
            _coords.Add(_coords[0]);
        }

        private Ellipse getEllipse(Point point, int num, double diameter)
        {
            Brush fillBrush = Brushes.LightGray;

            RotorCell cell = Cells[num];
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

        private static Brush GetFillBrush(RotorCell cell)
        {
            Brush brush = Brushes.LightGray;

            if(cell.AnalysisBarcode != string.Empty) {
                brush = Brushes.LightGreen;
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
