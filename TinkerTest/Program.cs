using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using SimWinInput;
using Tinkerforge;

namespace ConsoleApp6 {
    class Program {
        private static string HOST = "localhost";
        private static int PORT = 4223;
        private static string UID = "6eR9q9"; // Change XXYYZZ to the UID of your IMU Brick 2.0


        private static Quaternion _homeRotation;
        private static Quaternion _currentRotation;

        private static double _xRotation;
        private static double _yRotation;

        private static double _normalizedX;
        private static double _normalizedY;

        private static FifoMeanCalculator _normalizedXFilter = new FifoMeanCalculator(25);
        private static FifoMeanCalculator _normalizedYFilter = new FifoMeanCalculator(25);


        static void Main(string[] args) {
            IPConnection ipcon = new IPConnection(); // Create IP connection
            BrickIMUV2 imu = new BrickIMUV2(UID, ipcon); // Create device object


            ipcon.Connect(HOST, PORT); // Connect to brickd
            // Don't use device before ipcon is connected

            // Get current quaternion
            //  short w, x, y, z;
            // imu.GetQuaternion(out w, out x, out y, out z);

            //

            imu.SetSensorFusionMode(1);

            imu.GetQuaternion(out var w, out var x, out var y, out var z);

            _homeRotation = new Quaternion(x / 16383f, y / 16383f, z / 16383.0f, w / 16383f);

            imu.QuaternionCallback += (sender, newH, newX, newY, newZ) => {
                var newRotation = new Quaternion(newX / 16383f, newY / 16383f, newZ / 16383.0f, newH / 16383f);

                _currentRotation = Quaternion.Inverse(_homeRotation) * newRotation;

                //Console.WriteLine(_currentRotation.X.ToString("F2") + "  " + _currentRotation.Y.ToString("F2") + " " + _currentRotation.Z.ToString("F2"));

                _xRotation = _currentRotation.Z;
                _yRotation = _currentRotation.X;

                var normalizedX = _xRotation;
                var normalizedY = _yRotation;

                const double maxX = 0.05;
                const double maxY = 0.05;

                if (Math.Abs(normalizedX) > maxX) normalizedX = Math.Sign(normalizedX) * maxX;
                if (Math.Abs(normalizedY) > maxY) normalizedY = Math.Sign(normalizedY) * maxY;

                normalizedX = Remap(normalizedX, -maxX, maxX, 1, 0);
                normalizedY = Remap(normalizedY, -maxY, maxY, 1, 0);

                _normalizedX = normalizedX;
                _normalizedY = normalizedY;

                _normalizedXFilter.AddValue(_normalizedX);
                _normalizedYFilter.AddValue(_normalizedY);

                Console.WriteLine(normalizedX.ToString("F3") + "  " + normalizedY.ToString("F3"));
                //Console.WriteLine(_xRotation.ToString("F2") + "  " + _yRotation.ToString("F2"));
            };

            imu.SetQuaternionPeriod(1);

            Task.Run(() => {
                while (true) {
                    //MoveMouse(new PointD(_normalizedX * 2560, _normalizedY * 1440));
                    MoveMouse(new PointD(_normalizedXFilter.Mean * 2560, _normalizedYFilter.Mean * 1440));
                    Thread.Sleep(5);
                }
            });


            Thread.Sleep(-1);

            //Console.WriteLine("Quaternion [W]: " + w/16383.0);
            //Console.WriteLine("Quaternion [X]: " + x/16383.0);
            //Console.WriteLine("Quaternion [Y]: " + y/16383.0);
            //Console.WriteLine("Quaternion [Z]: " + z/16383.0);

            //Console.WriteLine("Press enter to exit");
            //Console.ReadLine();
            //ipcon.Disconnect();
        }

        struct PointD {
            public double X;
            public double Y;

            public PointD(double x, double y) {
                X = x;
                Y = y;
            }
        }

        private static void MoveMouse(PointD position) {
            SimMouse.Act(SimMouse.Action.MoveOnly, (int)position.X, (int)position.Y);
        }

        public static double Remap(double value, double from1, double to1, double from2, double to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static Vector3 FromQ2(Quaternion q1) {
            float sqw = q1.W * q1.W;
            float sqx = q1.X * q1.X;
            float sqy = q1.Y * q1.Y;
            float sqz = q1.Z * q1.Z;
            float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
            float test = q1.X * q1.W - q1.Y * q1.Z;
            Vector3 v;

            if (test > 0.4995f * unit) {
                // singularity at north pole
                v.Y = 2f * (float)Math.Atan2(q1.Y, q1.X);
                v.X = (float)Math.PI / 2;
                v.Z = 0;
                return NormalizeAngles(v * 57.2957795f);
            }

            if (test < -0.4995f * unit) {
                // singularity at south pole
                v.Y = -2f * (float)Math.Atan2(q1.Y, q1.X);
                v.X = -(float)Math.PI / 2;
                v.Z = 0;
                return NormalizeAngles(v * 57.2957795f);
            }

            Quaternion q = new Quaternion(q1.W, q1.Z, q1.X, q1.Y);
            v.Y = (float)Math.Atan2(2f * q.X * q.W + 2f * q.Y * q.Z, 1 - 2f * (q.Z * q.Z + q.W * q.W)); // Yaw
            v.X = (float)Math.Asin(2f * (q.X * q.Z - q.W * q.Y)); // Pitch
            v.Z = (float)Math.Atan2(2f * q.X * q.Y + 2f * q.Z * q.W, 1 - 2f * (q.Y * q.Y + q.Z * q.Z)); // Roll
            return NormalizeAngles(v * 57.2957795f);
        }

        static Vector3 NormalizeAngles(Vector3 angles) {
            angles.X = NormalizeAngle(angles.X);
            angles.Y = NormalizeAngle(angles.Y);
            angles.Z = NormalizeAngle(angles.Z);
            return angles;
        }

        static float NormalizeAngle(float angle) {
            while (angle > 360)
                angle -= 360;
            while (angle < 0)
                angle += 360;
            return angle;
        }
    }


    public static class MyExtensions {
        public static Vector3 Times(this Quaternion quat, Vector3 vec) {
            float num = quat.X * 2f;
            float num2 = quat.Y * 2f;
            float num3 = quat.Z * 2f;
            float num4 = quat.X * num;
            float num5 = quat.Y * num2;
            float num6 = quat.Z * num3;
            float num7 = quat.X * num2;
            float num8 = quat.X * num3;
            float num9 = quat.Y * num3;
            float num10 = quat.W * num;
            float num11 = quat.W * num2;
            float num12 = quat.W * num3;
            Vector3 result;
            result.X = (1f - (num5 + num6)) * vec.X + (num7 - num12) * vec.Y + (num8 + num11) * vec.Z;
            result.Y = (num7 + num12) * vec.X + (1f - (num4 + num6)) * vec.Y + (num9 - num10) * vec.Z;
            result.Z = (num8 - num11) * vec.X + (num9 + num10) * vec.Y + (1f - (num4 + num5)) * vec.Z;
            return result;
        }
    }

    public class FifoMeanCalculator {
        private readonly Queue<double> _dataQueue;
        private readonly Queue<double> _varianceQueue;
        private readonly object _lock = new object();
        private double _varianceSum;

        /// <summary>
        /// Current sum of all members in internal FIFO buffer.
        /// </summary>
        public double Sum { get; private set; }

        /// <summary>
        /// Current mean of all members in internal FIFO buffer.
        /// </summary>
        public double Mean { get; private set; }

        /// <summary>
        /// Current variance of all members in internal FIFO buffer.
        /// </summary>
        public double Variance { get; private set; }

        /// <summary>
        /// Shows if variance is calculated.
        /// </summary>
        public bool IsVarianceCalculated { get; private set; }

        /// <summary>
        /// Number of values in the buffer. 
        /// </summary>
        public int Count => _dataQueue.Count;

        /// <summary>
        /// Maximum number of values in the FIFO buffer.
        /// </summary>
        public int MaxCount { get; private set; }

        /// <summary>
        /// Initialized new instance of <see cref="FifoMeanCalculator"/>.
        /// </summary>
        /// <param name="maxCount">Number of values to use in calculation. (FIFO buffer size internally)</param>
        /// <param name="isVarianceCalculated">calculate variance or not?..</param>
        public FifoMeanCalculator(int maxCount, bool isVarianceCalculated = false) {
            IsVarianceCalculated = isVarianceCalculated;
            MaxCount = maxCount;
            if (maxCount < 1) MaxCount = 1;
            _dataQueue = new Queue<double>(maxCount);
            _varianceQueue = new Queue<double>(maxCount);
        }

        /// <summary>
        /// Adds value for calculation. You can read results right after this method returns.
        /// </summary>
        /// <param name="value"></param>
        public void AddValue(double value) {
            lock (_lock) {
                _dataQueue.Enqueue(value);

                if (_dataQueue.Count > MaxCount) {
                    var valueToRemove = _dataQueue.Dequeue();
                    Sum -= valueToRemove;
                }

                Sum += value;
                Mean = Sum / _dataQueue.Count;

                if (IsVarianceCalculated == false) return;

                var newDifference = Math.Pow(value - Mean, 2);
                _varianceQueue.Enqueue(newDifference);
                _varianceSum += newDifference;
                if (_varianceQueue.Count > MaxCount) {
                    var dequeuedTerm = _varianceQueue.Dequeue();
                    _varianceSum -= dequeuedTerm;
                }

                Variance = _varianceSum / _varianceQueue.Count;
            }
        }
    }
}