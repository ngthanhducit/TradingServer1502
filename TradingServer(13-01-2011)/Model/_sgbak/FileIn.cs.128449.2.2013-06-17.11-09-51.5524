using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace TradingServer.Model
{
    public class FileIn
    {
        public FileIn(string fileName, DateTime dateTime, bool isFolder,long size, FileStream file)
        {
            if (file != null)
            {
                try
                {
                    BinaryReader tre = new BinaryReader(file);
                    byte[] bin = tre.ReadBytes(Convert.ToInt32(file.Length));
                    this.FileContent = Convert.ToBase64String(bin);
                }
                catch
                {
                    this.FileContent = "";
                }
            }
            else
            {
                this.FileContent = "";
            }
            this.Size = size;
            this.IsFolder = isFolder;
            this.FileName = fileName;
            this.DateTime = dateTime;
        }
        public string FileName { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsFolder { get; set; }
        public string FileContent { get; set; }
        public long Size { get; set; }

    }
}
