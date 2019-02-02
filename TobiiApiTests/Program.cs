using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxMouseManipulator;
using Gma.System.MouseKeyHook;
using SimWinInput;
using Tobii.Interaction;


namespace TobiiApiTests {
    class Program {
        private static double _actualGazePointX;
        private static double _actualGazePointY;

        private static bool _isActivationButtonPressed;
        private static bool _isLeftMouseButtonPressed;
        private static bool _isRightMouseButtonPressed;

        private const Keys ActivationHotkey = Keys.F3;
        private const Keys EViacamEnableDisableKey = Keys.F11;

        private const Keys LeftMouseButtonHotkey = Keys.F2;
        private const Keys RightMouseButtonHotkey = Keys.F4;

        private const Keys ScrollingModeHotkey = Keys.F1;
        private static bool _isScrollingModeEnabled;
        private static Point _scrollingModeStartPoint;

        static void Main(string[] a) {
            var host = new Host();
            var gazePointDataStream = host.Streams.CreateGazePointDataStream();
            gazePointDataStream.GazePoint((gazePointX, gazePointY, _) => {
                _actualGazePointX = gazePointX;
                _actualGazePointY = gazePointY;
            });

            var globalKeyboardMouseEvents = Hook.GlobalEvents();

            globalKeyboardMouseEvents.KeyDown += (sender, args) => {
                if (args.KeyCode == ActivationHotkey) {
                    if (_isActivationButtonPressed == false) {
                        _isActivationButtonPressed = true;

                        SimMouse.Act(SimMouse.Action.MoveOnly, (int)_actualGazePointX, (int)_actualGazePointY);

                        // Enabling eViacam.
                        SimKeyboard.KeyDown((byte)EViacamEnableDisableKey);
                        SimKeyboard.KeyUp((byte)EViacamEnableDisableKey);
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
                        _scrollingModeStartPoint = Control.MousePosition;
                        _isScrollingModeEnabled = true;

                        // Enabling eViacam.
                        SimKeyboard.KeyDown((byte)EViacamEnableDisableKey);
                        SimKeyboard.KeyUp((byte)EViacamEnableDisableKey);
                    }

                    args.Handled = true;
                }
            };

            globalKeyboardMouseEvents.KeyUp += (sender, args) => {
                if (args.KeyCode == ActivationHotkey) {
                    _isActivationButtonPressed = false;

                    // Disabling eViacam.
                    SimKeyboard.KeyDown((byte)EViacamEnableDisableKey);
                    SimKeyboard.KeyUp((byte)EViacamEnableDisableKey);

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
                        SimKeyboard.KeyDown((byte)EViacamEnableDisableKey);
                        SimKeyboard.KeyUp((byte)EViacamEnableDisableKey);
                    }
                }
            };


            Task.Run(() => {
                var sleepBetweenScrolls = 100;

                while (true) {
                    if (_isScrollingModeEnabled) {
                        var deltaY = Control.MousePosition.Y - _scrollingModeStartPoint.Y;

                        if (Math.Abs(deltaY) > 10) {
                            sleepBetweenScrolls = 150;

                            if (Math.Abs(deltaY) > 20) {
                                sleepBetweenScrolls = 100;
                            }

                            if (Math.Abs(deltaY) > 100) {
                                sleepBetweenScrolls = 50;
                            }

                            if (Math.Abs(deltaY) > 200) {
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


            //var keyboardHook = new Hook("test");


            //keyboardHook.KeyDownEvent += eventArgs => {


            //    Console.WriteLine(eventArgs.Key);
            //};

            // Starting pumping. Otherwise global mouse/keyboard event won't arrive.
            Application.Run();

            /* SimMouse.Act(SimMouse.Action.MoveOnly, 0, 0);*/
        }
    }
}