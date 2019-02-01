using System;
using System.Drawing;
using Tobii.Interaction;

namespace PrecisionGazeMouse.WarpPointers {
    public class EyeXWarpPointer : IWarpPointer {
        private readonly GazePointDataStream _stream;

        //FixationDataStream stream;
        private Point _warpPoint;
        private readonly Point[] _samples;
        private int _sampleIndex;
        private int _sampleCount;
        private bool _setNewWarp;
        private readonly int _warpThreshold;

        public EyeXWarpPointer() {
            _samples = new Point[10];
            _warpThreshold = Properties.Settings.Default.EyeXWarpThreshold;

            _stream = Program.EyeXHost.Streams.CreateGazePointDataStream();
            if (_stream != null) {
                _stream.IsEnabled = true;
                _stream.Next += UpdateGazePosition;
            }
        }

        public bool IsStarted() {
            var status = Program.EyeXHost.States.GetGazeTrackingAsync().Result;
            return status.Value == Tobii.Interaction.Framework.GazeTracking.GazeTracked;
        }

        public bool IsWarpReady() {
            return _sampleCount > 10;
        }

        protected void UpdateGazePosition(object s, StreamData<GazePointData> streamData) {
            _sampleCount++;
            _sampleIndex++;
            if (_sampleIndex >= _samples.Length)
                _sampleIndex = 0;
            _samples[_sampleIndex] = new Point((int)streamData.Data.X, (int)streamData.Data.Y);
        }

        public Point CalculateSmoothedPoint() {
            return CalculateTrimmedMean();
        }

        private Point CalculateMean() {
            Point p = new Point(0, 0);
            for (int i = 0; i < _samples.Length; i++) {
                p.X += _samples[i].X;
                p.Y += _samples[i].Y;
            }

            p.X /= _samples.Length;
            p.Y /= _samples.Length;

            return p;
        }

        private Point CalculateTrimmedMean() {
            var p = CalculateMean();

            // Find the furthest point from the mean
            double maxDist = 0;
            var maxIndex = 0;
            for (var i = 0; i < _samples.Length; i++) {
                double dist = Math.Pow(_samples[i].X - p.X, 2) + Math.Pow(_samples[i].Y - p.Y, 2);
                if (dist > maxDist) {
                    maxDist = dist;
                    maxIndex = i;
                }
            }

            // Calculate a new mean without the furthest point
            p = new Point(0, 0);
            for (var i = 0; i < _samples.Length; i++) {
                if (i != maxIndex) {
                    p.X += _samples[i].X;
                    p.Y += _samples[i].Y;
                }
            }

            p.X /= (_samples.Length - 1);
            p.Y /= (_samples.Length - 1);

            return p;
        }

        public override string ToString() {
            return $"({_samples[_sampleIndex].X:0}, {_samples[_sampleIndex].Y:0})";
        }

        public Point GetGazePoint() {
            return _samples[_sampleIndex];
        }

        public int GetSampleCount() {
            return _sampleCount;
        }

        public int GetWarpThreshold() {
            return _warpThreshold;
        }

        public Point GetWarpPoint() {
            return _warpPoint;
        }

        public Point GetNextPoint(Point currentPoint) {
            var smoothedPoint = CalculateSmoothedPoint();
            var delta = Point.Subtract(smoothedPoint, new System.Drawing.Size(_warpPoint)); // whenever there is a big change from the past
            var distance = Math.Sqrt(Math.Pow(delta.X, 2) + Math.Pow(delta.Y, 2));
            if (!_setNewWarp && distance > GetWarpThreshold()) {
                _sampleCount = 0;
                _setNewWarp = true;
            }

            if (_setNewWarp && IsWarpReady()) {
                _warpPoint = smoothedPoint;
                _setNewWarp = false;
            }

            return _warpPoint;
        }

        public void Dispose() {
            _stream.IsEnabled = false;
        }

        public void RefreshTracking() {
            _sampleCount = 0;
            _setNewWarp = true;
        }
    }
}