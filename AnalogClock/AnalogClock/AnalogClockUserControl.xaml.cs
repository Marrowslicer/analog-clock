using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnalogСlock
{
    public partial class AnalogClockUserControl : UserControl
    {
        private const int RADIUS = 150;
        private const int SECOND_ARROW_SIZE = 140;
        private const int MINUTE_ARROW_SIZE = 120;
        private const int HOUR_ARROW_SIZE = 80;

        private DispatcherTimer timer;
        private Line secondArrow;
        private Line minuteArrow;
        private Line hourArrow;
        private Point center;

        public AnalogClockUserControl()
        {
            InitializeComponent();
            InitializeClockFace();
            InitializeTimer();
        }

        private void InitializeClockFace()
        {
            DrawCenterPoint();
            DrawHours();
            DrawMinutes();
            DrawSecondArrow();
            DrawMinuteArrow();
            DrawHourArrow();
        }

        private void DrawCenterPoint()
        {
            center = new Point(ClockFaceCanvas.Width / 2, ClockFaceCanvas.Height / 2);

            var centerEllipse = new Ellipse
            {
                Height = 10D,
                Width = 10D,
                Fill = Brushes.Black
            };

            ClockFaceCanvas.Children.Add(centerEllipse);
            Canvas.SetLeft(centerEllipse, center.X - centerEllipse.Width / 2);
            Canvas.SetTop(centerEllipse, center.Y - centerEllipse.Height / 2);
        }

        private void DrawHours()
        {
            Ellipse ellipse;

            for (int i = 0; i < 12; i++)
            {
                ellipse = new Ellipse
                {
                    Height = 10D,
                    Width = 10D,
                    Fill = Brushes.Black
                };

                ClockFaceCanvas.Children.Add(ellipse);

                var angle = 360 / 12 * (i + 1);
                var point = GetRotatedPoint(angle, RADIUS);

                Canvas.SetLeft(ellipse, center.X + point.X - ellipse.Width / 2);
                Canvas.SetTop(ellipse, center.Y + point.Y - ellipse.Height / 2);
            }
        }

        private void DrawMinutes()
        {
            Ellipse ellipse;

            for (int i = 0; i < 60; i++)
            {
                if ((i + 1) % 5 == 0)
                {
                    continue;
                }

                ellipse = new Ellipse
                {
                    Height = 5D,
                    Width = 5D,
                    Fill = Brushes.Black
                };

                ClockFaceCanvas.Children.Add(ellipse);

                var angle = 6D * (i + 1);
                var point = GetRotatedPoint(angle, RADIUS);

                Canvas.SetLeft(ellipse, center.X + point.X - ellipse.Width / 2);
                Canvas.SetTop(ellipse, center.Y + point.Y - ellipse.Height / 2);
            }
        }

        private void DrawSecondArrow()
        {
            secondArrow = new Line
            {
                Stroke = Brushes.Red,
                X1 = center.X,
                Y1 = center.Y,
                X2 = center.X,
                Y2 = center.Y - SECOND_ARROW_SIZE,
                StrokeThickness = 2,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Triangle
            };

            ClockFaceCanvas.Children.Add(secondArrow);
        }

        private void DrawMinuteArrow()
        {
            minuteArrow = new Line
            {
                Stroke = Brushes.Black,
                X1 = center.X,
                Y1 = center.Y,
                X2 = center.X,
                Y2 = center.Y - MINUTE_ARROW_SIZE,
                StrokeThickness = 4,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Triangle
            };

            ClockFaceCanvas.Children.Add(minuteArrow);
        }

        private void DrawHourArrow()
        {
            hourArrow = new Line
            {
                Stroke = Brushes.Black,
                X1 = center.X,
                Y1 = center.Y,
                X2 = center.X,
                Y2 = center.Y - HOUR_ARROW_SIZE,
                StrokeThickness = 8,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Triangle
            };

            ClockFaceCanvas.Children.Add(hourArrow);
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.1),
            };

            timer.Tick += OnTimerTick;
            timer.Start();
        }

        private void OnTimerTick(object sender, EventArgs args)
        {
            var seconds = DateTime.Now.Second;
            var minutes = DateTime.Now.Minute;
            var hours = DateTime.Now.Hour;

            RotateSecondArrow(seconds);
            RotateMinuteArrow(minutes);
            RotateHourArrow(hours, minutes);
        }

        private void RotateSecondArrow(int seconds)
        {
            var angle = seconds * 6 - 90;
            var point = GetRotatedPoint(angle, SECOND_ARROW_SIZE);

            secondArrow.X2 = center.X + point.X;
            secondArrow.Y2 = center.Y + point.Y;
        }

        private void RotateMinuteArrow(int minutes)
        {
            var angle = minutes * 6 - 90;
            var point = GetRotatedPoint(angle, MINUTE_ARROW_SIZE);

            minuteArrow.X2 = center.X + point.X;
            minuteArrow.Y2 = center.Y + point.Y;
        }

        private void RotateHourArrow(int hours, int minutes)
        {
            var angle = 360 / 12 * hours + 0.5 * minutes - 90;
            var point = GetRotatedPoint(angle, HOUR_ARROW_SIZE);

            hourArrow.X2 = center.X + point.X;
            hourArrow.Y2 = center.Y + point.Y;
        }

        private Point GetRotatedPoint(double angle, double radius)
        {
            var x = radius * Math.Cos(angle * Math.PI / 180);
            var y = radius * Math.Sin(angle * Math.PI / 180);

            return new Point(x, y);
        }
    }
}
