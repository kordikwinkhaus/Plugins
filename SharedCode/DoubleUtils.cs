using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;

namespace Okna.Plugins
{
    public static class DoubleUtils
    {
        private const double EPSILON = 0.0001;

        [DebuggerStepThrough]
        public static bool Equals(double a, double b)
        {
            return Math.Abs(a - b) < EPSILON;
        }

        [DebuggerStepThrough]
        public static bool Equals(Point p1, Point p2)
        {
            return Equals(p1.X, p2.X) && Equals(p1.Y, p2.Y);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct NanUnion
        {
            [FieldOffset(0)]
            internal double DoubleValue;

            [FieldOffset(0)]
            internal ulong UintValue;
        }

        [DebuggerStepThrough]
        public static bool GreaterThan(double value1, double value2)
        {
            return value1 > value2 && !DoubleUtils.AreClose(value1, value2);
        }

        [DebuggerStepThrough]
        public static bool AreClose(double value1, double value2)
        {
            if (value1 == value2)
            {
                return true;
            }
            double num = (Math.Abs(value1) + Math.Abs(value2) + 10.0) * 2.2204460492503131E-16;
            double num2 = value1 - value2;
            return -num < num2 && num > num2;
        }

        [DebuggerStepThrough]
        public static bool IsNaN(double value)
        {
            DoubleUtils.NanUnion nanUnion = default(DoubleUtils.NanUnion);
            nanUnion.DoubleValue = value;
            ulong num = nanUnion.UintValue & 18442240474082181120uL;
            ulong num2 = nanUnion.UintValue & 4503599627370495uL;
            return (num == 9218868437227405312uL || num == 18442240474082181120uL) && num2 > 0uL;
        }
    }
}
