using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxMouseManipulator;
using Gma.System.MouseKeyHook;
using SimWinInput;
using Tobii.Interaction;


namespace EyeMouse {
    struct PointD {
        public double X;
        public double Y;

        public PointD(double x, double y) {
            X = x;
            Y = y;
        }
    }

    class Program {
        private static PointD _actualGazePosition;

        private static PointD _previousHeadPosition;
        private static PointD _actualHeadPosition;
        private static PointD _deltaHeadPosition;

        private static readonly FifoMeanCalculator HeadXPositionFilter = new FifoMeanCalculator(3);
        private static readonly FifoMeanCalculator HeadYPositionFilter = new FifoMeanCalculator(3);

        private static PointD _newMousePosition;

        private static bool _isActivationButtonPressed;
        private static bool _isLeftMouseButtonPressed;
        private static bool _isRightMouseButtonPressed;

        private const Keys ActivationHotkey = Keys.F3;

        private const Keys LeftMouseButtonHotkey = Keys.F2;
        private const Keys RightMouseButtonHotkey = Keys.F4;

        private const Keys ScrollingModeHotkey = Keys.F1;
        private static bool _isScrollingModeEnabled;
        private static PointD _scrollingModeStartPoint;

        static void Main(string[] args2) {
            var host = new Host();
            var gazePointDataStream = host.Streams.CreateGazePointDataStream();
            gazePointDataStream.GazePoint((gazePointX, gazePointY, _) => {
                _actualGazePosition = new PointD(gazePointX, gazePointY);
            });

            var eyePositionStream = host.Streams.CreateEyePositionStream();

            eyePositionStream.EyePosition(eyePosition => {
                if (eyePosition.HasLeftEyePosition && eyePosition.HasRightEyePosition) {
                    HeadXPositionFilter.AddValue(eyePosition.RightEyeNormalized.X);
                    HeadYPositionFilter.AddValue((eyePosition.RightEyeNormalized.Y + eyePosition.LeftEyeNormalized.Y) / 2);

                    _previousHeadPosition = _actualHeadPosition;
                    _actualHeadPosition = new PointD(HeadXPositionFilter.Mean, HeadYPositionFilter.Mean);

                    _deltaHeadPosition = new PointD(_previousHeadPosition.X - _actualHeadPosition.X, _actualHeadPosition.Y - _previousHeadPosition.Y);
                }
            });

            var globalKeyboardMouseEvents = Hook.GlobalEvents();

            globalKeyboardMouseEvents.KeyDown += (sender, args) => {
                if (args.KeyCode == ActivationHotkey) {
                    if (_isActivationButtonPressed == false) {
                        MoveMouse(_actualGazePosition);

                        _newMousePosition = _actualGazePosition;

                        _isActivationButtonPressed = true;
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
                        _scrollingModeStartPoint = _actualHeadPosition;
                        _isScrollingModeEnabled = true;
                    }

                    args.Handled = true;
                }
            };

            globalKeyboardMouseEvents.KeyUp += (sender, args) => {
                if (args.KeyCode == ActivationHotkey) {
                    _isActivationButtonPressed = false;

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
                    }
                }
            };


            Task.Run(() => {
                var sleepBetweenScrolls = 100;

                while (true) {
                    if (_isScrollingModeEnabled) {
                        var deltaY = _actualHeadPosition.Y - _scrollingModeStartPoint.Y;

                        if (Math.Abs(deltaY) > 0.001) {
                            sleepBetweenScrolls = 200;

                            if (Math.Abs(deltaY) > 0.003) {
                                sleepBetweenScrolls = 150;
                                Console.WriteLine("2");
                            }

                            if (Math.Abs(deltaY) > 0.005) {
                                sleepBetweenScrolls = 100;
                                Console.WriteLine("3");
                            }

                            if (Math.Abs(deltaY) > 0.007) {
                                sleepBetweenScrolls = 25;
                                Console.WriteLine("4");
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
                        const double acceleration = 8d;
                        const double xSensitivity = 5000d;
                        const double ySensitivity = 5000d;

                        const double lowerDeadband = 0.00005;
                        const double upperDeadband = 0.001;

                        var deltaHeadPosition = _deltaHeadPosition;

                        if (Math.Abs(deltaHeadPosition.X) < lowerDeadband) deltaHeadPosition.X = 0;
                        if (Math.Abs(deltaHeadPosition.Y) < lowerDeadband) deltaHeadPosition.Y = 0;
                        if (Math.Abs(deltaHeadPosition.X) > upperDeadband) deltaHeadPosition.X = Math.Sign(deltaHeadPosition.X) * upperDeadband;
                        if (Math.Abs(deltaHeadPosition.Y) > upperDeadband) deltaHeadPosition.Y = Math.Sign(deltaHeadPosition.Y) * upperDeadband;

                        var xAccelerationCoefficient = Remap(Math.Abs(deltaHeadPosition.X), lowerDeadband, upperDeadband, 1, acceleration);
                        _newMousePosition.X += deltaHeadPosition.X * xSensitivity * xAccelerationCoefficient;

                        var yAccelerationCoefficient = Remap(Math.Abs(deltaHeadPosition.Y), lowerDeadband, upperDeadband, 1, acceleration);
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