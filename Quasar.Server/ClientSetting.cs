﻿using NewLife.Configuration;
using NewLife.Remoting.Clients;
using System;
using System.ComponentModel;

namespace Quasar.Server
{
    [Config("Client")]
    public class ClientSetting : Config<ClientSetting>, IClientSetting
    {
        #region 属性

        /// <summary>语音提示。默认true</summary>
        [Description("语音提示。默认true")]
        public Boolean SpeechTip { get; set; } = true;

        /// <summary>证书</summary>
        [Description("证书")]
        public String Code { get; set; }

        /// <summary>密钥</summary>
        [Description("密钥")]
        public String Secret { get; set; }

        /// <summary>服务地址端口。默认为空，子网内自动发现</summary>
        [Description("服务地址端口。默认为空，子网内自动发现")]
        public String Server { get; set; } = "";

        #endregion 属性

        #region 加载/保存

        protected override void OnLoaded()
        {
            if (NewLife.StringHelper.IsNullOrEmpty(Server)) Server = "http://47.113.219.65:6600";

            base.OnLoaded();
        }

        #endregion 加载/保存
    }
}