using System;
using System.Drawing;
using PrecisionGazeMouse.PrecisionPointers;
using PrecisionGazeMouse.WarpPointers;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PrecisionGazeMouse {
    class MouseController {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IWarpPointer _warp;
        private PrecisionPointer _prec;
        private Point _finalPoint;
        private DateTime _pauseTime;
        private Point _lastCursorPosition;
        private GazeCalibrator _calibrator;
        private readonly PrecisionGazeMouseForm _form;
        private bool _updatedAtLeastOnce;

        public enum Mode {
            EYEX_AND_EVIACAM,
            EYEX_AND_TRACKIR,
            EYEX_AND_SMARTNAV,
            EYEX_ONLY,
            TRACKIR_ONLY,
            EVIACAM_ONLY
        };

        private Mode _mode;

        public enum Movement {
            CONTINUOUS,
            HOTKEY
        };

        private Movement _movement;
        bool movementHotKeyDown = false;

        bool clickHotKeyDown = false;
        bool dragging = false;
        DateTime? timeSinceClickKeyUp;
        Point? lastClick;
        bool pauseMode = false;

        enum TrackingState {
            STARTING,
            PAUSED,
            RUNNING,
            ERROR
        };

        TrackingState state;

        int sensitivity;
        public int Sensitivity {
            get { return sensitivity; }
            set {
                sensitivity = value;
                if (_prec != null) {
                    _prec.Sensitivity = value;
                }
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public MouseController(PrecisionGazeMouseForm form) {
            _form = form;
        }

        public void setMovement(Movement movement) {
            _movement = movement;
        }

        public void setMode(Mode mode) {
            Log.Debug($"Setting mode to {mode}");
            if (_warp != null)
                _warp.Dispose();
            if (_prec != null)
                _prec.Dispose();

            _mode = mode;
            switch (mode) {
                case Mode.EYEX_AND_EVIACAM:
                    _warp = new EyeXWarpPointer();
                    _prec = new NoPrecisionPointer();
                    Log.Debug("State change to running");
                    state = TrackingState.RUNNING;
                    break;
                case Mode.EYEX_AND_TRACKIR:
                    _warp = new EyeXWarpPointer();
                    _prec = new TrackIRPrecisionPointer(PrecisionPointerMode.ROTATION, sensitivity);
                    break;
                case Mode.EYEX_AND_SMARTNAV:
                    _warp = new EyeXWarpPointer();
                    _prec = new NoPrecisionPointer();
                    Log.Debug("State change to running");
                    state = TrackingState.RUNNING;
                    break;
                case Mode.TRACKIR_ONLY:
                    _warp = new NoWarpPointer(getScreenCenter());
                    _prec = new TrackIRPrecisionPointer(PrecisionPointerMode.BOTH, sensitivity);
                    break;
                case Mode.EYEX_ONLY:
                    _warp = new EyeXWarpPointer();
                    _prec = new EyeXPrecisionPointer(sensitivity);
                    break;
                case Mode.EVIACAM_ONLY:
                    _warp = new NoWarpPointer();
                    _prec = new NoPrecisionPointer();
                    Log.Debug("State change to running");
                    state = TrackingState.RUNNING;
                    break;
            }

            _calibrator = new GazeCalibrator(this, _warp);

            if (!_warp.IsStarted()) {
                Log.Debug("State change to error");
                state = TrackingState.ERROR;
            }

            if (!_prec.IsStarted()) {
                Log.Debug("State change to error");
                state = TrackingState.ERROR;
            }
        }

        public void MovementHotKeyDown() {
            if (_movement != Movement.HOTKEY || state == TrackingState.ERROR || state == TrackingState.PAUSED)
                return;

            Log.Debug("Movement key down");
            if (!movementHotKeyDown) {
                if (!dragging) {
                    if (_mode == Mode.EYEX_AND_EVIACAM || _mode == Mode.EVIACAM_ONLY) {
                        Log.Debug("Pressing eViacam key");
                        SendKeys.Send("{" + Properties.Settings.Default.eViacamKey + "}"); // trigger eViacam to start tracking
                    }

                    _warp.RefreshTracking();
                    Log.Debug("State change to starting");
                    state = TrackingState.STARTING;
                    _updatedAtLeastOnce = false;
                }
            }

            movementHotKeyDown = true;
        }

        public void PauseHotKeyDown() {
            Log.Debug("Pause key down");
            if (pauseMode) {
                pauseMode = false;
                if (state != TrackingState.ERROR) {
                    _warp.RefreshTracking();
                    Log.Debug("State change to starting");
                    state = TrackingState.STARTING;
                    _updatedAtLeastOnce = false;
                }
            } else {
                pauseMode = true;
                if (state == TrackingState.STARTING || state == TrackingState.RUNNING) {
                    Log.Debug("State change to paused");
                    state = TrackingState.PAUSED;
                }
            }
        }

        public void MovementHotKeyUp() {
            if (state == TrackingState.ERROR || state == TrackingState.PAUSED)
                return;

            Log.Debug("Movement key up");
            movementHotKeyDown = false;

            if (_movement == Movement.HOTKEY && (_mode == Mode.EYEX_AND_EVIACAM || _mode == Mode.EVIACAM_ONLY)) {
                Log.Debug("Pressing eViacam key");
                SendKeys.Send("{" + Properties.Settings.Default.eViacamKey + "}"); // trigger eViacam to stop tracking
            }
        }

        public void ClickHotKeyDown() {
            if (state == TrackingState.ERROR || state == TrackingState.PAUSED)
                return;

            Log.Debug("Click key down");
            if (!clickHotKeyDown) {
                if (!dragging && movementHotKeyDown && timeSinceClickKeyUp != null && DateTime.Now.Subtract(timeSinceClickKeyUp.Value).TotalMilliseconds < 250) {
                    // it's a drag so click down and hold
                    uint X = (uint)lastClick.Value.X;
                    uint Y = (uint)lastClick.Value.Y;
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
                    dragging = true;
                    Log.Debug("Dragging mouse");
                }
            }

            clickHotKeyDown = true;
        }

        public void ClickHotKeyUp() {
            if (state == TrackingState.ERROR || state == TrackingState.PAUSED)
                return;

            Log.Debug("Movement key up");

            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;

            if (dragging) {
                mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                dragging = false;
                Log.Debug("Stopped dragging");
            } else {
                if (timeSinceClickKeyUp != null && System.DateTime.Now.Subtract(timeSinceClickKeyUp.Value).TotalMilliseconds < 500) {
                    // it's a double click so use the original click position
                    X = (uint)lastClick.Value.X;
                    Y = (uint)lastClick.Value.Y;
                    Log.Debug("Double click");
                }

                mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                lastClick = Cursor.Position;
            }

            timeSinceClickKeyUp = System.DateTime.Now;
            clickHotKeyDown = false;
        }

        public IWarpPointer WarpPointer {
            get { return _warp; }
        }

        public PrecisionPointer PrecisionPointer {
            get { return _prec; }
        }

        public Point GetFinalPoint() {
            return _finalPoint;
        }

        public GazeCalibrator GazeCalibrator {
            get { return _calibrator; }
        }

        public String GetTrackingStatus() {
            switch (state) {
                case TrackingState.STARTING:
                    return "Starting";
                case TrackingState.RUNNING:
                    return "Running";
                case TrackingState.PAUSED:
                    return "Paused";
                case TrackingState.ERROR:
                    if (!_warp.IsStarted())
                        return "No EyeX connection";
                    if (!_prec.IsStarted())
                        return "No TrackIR connection";
                    return "Error";
            }

            return "";
        }

        public void UpdateMouse(Point currentPoint) {
            switch (state) {
                case TrackingState.STARTING:
                    if (_warp.IsWarpReady()) {
                        Log.Debug("State change to running");
                        state = TrackingState.RUNNING;
                        _finalPoint = currentPoint;
                    }

                    break;
                case TrackingState.RUNNING:
                    if (_movement == Movement.HOTKEY) {
                        if (_updatedAtLeastOnce && !movementHotKeyDown)
                            break;
                    }

                    Point warpPoint = _warp.GetNextPoint(currentPoint);
                    if (_mode == Mode.EYEX_AND_SMARTNAV || _mode == Mode.EYEX_AND_EVIACAM || _mode == Mode.EVIACAM_ONLY) {
                        warpPoint = limitToScreenBounds(warpPoint);
                        if (warpPoint != _finalPoint) {
                            _finalPoint = warpPoint;
                            _form.SetMousePosition(_finalPoint);
                        }
                    } else {
                        if (PrecisionGazeMouseForm.MousePosition != _finalPoint) {
                            Log.Debug("State change to paused");
                            state = TrackingState.PAUSED;
                            _pauseTime = DateTime.Now;
                        }

                        _finalPoint = _prec.GetNextPoint(warpPoint);
                        _finalPoint = limitToScreenBounds(_finalPoint);
                        _form.SetMousePosition(_finalPoint);
                    }

                    _updatedAtLeastOnce = true;
                    break;
                case TrackingState.PAUSED:
                    // Keep pausing if the user is still moving the mouse
                    if (_lastCursorPosition != currentPoint) {
                        _lastCursorPosition = currentPoint;
                        _pauseTime = DateTime.Now;
                    }

                    if (!pauseMode && DateTime.Now.CompareTo(_pauseTime.AddSeconds(1)) > 0) {
                        Log.Debug("State change to starting");
                        state = TrackingState.STARTING;
                    }

                    break;
                case TrackingState.ERROR:
                    if (_warp.IsStarted() && _prec.IsStarted()) {
                        Log.Debug("State change to starting");
                        state = TrackingState.STARTING;
                    }

                    break;
            }
        }

        private Point getScreenCenter() {
            Rectangle screenSize = PrecisionGazeMouseForm.GetScreenSize();
            return new Point(screenSize.Width / 2, screenSize.Height / 2);
        }

        private Point limitToScreenBounds(Point p) {
            Rectangle screenSize = PrecisionGazeMouseForm.GetScreenSize();
            int margin = 10;

            if (p.X < margin)
                p.X = margin;
            if (p.Y < margin)
                p.Y = margin;
            if (p.X >= screenSize.Width - margin)
                p.X = screenSize.Width - margin;
            if (p.Y >= screenSize.Height - margin - 5)
                p.Y = screenSize.Height - margin - 5;

            return p;
        }
    }
}