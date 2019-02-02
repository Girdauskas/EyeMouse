using System;
using System.Threading;
using System.Windows.Forms;
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
            };


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