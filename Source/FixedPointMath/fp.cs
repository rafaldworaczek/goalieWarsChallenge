using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Unity.Mathematics.FixedPoint
{
    /// <summary>
    /// Represents a Q31.32 fixed-point number.
    /// </summary>
    [System.Serializable]
    public partial struct fp : IEquatable<fp>, IComparable<fp>
    {
        [UnityEngine.SerializeField]
        long m_rawValue;

        // Precision of this type is 2^-32, that is 2   ,3283064365386962890625E-10
        public static decimal precision => new fp(1L);//0.00000000023283064365386962890625m;
        public static fp max_value => new fp(MAX_VALUE);
        public static fp min_value => new fp(MIN_VALUE);
        public static fp one => new fp(ONE);
        public static fp zero => new fp();

        static fp Pi => new fp(PI);
        static fp PiOver2 => new fp(PI_OVER_2);
        static fp Log2Max => new fp(LOG2MAX);
        static fp Log2Min => new fp(LOG2MIN);
        static fp Ln2 => new fp(LN2);

        static readonly fp LutInterval = (fp)(LUT_SIZE - 1) / PiOver2;
        const long MAX_VALUE = long.MaxValue;
        const long MIN_VALUE = long.MinValue;
        const int NUM_BITS = 64;
        const int FRACTIONAL_PLACES = 32;
        const long ONE = 1L << FRACTIONAL_PLACES;
        const long PI_TIMES_2 = 0x6487ED511;
        const long PI = 0x3243F6A88;
        const long PI_OVER_2 = 0x1921FB544;
        const long LN2 = 0xB17217F7;
        const long LOG2MAX = 0x1F00000000;
        const long LOG2MIN = -0x2000000000;
        const int LUT_SIZE = (int)(PI_OVER_2 >> 15);

        /// <summary>
        /// Returns a number indicating the sign of a Fix64 number.
        /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
        /// </summary>
        internal static int Sign(fp value)
        {
            return
                value.m_rawValue < 0 ? -1 :
                value.m_rawValue > 0 ? 1 :
                0;
        }


        /// <summary>
        /// Returns the absolute value of a Fix64 number.
        /// Note: Abs(Fix64.MinValue) == Fix64.MaxValue.
        /// </summary>
        internal static fp Abs(fp value)
        {
            if (value.m_rawValue == MIN_VALUE)
            {
                return max_value;
            }

            // branchless implementation, see http://www.strchr.com/optimized_abs_function
            var mask = value.m_rawValue >> 63;
            return new fp((value.m_rawValue + mask) ^ mask);
        }

        /// <summary>
        /// Returns the absolute value of a Fix64 number.
        /// FastAbs(Fix64.MinValue) is undefined.
        /// </summary>
        internal static fp FastAbs(fp value)
        {
            // branchless implementation, see http://www.strchr.com/optimized_abs_function
            var mask = value.m_rawValue >> 63;
            return new fp((value.m_rawValue + mask) ^ mask);
        }


        /// <summary>
        /// Returns the largest integer less than or equal to the specified number.
        /// </summary>
        internal static fp Floor(fp value)
        {
            // Just zero out the fractional part
            return new fp((long)((ulong)value.m_rawValue & 0xFFFFFFFF00000000));
        }

        /// <summary>
        /// Returns the smallest integral value that is greater than or equal to the specified number.
        /// </summary>
        internal static fp Ceiling(fp value)
        {
            var hasFractionalPart = (value.m_rawValue & 0x00000000FFFFFFFF) != 0;
            return hasFractionalPart ? Floor(value) + one : value;
        }

        /// <summary>
        /// Rounds a value to the nearest integral value.
        /// If the value is halfway between an even and an uneven value, returns the even value.
        /// </summary>
        internal static fp Round(fp value)
        {
            var fractionalPart = value.m_rawValue & 0x00000000FFFFFFFF;
            var integralPart = Floor(value);
            if (fractionalPart < 0x80000000)
            {
                return integralPart;
            }
            if (fractionalPart > 0x80000000)
            {
                return integralPart + one;
            }
            // if number is halfway between two values, round to the nearest even number
            // this is the method used by System.Math.Round().
            return (integralPart.m_rawValue & ONE) == 0
                       ? integralPart
                       : integralPart + one;
        }

        /// <summary>
        /// Rounds a value to the nearest integral value towards zero.
        /// </summary>
        internal static fp Truncate(fp value)
        {
            var sign = Sign(value);
            var fractionalPart = value.m_rawValue & 0x00000000FFFFFFFF;
            var integralPart = Floor(value);

            if (sign < 0)
            {
                return integralPart + one;
            }
            else
            {
                return integralPart;
            }
        }

        /// <summary>
        /// Adds x and y. Performs saturating addition, i.e. in case of overflow, 
        /// rounds to MinValue or MaxValue depending on sign of operands.
        /// </summary>
        public static fp operator +(fp x, fp y)
        {
            var xl = x.m_rawValue;
            var yl = y.m_rawValue;
            var sum = xl + yl;
            // if signs of operands are equal and signs of sum and x are different
            if (((~(xl ^ yl) & (xl ^ sum)) & MIN_VALUE) != 0)
            {
                sum = xl > 0 ? MAX_VALUE : MIN_VALUE;
            }
            return new fp(sum);
        }

        /// <summary>
        /// Adds x and y witout performing overflow checking. Should be inlined by the CLR.
        /// </summary>
        internal static fp FastAdd(fp x, fp y)
        {
            return new fp(x.m_rawValue + y.m_rawValue);
        }

        /// <summary>
        /// Subtracts y from x. Performs saturating substraction, i.e. in case of overflow, 
        /// rounds to MinValue or MaxValue depending on sign of operands.
        /// </summary>
        public static fp operator -(fp x, fp y)
        {
            var xl = x.m_rawValue;
            var yl = y.m_rawValue;
            var diff = xl - yl;
            // if signs of operands are different and signs of sum and x are different
            if ((((xl ^ yl) & (xl ^ diff)) & MIN_VALUE) != 0)
            {
                diff = xl < 0 ? MIN_VALUE : MAX_VALUE;
            }
            return new fp(diff);
        }

        /// <summary>
        /// Subtracts y from x witout performing overflow checking. Should be inlined by the CLR.
        /// </summary>
        internal static fp FastSub(fp x, fp y)
        {
            return new fp(x.m_rawValue - y.m_rawValue);
        }

        static long AddOverflowHelper(long x, long y, ref bool overflow)
        {
            var sum = x + y;
            // x + y overflows if sign(x) ^ sign(y) != sign(sum)
            overflow |= ((x ^ y ^ sum) & MIN_VALUE) != 0;
            return sum;
        }

        public static fp operator *(fp x, fp y)
        {

            var xl = x.m_rawValue;
            var yl = y.m_rawValue;

            var xlo = (ulong)(xl & 0x00000000FFFFFFFF);
            var xhi = xl >> FRACTIONAL_PLACES;
            var ylo = (ulong)(yl & 0x00000000FFFFFFFF);
            var yhi = yl >> FRACTIONAL_PLACES;

            var lolo = xlo * ylo;
            var lohi = (long)xlo * yhi;
            var hilo = xhi * (long)ylo;
            var hihi = xhi * yhi;

            var loResult = lolo >> FRACTIONAL_PLACES;
            var midResult1 = lohi;
            var midResult2 = hilo;
            var hiResult = hihi << FRACTIONAL_PLACES;

            bool overflow = false;
            var sum = AddOverflowHelper((long)loResult, midResult1, ref overflow);
            sum = AddOverflowHelper(sum, midResult2, ref overflow);
            sum = AddOverflowHelper(sum, hiResult, ref overflow);

            bool opSignsEqual = ((xl ^ yl) & MIN_VALUE) == 0;

            // if signs of operands are equal and sign of result is negative,
            // then multiplication overflowed positively
            // the reverse is also true
            if (opSignsEqual)
            {
                if (sum < 0 || (overflow && xl > 0))
                {
                    return max_value;
                }
            }
            else
            {
                if (sum > 0)
                {
                    return min_value;
                }
            }

            // if the top 32 bits of hihi (unused in the result) are neither all 0s or 1s,
            // then this means the result overflowed.
            var topCarry = hihi >> FRACTIONAL_PLACES;
            if (topCarry != 0 && topCarry != -1 /*&& xl != -17 && yl != -17*/)
            {
                return opSignsEqual ? max_value : min_value;
            }

            // If signs differ, both operands' magnitudes are greater than 1,
            // and the result is greater than the negative operand, then there was negative overflow.
            if (!opSignsEqual)
            {
                long posOp, negOp;
                if (xl > yl)
                {
                    posOp = xl;
                    negOp = yl;
                }
                else
                {
                    posOp = yl;
                    negOp = xl;
                }
                if (sum > negOp && negOp < -ONE && posOp > ONE)
                {
                    return min_value;
                }
            }

            return new fp(sum);
        }

        /// <summary>
        /// Performs multiplication without checking for overflow.
        /// Useful for performance-critical code where the values are guaranteed not to cause overflow
        /// </summary>
        internal static fp FastMul(fp x, fp y)
        {

            var xl = x.m_rawValue;
            var yl = y.m_rawValue;

            var xlo = (ulong)(xl & 0x00000000FFFFFFFF);
            var xhi = xl >> FRACTIONAL_PLACES;
            var ylo = (ulong)(yl & 0x00000000FFFFFFFF);
            var yhi = yl >> FRACTIONAL_PLACES;

            var lolo = xlo * ylo;
            var lohi = (long)xlo * yhi;
            var hilo = xhi * (long)ylo;
            var hihi = xhi * yhi;

            var loResult = lolo >> FRACTIONAL_PLACES;
            var midResult1 = lohi;
            var midResult2 = hilo;
            var hiResult = hihi << FRACTIONAL_PLACES;

            var sum = (long)loResult + midResult1 + midResult2 + hiResult;
            return new fp(sum);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int CountLeadingZeroes(ulong x)
        {
            int result = 0;
            while ((x & 0xF000000000000000) == 0) { result += 4; x <<= 4; }
            while ((x & 0x8000000000000000) == 0) { result += 1; x <<= 1; }
            return result;
        }

        public static fp operator /(fp x, fp y)
        {
            var xl = x.m_rawValue;
            var yl = y.m_rawValue;

            if (yl == 0)
            {
                throw new DivideByZeroException();
            }

            var remainder = (ulong)(xl >= 0 ? xl : -xl);
            var divider = (ulong)(yl >= 0 ? yl : -yl);
            var quotient = 0UL;
            var bitPos = NUM_BITS / 2 + 1;


            // If the divider is divisible by 2^n, take advantage of it.
            while ((divider & 0xF) == 0 && bitPos >= 4)
            {
                divider >>= 4;
                bitPos -= 4;
            }

            while (remainder != 0 && bitPos >= 0)
            {
                int shift = CountLeadingZeroes(remainder);
                if (shift > bitPos)
                {
                    shift = bitPos;
                }
                remainder <<= shift;
                bitPos -= shift;

                var div = remainder / divider;
                remainder = remainder % divider;
                quotient += div << bitPos;

                // Detect overflow
                if ((div & ~(0xFFFFFFFFFFFFFFFF >> bitPos)) != 0)
                {
                    return ((xl ^ yl) & MIN_VALUE) == 0 ? max_value : min_value;
                }

                remainder <<= 1;
                --bitPos;
            }

            // rounding
            ++quotient;
            var result = (long)(quotient >> 1);
            if (((xl ^ yl) & MIN_VALUE) != 0)
            {
                result = -result;
            }

            return new fp(result);
        }

        public static fp operator %(fp x, fp y)
        {
            return new fp(
                x.m_rawValue == MIN_VALUE & y.m_rawValue == -1 ?
                0 :
                x.m_rawValue % y.m_rawValue);
        }

        /// <summary>
        /// Performs modulo as fast as possible; throws if x == MinValue and y == -1.
        /// Use the operator (%) for a more reliable but slower modulo.
        /// </summary>
        internal static fp FastMod(fp x, fp y)
        {
            return new fp(x.m_rawValue % y.m_rawValue);
        }

        public static fp operator -(fp x)
        {
            return x.m_rawValue == MIN_VALUE ? max_value : new fp(-x.m_rawValue);
        }

        public static fp operator +(fp x)
        {
            return x;
        }

        public static fp operator ++(fp x)
        {
            return x + one;
        }

        public static fp operator --(fp x)
        {
            return x - one;
        }

        public static bool operator ==(fp x, fp y)
        {
            return x.m_rawValue == y.m_rawValue;
        }

        public static bool operator !=(fp x, fp y)
        {
            return x.m_rawValue != y.m_rawValue;
        }

        public static bool operator >(fp x, fp y)
        {
            return x.m_rawValue > y.m_rawValue;
        }

        public static bool operator <(fp x, fp y)
        {
            return x.m_rawValue < y.m_rawValue;
        }

        public static bool operator >=(fp x, fp y)
        {
            return x.m_rawValue >= y.m_rawValue;
        }

        public static bool operator <=(fp x, fp y)
        {
            return x.m_rawValue <= y.m_rawValue;
        }

        /// <summary>
        /// Returns 2 raised to the specified power.
        /// Provides at least 6 decimals of accuracy.
        /// </summary>
        internal static fp Pow2(fp x)
        {
            if (x.m_rawValue == 0)
            {
                return one;
            }

            // Avoid negative arguments by exploiting that exp(-x) = 1/exp(x).
            bool neg = x.m_rawValue < 0;
            if (neg)
            {
                x = -x;
            }

            if (x == one)
            {
                return neg ? one / (fp)2 : (fp)2;
            }
            if (x >= Log2Max)
            {
                return neg ? one / max_value : max_value;
            }
            if (x <= Log2Min)
            {
                return neg ? max_value : zero;
            }

            /* The algorithm is based on the power series for exp(x):
             * http://en.wikipedia.org/wiki/Exponential_function#Formal_definition
             * 
             * From term n, we get term n+1 by multiplying with x/n.
             * When the sum term drops to zero, we can stop summing.
             */

            int integerPart = (int)Floor(x);
            // Take fractional part of exponent
            x = new fp(x.m_rawValue & 0x00000000FFFFFFFF);

            var result = one;
            var term = one;
            int i = 1;
            while (term.m_rawValue != 0)
            {
                term = FastMul(FastMul(x, term), Ln2) / (fp)i;
                result += term;
                i++;
            }

            result = FromRaw(result.m_rawValue << integerPart);
            if (neg)
            {
                result = one / result;
            }

            return result;
        }

        /// <summary>
        /// Returns the base-2 logarithm of a specified number.
        /// Provides at least 9 decimals of accuracy.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        internal static fp Log2(fp x)
        {
            if (x.m_rawValue <= 0)
            {
                throw new ArgumentOutOfRangeException("Non-positive value passed to Ln", "x");
            }

            // This implementation is based on Clay. S. Turner's fast binary logarithm
            // algorithm (C. S. Turner,  "A Fast Binary Logarithm Algorithm", IEEE Signal
            //     Processing Mag., pp. 124,140, Sep. 2010.)

            long b = 1U << (FRACTIONAL_PLACES - 1);
            long y = 0;

            long rawX = x.m_rawValue;
            while (rawX < ONE)
            {
                rawX <<= 1;
                y -= ONE;
            }

            while (rawX >= (ONE << 1))
            {
                rawX >>= 1;
                y += ONE;
            }

            var z = new fp(rawX);

            for (int i = 0; i < FRACTIONAL_PLACES; i++)
            {
                z = FastMul(z, z);
                if (z.m_rawValue >= (ONE << 1))
                {
                    z = new fp(z.m_rawValue >> 1);
                    y += b;
                }
                b >>= 1;
            }

            return new fp(y);
        }

        /// <summary>
        /// Returns the natural logarithm of a specified number.
        /// Provides at least 7 decimals of accuracy.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was non-positive
        /// </exception>
        internal static fp Ln(fp x)
        {
            return FastMul(Log2(x), Ln2);
        }

        /// <summary>
        /// Returns a specified number raised to the specified power.
        /// Provides about 5 digits of accuracy for the result.
        /// </summary>
        /// <exception cref="DivideByZeroException">
        /// The base was zero, with a negative exponent
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The base was negative, with a non-zero exponent
        /// </exception>
        internal static fp Pow(fp b, fp exp)
        {
            if (b == one)
            {
                return one;
            }
            if (exp.m_rawValue == 0)
            {
                return one;
            }
            if (b.m_rawValue == 0)
            {
                if (exp.m_rawValue < 0)
                {
                    throw new DivideByZeroException();
                }
                return zero;
            }

            fp log2 = Log2(b);
            return Pow2(exp * log2);
        }

        /// <summary>
        /// Returns the square root of a specified number.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The argument was negative.
        /// </exception>
        internal static fp Sqrt(fp x)
        {
            var xl = x.m_rawValue;
            if (xl < 0)
            {
                // We cannot represent infinities like Single and Double, and Sqrt is
                // mathematically undefined for x < 0. So we just throw an exception.
                throw new ArgumentOutOfRangeException("Negative value passed to Sqrt", "x");
            }

            var num = (ulong)xl;
            var result = 0UL;

            // second-to-top bit
            var bit = 1UL << (NUM_BITS - 2);

            while (bit > num)
            {
                bit >>= 2;
            }

            // The main part is executed twice, in order to avoid
            // using 128 bit values in computations.
            for (var i = 0; i < 2; ++i)
            {
                // First we get the top 48 bits of the answer.
                while (bit != 0)
                {
                    if (num >= result + bit)
                    {
                        num -= result + bit;
                        result = (result >> 1) + bit;
                    }
                    else
                    {
                        result = result >> 1;
                    }
                    bit >>= 2;
                }

                if (i == 0)
                {
                    // Then process it again to get the lowest 16 bits.
                    if (num > (1UL << (NUM_BITS / 2)) - 1)
                    {
                        // The remainder 'num' is too large to be shifted left
                        // by 32, so we have to add 1 to result manually and
                        // adjust 'num' accordingly.
                        // num = a - (result + 0.5)^2
                        //       = num + result^2 - (result + 0.5)^2
                        //       = num - result - 0.5
                        num -= result;
                        num = (num << (NUM_BITS / 2)) - 0x80000000UL;
                        result = (result << (NUM_BITS / 2)) + 0x80000000UL;
                    }
                    else
                    {
                        num <<= (NUM_BITS / 2);
                        result <<= (NUM_BITS / 2);
                    }

                    bit = 1UL << (NUM_BITS / 2 - 2);
                }
            }
            // Finally, if next bit would have been 1, round the result upwards.
            if (num > result)
            {
                ++result;
            }
            return new fp((long)result);
        }

        /// <summary>
        /// Returns the Sine of x.
        /// The relative error is less than 1E-10 for x in [-2PI, 2PI], and less than 1E-7 in the worst case.
        /// </summary>      
        public static explicit operator fp(long value)
        {
            return new fp(value * ONE);
        }
        public static explicit operator long(fp value)
        {
            return value.m_rawValue >> FRACTIONAL_PLACES;
        }
        public static explicit operator fp(float value)
        {
            return new fp((long)(value * ONE));
        }
        public static explicit operator float(fp value)
        {
            return (float)value.m_rawValue / ONE;
        }
        public static explicit operator fp(double value)
        {
            return new fp((long)(value * ONE));
        }
        public static explicit operator double(fp value)
        {
            return (double)value.m_rawValue / ONE;
        }
        public static implicit operator fp(decimal value)
        {
            return new fp((long)(value * ONE));
        }
        public static implicit operator decimal(fp value)
        {
            return (decimal)value.m_rawValue / ONE;
        }

        public override bool Equals(object obj)
        {
            return obj is fp && ((fp)obj).m_rawValue == m_rawValue;
        }

        public override int GetHashCode()
        {
            return m_rawValue.GetHashCode();
        }

        public bool Equals(fp other)
        {
            return m_rawValue == other.m_rawValue;
        }

        public int CompareTo(fp other)
        {
            return m_rawValue.CompareTo(other.m_rawValue);
        }

        public override string ToString()
        {
            // Up to 10 decimal places
            return ((decimal)this).ToString("0.##########");
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            // Up to 10 decimal places
            return ((decimal)this).ToString("0.##########", formatProvider);
        }

        public static fp FromRaw(long rawValue)
        {
            return new fp(rawValue);
        }
             
        // turn into a Console Application and use this to generate the look-up tables
        //static void Main(string[] args)
        //{
        //    GenerateSinLut();
        //    GenerateTanLut();
        //}

        /// <summary>
        /// The underlying integer representation
        /// </summary>
        public long RawValue => m_rawValue;

        /// <summary>
        /// This is the constructor from raw value; it can only be used interally.
        /// </summary>
        /// <param name="rawValue"></param>
        internal fp(long rawValue)
        {
            m_rawValue = rawValue;
        }

        public fp(int value)
        {
            m_rawValue = value * ONE;
        }
    }
}
