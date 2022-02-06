using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxMouseManipulator;
using Gma.System.MouseKeyHook;
using SimWinInput;
using Tobii.Interaction;


namespace EyeMouse
{
    struct PointD
    {
        public double X;
        public double Y;

        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    class Program
    {
        private static PointD _actualGazePosition;

        private static PointD _previousHeadPosition;
        private static PointD _actualHeadPosition;
        private static PointD _deltaHeadPosition;

        private static readonly FifoMeanCalculator HeadXPositionFilter = new FifoMeanCalculator(10);
        private static readonly FifoMeanCalculator HeadYPositionFilter = new FifoMeanCalculator(10);

        private static PointD _newMousePosition;

        private static bool _isActivationButtonPressed;
        private static bool _isLeftMouseButtonPressed;
        private static bool _isRightMouseButtonPressed;
        private static bool _isMiddleMouseButtonPressed;

        private const Keys ActivationHotkey = Keys.CapsLock;

        private const Keys AlternativeActivationModifier = Keys.LMenu;
        private static bool _isAlternativeActivation = false;

        private static bool _isAnyClicksPerformedDuringActivation = false;


        private static List<Keys> LeftMouseButtonHotkeys = new List<Keys>() { Keys.J, Keys.Left, Keys.Space };
        private static List<Keys> RightMouseButtonHotkeys = new List<Keys>() { Keys.Right, Keys.K };
        private static List<Keys> MiddleMouseButtonHotkeys = new List<Keys>() { Keys.Up, Keys.I };


        private const Keys ScrollingModeHotkey = Keys.RMenu;
        private static bool _isVertiaclScrollingModeEnabled;
        private static bool _isHorizontalScrollingModeEnabled;

        private static PointD _scrollingModeStartPoint;

        static void Main(string[] args2)
        {
            var host = new Host();
            var gazePointDataStream = host.Streams.CreateGazePointDataStream();
            gazePointDataStream.GazePoint((gazePointX, gazePointY, _) =>
            {
                _actualGazePosition = new PointD(gazePointX, gazePointY);
            });

            var eyePositionStream = host.Streams.CreateEyePositionStream();

            eyePositionStream.EyePosition(eyePosition =>
            {
                if (eyePosition.HasLeftEyePosition && eyePosition.HasRightEyePosition)
                {
                    HeadXPositionFilter.AddValue(eyePosition.RightEyeNormalized.X);
                    HeadYPositionFilter.AddValue((eyePosition.RightEyeNormalized.Y + eyePosition.LeftEyeNormalized.Y) / 2);

                    _previousHeadPosition = _actualHeadPosition;
                    _actualHeadPosition = new PointD(HeadXPositionFilter.Mean, HeadYPositionFilter.Mean);

                    _deltaHeadPosition = new PointD(_previousHeadPosition.X - _actualHeadPosition.X, _actualHeadPosition.Y - _previousHeadPosition.Y);
                }
            });

            var globalKeyboardMouseEvents = Hook.GlobalEvents();

            globalKeyboardMouseEvents.KeyDown += (sender, args) =>
            {
                if (args.KeyCode == ActivationHotkey)
                {
                    if (_isActivationButtonPressed == false)
                    {
                        MoveMouse(_actualGazePosition);

                        _newMousePosition = _actualGazePosition;

                        _isActivationButtonPressed = true;
                    }

                    args.Handled = true;
                }

                if (args.KeyCode == AlternativeActivationModifier && _isActivationButtonPressed)
                {
                    Console.WriteLine("Alternative mode");
                    _isAlternativeActivation = true;
                    args.Handled = true;
                }

                if (_isActivationButtonPressed && (args.KeyCode != ActivationHotkey) && (args.KeyCode != AlternativeActivationModifier))
                {
                    Console.WriteLine("Any");
                    _isAnyClicksPerformedDuringActivation = true;
                }

                if (LeftMouseButtonHotkeys.Contains(args.KeyCode))
                {
                    if (_isLeftMouseButtonPressed == false && _isActivationButtonPressed)
                    {
                        _isLeftMouseButtonPressed = true;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.LeftButtonDown, currentMousePosition.X, currentMousePosition.Y);
                        args.Handled = true;
                    }

                    if (_isLeftMouseButtonPressed)
                    {
                        args.Handled = true;
                    }
                }

                if (RightMouseButtonHotkeys.Contains(args.KeyCode) && _isActivationButtonPressed)
                {
                    if (_isRightMouseButtonPressed == false)
                    {
                        _isRightMouseButtonPressed = true;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.RightButtonDown, currentMousePosition.X, currentMousePosition.Y);
                    }

                    args.Handled = true;
                }

                if ((args.KeyCode == ScrollingModeHotkey) && args.Shift == false)
                {
                    if (_isVertiaclScrollingModeEnabled == false)
                    {
                        _scrollingModeStartPoint = _actualHeadPosition;
                        _isVertiaclScrollingModeEnabled = true;
                    }

                    args.Handled = true;
                    Console.WriteLine("Scroll");
                }

                if (args.KeyCode == ScrollingModeHotkey && args.Shift)
                {
                    if (_isHorizontalScrollingModeEnabled == false)
                    {
                        _scrollingModeStartPoint = _actualHeadPosition;
                        _isHorizontalScrollingModeEnabled = true;
                    }

                    _isVertiaclScrollingModeEnabled = false;

                    args.Handled = true;
                    Console.WriteLine("Horizontal scroll");
                }


                if (MiddleMouseButtonHotkeys.Contains(args.KeyCode) && _isActivationButtonPressed)
                {
                    if (_isMiddleMouseButtonPressed == false)
                    {
                        _isMiddleMouseButtonPressed = true;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.MiddleButtonDown, currentMousePosition.X, currentMousePosition.Y);
                    }

                    args.Handled = true;
                }
            };

            globalKeyboardMouseEvents.KeyUp += (sender, args) =>
            {
                if (args.KeyCode == ActivationHotkey)
                {
                    _isActivationButtonPressed = false;

                    if ((_isAnyClicksPerformedDuringActivation == false) && (_isLeftMouseButtonPressed == false))
                    {
                        var currentMousePosition = Control.MousePosition;
                        if (_isAlternativeActivation)
                        {
                            Console.WriteLine("Right click");
                            SimMouse.Click(MouseButtons.Right, currentMousePosition.X, currentMousePosition.Y);
                        }
                        else
                        {
                            Console.WriteLine("Click");
                            SimMouse.Click(MouseButtons.Left, currentMousePosition.X, currentMousePosition.Y);
                        }
                    }

                    _isAnyClicksPerformedDuringActivation = false;
                    _isAlternativeActivation = false;

                    args.Handled = true;
                }

                if (LeftMouseButtonHotkeys.Contains(args.KeyCode))
                {
                    if (_isLeftMouseButtonPressed)
                    {
                        _isLeftMouseButtonPressed = false;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.LeftButtonUp, currentMousePosition.X, currentMousePosition.Y);

                        if (_isActivationButtonPressed)
                        {
                            args.Handled = true;
                        }
                    }
                }

                if (RightMouseButtonHotkeys.Contains(args.KeyCode))
                {
                    if (_isRightMouseButtonPressed)
                    {
                        _isRightMouseButtonPressed = false;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.RightButtonUp, currentMousePosition.X, currentMousePosition.Y);

                        if (_isActivationButtonPressed)
                        {
                            args.Handled = true;
                        }
                    }
                }

                if (MiddleMouseButtonHotkeys.Contains(args.KeyCode))
                {
                    if (_isMiddleMouseButtonPressed)
                    {
                        _isMiddleMouseButtonPressed = false;

                        var currentMousePosition = Control.MousePosition;
                        SimMouse.Act(SimMouse.Action.MiddleButtonUp, currentMousePosition.X, currentMousePosition.Y);

                        if (_isActivationButtonPressed)
                        {
                            args.Handled = true;
                        }
                    }
                }

                if (args.KeyCode == ScrollingModeHotkey)
                {
                    if (_isVertiaclScrollingModeEnabled)
                    {
                        _isVertiaclScrollingModeEnabled = false;

                        args.Handled = true;
                    }

                    if (_isHorizontalScrollingModeEnabled)
                    {
                        _isHorizontalScrollingModeEnabled = false;

                        args.Handled = true;
                    }
                }
            };

            // Up/down scrolling.
            Task.Run(() =>
            {
                var sleepBetweenScrolls = 100;

                while (true)
                {
                    if (_isVertiaclScrollingModeEnabled)
                    {
                        var deltaY = _actualHeadPosition.Y - _scrollingModeStartPoint.Y;

                        if (Math.Abs(deltaY) > 0.001)
                        {
                            sleepBetweenScrolls = 200;

                            if (Math.Abs(deltaY) > 0.003)
                            {
                                sleepBetweenScrolls = 150;
                                //Console.WriteLine("2");
                            }

                            if (Math.Abs(deltaY) > 0.005)
                            {
                                sleepBetweenScrolls = 100;
                                //Console.WriteLine("3");
                            }

                            if (Math.Abs(deltaY) > 0.007)
                            {
                                sleepBetweenScrolls = 25;
                                //Console.WriteLine("4");
                            }

                            if (deltaY > 0)
                            {
                                MouseManipulator.ScrollMouseWheelDown(1);
                            }
                            else
                            {
                                MouseManipulator.ScrollMouseWheelUp(1);
                            }
                        }
                    }
                    else
                    {
                        sleepBetweenScrolls = 100;
                    }

                    Thread.Sleep(sleepBetweenScrolls);
                }
            });


            // Left/right scrolling.
            Task.Run(() =>
            {
                var sleepBetweenScrolls = 100;

                while (true)
                {
                    if (_isHorizontalScrollingModeEnabled)
                    {
                        var deltaX = _actualHeadPosition.X - _scrollingModeStartPoint.X;

                        if (Math.Abs(deltaX) > 0.001)
                        {
                            sleepBetweenScrolls = 200;

                            if (Math.Abs(deltaX) > 0.003)
                            {
                                sleepBetweenScrolls = 150;
                                //Console.WriteLine("2");
                            }

                            if (Math.Abs(deltaX) > 0.005)
                            {
                                sleepBetweenScrolls = 100;
                                //Console.WriteLine("3");
                            }

                            if (Math.Abs(deltaX) > 0.007)
                            {
                                sleepBetweenScrolls = 25;
                                //Console.WriteLine("4");
                            }

                            if (deltaX > 0)
                            {
                                InteropMouse.mouse_event(0x1000, 0, 0, -1 * 120, 0);
                            }
                            else
                            {
                                InteropMouse.mouse_event(0x1000, 0, 0, 1 * 120, 0);
                            }
                        }
                    }
                    else
                    {
                        sleepBetweenScrolls = 100;
                    }

                    Thread.Sleep(sleepBetweenScrolls);
                }
            });


            Task.Run(() =>
            {
                while (true)
                {
                    if (_isActivationButtonPressed)
                    {
                        const double acceleration = 7d;
                        const double xSensitivity = 5000d;
                        const double ySensitivity = 5000d;

                        const double lowerDeadband = 0.00002;
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

        private static void MoveMouse(PointD position)
        {
            lock (MouseMovingLock)
            {
                SimMouse.Act(SimMouse.Action.MoveOnly, (int)position.X, (int)position.Y);
            }
        }

        public static double Remap(double value, double from1, double to1, double from2, double to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}