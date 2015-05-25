using AppLimit.CloudComputing.SharpBox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Dac.Data
{
    public class ImageStorage
    {
        CloudStorage dropboxStorage;

        protected void dropboxConnection()
        {
            dropboxStorage = new CloudStorage();

            ICloudStorageConfiguration dropboxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

            ICloudStorageAccessToken accessToken = null;

            FileStream fs = File.Open(@"C:\Project\token\SharpDropBox.Token",
               FileMode.Open, FileAccess.Read, FileShare.None);
            //WebRequest req = HttpWebRequest.Create("https://www.dropbox.com/s/2hg8aqtb6c6r5m9/SharpDropBox.Token?dl=1");

            accessToken = dropboxStorage.DeserializeSecurityToken(fs);
            dropboxStorage.Open(dropboxConfig, accessToken);
            fs.Close();

        }
        public void UploadFileToStorage(byte[] stream, string fileName)
        {
            dropboxConnection();
            
            Stream ss = new MemoryStream(stream);

            ICloudDirectoryEntry publicFolder = dropboxStorage.GetFolder("/image");
            dropboxStorage.UploadFile(ss, fileName, publicFolder);
            //dropboxStorage.UploadFile()
            dropboxStorage.Close();
        }
        public string ReadFileFromStorage(string fileName)
        {
            dropboxConnection();

            ICloudDirectoryEntry targetFolder = dropboxStorage.GetFolder("/image");
            string fileUrl = dropboxStorage.GetFileSystemObjectUrl(fileName, targetFolder).OriginalString;
            
            dropboxStorage.Close();
            
            return fileUrl;
        }
    }
}