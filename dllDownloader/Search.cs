using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Collections;
using System.Net;

namespace dllDownloader
{
    class Search
    {
        readonly Uri dllPath;
        ArrayList searchList;

        public ArrayList FileName { get; } = new ArrayList();
        public ArrayList FileLink { get; } = new ArrayList();

        public Search(Uri uri)
        {
            dllPath = uri;
        }

        public ArrayList SearchName(string name)
        {
            searchList = new ArrayList();

            for (int i = 0; i < FileName.Count; i++)
            {
                if (FileName[i].ToString().Contains(name))
                {
                    searchList.Add(FileName[i]);
                }
            }
            return searchList;
        }

        public int SearchIndex(string name)
        {
            int index = 0;
            for (int i = 0; i < FileName.Count; i++)
            {
                if(FileName[i].ToString().Equals(name))
                {
                    index = i;
                    break;
                }
                else { index = -1;  }
            }
            return index;
        }

        public int ReadList()
        {
            string content;
            string[] contentArray;
            int n = 0, l = 1;
            int result = 0;

            try
            {
                WebRequest wRequest = WebRequest.Create(dllPath);
                WebResponse wResponse = wRequest.GetResponse();

                Stream stream = wResponse.GetResponseStream();
                StreamReader sr = new StreamReader(stream);

                content = sr.ReadToEnd();
                if (sr.EndOfStream)
                {
                    contentArray = content.Split(new char[] { '\n', '\r', '|' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < contentArray.Length / 2; i++)
                    {
                        FileName.Add(contentArray[n]);
                        FileLink.Add(contentArray[l]);
                        n = n + 2;
                        l = l + 2;
                    }
                }
                else
                {
                    result = -1;
                }
                sr.Close();
                wResponse.Dispose();
            }
            catch (Exception e)
            {
                result = -1;
            }

            return result;
        }

        public Uri ReDirection(Uri link)
        {
            if (!Environment.Is64BitOperatingSystem)
            {
                if (link.ToString() == "https://download.visualstudio.microsoft.com/download/pr/9fbed7c7-7012-4cc0-a0a3-a541f51981b5/e7eec15278b4473e26d7e32cef53a34c/vc_redist.x64.exe") //2017
                    link = new Uri("https://download.visualstudio.microsoft.com/download/pr/d0b808a8-aa78-4250-8e54-49b8c23f7328/9c5e6532055786367ee61aafb3313c95/vc_redist.x86.exe");

                else if (link.ToString() == "http://download.microsoft.com/download/6/A/A/6AA4EDFF-645B-48C5-81CC-ED5963AEAD48/vc_redist.x64.exe") //2015
                    link = new Uri("http://download.microsoft.com/download/6/A/A/6AA4EDFF-645B-48C5-81CC-ED5963AEAD48/vc_redist.x86.exe");

                else if (link.ToString() == "http://download.microsoft.com/download/B/4/6/B46720B7-1A9A-458A-8B07-633E6DE4E760/vcredist_x64.exe") //2013
                    link = new Uri("http://download.microsoft.com/download/B/4/6/B46720B7-1A9A-458A-8B07-633E6DE4E760/vcredist_x86.exe");

                else if (link.ToString() == "http://download.microsoft.com/download/0/D/8/0D8C2D7C-75DD-409D-B70A-FDC0953343C1/VSU4/vcredist_x64.exe") //2012
                    link = new Uri("http://download.microsoft.com/download/0/D/8/0D8C2D7C-75DD-409D-B70A-FDC0953343C1/VSU4/vcredist_x86.exe");

                else if (link.ToString() == "http://download.microsoft.com/download/A/8/0/A80747C3-41BD-45DF-B505-E9710D2744E0/vcredist_x64.exe") //2010
                    link = new Uri("http://download.microsoft.com/download/C/6/D/C6D0FD4E-9E53-4897-9B91-836EBA2AACD3/vcredist_x86.exe");

                else if (link.ToString() == "https://download.microsoft.com/download/0/4/1/041b9d9f-852b-4a67-9e15-f4ebf699fe63/vcredist_x64.exe") //2008 SP1
                    link = new Uri("https://download.microsoft.com/download/5/6/3/563256db-7faf-440e-839e-c12efe19388d/vcredist_x86.exe");

            }
            return link;
        }

    }
}
