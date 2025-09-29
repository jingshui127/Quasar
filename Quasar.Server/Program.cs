using NewLife;
using NewLife.Log;
using NewLife.Model;
using NewLife.Remoting.Clients;
using Quasar.Server.Forms;
using Stardust;
using System;
using System.Net;
using System.Windows.Forms;

namespace Quasar.Server
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            XTrace.UseWinForm();
            MachineInfo.RegisterAsync();

            StartClient();

            var set = ClientSetting.Current;

            // 启用语音提示
            StringHelper.EnableSpeechTip = set.SpeechTip;

            if (set.IsNew)
            {
                "新朋友您好！欢迎使用科控物联远程服务！".SpeechTip();
            }
            else
            {
                "欢迎您再次使用科控物联远程服务！".SpeechTip();
            }

            // 启用 TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
        private static StarFactory _factory;
        private static StarClient _Client;
        private static void StartClient()
        {
            var set = ClientSetting.Current;
            var server = set.Server;
            if (NewLife.StringHelper.IsNullOrEmpty(server)) return;

            XTrace.WriteLine("初始化服务端地址：{0}", server);

            _factory = new StarFactory(server, null, null)
            {
                Log = XTrace.Log,
            };

            var client = new StarClient(server)
            {
                Code = set.Code,
                Secret = set.Secret,
                ProductCode = _factory.AppId,
                Setting = set,

                Tracer = _factory.Tracer,
                Log = XTrace.Log,
            };

            client.Open();

            Host.RegisterExit(() => client.Logout("ApplicationExit"));

            _Client = client;
        }
    }
}
