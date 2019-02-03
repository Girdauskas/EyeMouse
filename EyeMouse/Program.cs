using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxMouseManipulator;
using Gma.System.MouseKeyHook;
using SimWinInput;
using Tobii.Interaction;
using Tobii.Interaction.Framework;


namespace EyeMouse {
    struct PointD {
        public double X;
        public double Y;

        public PointD(double x, double y) {
            X = x;
            Y = y;
        }
    }

    enum HeadSpeed {
        Still,
        Slow,
        Fast
    }

    class Program {
        private static PointD _actualGazePosition;

        private static PointD _previousHeadPosition;
        private static PointD _actualHeadPosition;
        private static PointD _deltaHeadPosition;

        private static readonly FifoMeanCalculator LeftEyeXFilter = new FifoMeanCalculator(3);
        private static readonly FifoMeanCalculator LeftEyeYFilter = new FifoMeanCalculator(3);

        private static PointD _headPositionOnceActivated;
        private static PointD _mouseStartPoint;
        private static PointD _newMousePosition;

        private static bool _isActivationButtonPressed;
        private static bool _isLeftMouseButtonPressed;
        private static bool _isRightMouseButtonPressed;

        private const Keys ActivationHotkey = Keys.F3;
        private const Keys EViacamEnableDisableKey = Keys.Scroll;

        private const Keys LeftMouseButtonHotkey = Keys.F2;
        private const Keys RightMouseButtonHotkey = Keys.F4;

        private const Keys ScrollingModeHotkey = Keys.F1;
        private static bool _isScrollingModeEnabled;
        private static Point _scrollingModeStartPoint;

        static void Main(string[] args2) {
            var host = new Host();
            var gazePointDataStream = host.Streams.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered, true);
            gazePointDataStream.GazePoint((gazePointX, gazePointY, _) => {
                _actualGazePosition = new PointD(gazePointX, gazePointY);
            });

            var eyePositionStream = host.Streams.CreateEyePositionStream(true);


            eyePositionStream.EyePosition(eyePosition => {
                if (eyePosition.HasLeftEyePosition && eyePosition.HasRightEyePosition) {
                    LeftEyeXFilter.AddValue(eyePosition.RightEyeNormalized.X);
                    LeftEyeYFilter.AddValue((eyePosition.RightEyeNormalized.Y + eyePosition.LeftEyeNormalized.Y) / 2);

                    _previousHeadPosition = _actualHeadPosition;
                    _actualHeadPosition = new PointD(LeftEyeXFilter.Mean, LeftEyeYFilter.Mean);

                    _deltaHeadPosition = new PointD(_previousHeadPosition.X - _actualHeadPosition.X, _actualHeadPosition.Y - _previousHeadPosition.Y);

                    //var a = _previousHeadPosition.X - _actualHeadPosition.X;
                    //var b = _previousHeadPosition.Y - _actualHeadPosition.Y;
                    //_headMovementSpeed = Math.Sqrt(a * a + b * b);
                    //if (_headMovementSpeed < 0.0001) {
                    //    _headMovementSpeedDescription = HeadSpeed.Still;
                    //} else if (_headMovementSpeed >= 0.0001 && _headMovementSpeed < 0.0003) {
                    //    _headMovementSpeedDescription = HeadSpeed.Slow;
                    //} else {
                    //    _headMovementSpeedDescription = HeadSpeed.Fast;
                    //}

                    //Console.WriteLine(_headMovementSpeedDescription);
                }

                //  Console.WriteLine("Has Left eye position: {0}", eyePosition.HasLeftEyePosition);
                //Console.WriteLine("Left eye position: X:{0} Y:{1} Z:{2}", eyePosition.LeftEye.X, eyePosition.LeftEye.Y, eyePosition.LeftEye.Z);
                //Console.WriteLine("Left eye position (normalized): X:{0} Y:{1} Z:{2}", eyePosition.LeftEyeNormalized.X, eyePosition.LeftEyeNormalized.Y, eyePosition.LeftEyeNormalized.Z);

                //Console.WriteLine("Has Right eye position: {0}", eyePosition.HasRightEyePosition);
                //Console.WriteLine("Right eye position: X:{0} Y:{1} Z:{2}", eyePosition.RightEye.X, eyePosition.RightEye.Y, eyePosition.RightEye.Z);
                //Console.WriteLine("Right eye position (normalized): X:{0} Y:{1} Z:{2}", eyePosition.RightEyeNormalized.X, eyePosition.RightEyeNormalized.Y, eyePosition.RightEyeNormalized.Z);
                //Console.WriteLine();
            });

            var globalKeyboardMouseEvents = Hook.GlobalEvents();

            globalKeyboardMouseEvents.KeyDown += (sender, args) => {
                if (args.KeyCode == ActivationHotkey) {
                    if (_isActivationButtonPressed == false) {
                        MoveMouse(_actualGazePosition);

                        _headPositionOnceActivated = _actualHeadPosition;

                        _mouseStartPoint = new PointD(_actualGazePosition.X, _actualGazePosition.Y);
                        _newMousePosition = _actualGazePosition;

                        _isActivationButtonPressed = true;

                        // Enabling eViacam.
                        //SimKeyboard.KeyDown((byte)EViacamEnableDisableKey);
                        //SimKeyboard.KeyUp((byte)EViacamEnableDisableKey);
                    }

                    args.Handled = true;
                }

                if (args.KeyCode == LeftMouseButtonHotkey) {
                    if (_isLeftMouseButtonPressed == false) {
                        _isLeftMouseButtonPressed = true;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.LeftButtonDown, currentMousePosition.X, currentMousePosition.Y);
                    }

                    args.Handled = true;
                }

                if (args.KeyCode == RightMouseButtonHotkey) {
                    if (_isRightMouseButtonPressed == false) {
                        _isRightMouseButtonPressed = true;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.RightButtonDown, currentMousePosition.X, currentMousePosition.Y);
                    }

                    args.Handled = true;
                }

                if (args.KeyCode == ScrollingModeHotkey) {
                    if (_isScrollingModeEnabled == false) {
                        //SimMouse.Act(SimMouse.Action.MoveOnly, (int)_actualGazePointX, (int)_actualGazePointY);
                        //_scrollingModeStartPoint = new Point((int)_actualGazePointX, (int)_actualGazePointY);
                        _scrollingModeStartPoint = Cursor.Position;
                        _isScrollingModeEnabled = true;

                        // Enabling eViacam.
                        //SimKeyboard.KeyDown((byte)EViacamEnableDisableKey);
                        //SimKeyboard.KeyUp((byte)EViacamEnableDisableKey);
                    }

                    args.Handled = true;
                }
            };

            globalKeyboardMouseEvents.KeyUp += (sender, args) => {
                if (args.KeyCode == ActivationHotkey) {
                    _isActivationButtonPressed = false;

                    // Disabling eViacam.
                    //  SimKeyboard.KeyDown((byte)EViacamEnableDisableKey);
                    //  SimKeyboard.KeyUp((byte)EViacamEnableDisableKey);

                    args.Handled = true;
                }

                if (args.KeyCode == LeftMouseButtonHotkey) {
                    if (_isLeftMouseButtonPressed) {
                        _isLeftMouseButtonPressed = false;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.LeftButtonUp, currentMousePosition.X, currentMousePosition.Y);

                        args.Handled = true;
                    }
                }

                if (args.KeyCode == RightMouseButtonHotkey) {
                    if (_isRightMouseButtonPressed) {
                        _isRightMouseButtonPressed = false;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.RightButtonUp, currentMousePosition.X, currentMousePosition.Y);

                        args.Handled = true;
                    }
                }

                if (args.KeyCode == ScrollingModeHotkey) {
                    if (_isScrollingModeEnabled) {
                        _isScrollingModeEnabled = false;

                        // Disabling eViacam.
                        // SimKeyboard.KeyDown((byte)EViacamEnableDisableKey);
                        //SimKeyboard.KeyUp((byte)EViacamEnableDisableKey);
                    }
                }
            };


            Task.Run(() => {
                var sleepBetweenScrolls = 100;

                while (true) {
                    if (_isScrollingModeEnabled) {
                        var deltaY = Control.MousePosition.Y - _scrollingModeStartPoint.Y;

                        if (Math.Abs(deltaY) > 10) {
                            sleepBetweenScrolls = 200;

                            if (Math.Abs(deltaY) > 20) {
                                sleepBetweenScrolls = 150;
                            }

                            if (Math.Abs(deltaY) > 40) {
                                sleepBetweenScrolls = 100;
                            }

                            if (Math.Abs(deltaY) > 100) {
                                sleepBetweenScrolls = 25;
                            }

                            if (deltaY > 0) {
                                MouseManipulator.ScrollMouseWheelDown(1);
                            } else {
                                MouseManipulator.ScrollMouseWheelUp(1);
                            }
                        }
                    } else {
                        sleepBetweenScrolls = 100;
                    }

                    Thread.Sleep(sleepBetweenScrolls);
                }
            });


            Task.Run(() => {
                while (true) {
                    if (_isActivationButtonPressed) {
                        const double acceleration = 10d;
                        const double xSensitivity = 5000d;
                        const double ySensitivity = 5000d;

                        const double lowerDeadband = 0.00008;
                        const double upperDeadband = 0.001;

                        var deltaHeadPosition = _deltaHeadPosition;

                        if (Math.Abs(deltaHeadPosition.X) < lowerDeadband) deltaHeadPosition.X = 0;
                        if (Math.Abs(deltaHeadPosition.Y) < lowerDeadband) deltaHeadPosition.Y = 0;
                        if (Math.Abs(deltaHeadPosition.X) > upperDeadband) deltaHeadPosition.X = Math.Sign(deltaHeadPosition.X) * upperDeadband;
                        if (Math.Abs(deltaHeadPosition.Y) > upperDeadband) deltaHeadPosition.Y = Math.Sign(deltaHeadPosition.Y) * upperDeadband;

                        var xAccelerationCoefficient = Remap(Math.Abs(deltaHeadPosition.X), 0, upperDeadband, 1, acceleration);
                        _newMousePosition.X += deltaHeadPosition.X * xSensitivity * xAccelerationCoefficient;

                        var yAccelerationCoefficient = Remap(Math.Abs(deltaHeadPosition.Y), 0, upperDeadband, 1, acceleration);
                        _newMousePosition.Y += deltaHeadPosition.Y * ySensitivity * yAccelerationCoefficient;

                        MoveMouse(_newMousePosition);
                    }

                    Thread.Sleep(16);
                }
            });


            Application.Run();
        }


        private static readonly object MouseMovingLock = new object();

        private static void MoveMouse(PointD position) {
            lock (MouseMovingLock) {
                SimMouse.Act(SimMouse.Action.MoveOnly, (int)position.X, (int)position.Y);
            }
        }

        public static double Remap(double value, double from1, double to1, double from2, double to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}