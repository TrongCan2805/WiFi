using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace BAI9_ETHERNET
{
    public partial class Form1 : Form
    {
        IPEndPoint ipe;
        Socket server;
        Socket client;
        byte[] datasend = new byte[1]; // gui du lieu
        byte[] datareceive = new byte[1];  // nhan du lieu 
        int count = 0;

        // chuomg trinh khoi dong 
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // dong chuong trinh 
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            DialogResult answer = MessageBox.Show("Do you want to exits? ", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                if (bntDiscon.Enabled == true)
                {
                    datasend = Encoding.ASCII.GetBytes("Z");
                    client.Send(datasend, datasend.Length, SocketFlags.None);
                    server.Close();
                    client.Close();

                }
            }
        }

        // xu ly nut nhan conncet ( tao ket noi sever ) 
        private void bntCon_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(EndPoint_Thread);
            thread.IsBackground = true;
            thread.Start();

            textBox1.BackColor = Color.Yellow;
            textBox1.Text = " Waiting for device to connect";

            // khoa IP va Port 
            textBox_ServerIP_1.Enabled = false;
            textBox_ServerIP_2.Enabled = false;
            textBox_ServerIP_3.Enabled = false;
            textBox_ServerIP_4.Enabled = false;
            textBox_ServerPort.Enabled = false;
        }

        // tao ket noi vs server chinh 
        void EndPoint_Thread()
        {
            try
            {
                // thiet lap IPEndPoint va Socket

                string ip = textBox_ServerIP_1.Text.Trim() + "." +
                    textBox_ServerIP_2.Text.Trim() + "." +
                    textBox_ServerIP_3.Text.Trim() + "." +
                    textBox_ServerIP_4.Text.Trim();
                int port = int.Parse(textBox_ServerPort.Text.Trim());
                ipe = new IPEndPoint(IPAddress.Parse(ip), port);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // ket noi socket vs ipendpoint mo cong ket noi 
                server.Bind(ipe);

                // lang nghe tu Client 
                server.Listen(10); // cho phep ket noi 10 thieet bi

                // chap nhan ket noi 
                client = server.Accept();
                textBox1.BackColor = Color.Lime;
                // hieu chinh mau va thong tin 
                textBox1.Text = " Connected with: " + client.RemoteEndPoint.ToString();

                Thread thread = new Thread(Receive);
                thread.IsBackground = true;
                thread.Start();

                // cho phep cac nut nhan hoat dong 
                bntCon.Enabled = false;
                bntDiscon.Enabled = true;
                button3.Enabled = true;
                button4.Enabled = true;

            }
            catch (Exception)
            {
                MessageBox.Show(" Check the connection again. ", " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // hieu chinh mau va thong tin 
                textBox1.BackColor = Color.Red;
                textBox1.Text = "Not connect";

                // mo IP va Port 
                textBox_ServerIP_1.Enabled = true;
                textBox_ServerIP_2.Enabled = true;
                textBox_ServerIP_3.Enabled = true;
                textBox_ServerIP_4.Enabled = true;
                textBox_ServerPort.Enabled = true;

                // cam cac nut dieu khien hoat dong 
                bntCon.Enabled = true;
                bntDiscon.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
            }
        }


        // dong chuong trinh ngat ket noi 
        private void bntDiscon_Click(object sender, EventArgs e)
        {
            try
            {
                datasend = Encoding.ASCII.GetBytes("Z");
                client.Send(datasend, datasend.Length, SocketFlags.None);
                server.Close();
                client.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(" Check the connection again. ", " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // hieu chinh mau va thong tin 
                textBox1.BackColor = Color.Red;
                textBox1.Text = "Not connect";

                // mo IP va Port 
                textBox_ServerIP_1.Enabled = true;
                textBox_ServerIP_2.Enabled = true;
                textBox_ServerIP_3.Enabled = true;
                textBox_ServerIP_4.Enabled = true;
                textBox_ServerPort.Enabled = true;

                // cam cac nut dieu khien hoat dong 
                bntCon.Enabled = true;
                bntDiscon.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
            }
        }

        // nhan du lieu va xu ly anh giao dien 
        private void Receive()
        {
            try
            {
                while(true)
                {
                    int temp = client.Receive(datareceive);
                    string s = Encoding.ASCII.GetString(datareceive, 0, temp);
                    //tren empty
                    if (s == "q")
                    {
                        pictureBox1.Image = BAI9_ETHERNET.Properties.Resources.M1ON;
                        pictureBox2.Image = BAI9_ETHERNET.Properties.Resources.trenEMPTY;
                    }
                    //duoi Empty
                    else if (s == "w")
                    {
                        pictureBox1.Image = BAI9_ETHERNET.Properties.Resources.M1OFF;
                        pictureBox2.Image = BAI9_ETHERNET.Properties.Resources.duoiEMPTY;
                    }
                    //tren full
                    else if (s == "e")
                    {
                        pictureBox1.Image = BAI9_ETHERNET.Properties.Resources.M1ON;
                        pictureBox2.Image = BAI9_ETHERNET.Properties.Resources.trenFULL;

                    }
                }
            }
            catch(Exception)
            {
                client.Close();

            }
        }


        //  nut nhan START 
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                datasend = Encoding.ASCII.GetBytes("h");
                client.Send(datasend, datasend.Length, SocketFlags.None);

            }
            catch(Exception)
            {
                MessageBox.Show(" Check the connection again. ", " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        // nhan nut STOP 
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                datasend = Encoding.ASCII.GetBytes("j");
                client.Send(datasend, datasend.Length, SocketFlags.None);

            }
            catch (Exception)
            {
                MessageBox.Show(" Check the connection again. ", " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

     

        // kiem tra chi nhan gia tri so 
        private void textBox_ServerIP_1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // gioi han gia tri va khong bo trong 
        private void textBox_ServerIP_1_Validated(object sender, EventArgs e)
        {
            if( textBox_ServerIP_1.Text == "")
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 1 and 223 ", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_1.Text = "192";
                textBox_ServerIP_1.Focus();

            }
            else if (Int16.Parse(textBox_ServerIP_1.Text) < 0)
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 0 and 223 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_1.Text = "192";
                textBox_ServerIP_1.Focus();
            }
            else if (Int16.Parse(textBox_ServerIP_1.Text) > 223)
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 1 and 223 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_1.Text = "192";
                textBox_ServerIP_1.Focus();
            }
        }

        private void textBox_ServerIP_2_Validated(object sender, EventArgs e)
        {
            if (textBox_ServerIP_2.Text == "")
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 1 and 223 ", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_2.Text = "168";
                textBox_ServerIP_2.Focus();

            }
            else if (Int16.Parse(textBox_ServerIP_2.Text) < 0)
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 0 and 223 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_2.Text = "168";
                textBox_ServerIP_2.Focus();
            }
            else if (Int16.Parse(textBox_ServerIP_2.Text) > 223)
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 1 and 223 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_2.Text = "168";
                textBox_ServerIP_2.Focus();
            }
        }

        private void textBox_ServerIP_3_Validated(object sender, EventArgs e)
        {
            if (textBox_ServerIP_3.Text == "")
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 1 and 223 ", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_3.Text = "1";
                textBox_ServerIP_3.Focus();

            }
            else if (Int16.Parse(textBox_ServerIP_3.Text) < 0)
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 0 and 223 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_3.Text = "1";
                textBox_ServerIP_3.Focus();
            }
            else if (Int16.Parse(textBox_ServerIP_3.Text) > 223)
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 1 and 223 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_3.Text = "1";
                textBox_ServerIP_3.Focus();
            }
        }

        private void textBox_ServerIP_4_Validated(object sender, EventArgs e)
        {
            if (textBox_ServerIP_4.Text == "")
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 1 and 223 ", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_4.Text = "200";
                textBox_ServerIP_4.Focus();

            }
            else if (Int16.Parse(textBox_ServerIP_4.Text) < 0)
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 0 and 223 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_4.Text = "200";
                textBox_ServerIP_4.Focus();
            }
            else if (Int16.Parse(textBox_ServerIP_4.Text) > 223)
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 1 and 223 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerIP_4.Text = "200";
                textBox_ServerIP_4.Focus();
            }
        }

        private void textBox_ServerPort_Validated(object sender, EventArgs e)
        {
            if (textBox_ServerPort.Text == "")
            {
                MessageBox.Show(" Blank is not a validentry. Please specify a value between 0 and 65535 ", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerPort.Text = "8001";
                textBox_ServerPort.Focus();

            }
            else if (Int32.Parse(textBox_ServerPort.Text) < 0)
            {
                MessageBox.Show("  Blank is not a validentry. Please specify a value between 0 and 65535 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerPort.Text = "8001";
                textBox_ServerPort.Focus();
            }
            else if (Int32.Parse(textBox_ServerPort.Text) > 65535)
            {
                MessageBox.Show("  Blank is not a validentry. Please specify a value between 0 and 65535 ", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                textBox_ServerPort.Text = "8001";
                textBox_ServerPort.Focus();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            button6.Enabled = false;
            try
            {
                datasend = Encoding.ASCII.GetBytes("g");
                client.Send(datasend, datasend.Length, SocketFlags.None);

            }
            catch (Exception)
            {
                MessageBox.Show(" Check the connection again. ", " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            button5.Enabled = true;
            button6.Enabled = true;
            try
            {
                datasend = Encoding.ASCII.GetBytes("m");
                client.Send(datasend, datasend.Length, SocketFlags.None);

            }
            catch (Exception)
            {
                MessageBox.Show(" Check the connection again. ", " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

