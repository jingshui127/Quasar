﻿using System;

namespace Quasar.Client.IpGeoLocation
{
    /// <summary>
    /// 工厂类，用于检索和缓存最近的IP地理位置信息，缓存时间为 <see cref="MINIMUM_VALID_TIME"/> 分钟。
    /// </summary>
    public static class GeoInformationFactory
    {
        /// <summary>
        /// 用于获取WAN IP地址地理位置信息的检索器。
        /// </summary>
        private static readonly GeoInformationRetriever Retriever = new GeoInformationRetriever();

        /// <summary>
        /// 用于缓存最新的IP地理位置信息。
        /// </summary>
        private static GeoInformation _geoInformation;

        /// <summary>
        /// 上次成功获取位置信息的时间。
        /// </summary>
        private static DateTime _lastSuccessfulLocation = new DateTime(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 成功获取IP地理位置信息的最小有效分钟数。
        /// </summary>
        private const int MINIMUM_VALID_TIME = 60 * 12;

        /// <summary>
        /// 获取IP地理位置信息，如果超过 <see cref="MINIMUM_VALID_TIME"/> 分钟则重新获取，否则使用缓存。
        /// </summary>
        /// <returns>最新的IP地理位置信息。</returns>
        public static GeoInformation GetGeoInformation()
        {
            var passedTime = new TimeSpan(DateTime.UtcNow.Ticks - _lastSuccessfulLocation.Ticks);

            if (_geoInformation == null || passedTime.TotalMinutes > MINIMUM_VALID_TIME)
            {
                _geoInformation = Retriever.Retrieve();
                _lastSuccessfulLocation = DateTime.UtcNow;
            }

            return _geoInformation;
        }
    }
}
