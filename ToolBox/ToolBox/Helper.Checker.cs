using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public partial class Helper
    {
        public class Checker
        {
            public static bool IsNumType<T>()
            {
                return typeof(T) == typeof(int) ||
                    typeof(T) == typeof(sbyte) ||
                    typeof(T) == typeof(byte) ||
                    typeof(T) == typeof(short) ||
                    typeof(T) == typeof(ushort) ||
                    typeof(T) == typeof(uint) ||
                    typeof(T) == typeof(long) ||
                    typeof(T) == typeof(ulong) ||
                    typeof(T) == typeof(float) ||
                    typeof(T) == typeof(decimal) ||
                    typeof(T) == typeof(double) ||
                    typeof(T) == typeof(Int16) ||
                    typeof(T) == typeof(Int32) ||
                    typeof(T) == typeof(Int64) ||
                    typeof(T) == typeof(Decimal) ||
                    typeof(T) == typeof(Double);
            }
            
            public static bool IsEmpty(object po_Value)
            {
                return po_Value == null || po_Value == System.DBNull.Value;
            }

            public static bool IsString<T>()
            {
                return typeof(T) == typeof(string) ||
                    typeof(T) == typeof(String);
            }
        }
    }
}
