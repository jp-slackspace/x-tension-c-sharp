using System;
using System.Linq;
using System.Text;

namespace ByteExtensions
{
    /// <summary>
    /// Custom extensions for byte[].
    /// </summary>
    static class ByteExtensions
    {
        public static DataTypes.Endianness _Endianness = DataTypes.Endianness.LittleEndian;

        /// <summary>
        /// Returns an 16-bit integer from a specific position in a byte array. 
        /// </summary>
        /// <param name="x">The byte array.</param>
        /// <param name="offset">The offset to read the integer from.</param>
        /// <param name="endianness">The read order.</param>
        /// <returns></returns>
        public static int GetInt16(this byte[] x, int offset
            , DataTypes.Endianness endianness = DataTypes.Endianness.LittleEndian)
        {
            int result = -1;

            if (offset >= 0 && offset <= x.Count() - 2)
            {
                switch (endianness)
                {
                    case DataTypes.Endianness.BigEndian:
                        result = x[offset] << 8 | x[offset + 1];
                        break;

                    case DataTypes.Endianness.LittleEndian:
                        result = x[offset + 1] << 8 | x[offset];
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns an 32-bit integer from a specific position in a byte array.
        /// </summary>
        /// <param name="x">The byte array.</param>
        /// <param name="offset">The offset to read the integer from.</param>
        /// <param name="endianness">The read order.</param>
        /// <returns></returns>
        public static int GetInt32(this byte[] x, int offset
            , DataTypes.Endianness endianness = DataTypes.Endianness.LittleEndian)
        {
            int result = -1;

            if (offset >= 0 && offset <= x.Count() - 4)
            {
                switch (endianness)
                {
                    case DataTypes.Endianness.BigEndian:
                        result = x[offset] << 24 | x[offset + 1] << 16
                            | x[offset + 2] << 8 | x[offset + 3];
                        break;

                    case DataTypes.Endianness.LittleEndian:
                        result = x[offset + 3] << 24 | x[offset + 2] << 16
                            | x[offset + 1] << 8 | x[offset];
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns an 64-bit integer from a specific position in a byte array.
        /// </summary>
        /// <param name="x">The byte array.</param>
        /// <param name="offset">The offset to read the integer from.</param>
        /// <param name="endianness">The read order.</param>
        /// <returns></returns>
        public static long GetInt64(this byte[] x, int offset
            , DataTypes.Endianness endianness = DataTypes.Endianness.LittleEndian)
        {
            int result = -1;

            if (offset >= 0 && offset <= x.Count() - 8)
            {
                switch (endianness)
                {
                    case DataTypes.Endianness.BigEndian:
                        result = x[offset] << 56 | x[offset + 1] << 48 
                            | x[offset + 2] << 40 | x[offset + 3] << 32 
                            | x[offset + 4] << 24 | x[offset + 5] << 16 
                            | x[offset + 6] << 8 | x[offset + 7];
                        break;

                    case DataTypes.Endianness.LittleEndian:
                        result = x[offset + 7] << 56 | x[offset + 6] << 48 
                            | x[offset + 5] << 40 | x[offset + 4] << 32 
                            | x[offset + 3] << 24 | x[offset + 2] << 16 
                            | x[offset + 1] << 8 | x[offset];
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a string of a spcified length from a specific position in a byte 
        /// array.
        /// </summary>
        /// <param name="x">The byte array.</param>
        /// <param name="offset">The offset to read the integer from.</param>
        /// <param name="length">The length of the string.</param>
        /// <returns></returns>
        public static string GetString(this byte[] x, int offset, int length)
        {
            string result = "";
            byte[] temp = new byte[length];

            Array.Copy(x, offset, temp, 0, length);
            result = Encoding.Default.GetString(x, offset, length);

            return result;
        }
    }
}
