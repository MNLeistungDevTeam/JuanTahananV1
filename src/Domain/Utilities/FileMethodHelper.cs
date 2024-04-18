using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Domain.Utilities
{
    public class FileMethodHelper
    {
        public static async Task<string?> SaveFile(IFormFile? file, string location, string rootPath)
        {
            string toReturn = null;
            try
            {
                if (file == null)
                {
                    return null;
                }

                string fileName = await ExistingFileCheck(Path.Combine(rootPath, location, file.FileName));
                Directory.CreateDirectory(Path.Combine(rootPath, location));
                using var stream = new FileStream(Path.Combine(rootPath.Trim(), location.Trim(), fileName.Trim()), FileMode.Create);
                await file.CopyToAsync(stream);

                toReturn = "/" + location.Replace("\\", "/") + "/" + fileName;
            }
            catch (Exception)
            {
                return null;
            }

            return toReturn;
        }

        public static async Task<string> ExistingFileCheck(string FileNameLocation)
        {
            try
            {
                string NewName = "";

                await Task.Factory.StartNew(() =>
                {
                    if (File.Exists(FileNameLocation))
                    {
                        Random generator = new();
                        NewName = "uploadFile_" + generator.Next(0, 999999).ToString("D6") + Path.GetExtension(FileNameLocation);
                    }
                    else
                    {
                        NewName = Path.GetFileName(FileNameLocation);
                    }
                });

                return NewName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[]? FileToByteArray(string fileName)
        {
            byte[]? fileContent;

            using (FileStream fs = new(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryReader = new(fs))
            {
                long byteLength = new FileInfo(fileName).Length;
                fileContent = binaryReader.ReadBytes((int)byteLength);
            }

            return fileContent;
        }

        public static string GetStringBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }
    }
}
