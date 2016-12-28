using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;

namespace stCore
{
    public static class stSerializer
    {
        private const string thisClass = "[stSerializer]: ";
        private static Object ioLock = new Object();

        /// <summary>
        /// Use a binary formatter to save the List Type data to same name file,
        /// modify file extension to .bin
        /// </summary>
        /// <param name="path">full path to destination binary save file</param>
        /// <param name="data">List T data</param>
        /// <param name="isZip">Compression Mode GZip Stream</param>
        public static void stSerialize<T>(this List<T> data, string path, bool isZip = false) where T : class
        {
            if ((string.IsNullOrWhiteSpace(path)) || (!Directory.Exists(Path.GetDirectoryName(path))))
            {
                throw new ArgumentException(Properties.Resources.FilePathisNullError);
            }
            if ((data == null) || (data.Count == 0))
            {
                throw new ArgumentNullException(thisClass + typeof(T).ToString());
            }

            FileStream fs = null;
            GZipStream gz = null;
            path = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + ".bin");

            try
            {
                lock (ioLock)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);
                    if (isZip)
                    {
                        gz = new GZipStream(fs, CompressionMode.Compress);
                        formatter.Serialize(gz, data);
                    }
                    else
                    {
                        formatter.Serialize(fs, data);
                    }
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(thisClass + e.Message);
            }
            finally
            {
                if (gz != null)
                {
                    gz.Close();
                    gz.Dispose();
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
        }
        /// <summary>
        /// Deserialize an existing file back into a
        /// list of type T
        /// </summary>
        /// <param name="path">full path to source binary read file</param>
        /// <param name="isZip">Decompression Mode GZip Stream</param>
        /// <returns>List T data</returns>
        public static List<T> stDeserialize<T>(this string path, bool isZip = false) where T : class
        {
            if ((string.IsNullOrWhiteSpace(path)) || (!File.Exists(path)))
            {
                throw new ArgumentException(Properties.Resources.FilePathisNullError);
            }

            FileStream fs = null;
            GZipStream gz = null;

            try
            {
                List<T> data = new List<T>();

                lock (ioLock)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (isZip)
                    {
                        gz = new GZipStream(fs, CompressionMode.Decompress);
                        data = ((List<T>)(formatter.Deserialize(gz)));
                    }
                    else
                    {
                        data = ((List<T>)(formatter.Deserialize(fs)));
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                throw new ArgumentException(thisClass + e.Message);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (gz != null)
                {
                    gz.Close();
                    gz.Dispose();
                }
            }
        }
    }
}
