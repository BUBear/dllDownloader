using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.IO;
using System.Diagnostics;
using System.Management;

namespace dllDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Uri dllListUri = new Uri("리스트링크");
        Search search;
        ArrayList searchList = new ArrayList();


        string tempPath = Application.StartupPath + @"\DllFile\"; //Path.GetTempPath() 임시폴더 - %userprofile%\appdata\Local\Temp\
        string movePath = Environment.GetFolderPath(Environment.SpecialFolder.System);
        string movePathX86 = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86); //SysWOW64
        string fileName;

        private void Form1_Load(object sender, EventArgs e)
        {
            ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                InfoText.Text = mo["Caption"].ToString() + " " + mo["OSArchitecture"].ToString();
            }

            search = new Search(dllListUri);
            if (search.ReadList() != -1)
            {
                foreach (string list in search.FileName)
                {
                    listBox1.Items.Add(list);
                }
            }
            else
            {
                MessageBox.Show("오류: 파일 리스트를 읽지 못했습니다.", "오류");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (textBox1.Text != string.Empty)
            {
                searchList = search.SearchName(textBox1.Text);

                foreach (string list in searchList)
                {
                    listBox1.Items.Add(list);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string index = listBox1.SelectedItem.ToString();
            string link = search.FileLink[search.SearchIndex(index)].ToString();
            fileName = Path.GetFileName(link);

            if(!Directory.Exists(tempPath))
                Directory.CreateDirectory(Application.StartupPath + @"\DllFile");

            WebClient wc = new WebClient();
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgress);
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompleted);
            wc.DownloadFileAsync(search.ReDirection(new Uri(link)), tempPath + fileName);
        }

        private void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            bool dllMoveCheck = false;
            Process proc = new Process();

            MessageBox.Show("파일 다운로드 완료 >>" + tempPath + fileName);
            progressBar1.Value = 0;

            if (checkBox1.Checked)
            {
                if (IsDll(tempPath + fileName))
                {
                    if (checkBox2.Checked)
                    {
                        if (Environment.Is64BitOperatingSystem)
                        {
                            File.Copy(tempPath + fileName, movePath);
                            File.Copy(tempPath + fileName, movePathX86);
                        }
                        else
                            File.Copy(tempPath + fileName, movePath);
                    }
                    else
                        dllMoveCheck = true;
                }
                else
                {
                    try
                    {
                        proc.EnableRaisingEvents = true;
                        proc.Exited += new EventHandler(FileExited);
                        proc = Process.Start(tempPath + fileName);
                        proc.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("오류: " + ex.Message, "오류");
                    }
                }
            }

            if(checkBox3.Checked)
            {
                if (!dllMoveCheck)
                {
                    if (proc.HasExited)
                    {
                        Directory.Delete(tempPath, true);
                    }
                }
            }


        }

        private void FileExited(object sender, EventArgs e)
        {
            Directory.Delete(tempPath, true);
        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Maximum = (int)e.TotalBytesToReceive;
            progressBar1.Value = (int)e.BytesReceived;
        }

        private bool IsDll(string file)
        {
            string fileExtension;
            bool result;

            FileInfo fileinfo = new FileInfo(file);
            fileExtension = fileinfo.Extension;
            if (fileExtension == "dll" || fileExtension == "DLL")
                result = true;
            else
                result = false;

            return result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            if (textBox1.Text != string.Empty)
            {
                searchList = search.SearchName(textBox1.Text);

                foreach (string list in searchList)
                {
                    listBox1.Items.Add(list);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
