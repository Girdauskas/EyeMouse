using System;
using System.Drawing;

namespace PrecisionGazeMouse.WarpPointers {
    public interface IWarpPointer : IDisposable {
        /// <summary>
        ///  Whether it's started tracking gaze.
        /// </summary>
        bool IsStarted();

        /// <summary>
        /// Whether it's ready to warp to a new point.
        /// </summary>
        /// <returns></returns>
        bool IsWarpReady();

        /// <summary>
        ///  Smoothed point for drawing
        /// </summary>
        Point CalculateSmoothedPoint();

        /// <summary>
        /// Gaze point for drawing.
        /// </summary>
        /// <returns></returns>
        Point GetGazePoint();

        /// <summary>
        /// Sample count for printing.
        /// </summary>
        /// <returns></returns>
        int GetSampleCount();

        /// <summary>
        /// Warp threshold in pixels
        /// </summary>
        int GetWarpThreshold();

        /// <summary>
        /// Warp point for drawing, no update made
        /// </summary>
        Point GetWarpPoint();

        /// <summary>
        /// Get the next warp point based on the current pointer location and gaze.
        /// </summary>
        Point GetNextPoint(Point currentPoint);

        /// <summary>
        /// Refresh the tracking buffer for a fresh start.
        /// </summary>
        void RefreshTracking();
    }
}