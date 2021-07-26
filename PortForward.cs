using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System;


namespace HTTP_Web_Server
{
    public class PortForward
    {
        public static string PFOut;
        private  static string _cmdreq;
        private static string cmdreq;

        public static void PF(string _ip , int _port , int _EXPort , string _protocol)
        {
            _cmdreq = "'"+"upnpc -a " + _ip + " " + _port + " " + _EXPort + " " + _protocol + " 0"+" "+"'";
            ProcessStartInfo procStartInfo = new ProcessStartInfo("/bin/bash" ,  " -c " +  _cmdreq);
            Console.WriteLine("/bin/bash" + " -c " + _cmdreq);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            PFOut = proc.StandardOutput.ReadToEnd();
        }

        public static void REMPF(int _Eport , string _protocol)
        {
            cmdreq = "upnpc -d " + _Eport + " " + _protocol;
            ProcessStartInfo procStartInfo = new ProcessStartInfo("/bin/bash" ,  " -c " + cmdreq);
            Console.WriteLine("/bin/bash" + " -c " + _cmdreq);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            PFOut = proc.StandardOutput.ReadToEnd();
        }
    }
}