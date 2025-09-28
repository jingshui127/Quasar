﻿using Open.Nat;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Quasar.Server.Networking
{
    public class UPnPService
    {
        /// <summary>
        /// 用于跟踪所有创建的映射。
        /// </summary>
        private readonly Dictionary<int, Mapping> _mappings = new Dictionary<int, Mapping>();

        /// <summary>
        /// 发现的UPnP设备。
        /// </summary>
        private NatDevice _device;

        /// <summary>
        /// 用于发现NAT-UPnP设备的NAT发现器。
        /// </summary>
        private NatDiscoverer _discoverer;

        /// <summary>
        /// 初始化新UPnP设备的发现。
        /// </summary>
        public UPnPService()
        {
            _discoverer = new NatDiscoverer();
        }

        /// <summary>
        /// 在UPnP设备上创建新的端口映射。
        /// </summary>
        /// <param name="port">要映射的端口。</param>
        public async void CreatePortMapAsync(int port)
        {
            try
            {
                var cts = new CancellationTokenSource(10000);
                _device = await _discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                Mapping mapping = new Mapping(Protocol.Tcp, port, port);

                await _device.CreatePortMapAsync(mapping);

                if (_mappings.ContainsKey(mapping.PrivatePort))
                    _mappings[mapping.PrivatePort] = mapping;
                else
                    _mappings.Add(mapping.PrivatePort, mapping);
            }
            catch (Exception ex) when (ex is MappingException || ex is NatDeviceNotFoundException)
            {
            }
        }

        /// <summary>
        /// 删除现有的端口映射。
        /// </summary>
        /// <param name="port">要删除的端口映射。</param>
        public async void DeletePortMapAsync(int port)
        {
            if (_mappings.TryGetValue(port, out var mapping))
            {
                try
                {
                    await _device.DeletePortMapAsync(mapping);
                    _mappings.Remove(mapping.PrivatePort);
                }
                catch (Exception ex) when (ex is MappingException || ex is NatDeviceNotFoundException)
                {
                }
            }
        }
    }
}
