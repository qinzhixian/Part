using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ImageCenter
{
    public static class Image
    {
        /// <summary>
        /// 保存图片  多图片之间用","隔开
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static string SaveImage(HttpFileCollectionBase files)
        {
            return SaveImage(files, ImageTypeEnum.Default);
        }

        /// <summary>
        ///  保存图片  多图片之间用","隔开
        /// </summary>
        /// <param name="files"></param>
        /// <param name="imageType"></param>
        /// <returns></returns>
        public static string SaveImage(HttpFileCollectionBase files, ImageTypeEnum imageType)
        {
            return ImageDao.Save(files, imageType);
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="filePathAndName"></param>
        /// <returns></returns>
        public static byte[] GetImage(string filePathAndName)
        {
            filePathAndName = filePathAndName.Replace(' ', '+');
            var image = ImageDao.GetImage(filePathAndName);

            return image.ToArray();
        }
    }
}
