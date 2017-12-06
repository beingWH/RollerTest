using CdioCs;

namespace RollerTest.WebUI.ExternalProgram
{
    public class CdioMethod
    {
        private short m_Id;
        private int Ret;
        Cdio dio = new Cdio();
        public CdioMethod()
        {
            Ret = dio.Init("DIO000", out m_Id);
            //Ret = 0;//模拟电机运行
        }
        public string InitDio()
        {
            string ErrorString;
            dio.GetErrorString(Ret, out ErrorString);
            if (Ret != 0)
            {
                return ErrorString;
            }
            else
            {
                writeBit(0, 0);
                writeBit(1, 0);
                writeBit(2, 0);
                writeBit(3, 0);
                writeBit(4, 0);
                writeBit(5, 0);
                writeBit(6, 0);
                return "Success";
            }
        }
        public string InitDioState()
        {
            string ErrorString;
            dio.GetErrorString(Ret, out ErrorString);
            if (Ret != 0)
            {
                return ErrorString;
            }
            else
            {
                return "Success";
            }
        }
        public void MotoStop()
        {
            writeBit(0, 0);
            writeBit(1, 0);
        }
        public void MotoRun(int REV)
        {
            if (REV == 0)
            {
                writeBit(0, 1);
                writeBit(1, 0);
            }
            else
            {
                writeBit(0, 0);
                writeBit(1, 1);
            }
        }
        private bool readBit(short InpBitNo)
        {
            int Ret;
            byte InpBitData;
            Ret = dio.InpBit(m_Id, InpBitNo, out InpBitData);
            if (InpBitData == 1)
                return true;
            else
                return false;
        }

        private void writeBit(short OutBitNo, byte OutBitData)
        {
            int Ret;
            Ret = dio.OutBit(m_Id, OutBitNo, OutBitData);
        }
    }  
}