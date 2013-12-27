using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Windows.Forms;

namespace PEBrowser
{
    internal static class ExtensionMethods
    {
        // http://stackoverflow.com/questions/1130698/checking-if-an-object-is-a-number-in-c-sharp

        public static bool IsSignedInteger(this object o) {
            return o is SByte
                   || o is Int16
                   || o is Int32
                   || o is Int64
                   || o is IntPtr;
        }

        public static bool IsUnsignedInteger(this object o) {
            return o is Byte
                   || o is UInt16
                   || o is UInt32
                   || o is UInt64
                   || o is UIntPtr;
        }

        public static bool IsInteger(this object o) {
            return o.IsSignedInteger() || o.IsUnsignedInteger();
        }

        public static bool IsFloatingPoint(this object o) {
            return o is Single || o is Double;
        }

        public static bool IsNumber(this object o) {
            return o.IsInteger() || o.IsFloatingPoint() || o is Decimal;
        }



        public static object GetPrivateField<TObj>(this TObj obj, string fieldName)
        {
            var fieldInfo = typeof(TObj).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
                throw new Exception(String.Format("Field name \"{0}\" could not be found.", fieldName));
            return fieldInfo.GetValue(obj);
        }
    }


}
