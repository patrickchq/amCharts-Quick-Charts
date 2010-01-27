﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Data;

namespace AmCharts.Windows.QuickCharts
{
    public class ColumnGraph : SerialGraph
    {
        public ColumnGraph()
        {
            this.DefaultStyleKey = typeof(ColumnGraph);
            _columnGraph = new Path();
            _columnGraphGeometry = new PathGeometry();
            _columnGraph.Data = _columnGraphGeometry;

            BindBrush();
        }

        private void BindBrush()
        {
            Binding brushBinding = new Binding("Brush");
            brushBinding.Source = this;
            _columnGraph.SetBinding(Path.FillProperty, brushBinding);
        }

        private Canvas _graphCanvas;
        private Path _columnGraph;
        private PathGeometry _columnGraphGeometry;

        public override void OnApplyTemplate()
        {
            _graphCanvas = (Canvas)TreeHelper.TemplateFindName("PART_GraphCanvas", this);
            _graphCanvas.Children.Add(_columnGraph);

        }

        public override void Render()
        {
            if (Locations != null)
            {
                int changeCount = Math.Min(Locations.Count, _columnGraphGeometry.Figures.Count);
                ChangeColumns(changeCount);
                int diff = Locations.Count - _columnGraphGeometry.Figures.Count;
                if (diff > 0)
                {
                    AddColumns(changeCount);
                }
                else if (diff < 0)
                {
                    RemoveColumns(changeCount);
                }
            }
        }

        private void AddColumns(int changeCount)
        {
            for (int i = changeCount; i < Locations.Count; i++)
            {
                PathFigure column = new PathFigure();
                _columnGraphGeometry.Figures.Add(column);
                for (int si = 0; si < 4; si++)
                {
                    column.Segments.Add(new LineSegment());
                }
                SetColumnSegments(i);
            }
        }

        private void RemoveColumns(int changeCount)
        {
            for (int i = _columnGraphGeometry.Figures.Count - 1; i >= changeCount ; i--)
            {
                _columnGraphGeometry.Figures.RemoveAt(i);
            }
        }

        private void ChangeColumns(int changeCount)
        {
            for (int i = 0; i < changeCount; i++)
            {
                SetColumnSegments(i);
            }
        }

        private void SetColumnSegments(int index)
        {
            // TODO: column width allocation
            double left = Locations[index].X - XStep / 2;
            double right = left + XStep;
            double y1 = GroundLevel;
            double y2 = Locations[index].Y;

            _columnGraphGeometry.Figures[index].StartPoint = new Point(left, y1);
            (_columnGraphGeometry.Figures[index].Segments[0] as LineSegment).Point = new Point(right, y1);
            (_columnGraphGeometry.Figures[index].Segments[1] as LineSegment).Point = new Point(right, y2);
            (_columnGraphGeometry.Figures[index].Segments[2] as LineSegment).Point = new Point(left, y2);
            (_columnGraphGeometry.Figures[index].Segments[3] as LineSegment).Point = new Point(left, y1);
        }

    }
}
