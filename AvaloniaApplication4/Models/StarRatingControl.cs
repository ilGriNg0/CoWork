using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using System.Collections.Generic;

namespace AvaloniaApplication4.Models
{
        public class StarRatingControl : TemplatedControl
        {
            public static readonly StyledProperty<int> RatingProperty =
                AvaloniaProperty.Register<StarRatingControl, int>(nameof(Rating), defaultValue: 0);

            public int Rating
            {
                get => GetValue(RatingProperty);
                set
                {
                    SetValue(RatingProperty, value);
                    OnRatingChanged(this);
                    InvalidateVisual();
                }
            }
            private int lastRating;

            public event EventHandler<int>? RatingChanged;
            private static void OnRatingChanged(object sender)
            {
                if (sender is StarRatingControl control)
                {
                    control.RatingChanged?.Invoke(control, control.lastRating);
                }
            }

            public StarRatingControl()
            {
                this.PointerPressed += OnPointerPressed;
            }

            private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
            {
                var point = e.GetCurrentPoint(this);
                int newRating = (int)(point.Position.X / (Bounds.Width / 5)) + 1;
                lastRating = Rating;
                Rating = newRating;
            }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            double starWidth = Bounds.Width / 5;
            double starHeight = Bounds.Height;
            var starGeometry = CreateStarGeometry(starWidth, starHeight);

            for (int i = 0; i < 5; i++)
            {
                var translation = Matrix.CreateTranslation(i * starWidth, 0);
                var transformedStar = starGeometry.Clone();
                transformedStar.Transform = new MatrixTransform(translation);

                if (i < Rating)
                {
                    context.DrawGeometry(Brushes.Gold, null, transformedStar);
                }
                else
                {
                    context.DrawGeometry(Brushes.Gray, null, transformedStar);
                }
            }
        }

        private StreamGeometry CreateStarGeometry(double width, double height)
        {
            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                context.BeginFigure(new Point(width * 0.5, 0), true);

                context.LineTo(new Point(width * 0.61, height * 0.35));
                context.LineTo(new Point(width * 0.98, height * 0.35));
                context.LineTo(new Point(width * 0.68, height * 0.57));
                context.LineTo(new Point(width * 0.79, height * 0.91));
                context.LineTo(new Point(width * 0.5, height * 0.7));
                context.LineTo(new Point(width * 0.21, height * 0.91));
                context.LineTo(new Point(width * 0.32, height * 0.57));
                context.LineTo(new Point(width * 0.02, height * 0.35));
                context.LineTo(new Point(width * 0.39, height * 0.35));

                context.EndFigure(true);
            }

            return geometry;
        }
    }
}
