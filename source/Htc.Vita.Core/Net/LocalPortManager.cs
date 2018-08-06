﻿using System;
using System.Net;
using System.Net.Sockets;
using Htc.Vita.Core.Log;

namespace Htc.Vita.Core.Net
{
    public class LocalPortManager
    {
        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            try
            {
                listener.Start();
                return ((IPEndPoint)listener.LocalEndpoint).Port;
            }
            catch (Exception e)
            {
                Logger.GetInstance(typeof(LocalPortManager)).Error("Can not get available port: " + e);
            }
            finally
            {
                listener.Stop();
            }
            return -1;
        }

        public static PortStatus GetPortStatus(int portNumber)
        {
            var listener = new TcpListener(IPAddress.Loopback, portNumber);
            try
            {
                listener.Start();
                return PortStatus.Available;
            }
            catch (SocketException e)
            {
                Logger.GetInstance(typeof(LocalPortManager)).Warn("Can not get available port: " + e.Message);
                return PortStatus.InUse;
            }
            catch (Exception e)
            {
                Logger.GetInstance(typeof(LocalPortManager)).Error("Can not get detect port status: " + e);
            }
            finally
            {
                listener.Stop();
            }

            return PortStatus.Unknown;
        }

        public enum PortStatus
        {
            Unknown,
            InUse,
            Available
        }
    }
}
