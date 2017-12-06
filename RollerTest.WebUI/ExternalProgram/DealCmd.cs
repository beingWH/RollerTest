using System;
using System.Collections.Generic;

namespace RollerTest.WebUI.ExternalProgram
{
    public class DealCmd
    {
        /// <summary>
        /// 启动采样命令
        /// </summary>
        /// <returns></returns>
        public static byte[] GetCmdStartSample()
        {
            byte[] b = new byte[12];
            b[0] = 0x55;
            b[1] = 0xaa;
            b[2] = 0xaa;
            b[3] = 0x55;
            b[4] = 0x71;
            b[5] = 0x00;
            b[6] = 0x00;
            b[7] = 0x00;
            b[8] = 0x00;
            b[9] = 0x00;
            b[10] = 0x00;
            b[11] = 0x00;
            return b;
        }

        /// <summary>
        /// 停止采样命令
        /// </summary>
        /// <returns></returns>
        public static byte[] GetCmdStopSample()
        {
            byte[] b = new byte[12];
            b[0] = 0x55;
            b[1] = 0xaa;
            b[2] = 0xaa;
            b[3] = 0x55;
            b[4] = 0x73;
            b[5] = 0x00;
            b[6] = 0x00;
            b[7] = 0x00;
            b[8] = 0x00;
            b[9] = 0x00;
            b[10] = 0x00;
            b[11] = 0x00;
            return b;
        }

        /// <summary>
        /// 获取信号命令
        /// </summary>
        /// <returns></returns>
        public static byte[] GetCmdGetSerialSignal()
        {
            byte[] b = new byte[16];
            b[0] = 0x55;
            b[1] = 0xaa;
            b[2] = 0xaa;
            b[3] = 0x55;
            b[4] = 0x7f;
            b[5] = 0x00;
            b[6] = 0x00;
            b[7] = 0x00;
            b[8] = 0x04;
            b[9] = 0x00;
            b[10] = 0x00;
            b[11] = 0x00;
            b[12] = 0x00;
            b[13] = 0x00;
            b[14] = 0x00;
            b[15] = 0x00;
            return b;
        }
        public static byte[] GetCmdGetBlockSignal()
        {
            byte[] b = new byte[16];
            b[0] = 0x55;
            b[1] = 0xaa;
            b[2] = 0xaa;
            b[3] = 0x55;
            b[4] = 0x7f;
            b[5] = 0x00;
            b[6] = 0x00;
            b[7] = 0x00;
            b[8] = 0x04;
            b[9] = 0x00;
            b[10] = 0x00;
            b[11] = 0x00;
            b[12] = 0x01;
            b[13] = 0x00;
            b[14] = 0x00;
            b[15] = 0x00;
            return b;
        }
        public static byte[] GetCmdGetStatSignal()
        {
            byte[] b = new byte[16];
            b[0] = 0x55;
            b[1] = 0xaa;
            b[2] = 0xaa;
            b[3] = 0x55;
            b[4] = 0x7f;
            b[5] = 0x00;
            b[6] = 0x00;
            b[7] = 0x00;
            b[8] = 0x04;
            b[9] = 0x00;
            b[10] = 0x00;
            b[11] = 0x00;
            b[12] = 0x02;
            b[13] = 0x00;
            b[14] = 0x00;
            b[15] = 0x00;
            return b;
        }

        /// <summary>
        /// 处理 Cmd 108   获取信号
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public static string DealGetSignal(PackData pd)
        {
            List<byte> lstByte = pd.m_ByteData;
            byte[] tmpByte = new byte[4];
            byte[] tmpdata;
            tmpByte[0] = lstByte[12];
            tmpByte[1] = lstByte[13];
            tmpByte[2] = lstByte[14];
            tmpByte[3] = lstByte[15];
            pd.m_SignalType = BitConverter.ToInt32(tmpByte, 0);

            tmpdata = new byte[lstByte.Count - 16];
            for (int m = 0; m < lstByte.Count - 16; m++)
            {
                tmpdata[m] = lstByte[m + 16];
            }
            string res = System.Text.Encoding.Default.GetString(tmpdata);
            return res;
        }

        /// <summary>
        /// 处理 Cmd 124 //时间序列数据
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public static string DealSerialData(PackData pd)
        {
            List<byte> lstByte = pd.m_ByteData;
            byte[] tmpByte = new byte[4];
            byte[] tmpdata;
            //LONGLONG位置
            tmpByte = new byte[8];
            tmpByte[0] = lstByte[12];
            tmpByte[1] = lstByte[13];
            tmpByte[2] = lstByte[14];
            tmpByte[3] = lstByte[15];
            tmpByte[4] = lstByte[16];
            tmpByte[5] = lstByte[17];
            tmpByte[6] = lstByte[18];
            tmpByte[7] = lstByte[19];
            pd.m_Position = BitConverter.ToInt32(tmpByte, 0);

            //数据量
            tmpByte = new byte[4];
            tmpByte[0] = lstByte[20];
            tmpByte[1] = lstByte[21];
            tmpByte[2] = lstByte[22];
            tmpByte[3] = lstByte[23];
            pd.m_DataCount = BitConverter.ToInt32(tmpByte, 0);

            //通道数
            tmpByte = new byte[4];
            tmpByte[0] = lstByte[24];
            tmpByte[1] = lstByte[25];
            tmpByte[2] = lstByte[26];
            tmpByte[3] = lstByte[27];
            pd.m_SignalCount = BitConverter.ToInt32(tmpByte, 0);

            // 所有byte长度去除 (LONGLONG位置+数据量+通道数的数据) 再去除数据长度 剩下即为信号信息长度
            int m_SignalInfoLength = lstByte.Count - 28 - pd.m_DataCount * pd.m_SignalCount * 4;

            tmpdata = new byte[lstByte.Count - 28 - m_SignalInfoLength];
            for (int m = 0; m < lstByte.Count - 28 - m_SignalInfoLength; m++)
            {
                tmpdata[m] = lstByte[m + 28];
            }
            tmpByte = new byte[4];
            for (int m = 0; m < tmpdata.Length; m++)
            {
                tmpByte[m % 4] = tmpdata[m];
                if (m % 4 == 3)
                {
                    float data = BitConverter.ToSingle(tmpByte, 0);
                    pd.m_lstSerialSignalData.Add(data);
                }
            }

            //信号信息
            if (m_SignalInfoLength > 0)
            {
                //信号信息长度
                tmpByte = new byte[4];
                tmpByte[0] = lstByte[28 + pd.m_DataCount * pd.m_SignalCount * sizeof(int)];
                tmpByte[1] = lstByte[29 + pd.m_DataCount * pd.m_SignalCount * sizeof(int)];
                tmpByte[2] = lstByte[30 + pd.m_DataCount * pd.m_SignalCount * sizeof(int)];
                tmpByte[3] = lstByte[31 + pd.m_DataCount * pd.m_SignalCount * sizeof(int)];
                pd.m_SignalInfoLength = BitConverter.ToInt32(tmpByte, 0);

                tmpByte = new byte[pd.m_SignalInfoLength];
                for (int m = 0; m < pd.m_SignalInfoLength; m++)
                {
                    tmpByte[m] = lstByte[m + 28 + pd.m_DataCount * pd.m_SignalCount * sizeof(float) + 4];
                }
                pd.m_SignalInfo = System.Text.Encoding.Default.GetString(tmpByte);
            }

            //现在 pd.m_lstSignalData中的数据按照（通道1，通道2……通道n|通道1，通道2……通道n|...）如此排列
            //现修改为 （通道1数据，通道2数据……通道n数据）
            string txtSignalData = pd.m_SignalInfo;
            for (int n = 0; n < pd.m_SignalCount; n++)
            {
                txtSignalData += Environment.NewLine;
                for (int m = 0; m < pd.m_DataCount; m++)
                {
                    txtSignalData += pd.m_lstSerialSignalData[m * pd.m_SignalCount + n] + " ";
                }
            }
            txtSignalData += Environment.NewLine;
            return txtSignalData;
        }

        /// <summary>
        /// 处理 Cmd 123   启动采样前，发送数据的信号信息
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public static string DealTransferDataSignal(PackData pd)
        {
            List<byte> lstByte = pd.m_ByteData;
            byte[] tmpByte = new byte[4];
            byte[] tmpdata;
            tmpByte[0] = lstByte[12];
            tmpByte[1] = lstByte[13];
            tmpByte[2] = lstByte[14];
            tmpByte[3] = lstByte[15];
            pd.m_SignalType = BitConverter.ToInt32(tmpByte, 0);

            tmpdata = new byte[lstByte.Count - 16];
            for (int m = 0; m < lstByte.Count - 16; m++)
            {
                tmpdata[m] = lstByte[m + 16];
            }
            string res = System.Text.Encoding.Default.GetString(tmpdata);
            return res;
        }



        /// <summary>
        /// 处理 Cmd 125 // 统计数据
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public static string DealStatData(PackData pd)
        {
            //ChnCount + float * 2 * ChnCount
            List<byte> lstByte = pd.m_ByteData;
            byte[] tmpByte = new byte[4];
            byte[] tmpdata;

            //通道数
            tmpByte = new byte[4];
            tmpByte[0] = lstByte[12];
            tmpByte[1] = lstByte[13];
            tmpByte[2] = lstByte[14];
            tmpByte[3] = lstByte[15];
            pd.m_SignalCount = BitConverter.ToInt32(tmpByte, 0);

            int m_SignalInfoLength = lstByte.Count - 16 - pd.m_SignalCount * 2 * sizeof(float);

            tmpdata = new byte[lstByte.Count - 16 - m_SignalInfoLength];
            for (int m = 0; m < lstByte.Count - 16 - m_SignalInfoLength; m++)
            {
                tmpdata[m] = lstByte[m + 16];
            }
            tmpByte = new byte[4];
            StatData sd = new StatData(); ;
            for (int m = 0; m < tmpdata.Length; m++)
            {
                tmpByte[m % 4] = tmpdata[m];
                if (m % 4 == 3)
                {
                    if (m % 8 == 3)
                    {
                        sd = new StatData();
                        sd.m_Time = BitConverter.ToSingle(tmpByte, 0);
                    }
                    else if (m % 8 == 7)
                    {
                        sd.m_Data = BitConverter.ToSingle(tmpByte, 0);
                        pd.m_lstStatSignalData.Add(sd);
                    }
                }
            }

            //信号信息
            if (m_SignalInfoLength > 0)
            {
                //信号信息长度
                tmpByte = new byte[4];
                tmpByte[0] = lstByte[16 + pd.m_SignalCount * 2 * sizeof(int)];
                tmpByte[1] = lstByte[17 + pd.m_SignalCount * 2 * sizeof(int)];
                tmpByte[2] = lstByte[18 + pd.m_SignalCount * 2 * sizeof(int)];
                tmpByte[3] = lstByte[19 + pd.m_SignalCount * 2 * sizeof(int)];
                pd.m_SignalInfoLength = BitConverter.ToInt32(tmpByte, 0);

                tmpByte = new byte[pd.m_SignalInfoLength];
                for (int m = 0; m < pd.m_SignalInfoLength; m++)
                {
                    tmpByte[m] = lstByte[m + 16 + pd.m_SignalCount * 2 * sizeof(float) + 4];
                }
                pd.m_SignalInfo = System.Text.Encoding.Default.GetString(tmpByte);
            }

            string txtSignalData = pd.m_SignalInfo;
            for (int i = 0; i < pd.m_lstStatSignalData.Count; i++)
            {
                txtSignalData += Environment.NewLine;
                txtSignalData += pd.m_lstStatSignalData[i].m_Time + "," + pd.m_lstStatSignalData[i].m_Data + ";";
            }
            txtSignalData += Environment.NewLine;
            return txtSignalData;
        }

        /// <summary>
        /// 处理 Cmd 126 // 块数据
        /// </summary>
        /// <param name="pd"></param>
        /// <returns></returns>
        public static string DealBlockData(PackData pd)
        {
            //信号字符串长度 + 信号名 + 一个数据中包含几个float(1个Y或2个Y) + 数据量 + X数据 + Y
            List<byte> lstByte = pd.m_ByteData;
            byte[] tmpByte = new byte[4];
            byte[] tmpdata;

            //信号字符串长度
            tmpByte = new byte[4];
            tmpByte[0] = lstByte[12];
            tmpByte[1] = lstByte[13];
            tmpByte[2] = lstByte[14];
            tmpByte[3] = lstByte[15];
            pd.m_SignalNameLength = BitConverter.ToInt32(tmpByte, 0);

            //信号名
            tmpByte = new byte[pd.m_SignalNameLength];
            for (int i = 0; i < pd.m_SignalNameLength; i++)
            {
                tmpByte[i] = lstByte[16 + i];
            }
            pd.m_SignalName = System.Text.Encoding.Default.GetString(tmpByte);

            //一个数据中包含几个float
            tmpByte = new byte[4];
            tmpByte[0] = lstByte[16 + pd.m_SignalNameLength];
            tmpByte[1] = lstByte[17 + pd.m_SignalNameLength];
            tmpByte[2] = lstByte[18 + pd.m_SignalNameLength];
            tmpByte[3] = lstByte[19 + pd.m_SignalNameLength];
            pd.m_YCount = BitConverter.ToInt32(tmpByte, 0);

            //数据量
            tmpByte = new byte[4];
            tmpByte[0] = lstByte[20 + pd.m_SignalNameLength];
            tmpByte[1] = lstByte[21 + pd.m_SignalNameLength];
            tmpByte[2] = lstByte[22 + pd.m_SignalNameLength];
            tmpByte[3] = lstByte[23 + pd.m_SignalNameLength];
            pd.m_DataCount = BitConverter.ToInt32(tmpByte, 0);

            tmpdata = new byte[lstByte.Count - 24 - pd.m_SignalNameLength];
            for (int m = 0; m < lstByte.Count - 24 - pd.m_SignalNameLength; m++)
            {
                tmpdata[m] = lstByte[m + 24 + pd.m_SignalNameLength];
            }

            tmpByte = new byte[4];
            BlockData bd = new BlockData(); ;
            for (int m = 0; m < tmpdata.Length; m++)
            {
                tmpByte[m % 4] = tmpdata[m];
                if (m % 4 == 3)
                {
                    if (pd.m_YCount == 2)
                    {
                        if (m % 8 == 3)
                        {
                            bd = new BlockData();
                            bd.m_Time = BitConverter.ToSingle(tmpByte, 0);
                        }
                        else if (m % 8 == 7)
                        {
                            bd.m_Data1 = BitConverter.ToSingle(tmpByte, 0);
                            pd.m_lstBlockSignalData.Add(bd);
                        }
                    }
                    else if (pd.m_YCount == 3)
                    {
                        if (m % 12 == 3)
                        {
                            bd = new BlockData();
                            bd.m_Time = BitConverter.ToSingle(tmpByte, 0);
                        }
                        else if (m % 12 == 7)
                        {
                            bd.m_Data1 = BitConverter.ToSingle(tmpByte, 0);
                        }
                        else if (m % 12 == 11)
                        {
                            bd.m_Data2 = BitConverter.ToSingle(tmpByte, 0);
                            pd.m_lstBlockSignalData.Add(bd);
                        }
                    }
                }
            }
            string txtSignalData = "";
            for (int i = 0; i < pd.m_lstBlockSignalData.Count; i++)
            {
                txtSignalData += pd.m_lstBlockSignalData[i].m_Time + "," + pd.m_lstBlockSignalData[i].m_Data1;
                if (pd.m_YCount == 3)
                {
                    txtSignalData += "," + pd.m_lstBlockSignalData[i].m_Data2;
                }
            }
            return txtSignalData;
        }
    }

    public enum DATA_TYPE
    {
        DATA_TYPE_SERIAL,
        DATA_TYPE_BLOCK,
        DATA_TYPE_STAT
    }


}

