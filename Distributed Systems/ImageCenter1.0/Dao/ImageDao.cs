using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ImageCenter
{
    internal class ImageDao
    {
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="files"></param>
        /// <param name="imageType"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        internal static string Save(HttpFileCollectionBase files, ImageTypeEnum imageType, int width = 1366, int height = 768)
        {
            string locaAbsPath = string.Format("{0}/{1}/", Static.FileDirPath, Util.Enum.GetDescriptionString(imageType));

            Util.IO.Directory.Create(locaAbsPath);

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < files.Count; i++)
            {
                var file = files[i];

                if (file.ContentLength <= 0)
                {
                    continue;
                }

                string filename = Util.Utility.MD5.GetMD5(string.Format("{0}{1}{2}{3}", DateTime.Now.ToString(), Static.ImageFileOrg, i, Guid.NewGuid()));
                string fileAbsPath = string.Format("{0}{1}{2}", locaAbsPath, filename, ".jpg");
                string newfileAbsPath = EncryptImageNameAndPath(fileAbsPath);

                file.SaveAs(fileAbsPath);

                Util.IO.StreamUtil.ResizeImage(fileAbsPath, width, height);

                result.Append(string.Format("{0},", newfileAbsPath));
            }

            if (result.Length > 0)
            {
                result.Length--;
            }
            return result.ToString();
        }

        /// <summary>
        /// 加密文件名和路径
        /// </summary>
        /// <param name="imageNameAndPath"></param>
        /// <returns></returns>
        private static string EncryptImageNameAndPath(string imageNameAndPath)
        {
            return Util.Utility.AES.AesEncrypt(imageNameAndPath, Static.ServerAesKey);
        }

        /// <summary>
        /// 解密文件名和路径
        /// </summary>
        /// <param name="imageNameAndPath"></param>
        /// <returns></returns>
        private static string DecryptImagePath(string imageNameAndPath)
        {
            return Util.Utility.AES.AesDecrypt(imageNameAndPath, Static.ServerAesKey);
        }

        /// <summary>
        /// 获取图片流
        /// </summary>
        /// <param name="fileNameAndPath"></param>
        /// <returns></returns>
        internal static byte[] GetImage(string fileNameAndPath)
        {
            var path = DecryptImagePath(fileNameAndPath);

            System.Drawing.Image img = System.Drawing.Image.FromFile(path); //这里我把路径给出了，他只用给我文件名

            MemoryStream ms = new MemoryStream();

            img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            var data = ms.ToArray();
            ms.Close();
            return data;
        }

    }
}
