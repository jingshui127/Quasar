using System.ComponentModel;

namespace Quasar.Common.Enums
{
    public static class UserStatusExtensions
    {
        /// <summary>
        /// 将UserStatus枚举值转换为中文显示文本
        /// </summary>
        /// <param name="status">UserStatus枚举值</param>
        /// <returns>中文显示文本</returns>
        public static string ToChineseString(this UserStatus status)
        {
            switch (status)
            {
                case UserStatus.Active:
                    return "活动";
                case UserStatus.Idle:
                    return "空闲";
                default:
                    return status.ToString();
            }
        }
    }
}