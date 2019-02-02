using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Gma.System.MouseKeyHook;
using System.IO;
using PrecisionGazeMouse.WarpPointers;

namespace PrecisionGazeMouse {
    public struct Event {
        public DateTime Time;
        public Point Location;
        public Point Delta;
    }

    class GazeCalibrator {
        private readonly IKeyboardMouseEvents _mouseGlobalHook;
        private readonly IWarpPointer _warp;
        private readonly MouseController _controller;
        private readonly List<Event> _events;
        private readonly bool _saveCalibration = false;

        public GazeCalibrator(MouseController controller, IWarpPointer warp) {
            _controller = controller;
            _warp = warp;
            _events = new List<Event>();

            if (_saveCalibration) {
                _mouseGlobalHook = Hook.GlobalEvents();
                _mouseGlobalHook.MouseDownExt += GlobalHookMouseDownExt;
            }
        }

        public List<Event> GetEvents() {
            return _events;
        }

        private void GlobalHookMouseDownExt(object sender, MouseEventExtArgs e) {
            if (_controller.GetTrackingStatus() == "Running") {
                var curr = new Point(e.X, e.Y);
                var gaze = _warp.CalculateSmoothedPoint();
                var d = Point.Subtract(gaze, new Size(curr));

                var evt = new Event() { Time = DateTime.Now, Location = curr, Delta = d };
                _events.Add(evt);

                var csv = new StringBuilder();
                var newLine = $"{DateTime.Now},{curr.X},{curr.Y},{d.X},{d.Y}";
                csv.AppendLine(newLine);
                File.AppendAllText("CalibrationData.csv", csv.ToString());
            }
        }
    }
}