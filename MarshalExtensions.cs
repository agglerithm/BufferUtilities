using System;
using System.Runtime.InteropServices;

namespace BufferUtilities
{
    public static class MarshalExtensions
    {
        public static byte[] DereferenceByteArray(this IntPtr ptr)
        {
            var str = Marshal.PtrToStringAnsi(ptr);
            return str.StringToByteArray();
        }

        public static T DereferenceStruct<T>(this IntPtr ptr)
        {
            return (T)Marshal.PtrToStructure(ptr, typeof(T));
        }

        public static IntPtr PtrFromByteArray(this byte[] arr)
        {
            var size = arr.Length;
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(arr, 0, ptr, size);
            return ptr;
        }

        public static IntPtr PtrFromNum(this int num) 
        {
            return BitConverter.GetBytes(num).PtrFromByteArray();
        }
        public static IntPtr PtrFromStruct<T>(this T structure)
        {
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, ptr, false);
            return ptr;
        }

        public static int SizeOf<T>()
        {
            return Marshal.SizeOf(typeof(T));
        }
        public static IntPtr Allocate(int size)
        {
            return Marshal.AllocHGlobal(size);
        }

        public static Span<T> ByteArrayToSpan<T>(this byte[] buff) where T : struct
        {
            return  MemoryMarshal.Cast<byte, T>(buff.AsSpan());
        }
    }
}