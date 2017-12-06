using Microsoft.AspNet.SignalR;
using RollerTest.Domain.Concrete;
using RollerTest.Domain.Entities;
using RollerTest.WebUI.Helpers;
using RollerTest.WebUI.IniFiles;
using RollerTest.WebUI.Models.PROCEDURE;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace RollerTest.WebUI.ExternalProgram
{
    public class DealControl
    {
        private static DealControl instance;
        private static readonly object locker = new object();
        private CancellationTokenSource Receivects;
        private CancellationTokenSource Dealcts;
        private CancellationTokenSource Queuects;
        private Task ReceiveTask;
        private Task DealTask;
        private Task QueueTask;
        private bool connectState=false;
        private List<ChannelData> channelList = new List<ChannelData>();
        private List<string> channelNum = new List<string>();
        private FaultData faultdata = new FaultData();
        private EFRecordinfoRepository recordrepo = new EFRecordinfoRepository();
        private EFSampleinfoRepository samplerepo = new EFSampleinfoRepository();
        private EFBaseRepository baserepo = new EFBaseRepository();
        private Queue<int> Queue1 = new Queue<int>(5);
        private Queue<int> Queue2 = new Queue<int>(5);
        private Queue<int> Queue3 = new Queue<int>(5);
        private Queue<int> Queue4 = new Queue<int>(5);
        private Queue<int> Queue5 = new Queue<int>(5);
        private Queue<int> Queue6 = new Queue<int>(5);
        private Queue<int> Queue7 = new Queue<int>(5);
        private Queue<int> Queue8 = new Queue<int>(5);
        private Queue<int> Queue9 = new Queue<int>(5);
        private Queue<int> Queue10 = new Queue<int>(5);
        private Queue<int> Queue11 = new Queue<int>(5);
        private Queue<int> Queue12 = new Queue<int>(5);
        private object m_lock = new object();
        private const int ReceiveDataCount = 4048;
        private const int PacketHeadSize = 12;
        private int nNetPos = 0;
        private Socket s;
        //网络缓存数据
        List<byte> m_NetData = new List<byte>();
        //处理缓存数据
        private byte[] m_TmpData;
        //所有待处理的包数据
        private List<PackData> m_lstPackData = new List<PackData>();
        private CdioControl cdioControl = CdioControl.GetInstance();
        private RollerTimer rollertimer = RollerTimer.GetInstance();
        private TestSampleInfo tsi = TestSampleInfo.GetInstance();
        private RollerForcer rollerforcer = RollerForcer.GetInstance();

        private DealControl()
        {
        
        }
   
  
        public static DealControl GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new DealControl();
                    }
                }
            }
            return instance;
        }
        public void DealConnect()
        {
            if (connectState == false)
            {
                connectState = this.SocketConnectState();
                Receivects = new CancellationTokenSource();
                Dealcts = new CancellationTokenSource();
                Queuects = new CancellationTokenSource();
                ReceiveTask = new Task(() => ReceiveData(), Receivects.Token);
                DealTask = new Task(() => DealData(), Dealcts.Token);
                ReceiveTask.Start();
                DealTask.Start();
                this.GetSignalInfo();
            }
        }
        public void DealConnectDis()
        {
            if (connectState == true)
            {
                sendExit();
                connectState = false;
            }
        }

        public bool getConnectState()
        {
            return this.connectState;
        }
        //连接用函数
        private bool SocketConnectState()
        {
            string txtIp = "192.168.0.30";
            string txtPort = "5003";
            IPEndPoint removeServer = new IPEndPoint(IPAddress.Parse(txtIp), int.Parse(txtPort));
            s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(removeServer);
            if (s.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void GetSignalInfo()
        {
            s.Send(DealCmd.GetCmdGetSerialSignal());
            s.Send(DealCmd.GetCmdGetBlockSignal());
            s.Send(DealCmd.GetCmdGetStatSignal());
        }

        //接收数据调用函数

        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveData()
        {
                while (!Receivects.IsCancellationRequested)
                {
                    byte[] recvData = new byte[ReceiveDataCount];
                    try
                    {
                        int nRecvCount = s.Receive(recvData); //接收数据，返回每次接收的字节总数
                        for (int i = 0; i < nRecvCount; i++)
                        {
                            m_NetData.Add(recvData[i]);
                        }
                        ParseBuffer();
                    }
                    catch
                    {
                        Console.WriteLine("ReceiveData Catch>>" + DateTime.Now);
                    }
                }
        }

        /// <summary>
        /// 处理缓存数据
        /// </summary>
        private void ParseBuffer()
        {
            m_TmpData = new byte[m_NetData.Count - nNetPos];
            Array.Copy(m_NetData.ToArray(), nNetPos, m_TmpData, 0, (m_NetData.Count - nNetPos));

            int nAlreadyRecCount = m_TmpData.Length;
            if (nAlreadyRecCount <= 0)
                return;

            // 查找包文头信息
            for (int i = 0; i < m_TmpData.Length - 3; i++)
            {
                if (m_TmpData[i] == 0x55 && m_TmpData[i + 1] == 0xaa && m_TmpData[i + 2] == 0xaa && m_TmpData[i + 3] == 0x55)
                {
                    FindPackHead(nAlreadyRecCount, i);
                }
            }
        }

        /// <summary>
        /// 找到包头标志后，对数据进行处理
        /// </summary>
        /// <param name="nAlreadyRecCount">接收的数据长度</param>
        /// <param name="nDataPointer">第几个字节开始为包头位置</param>
        private void FindPackHead(int nAlreadyRecCount, int nDataPointer)
        {
            // 找到一个数据报头信息
            // 网络包长度小于包头时不是一个完整的包
            if (nAlreadyRecCount - nDataPointer < PacketHeadSize)
                return;

            PackHead m_PackHead = new PackHead();
            SetPackHead(nDataPointer, ref m_PackHead);

            // 判断获得的数据除去表头 是否大于数据长度
            if (nAlreadyRecCount - nDataPointer - PacketHeadSize < m_PackHead.DataLength)
                return;

            //完整的包数据
            byte[] m_ComData = new byte[PacketHeadSize + m_PackHead.DataLength];
            Array.Copy(m_TmpData, nDataPointer, m_ComData, 0, PacketHeadSize + m_PackHead.DataLength);

            PackData pd = new PackData();
            pd.m_Index = m_lstPackData.Count;
            pd.m_Head = m_PackHead;
            pd.m_ByteData = m_ComData.ToList();

            lock (m_lock)
            {
                nNetPos += PacketHeadSize + pd.m_Head.DataLength;
                m_lstPackData.Add(pd);
            }
        }
        /// <summary>
        /// 设置包头
        /// </summary>
        private void SetPackHead(int nDataPointer, ref PackHead m_PackHead)
        {
            m_PackHead.Reset();
            m_PackHead.m_Signature[0] = m_TmpData[nDataPointer];
            m_PackHead.m_Signature[1] = m_TmpData[nDataPointer + 1];
            m_PackHead.m_Signature[2] = m_TmpData[nDataPointer + 2];
            m_PackHead.m_Signature[3] = m_TmpData[nDataPointer + 3];
            m_PackHead.m_Command[0] = m_TmpData[nDataPointer + 4];
            m_PackHead.m_Command[1] = m_TmpData[nDataPointer + 5];
            m_PackHead.m_Command[2] = m_TmpData[nDataPointer + 6];
            m_PackHead.m_Command[3] = m_TmpData[nDataPointer + 7];
            m_PackHead.m_Length[0] = m_TmpData[nDataPointer + 8];
            m_PackHead.m_Length[1] = m_TmpData[nDataPointer + 9];
            m_PackHead.m_Length[2] = m_TmpData[nDataPointer + 10];
            m_PackHead.m_Length[3] = m_TmpData[nDataPointer + 11];

            m_PackHead.DataLength = BitConverter.ToInt32(m_PackHead.m_Length, 0);
            m_PackHead.DataCommand = BitConverter.ToInt32(m_PackHead.m_Command, 0);
        }



        //处理数据调用函数

        /// <summary>
        /// 处理数据
        /// </summary>
        private void DealData()
        {
                while (!Dealcts.IsCancellationRequested)
                {
                    lock (m_lock)
                    {
                        if (m_lstPackData.Count == 0)
                        {
                            Thread.Sleep(100);
                        }
                        else
                        {
                            DealComData();
                            //处理完包数据后，将m_NetData中用过的数据清除
                            // 一直到 位置nNetPos 的m_NetData的数据已经用过
                            
                            m_NetData.RemoveRange(0, nNetPos);
                            nNetPos = 0;
                        }
                    }
                }   
        }


        /// <summary>
        /// 处理完整包数据
        /// </summary>
        public void DealComData()
        {
            string txtres = "";
            string res = "";
            List<String> value = new List<string>();
            for (int i = 0; i < m_lstPackData.Count; i++)
            {
                PackData pd = m_lstPackData[i];
                switch (m_lstPackData[i].m_Head.DataCommand)
                {
                    case 128:
                        res = DealCmd.DealGetSignal(pd);
                        if (res != string.Empty)
                        {
                            channelNum = GetChannel(res);
                        }
                        //txtSignalInfo += "Commond 128  信号类型：" + pd.m_SignalType + ";内容:" + res + Environment.NewLine;
                        break;
                    case 123:
                        res = DealCmd.DealTransferDataSignal(pd);
                        txtres += "Commond 123  信号类型：" + pd.m_SignalType + ";信号名称:" + res + Environment.NewLine;
                        break;
                    case 124:
                        res = DealCmd.DealSerialData(pd);
                        string[] sArray = res.Split(new char[] { '\r', '\n' },StringSplitOptions.RemoveEmptyEntries);
                        
                        int j = 0;
                        foreach(var p in channelNum)
                        {
                            string[] ssArray = sArray[j].Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries);
                            float[] klist = new float[ssArray.Count()];
                            i = 0;
                            foreach(var k in ssArray)
                            {
                                klist[i] = k.Trim().AsFloat();
                                i++;
                            }
                            ChannelData channeldata = new ChannelData()
                            {
                                channel = p,
                                
                                data =(int) klist.Average(),
                            };
                            channelList.Add(channeldata);
                            j++;
                        }
                        break;
                    case 125:
                        res = DealCmd.DealStatData(pd);
                        txtres += "Commond 125Stat  信号数：" + pd.m_SignalCount + ";数据：" + res + Environment.NewLine;
                        break;
                    case 126:
                        res = DealCmd.DealBlockData(pd);
                        txtres += "Commond 126Block  信号字符串长度：" + pd.m_SignalNameLength + ";信号名:" + pd.m_SignalName
                                + ";一个数据中包含几个float：" + pd.m_YCount + ";数据量：" + pd.m_DataCount + ";数据：" + res + Environment.NewLine;
                        break;
                }
            }
            m_lstPackData.Clear();
            if (channelList!=null)
            {
                HandleGetData(channelList);
                channelList.Clear();
            }
        }

        private void HandleGetData(List<ChannelData> info)
        {
            foreach(var p in info)
            {
                SendQueueData(p);
            }
        }

        private void SendQueueData(ChannelData channeldata) {
            try
            {
                switch (channeldata.channel)
                {
                    case "AI1-1-01": if (rollerforcer.getLimitSwitch("Forcer1")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer1")) { Queue1.Enqueue(channeldata.data); }; break;
                    case "AI1-1-02": if (rollerforcer.getLimitSwitch("Forcer2")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer2")) { Queue2.Enqueue(channeldata.data); }; break;
                    case "AI1-1-03": if (rollerforcer.getLimitSwitch("Forcer3")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer3")) { Queue3.Enqueue(channeldata.data); }; break;
                    case "AI1-1-04": if (rollerforcer.getLimitSwitch("Forcer4")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer4")) { Queue4.Enqueue(channeldata.data); }; break;
                    case "AI1-1-05": if (rollerforcer.getLimitSwitch("Forcer5")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer5")) { Queue5.Enqueue(channeldata.data); }; break;
                    case "AI1-1-06": if (rollerforcer.getLimitSwitch("Forcer6")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer6")) { Queue6.Enqueue(channeldata.data); }; break;
                    case "AI1-1-07": if (rollerforcer.getLimitSwitch("Forcer7")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer7")) { Queue7.Enqueue(channeldata.data); }; break;
                    case "AI1-1-08": if (rollerforcer.getLimitSwitch("Forcer8")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer8")) { Queue8.Enqueue(channeldata.data); }; break;
                    case "AI1-1-09": if (rollerforcer.getLimitSwitch("Forcer9")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer9")) { Queue9.Enqueue(channeldata.data); }; break;
                    case "AI1-1-10": if (rollerforcer.getLimitSwitch("Forcer10")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer10")) { Queue10.Enqueue(channeldata.data); }; break;
                    case "AI1-1-11": if (rollerforcer.getLimitSwitch("Forcer11")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer11")) { Queue11.Enqueue(channeldata.data); }; break;
                    case "AI1-1-12": if (rollerforcer.getLimitSwitch("Forcer12")) { Send(channeldata.channel, channeldata.data); }; if (rollerforcer.getJudgeSwitch("Forcer12")) { Queue12.Enqueue(channeldata.data); }; break;
                    default: break;
                }
                QueueTask = new Task(() => QueueData(), Queuects.Token);
                QueueTask.Start();
                QueueTask.Wait();
            }catch(Exception ex)
            {
                LogHelper.WriteLog(this.GetType(), ex);
            }
        }
        private void QueueData() {
            try {
                if (Queue1.Count > 5) { if (JudegLimit(Queue1, "1#")) { rollerforcer.setJudgeSwitch("Forcer1", false); }; Queue1.Dequeue(); }
                if (Queue2.Count > 5) { if (JudegLimit(Queue2, "2#")) { rollerforcer.setJudgeSwitch("Forcer2", false); }; Queue2.Dequeue(); }
                if (Queue3.Count > 5) { if (JudegLimit(Queue3, "3#")) { rollerforcer.setJudgeSwitch("Forcer3", false); }; Queue3.Dequeue(); }
                if (Queue4.Count > 5) { if (JudegLimit(Queue4, "4#")) { rollerforcer.setJudgeSwitch("Forcer4", false); }; Queue4.Dequeue(); }
                if (Queue5.Count > 5) { if (JudegLimit(Queue5, "5#")) { rollerforcer.setJudgeSwitch("Forcer5", false); }; Queue5.Dequeue(); }
                if (Queue6.Count > 5) { if (JudegLimit(Queue6, "6#")) { rollerforcer.setJudgeSwitch("Forcer6", false); }; Queue6.Dequeue(); }
                if (Queue7.Count > 5) { if (JudegLimit(Queue7, "7#")) { rollerforcer.setJudgeSwitch("Forcer7", false); }; Queue7.Dequeue(); }
                if (Queue8.Count > 5) { if (JudegLimit(Queue8, "8#")) { rollerforcer.setJudgeSwitch("Forcer8", false); }; Queue8.Dequeue(); }
                if (Queue9.Count > 5) { if (JudegLimit(Queue9, "9#")) { rollerforcer.setJudgeSwitch("Forcer9", false); }; Queue9.Dequeue(); }
                if (Queue10.Count > 5) { if (JudegLimit(Queue10, "10#")) { rollerforcer.setJudgeSwitch("Forcer10", false); }; Queue10.Dequeue(); }
                if (Queue11.Count > 5) { if (JudegLimit(Queue11, "11#")) { rollerforcer.setJudgeSwitch("Forcer11", false); }; Queue11.Dequeue(); }
                if (Queue12.Count > 5) { if (JudegLimit(Queue12, "12#")) { rollerforcer.setJudgeSwitch("Forcer12", false); }; Queue12.Dequeue(); }
            }catch(Exception ex)
            {
                LogHelper.WriteLog(this.GetType(), ex);
            }


        }
        private bool JudegLimit(Queue<int> queue,string station)
        {
            int ForcerDn = tsi.rollersampleinfos.FirstOrDefault(x => x.RollerBaseStation.Station == station).DnLimit;
            int ForcerUp = tsi.rollersampleinfos.FirstOrDefault(x => x.RollerBaseStation.Station == station).UpLimit;
            if (queue.Max() < ForcerDn || queue.Max() > ForcerUp)
                {
                    faultdata.station = station;
                    faultdata.UpLimit = ForcerUp.ToString();
                    faultdata.DnLimit = ForcerDn.ToString();
                    faultdata.Value = queue.Max().ToString();
                    Task faulttask = new Task(() => HandleFaultData());
                    faulttask.Start();
                    faulttask.Wait();
                    return true;
                }   
            return false;
        }

        private void HandleFaultData()
        {
            try {
                rollertimer.CloseRollerTimeSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == faultdata.station).TimerCfg.TimerName);
                rollerforcer.CloseRollerForcerSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == faultdata.station).ForcerCfg.ForcerName);
                int sampleId = samplerepo.RollerSampleInfos.FirstOrDefault(x => x.RollerBaseStation.Station == faultdata.station && x.State.Equals("开始")).RollerSampleInfoID;
                string totaltime = rollertimer.ReadTimeData(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == faultdata.station).TimerCfg.TimerName);
                recordrepo.SaveRollerRecordInfo(new RollerRecordInfo()
                {
                    CurrentTime = DateTime.Now,
                    SampleStatus = false,
                    RollerSampleInfoID = sampleId,
                    TotalTime = totaltime,
                    RecordInfo = "上限值：" + faultdata.UpLimit + "|下限值：" + faultdata.DnLimit + "|实际值：" + faultdata.Value
                });
                samplerepo.setsampleState(sampleId, "故障");
                Entities context = new Entities();
                context.PROCEDURE_ROLLERRECORDINFO(0);
                context.PROCEDURE_ROLLERSAMPLEINFO(0);
                context.SaveChanges();
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog(this.GetType(), ex);
            }

        }

        private List<string> GetChannel(string res)
        {
            List<string> tmpChannel = new List<string>();
            string[] temp = res.Split('|');
            for (int i = 0; i < temp.Length - 1; i++)
            {
                temp[i] = temp[i].Substring(0, 8);
                tmpChannel.Add(temp[i]);
            }
            return tmpChannel;
        }


        // 操作方法

        //发送“服务器退出提示”
        private void sendExit()
        {

            Receivects.Cancel();
            Dealcts.Cancel();
            Queuects.Cancel();
            ClearQueue();
            s.Shutdown(SocketShutdown.Both);
            s.Close();
        }
        private void ClearQueue()
        {
            Queue1.Clear(); Queue2.Clear(); Queue3.Clear(); Queue4.Clear();
            Queue5.Clear(); Queue6.Clear(); Queue7.Clear(); Queue8.Clear();
            Queue9.Clear(); Queue10.Clear(); Queue11.Clear(); Queue12.Clear();
        }

        bool CheckSocket()
        {
            bool res = false;
            if (s == null || s.Connected == false)
            {
                //MessageBox.Show("请点击连接...");
                res = true;
            }
            return res;
        }
        public void Send(string station, int data)
        {
            var dataHub = GlobalHost.ConnectionManager.GetHubContext("dataHub");
            dataHub.Clients.All.addNewDataToPage(station, data);
        }


    }

    public class ChannelData
    {
        public string channel;
        public int data;
    }
    public class FaultData
    {
        public string station;
        public string UpLimit;
        public string DnLimit;
        public string Value;
    }

    public class PackData
    {
        public int m_Index;
        public PackHead m_Head;
        /// <summary>
        /// 完整包数据
        /// </summary>
        public List<byte> m_ByteData;

        //命令123的相关信息
        public int m_SignalType;
        public List<string> m_lstSignalName;

        //命令124的相关信息
        /// <summary>
        /// 位置
        /// </summary>
        public long m_Position;
        /// <summary>
        /// 数据量 每个通道发过来的数据数量
        /// </summary>
        public int m_DataCount;
        /// <summary>
        /// 通道数
        /// </summary>
        public int m_SignalCount;
        /// <summary>
        /// 发来的时间序列数据
        /// </summary>
        public List<float> m_lstSerialSignalData = new List<float>();

        //命令125的相关信息
        /// <summary>
        /// 发来的统计数据
        /// </summary>
        public List<StatData> m_lstStatSignalData = new List<StatData>();

        //命令126的相关信息
        /// <summary>
        /// 信号字符串长度
        /// </summary>
        public int m_SignalNameLength;
        /// <summary>
        /// 信号名
        /// </summary>
        public string m_SignalName;
        /// <summary>
        /// 信号信息
        /// </summary>
        public string m_SignalInfo = "";
        /// <summary>
        /// 数据量 每个通道发过来的数据数量
        /// </summary>
        public int m_SignalInfoLength;
        /// <summary>
        /// 一个数据中包含几个float
        /// </summary>
        public int m_YCount;
        /// <summary>
        /// 发来的块数据
        /// </summary>
        public List<BlockData> m_lstBlockSignalData = new List<BlockData>();
    }

    public class StatData
    {
        public float m_Time;
        public float m_Data;
    }

    public class BlockData
    {
        public float m_Time;
        /// <summary>
        /// 一个数据中包含1个float 则使用m_Data1,否则用m_Data1，m_Data2
        /// </summary>
        public float m_Data1;
        public float m_Data2;
    }

    public class PackHead
    {
        public byte[] m_Signature;
        public byte[] m_Command;
        public byte[] m_Length;
        public int DataLength;
        public int DataCommand;

        public void Reset()
        {
            m_Signature = new byte[4];
            m_Command = new byte[4];
            m_Length = new byte[4];
        }
    }

    public class RollerForcer
    {
        private RollerForcer()
        {

        }
        private EFBaseRepository baserepo = new EFBaseRepository();
        private static RollerForcer instance;
        private static readonly object locker = new object();
        public static RollerForcer GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new RollerForcer();
                    }
                }
            }
            return instance;
        }
        public bool JudgeLimitSwitch1;
        public bool JudgeLimitSwitch2;
        public bool JudgeLimitSwitch3;
        public bool JudgeLimitSwitch4;
        public bool JudgeLimitSwitch5;
        public bool JudgeLimitSwitch6;
        public bool JudgeLimitSwitch7;
        public bool JudgeLimitSwitch8;
        public bool JudgeLimitSwitch9;
        public bool JudgeLimitSwitch10;
        public bool JudgeLimitSwitch11;
        public bool JudgeLimitSwitch12;
        public bool LimitSwitch1;
        public bool LimitSwitch2;
        public bool LimitSwitch3;
        public bool LimitSwitch4;
        public bool LimitSwitch5;
        public bool LimitSwitch6;
        public bool LimitSwitch7;
        public bool LimitSwitch8;
        public bool LimitSwitch9;
        public bool LimitSwitch10;
        public bool LimitSwitch11;
        public bool LimitSwitch12;
        public void OpenRollerForcerSwitch(string ForceName)
        {
            switch (ForceName)
            {
                case "Forcer1":
                    LimitSwitch1 = true; baserepo.SaveForceSwitch("Forcer1", true);
                    break;
                case "Forcer2":
                    LimitSwitch2 = true; baserepo.SaveForceSwitch("Forcer2", true);
                    break;
                case "Forcer3":
                    LimitSwitch3 = true; baserepo.SaveForceSwitch("Forcer3", true);
                    break;
                case "Forcer4":
                    LimitSwitch4 = true; baserepo.SaveForceSwitch("Forcer4", true);
                    break;
                case "Forcer5":
                    LimitSwitch5 = true; baserepo.SaveForceSwitch("Forcer5", true);
                    break;
                case "Forcer6":
                    LimitSwitch6 = true; baserepo.SaveForceSwitch("Forcer6", true);
                    break;
                case "Forcer7":
                    LimitSwitch7 = true; baserepo.SaveForceSwitch("Forcer7", true);
                    break;
                case "Forcer8":
                    LimitSwitch8 = true; baserepo.SaveForceSwitch("Forcer8", true);
                    break;
                case "Forcer9":
                    LimitSwitch9 = true; baserepo.SaveForceSwitch("Forcer9", true);
                    break;
                case "Forcer10":
                    LimitSwitch10 = true; baserepo.SaveForceSwitch("Forcer10", true);
                    break;
                case "Forcer11":
                    LimitSwitch11 = true; baserepo.SaveForceSwitch("Forcer11", true);
                    break;
                case "Forcer12":
                    LimitSwitch12 = true; baserepo.SaveForceSwitch("Forcer12", true);
                    break;
                default: break;
            }
        }
        public void CloseRollerForcerSwitch(string ForceName)
        {
            switch (ForceName)
            {
                case "Forcer1":
                    LimitSwitch1 = false; baserepo.SaveForceSwitch("Forcer1", false);
                    break;
                case "Forcer2":
                    LimitSwitch2 = false; baserepo.SaveForceSwitch("Forcer2", false);
                    break;
                case "Forcer3":
                    LimitSwitch3 = false; baserepo.SaveForceSwitch("Forcer3", false);
                    break;
                case "Forcer4":
                    LimitSwitch4 = false; baserepo.SaveForceSwitch("Forcer4", false);
                    break;
                case "Forcer5":
                    LimitSwitch5 = false; baserepo.SaveForceSwitch("Forcer5", false);
                    break;
                case "Forcer6":
                    LimitSwitch6 = false; baserepo.SaveForceSwitch("Forcer6", false);
                    break;
                case "Forcer7":
                    LimitSwitch7 = false; baserepo.SaveForceSwitch("Forcer7", false);
                    break;
                case "Forcer8":
                    LimitSwitch8 = false; baserepo.SaveForceSwitch("Forcer8", false);
                    break;
                case "Forcer9":
                    LimitSwitch9 = false; baserepo.SaveForceSwitch("Forcer9", false);
                    break;
                case "Forcer10":
                    LimitSwitch10 = false; baserepo.SaveForceSwitch("Forcer10", false);
                    break;
                case "Forcer11":
                    LimitSwitch11 = false; baserepo.SaveForceSwitch("Forcer11", false);
                    break;
                case "Forcer12":
                    LimitSwitch12 = false; baserepo.SaveForceSwitch("Forcer12", false);
                    break;
                default: break;
            }
        }
        public bool getLimitSwitch(string ForcerName)
        {
            bool state = false;
            switch (ForcerName)
            {
                case "Forcer1": state =LimitSwitch1; break;
                case "Forcer2": state = LimitSwitch2; break;
                case "Forcer3": state = LimitSwitch3; break;
                case "Forcer4": state = LimitSwitch4; break;
                case "Forcer5": state = LimitSwitch5; break;
                case "Forcer6": state = LimitSwitch6; break;
                case "Forcer7": state = LimitSwitch7; break;
                case "Forcer8": state =LimitSwitch8; break;
                case "Forcer9": state = LimitSwitch9; break;
                case "Forcer10": state = LimitSwitch10; break;
                case "Forcer11": state = LimitSwitch11; break;
                case "Forcer12": state =LimitSwitch12; break;
                default: break;
            }
            return state;
        }
        public void OpenAllLimitSwtich()
        {
            OpenRollerForcerSwitch("Forcer1");
            OpenRollerForcerSwitch("Forcer2");
            OpenRollerForcerSwitch("Forcer3");
            OpenRollerForcerSwitch("Forcer4");
            OpenRollerForcerSwitch("Forcer5");
            OpenRollerForcerSwitch("Forcer6");
            OpenRollerForcerSwitch("Forcer7");
            OpenRollerForcerSwitch("Forcer8");
            OpenRollerForcerSwitch("Forcer9");
            OpenRollerForcerSwitch("Forcer10");
            OpenRollerForcerSwitch("Forcer11");
            OpenRollerForcerSwitch("Forcer12");
        }
        public void CloseAllLimitSwtich()
        {
            CloseRollerForcerSwitch("Forcer1");
            CloseRollerForcerSwitch("Forcer2");
            CloseRollerForcerSwitch("Forcer3");
            CloseRollerForcerSwitch("Forcer4");
            CloseRollerForcerSwitch("Forcer5");
            CloseRollerForcerSwitch("Forcer6");
            CloseRollerForcerSwitch("Forcer7");
            CloseRollerForcerSwitch("Forcer8");
            CloseRollerForcerSwitch("Forcer9");
            CloseRollerForcerSwitch("Forcer10");
            CloseRollerForcerSwitch("Forcer11");
            CloseRollerForcerSwitch("Forcer12");

        }
        public void setJudgeSwitch(string ForcerName, bool state)
        {
            switch (ForcerName)
            {
                case "Forcer1": JudgeLimitSwitch1 = state; break;
                case "Forcer2": JudgeLimitSwitch2 = state; break;
                case "Forcer3": JudgeLimitSwitch3 = state; break;
                case "Forcer4": JudgeLimitSwitch4 = state; break;
                case "Forcer5": JudgeLimitSwitch5 = state; break;
                case "Forcer6": JudgeLimitSwitch6 = state; break;
                case "Forcer7": JudgeLimitSwitch7 = state; break;
                case "Forcer8": JudgeLimitSwitch8 = state; break;
                case "Forcer9": JudgeLimitSwitch9 = state; break;
                case "Forcer10": JudgeLimitSwitch10 = state; break;
                case "Forcer11": JudgeLimitSwitch11 = state; break;
                case "Forcer12": JudgeLimitSwitch12 = state; break;
                default: break;
            }
        }
        public bool getJudgeSwitch(string ForcerName)
        {
            bool state = false;
            switch (ForcerName)
            {
                case "Forcer1": state=JudgeLimitSwitch1; break;
                case "Forcer2": state = JudgeLimitSwitch2 ; break;
                case "Forcer3": state = JudgeLimitSwitch3; break;
                case "Forcer4": state = JudgeLimitSwitch4 ; break;
                case "Forcer5": state = JudgeLimitSwitch5; break;
                case "Forcer6": state = JudgeLimitSwitch6; break;
                case "Forcer7": state = JudgeLimitSwitch7; break;
                case "Forcer8": state = JudgeLimitSwitch8 ; break;
                case "Forcer9": state = JudgeLimitSwitch9; break;
                case "Forcer10": state = JudgeLimitSwitch10; break;
                case "Forcer11": state = JudgeLimitSwitch11 ; break;
                case "Forcer12": state = JudgeLimitSwitch12; break;
                default: break;
            }
            return state;
        }

    }
}