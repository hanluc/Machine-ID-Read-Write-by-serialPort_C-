using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //set id respond
        byte[] setIdResp = { 0x01,0x10,0x00,0x06,0x80,0x1f}; //going to add crcl and crch
        //message to aquire id
        byte[] aquireIdReq = { 0x01, 0x11, 0x00, 0x00, 0x00, 0x03, 0xBD, 0xC8 };

        //CRC
        byte[] aucCRCHi = {
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
		0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 
		0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
		0x00, 0xC1, 0x81, 0x40
        };

        byte[] aucCRCLo = {
        0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
		0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
		0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
		0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
		0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
		0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
		0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
		0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 
		0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
		0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
		0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
		0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
		0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 
		0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
		0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
		0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
		0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
		0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
		0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
		0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
		0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
		0x41, 0x81, 0x80, 0x40
};

        
        public Form1()
        {
            InitializeComponent();
        }

        /*------“获取”ID键---------*/
        private void button2_Click(object sender, EventArgs e)
        {
            //发送请求
            if (serialPort1.IsOpen)
            {
                serialPort1.Write(aquireIdReq, 0, 8);
            }

            //**test**  读取短路回传数据
            //byte[] BufferRead = new byte[11];
            //if (serialPort1.IsOpen)
            //    serialPort1.Read(BufferRead, 0, 11);
            ////serialPort1.DiscardInBuffer();
            //string id = "";
            //foreach (byte b in BufferRead)
            //{
            //    string str = b.ToString("x");  //type changes
            //    //id = id + str;
            //    id = id + "0x" + (str.Length == 1 ? "0" + str : str) + " ";
            //}
            //textBox2.Text = id;

            System.Threading.Thread.Sleep(10);     //等待IO数据收取完毕，如果出现数据收发错误请调整此数值
            //正式功能： 显示回传ID值
            textBox2.Text = recieveId(serialPort1);
            MessageBox.Show("读取完毕！");

        }

        /*------- “设置”ID键 -------------*/
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                MessageBox.Show("请输入一个ID号");
                return;
            }
            string idstr = textBox1.Text;   
            
            //发送ID值
            bool isSent = sendId(idstr,serialPort1);

            //检查回复
            if (isSent)
            {
                //**test** 显示短路返回的ID值
                //textBox4.Text = recieveId(serialPort1);

                //收取并检查回复信息
                bool hasReply = true;
                byte[] reciv = new byte[6];
                System.Threading.Thread.Sleep(10);    //10ms，等待IO数据收取完毕，如果出现数据收发错误请调整此数值  
                try
                {
                    serialPort1.Read(reciv, 0, 6);
                }
                catch
                {
                    MessageBox.Show("收取回复信息错误");
                }
                //检查校验码
                if (!checkCRC(reciv))
                {
                    MessageBox.Show("返回信息校验码错误，串口错误，请点击获取查看是否设置成功");
                    hasReply = false;
                }
                //检查内容
                for (int i = 0; i < 4; i++)
                {
                    //**test**
                    if (reciv[i] != setIdResp[i])
                        hasReply = false;
                }

                if (!hasReply)
                    MessageBox.Show("答复消息格式错误，ID设置失败");
                else
                    MessageBox.Show("设置ip成功！");
                    
                //显示回复的数据
                textBox4.Text = "";
                foreach (byte b in reciv)
                {
                    string str = b.ToString("x").ToUpper();
                    textBox4.Text += "0x" + (str.Length == 1 ? "0" + str : str) + " ";
                }
                    
            }
                       

        }

        /*---given an id String, send it by serial port---
         * 将字符串发送到指定端口
         * return bool isSent
         */
        private bool sendId(string idstr,SerialPort s)
        {
            string IDstr = idstr.ToUpper();
            byte[] BufferWrite = new byte[12];
            //消息前缀：
            BufferWrite[0] = 0x01;
            BufferWrite[1] = 0x10;
            BufferWrite[2] = 0x00;
            BufferWrite[3] = 0x06;

            //格式：6byte，前2byte为ascii，后四个byte为十六进制
            //first 2 chars are ascii chars, turn them into ASCII numbers,each is stored in 1 bytes
            //for (int i = 0; i < 2; i++)
            //{
               // char c = IDstr[i];
                //int mid = c - 'A';
                //MessageBox.Show(mid.ToString("x"));
                //BufferWrite[i+4] = Convert.ToByte(mid);
                
               // Console.Out.Write(BufferWrite[i + 4]);    //why cannot print out?
           // }

            //序列号前两位字符编码
            byte[] IDletters = Encoding.ASCII.GetBytes(IDstr.Substring(0,2));
            BufferWrite[4] = IDletters[0];
            BufferWrite[5] = IDletters[1];

            //序列号后4byte  last 4 bytes: " "  ;based on 16
            for (int i = 2; i < 6; i++)
            {
                byte Idnum = Convert.ToByte(IDstr.Substring(2*i-2, 2),16);//convert the string on basis of 16
                BufferWrite[i+4] = Idnum;
            }

            //校验码计算
            byte[] checksums = calCheckSum(BufferWrite);
            BufferWrite[10] = checksums[0];
            BufferWrite[11] = checksums[1];

            //发送序列号消息
            try
            {
                if (s.IsOpen)
                {
                    //s.Close();
                    //s.Open();
                    s.Write(BufferWrite, 0, 12);
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("数据发送出错！");
                return false;
            }

            return false;

        }


        /*-----given port , convert the byte data to ID---
         * 校验码检查+翻译字符串；接受11位数据
         * return ID string
         */
        private string recieveId(SerialPort s)
        {
            byte[] BufferedRead = new byte[11];
            if (s.IsOpen)
            {
                s.Read(BufferedRead, 0, 11);
                serialPort1.DiscardInBuffer();
            }
            else return "";

            string id = "";

            //消息头检查--3byte
            if (BufferedRead[0] != 0x01 || BufferedRead[1] != 0x11 || BufferedRead[2] != 0x06)
                MessageBox.Show("接收所得消息头格式错误！");

            //校验码检查
            if (!checkCRC(BufferedRead))
            {
                MessageBox.Show("数据校验码出错，请重新发送请求");
                //return null;
            }

            //ascii文字转换
            string prefixlet = Encoding.ASCII.GetString(BufferedRead,3,2);
            id += prefixlet;

            //numbers
            for (int i = 5; i < 9;i++ )
            {
                byte b = BufferedRead[i];
                string str = b.ToString("x").ToUpper();
                id += (str.Length == 1 ? "0" + str : str);
            }
            return id;
        }


        /* 计算byte串的校验码（最后2位为校验码）
         * 返回校验码数组
         */
        private byte[] calCheckSum(byte[] mesg)
        {
            byte ucCRCHi = 0xff;
            byte ucCRCLo = 0xff;
            UInt16 iIndex;

            for (int i = 0; i < mesg.Length - 2; i++)
            {
                iIndex = (ushort)Convert.ToInt16( ucCRCLo ^ mesg[i]);
                ucCRCLo = Convert.ToByte( ucCRCHi ^ aucCRCHi[iIndex]);
                ucCRCHi = aucCRCLo[iIndex];
            }
            byte[] CRC_CCITT = new byte[2];

            CRC_CCITT[0] = ucCRCLo;
            CRC_CCITT[1] = ucCRCHi;
            return CRC_CCITT;
        }

        /* 检查byte串中校验码是否正确
         * 输入：整串字符（含校验码）
         * 返回：检查结果 T or F
         */
        private bool checkCRC(byte[] mesg)
        {
            byte[] calCRC = calCheckSum(mesg);
            int Length = mesg.Length;
            //**test**显示校验码
            //MessageBox.Show("重新计算的校验码："+calCRC[0].ToString("x")+calCRC[1].ToString("x"));
            //MessageBox.Show("原校验码"+mesg[Length - 2].ToString("x") + mesg[Length-1].ToString("x"));


            if ((calCRC[0] == mesg[Length - 2]) && (calCRC[1] == mesg[Length - 1]))
                return true;
            return false;
        }

        /*--------“打开端口”键 -----------*/
        private void button3_Click(object sender, EventArgs e)
        {
            string str = comboBox1.Text;
          
            if(!serialPort1.IsOpen)
                serialPort1.PortName = str;
            byte[] buffer =new byte[1];
            buffer[0] = Convert.ToByte("ff", 16);
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    button3.Text = "打开端口";
                    MessageBox.Show("端口" + str + "已关闭", "端口提示");
                    button2.Enabled = false;
                    button1.Enabled = false;
                }
                else
                {
                    serialPort1.Open();
                    //serialPort1.Write(buffer, 0, 1);
                    //serialPort1.Close();
                    button3.Text = "关闭端口";
                    MessageBox.Show("端口" + str + "已打开", "端口提示");
                    button2.Enabled = true;
                    button1.Enabled = true;
                }
            }
            catch
            {
                if (serialPort1.IsOpen)
                    serialPort1.Close();
                MessageBox.Show("端口"+str+"连接错误","端口错误");

            }
        }

        /* 初始表格加载 */
        private void Form1_Load(object sender, EventArgs e)
        {

            //this.reportViewer1.RefreshReport();
            for (int i = 0;i<100;i++){
                comboBox1.Items.Add("COM"+i.ToString());
            }
            comboBox1.SelectedIndex = 3 ;
            button2.Enabled = false;
            button1.Enabled = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
