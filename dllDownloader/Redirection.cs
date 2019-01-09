using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace dllDownloader
{
    class ReDirection
    {
        public Uri ReDirectionX86(Uri link)
        {
            string fileName = Path.GetFileName(link.ToString());
            if (!Environment.Is64BitOperatingSystem)
            {
                if(link.ToString() == "https://download.visualstudio.microsoft.com/download/pr/9fbed7c7-7012-4cc0-a0a3-a541f51981b5/e7eec15278b4473e26d7e32cef53a34c/vc_redist.x64.exe") //2017
                    link = new Uri("https://download.visualstudio.microsoft.com/download/pr/d0b808a8-aa78-4250-8e54-49b8c23f7328/9c5e6532055786367ee61aafb3313c95/vc_redist.x86.exe");

                else if(link.ToString() == "https://download.microsoft.com/download/9/3/F/93FCF1E7-E6A4-478B-96E7-D4B285925B00/vc_redist.x64.exe") //2015
                    link = new Uri("https://download.microsoft.com/download/9/3/F/93FCF1E7-E6A4-478B-96E7-D4B285925B00/vc_redist.x86.exe");

                else if (link.ToString() == "http://download.microsoft.com/download/f/8/d/f8d970bd-4218-49b9-b515-e6f1669d228b/vcredist_x64.exe") //2013
                    link = new Uri("http://download.microsoft.com/download/F/8/D/F8D970BD-4218-49B9-B515-E6F1669D228B/vcredist_x86.exe");

                else if(link.ToString() == "https://download.microsoft.com/download/0/D/8/0D8C2D7C-75DD-409D-B70A-FDC0953343C1/VSU4/vcredist_x64.exe") //2012
                    link = new Uri("https://download.microsoft.com/download/0/D/8/0D8C2D7C-75DD-409D-B70A-FDC0953343C1/VSU4/vcredist_x86.exe");

                else if (link.ToString() == "https://download.microsoft.com/download/3/2/2/3224B87F-CFA0-4E70-BDA3-3DE650EFEBA5/vcredist_x64.exe") //2010
                    link = new Uri("https://download.microsoft.com/download/5/B/C/5BC5DBB3-652D-4DCE-B14A-475AB85EEF6E/vcredist_x86.exe");
            }
            return link;
        }
    }
}
