using System;
using System.Drawing;

namespace PrecisionGazeMouse.WarpPointers {
    class NoWarpPointer : IWarpPointer {
        private Point _warpPoint;
        private readonly bool _warpToInitialPoint;

        public NoWarpPointer() {
            _warpPoint = new Point(0, 0);
            _warpToInitialPoint = false;
        }

        public NoWarpPointer(Point warpPoint) {
            _warpPoint = warpPoint;
            _warpToInitialPoint = true;
        }

        public bool IsStarted() {
            return true;
        }

        public bool IsWarpReady() {
            return true;
        }

        public Point CalculateSmoothedPoint() {
            return _warpPoint;
        }

        public override String ToString() {
            return $"({_warpPoint.X:0}, {_warpPoint.Y:0})";
        }

        public Point GetGazePoint() {
            return _warpPoint;
        }

        public int GetSampleCount() {
            return 1;
        }

        public int GetWarpThreshold() {
            return 0;
        }

        public Point GetWarpPoint() {
            return _warpPoint;
        }

        public Point GetNextPoint(Point currentPoint) {
            if (_warpToInitialPoint) {
                return _warpPoint;
            } else {
                return currentPoint;
            }
        }

        public void Dispose() {}

        public void RefreshTracking() {}
    }
}