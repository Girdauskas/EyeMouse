using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeMouse {
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