using Microsoft.Maui.Graphics;

namespace HealthApp.Views.Charts
{
    public class ChartDrawable : IDrawable
    {
        public enum ChartType
        {
            Line
            // later: Bar, MultiLine, etc.
        }

        private List<double> _values = new();
        private List<string> _xLabels = new();
        private ChartType _type = ChartType.Line;

        public void SetLineData(List<double> values, List<string> xLabels)
        {
            _values = values ?? new List<double>();
            _xLabels = xLabels ?? new List<string>();
            _type = ChartType.Line;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (_values.Count < 2)
                return;

            switch (_type)
            {
                case ChartType.Line:
                    DrawLineChart(canvas, dirtyRect);
                    break;
            }
        }

        // ------------------------
        // LINE CHART DRAWING
        // ------------------------
        private void DrawLineChart(ICanvas canvas, RectF dirtyRect)
        {
            if (_values == null || _values.Count < 2)
                return;

            float paddingLeft = 55;
            float paddingBottom = 40;
            float paddingTop = 40;
            float paddingRight = 45;

            float width = dirtyRect.Width - paddingLeft - paddingRight;
            float height = dirtyRect.Height - paddingTop - paddingBottom;

            double min = _values.Min();
            double max = _values.Max();

            double roundedMin = Math.Floor(min);
            double roundedMax = Math.Ceiling(max);

            if (roundedMax - roundedMin < 2)
                roundedMax = roundedMin + 2;

            // ? BACKGROUND
            canvas.FillColor = Colors.White;
            canvas.FillRectangle(dirtyRect);

            int gridLines = 4;

            // ? GRID + Y AXIS LABELS
            canvas.StrokeColor = Colors.LightGray;
            canvas.StrokeSize = 0.5f;
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 10;

            for (int i = 0; i <= gridLines; i++)
            {
                float y = paddingTop + (i / (float)gridLines) * height;

                canvas.DrawLine(paddingLeft, y, paddingLeft + width, y);

                double value = roundedMax - (i / (double)gridLines) * (roundedMax - roundedMin);

                canvas.DrawString(value.ToString("0.0"),
                    2, y - 6, paddingLeft - 5, 12,
                    HorizontalAlignment.Right, VerticalAlignment.Center);
            }

            // ? X GRID + DATE LABELS
            int xPoints = _values.Count;
            int labelStep = Math.Max(1, xPoints / 4);

            for (int i = 0; i < xPoints; i++)
            {
                float x = paddingLeft + (i / (float)(xPoints - 1)) * width;

                canvas.DrawLine(x, paddingTop, x, paddingTop + height);

                if (i % labelStep == 0 && i < _xLabels.Count)
                {
                    canvas.DrawString(_xLabels[i],
                        x - 35, paddingTop + height + 10, 70, 30,
                        HorizontalAlignment.Center, VerticalAlignment.Top);
                }
            }

            // ? AXIS BORDER
            canvas.StrokeColor = Colors.Gray;
            canvas.StrokeSize = 1;
            canvas.DrawRectangle(paddingLeft, paddingTop, width, height);

            // ?? PURPLE LINE
            canvas.StrokeColor = Colors.Purple;
            canvas.StrokeSize = 3;

            for (int i = 0; i < _values.Count - 1; i++)
            {
                float x1 = paddingLeft + (float)i / (xPoints - 1) * width;
                float x2 = paddingLeft + (float)(i + 1) / (xPoints - 1) * width;

                float y1 = paddingTop + height -
                    (float)((_values[i] - roundedMin) / (roundedMax - roundedMin) * height);

                float y2 = paddingTop + height -
                    (float)((_values[i + 1] - roundedMin) / (roundedMax - roundedMin) * height);

                canvas.DrawLine(x1, y1, x2, y2);
            }

            // ?? DRAW DATA POINT CIRCLES
            canvas.FillColor = Colors.White;
            canvas.StrokeColor = Colors.Purple;
            canvas.StrokeSize = 2;

            float radius = 5f;

            for (int i = 0; i < _values.Count; i++)
            {
                float x = paddingLeft + (float)i / (xPoints - 1) * width;

                float y = paddingTop + height -
                    (float)((_values[i] - roundedMin) / (roundedMax - roundedMin) * height);

                canvas.FillCircle(x, y, radius);
                canvas.DrawCircle(x, y, radius);
            }
        }
    }
}