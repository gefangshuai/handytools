using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace HandyTools.Common
{
    public class FileHelper
    {
        private const string ChangyongFileName = "changyong";
        /// <summary>
        /// 获取本地公共服务号码文件内容
        /// </summary>
        /// <returns></returns>
        public async static Task<string> GetChangyongDataFromLocalFile()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile storageFile = await storageFolder.GetFileAsync(ChangyongFileName);
                return await FileIO.ReadTextAsync(storageFile);
            }
            catch (FileNotFoundException exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 将公共服务号码内容写入本地文件
        /// </summary>
        /// <param name="data"></param>
        internal static async Task WriteChangyongDataToLocalFile(string data)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile storageFile =
                    await storageFolder.CreateFileAsync(ChangyongFileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(storageFile, data);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}
