using System.Runtime.CompilerServices;

namespace Quasar.Common.Cryptography
{
    public class SafeComparison
    {
        /// <summary>
        /// 比较两个字节数组是否相等。
        /// </summary>
        /// <param name="a1">要比较的字节数组</param>
        /// <param name="a2">要比较的字节数组</param>
        /// <returns>如果相等则返回true，否则返回false</returns>
        /// <remarks>
        /// 假设字节数组具有相同的长度。
        /// 此方法可防止计时攻击。
        /// </remarks>
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static bool AreEqual(byte[] a1, byte[] a2)
        {
            bool result = true;
            for (int i = 0; i < a1.Length; ++i)
            {
                if (a1[i] != a2[i])
                    result = false;
            }
            return result;
        }
    }
}
