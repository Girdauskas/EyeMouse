using System;
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
    class Program {
        private static double _actualGazePointX;
        private static double _actualGazePointY;

        private static double _actualLeftEyeX;
        private static double _actualLeftEyeY;

        private static double _eyeXOnceActivated;
        private static double _eyeYOnceActivated;
        private static Point _mouseStartPoint;
        private static double _newMouseX;
        private static double _newMouseY;


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

        static void Main(string[] a) {
            var host = new Host();
            var gazePointDataStream = host.Streams.CreateGazePointDataStream(GazePointDataMode.LightlyFiltered, true);
            gazePointDataStream.GazePoint((gazePointX, gazePointY, _) => {
                _actualGazePointX = gazePointX;
                _actualGazePointY = gazePointY;
            });

            var eyePositionStream = host.Streams.CreateEyePositionStream(true);
            eyePositionStream.EyePosition(eyePosition => {
                if (eyePosition.HasLeftEyePosition && eyePosition.HasRightEyePosition) {
                    _actualLeftEyeX = eyePosition.LeftEyeNormalized.X;
                    _actualLeftEyeY = eyePosition.LeftEyeNormalized.Y;
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
                        MoveMouse((int)_actualGazePointX, (int)_actualGazePointY);

                        _eyeXOnceActivated = _actualLeftEyeX;
                        _eyeYOnceActivated = _actualLeftEyeY;

                        _mouseStartPoint = new Point((int)_actualGazePointX, (int)_actualGazePointY);
                        _newMouseX = (int)_actualGazePointX;
                        _newMouseY = (int)_actualGazePointY;

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
                        var deltaX = _eyeXOnceActivated - _actualLeftEyeX;
                        var deltaY = _actualLeftEyeY - _eyeYOnceActivated;

                        _newMouseX = _mouseStartPoint.X + deltaX * 10000;
                        _newMouseY = _mouseStartPoint.Y + deltaY * 10000;

                        //Console.WriteLine(deltaX);

                        MoveMouse((int)_newMouseX, (int)_newMouseY);
                    }

                    Thread.Sleep(16);
                }
            });


            Application.Run();
        }


        private static readonly object MouseMovingLock = new object();

        private static void MoveMouse(int newX, int newY) {
            lock (MouseMovingLock) {
                SimMouse.Act(SimMouse.Action.MoveOnly, newX, newY);
            }
        }
    }
}