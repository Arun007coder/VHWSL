using System.Linq;
using System.ComponentModel;
using System.Data.Common;
using System.Security.Cryptography;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Net.NetworkInformation;

namespace HTTP_Web_Server
{
    public class HTTPServer
    {
        public const string VERSION = "HTTP/1.1";
        public const string NAME = "VHWSL v1.0.0";
        public static string MSG_DIR;
        public static string WEB_DIR;
        public static string LOG_DIR;
        private static string HNAme = Dns.GetHostName();
        public static IPAddress IP_Address;
        public static IPAddress IP;
        public static string DEF_WEB_DIR = Environment.CurrentDirectory + "/Root/web/";
        public static string DEF_MSG_DIR = Environment.CurrentDirectory + "/Root/msg/";
        public static string DEF_LOG_DIR = Environment.CurrentDirectory + "/Root/logs/";
        public static string SET_WEB_DIR;
        public static string SET_MSG_DIR;
        private static ProcessStartInfo StartInfo;
        public static string SET_LOG_DIR;
        private static IPAddress IPA;
        public static bool Lbool;
        public static bool CMDBool = true;
        public static string CMD1;
        public static string CMD2;
        public static string CMD3;
        public static string CMD1_Output;
        public static string CMD2_Output;
        public static string CMD3_output;
        public static string CMDEDIN6;
        public static string CMDEDIN7;
        public static string CMDEDIN5;

        private int Port;
        public static int EPort;
        public static bool CanFWD;
        public static System.Timers.Timer timer;
        private bool isRunning = false;
        public static string time;
        public static string FileName;
        public static bool klol = true;
        public static string msg;
        public static string nme = " Log.txt";
        public static bool isHacking = false;
        //private static PortForward FWD;

        private TcpListener TL;

        


        public static void SetTimer()
        {
            // Create a timer with a 1 second interval.
            timer = new System.Timers.Timer(1000);
            timer.Enabled = true;
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += TimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public static void TimedEvent(object source, ElapsedEventArgs l)
        {
            string time = DateTime.Now.ToString("dd/MM/yyyy hh:mm.ss tt");
            FileName = time + nme;
        }
        public HTTPServer(int _port)
        {
            
            Port = _port;
            if(IP_Address.ToString().Contains("192.168.1.") || IP_Address.ToString() == "127.0.0.1" )
            {
                if (Program.canPF)
                {
                    PortForward.PF(IP_Address.ToString() , Port , Port , "TCP");
                    Console.WriteLine("successfuly forwarded the port :" + Port + " to the internet");
                }
                else
                {
                    Console.WriteLine("Port Forwarding is disabled. You have to manually port forward ");
                }
                TL = new TcpListener(IP_Address, Port);
                Console.WriteLine("The server is listening to " + IP_Address + ":" + Port);
            }
            else
            {
                throw new Exception("IP_Address not correct");
            }
            
            

        }

        public void start()
        {
            Thread serverThread = new Thread(new ThreadStart(Run));
            serverThread.Start();
            
        }

        public static void stop()
        {
            if(Program.canPF)
            {
                PortForward.REMPF(30 , "TCP");
            }
            Console.WriteLine("Server is stopping ...");
            log(WEB_DIR + "/CMDOUT/CMD1_Output.txt", "null");
            log(WEB_DIR + "/CMDOUT/CMD2_Output.txt", "null");
            log(WEB_DIR + "/CMDOUT/CMD3_Output.txt", "null");
            Environment.Exit(0);

        }

        public static string GetLine(string txt, int lineNo)
        {
            //Console.WriteLine("debugreq : " + txt);
            string[] lines = txt.Replace("\r", "").Split('\n');
            return lines.Length >= lineNo ? lines[lineNo - 1] : null;
        }

        private void Run()
        {
            isRunning = true;
            TL.Start();

            while (isRunning == true)
            {



                try
                {

                    if (!isHacking)
                    {
                        Console.WriteLine("Waiting for connection ...");

                        TcpClient client = TL.AcceptTcpClient();

                        Console.WriteLine("Client connected!");

                        HandleClient(client);

                        client.Close();
                    }
                    else
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Thread.Sleep(30000);

                            isHacking = false;

                            Console.WriteLine("Waiting for connection ...");

                            TcpClient client = TL.AcceptTcpClient();

                            Console.WriteLine("Client connected!");

                            HandleClient(client);

                            client.Close();
                        });
                    }

                    
                }
                catch(IndexOutOfRangeException e)
                {
                    isHacking = true;
                    log(LOG_DIR +"Server was hacked or Index  outofbounds Exeption", e + "S002:Someone is trying to hack the server or trying to use nikto");;
                    Console.WriteLine("S002:Someone is trying to hack the server or trying to use nikto");
                    Console.WriteLine("Server is stopping because of security reasons.Refer Errors.txt");
                    Process.Start("notepad.exe", "Errors.txt");

                    Task.Factory.StartNew(() =>
                    {

                        System.Threading.Thread.Sleep(8000);
                        stop();
                    });
                }
                
            }

            isRunning = false;

            TL.Stop();
        }

        private void HandleClient(TcpClient client)
        {
            StreamReader reader = new StreamReader(client.GetStream());

            string msg = "";
            while (reader.Peek() != -1)
            {
                msg += reader.ReadLine() + "\n";
                string l = msg;
                //Console.WriteLine(l);
                
                CMDEDIN6 = GetLine(l, 6);
                CMDEDIN7 = GetLine(l, 7);
                CMDEDIN5 = GetLine(l, 8);
                log(LOG_DIR + "Debug Log.txt", CMDEDIN5 + "/n" + CMDEDIN6 + "/n" + CMDEDIN7);
                try
                {
                    if (msg.Contains("/"))
                    {
                        if (Lbool)
                        {
                            log(LOG_DIR + FileName, msg);
                        }
                    }
                    else
                    {
                        Console.WriteLine("loging service has occured an error");
                    }

                }
                catch(NullReferenceException)
                {
                    Console.WriteLine("loging service has occured an error");
                }
                
            }

            Debug.WriteLine("$ Request: \n" + msg);
            Console.WriteLine("$ Request: \n" + msg);
            Request req = Request.GetRequest(msg);
            Response resp = Response.From(req);
            resp.Post(client.GetStream());

        }

        


        public static void log(string _file, string _res)
        {
            string dir = LOG_DIR;
            // If directory does not exist, create it
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string Log = "$ Request : \n" + _res;
            using (StreamWriter sw = File.CreateText(_file))
            {
                sw.WriteLine(Log);
                sw.Close();
            }
        }
        public static string CMDCOM(string _cmdreq)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("/bin/bash" ,  " -c " + _cmdreq);
            Console.WriteLine("/bin/bash" + " -c " + _cmdreq);
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();

            return proc.StandardOutput.ReadToEnd();
        }

        public static void CMD(int slot)
        {
            if(slot == 0)
            {
                Console.WriteLine("Command is " + CMD1);
                CMD1_Output += CMDCOM(CMD1);
                Console.WriteLine(CMD1_Output);
                log(WEB_DIR + "/CMDOUT/CMD1_Output.txt", CMD1_Output);
            }

            if(slot == 1)
            {
                Console.WriteLine("Command is " + CMD2);
                CMD2_Output += CMDCOM(CMD2);
                Console.WriteLine(CMD2_Output);
                log(WEB_DIR + "/CMDOUT/CMD2_Output.txt", CMD2_Output);
            }

            if(slot == 2)
            {
                Console.WriteLine("Command is " + CMD3);
                CMD3_output += CMDCOM(CMD3);
                Console.WriteLine(CMD3_output);
                log(WEB_DIR + "/CMDOUT/CMD3_Output.txt", CMD3_output);
            }
        }

        public static void CLR(int slot)
        {
            if(slot == 0)
            {
                CMD1_Output = string.Empty;
                log(WEB_DIR + @"/CMDOUT/CMD1_Output.txt", "null");
            }

            if(slot == 1)
            {
                CMD2_Output = string.Empty;
                log(WEB_DIR + @"/CMDOUT/CMD2_Output.txt", "null");
            }

            if(slot == 2)
            {
                CMD3_output = string.Empty;
                log(WEB_DIR + @"/CMDOUT/CMD3_Output.txt", "null");
            }

            Console.WriteLine(slot + " cleared");

        }

        public static void EXT()
        {
            Console.WriteLine("Closing server ... ");
            stop();
            Environment.Exit(0);
             
        }

        public static void CMDET(int _slot ,  string _COMM)
        {
            if(_slot == 0)
            {
                CMD1 = _COMM;
                CMD(0);
            }

            if(_slot == 1)
            {
                CMD2 = _COMM;
                CMD(1);
            }
            if(_slot == 2)
            {
                CMD3 = _COMM;
                CMD(2);
            }
        }

        



    }



}
