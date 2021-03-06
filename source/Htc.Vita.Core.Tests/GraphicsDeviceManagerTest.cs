﻿using Htc.Vita.Core.IO;
using Htc.Vita.Core.Log;
using Xunit;

namespace Htc.Vita.Core.Tests
{
    public static class GraphicsDeviceManagerTest
    {
        [Fact]
        public static void Default_0_GetGraphicsAdapterList()
        {
            var graphicsAdapterList = GraphicsDeviceManager.GetGraphicsAdapterList();
            Assert.NotNull(graphicsAdapterList);
            Assert.NotEmpty(graphicsAdapterList);
            var index = 0;
            foreach (var graphicsAdapterInfo in graphicsAdapterList)
            {
                Logger.GetInstance(typeof(GraphicsDeviceManagerTest)).Info($"graphicsAdapterInfo[{index}].Description: {graphicsAdapterInfo.Description}");
                Logger.GetInstance(typeof(GraphicsDeviceManagerTest)).Info($"graphicsAdapterInfo[{index}].VendorId: {graphicsAdapterInfo.VendorId}");
                Logger.GetInstance(typeof(GraphicsDeviceManagerTest)).Info($"graphicsAdapterInfo[{index}].DeviceId: {graphicsAdapterInfo.DeviceId}");
                Logger.GetInstance(typeof(GraphicsDeviceManagerTest)).Info($"graphicsAdapterInfo[{index}].SubsystemVendorId: {graphicsAdapterInfo.SubsystemVendorId}");
                Logger.GetInstance(typeof(GraphicsDeviceManagerTest)).Info($"graphicsAdapterInfo[{index}].SubsystemDeviceId: {graphicsAdapterInfo.SubsystemDeviceId}");
                Logger.GetInstance(typeof(GraphicsDeviceManagerTest)).Info($"graphicsAdapterInfo[{index}].RevisionId: {graphicsAdapterInfo.RevisionId}");
                var index1 = 0;
                foreach (var graphicsDisplayInfo in graphicsAdapterInfo.DisplayList)
                {
                    Logger.GetInstance(typeof(GraphicsDeviceManagerTest)).Info($"graphicsAdapterInfo[{index}].DisplayList[{index1}].Name: {graphicsDisplayInfo.Name}");
                    var index2 = 0;
                    foreach (var graphicsMonitorInfo in graphicsDisplayInfo.MonitorList)
                    {
                        Logger.GetInstance(typeof(GraphicsDeviceManagerTest)).Info($"graphicsAdapterInfo[{index}].DisplayList[{index1}].MonitorList[{index2}].Name: {graphicsMonitorInfo.Name}");
                        Logger.GetInstance(typeof(GraphicsDeviceManagerTest)).Info($"graphicsAdapterInfo[{index}].DisplayList[{index1}].MonitorList[{index2}].Description: {graphicsMonitorInfo.Description}");
                        index2++;
                    }
                    index1++;
                }
                index++;
            }
        }
    }
}
