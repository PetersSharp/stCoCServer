using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using stDokuWiki.WikiEngine.Exceptions;

namespace stDokuWiki.WikiEngine
{
    public partial class WikiFile
    {
        #region internal Wiki Cache Search/Find/Get
        /// <summary>
        /// Read Cache
        /// </summary>
        /// <param name="cacheid">string Cache ID</param>
        /// <returns><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo</returns>
        internal WikiFolderInfo _WikiFileCacheRead(string cacheid)
        {
            try
            {
                string cachePath = _WikiFileCacheFilePath(cacheid);
                if (File.Exists(cachePath))
                {
                    if (this._lockCache.TryEnterReadLock(waitReadFsOnProcess))
                    {
                        try
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            using (FileStream fs = new FileStream(cachePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                return ((WikiFolderInfo)(formatter.Deserialize(fs)));
                            }
                        }
                        finally
                        {
                            this._lockCache.ExitWriteLock();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e));
            }
            return default(WikiFolderInfo);
        }
        /// <summary>
        /// Write Cache
        /// </summary>
        /// <param name="wfi"><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo input</param>
        /// <param name="cacheid">string Cache ID</param>
        /// <returns><see cref="WikiEngine.WikiFolderInfo"/>WikiEngine.WikiFolderInfo</returns>
        internal void _WikiFileCacheWrite(WikiFolderInfo wfi, string cacheid)
        {
            try
            {
                string cachePath = _WikiFileCacheFilePath(cacheid);
                if (!Directory.Exists(Path.GetDirectoryName(cachePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(cachePath));
                }
                if (this._lockCache.TryEnterWriteLock(waitWriteFsOnProcess))
                {
                    try
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        using (FileStream fs = new FileStream(cachePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                        {
                            formatter.Serialize(fs, wfi);
                            fs.Close();
                        }
                    }
                    finally
                    {
                        this._lockCache.ExitWriteLock();
                    }
                }
            }
            catch (Exception e)
            {
                this.Fire_ProcessError(new WikiErrorEventArgs(e));
            }
        }
        /// <summary>
        /// Clear Cache
        /// </summary>
        internal void _WikiFileCacheClear()
        {
            if (this._isCacheEnable)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(this._cacheDirPath))
                    {
                        throw new WikiEngineInternalCacheException("Cache directory path empty");
                    }
                    if (this._lockCache.TryEnterWriteLock(waitWriteFsOnProcess))
                    {
                        try
                        {
                            new DirectoryInfo(this._cacheDirPath).GetFiles().ToList().ForEach(o =>
                            {
                                try
                                {
                                    File.Delete(o.FullName);
                                }
                                catch (Exception e)
                                {
                                    this.Fire_ProcessError(new WikiErrorEventArgs(e));
                                }
                            });
                        }
                        finally
                        {
                            this._lockCache.ExitWriteLock();
                        }
                    }
                }
                catch (Exception e)
                {
                    this.Fire_ProcessError(new WikiErrorEventArgs(e));
                }
            }
        }
        /// <summary>
        /// Create Cache path
        /// </summary>
        /// <param name="cacheid">string Cache ID</param>
        /// <returns></returns>
        internal string _WikiFileCacheFilePath(string cacheid)
        {
            return Path.Combine(
                 this._cacheDirPath,
                 string.Concat(cacheid, ".", cacheExtension)
            );
        }
        #endregion
    }
}
