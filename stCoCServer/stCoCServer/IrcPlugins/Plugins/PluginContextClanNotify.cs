
#if DEBUG
#define DEBUG_ClanNotify
#endif

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Globalization;
using stCore;
using stNet;

namespace stCoCServer.plugins
{
    public partial class IrcCommand
    {
        #region CMD Loop Clan Notify

        private Task PluginLoopClanNotify_task = null;
        private CancellationTokenSource PluginLoopClanNotify_canceler = null;

        private void IRCPluginLoopClanNotifyStart(string channel)
        {
            this.IRCPluginLoopClanNotify(false, channel);
        }
        private void IRCPluginLoopClanNotifyStop()
        {
            if ((PluginLoopClanNotify_task != null) && (this.PluginLoopClanNotify_canceler != null))
            {
                try
                {
                    this.PluginLoopClanNotify_canceler.Cancel();
                }
#if DEBUG_ClanNotify
                catch (Exception e)
                {
                    Conf.ILog.LogError(
                        string.Format(
                            (string)Properties.Resources.ResourceManager.GetString("cmdClanNotifyError", this._ci),
                            e.Message
                        )
                    );
#else
                catch (Exception)
                {
#endif
                }
            }
        }
        private void IRCPluginLoopClanNotify(bool isPrivate, string channel)
        {
            if (
                (this.PluginLoopClanNotify_task != null) ||
                (this.PluginLoopClanNotify_canceler != null) ||
                (Conf.Api == null)
               )
            {
                return;
            }

            Conf.ILog.LogInfo(
                string.Format(
                    (string)Properties.Resources.ResourceManager.GetString("PrnIrcPluginLoopClanNotifyStart", this._ci),
                    Conf.Opt.IRCPluginLoopClanNotifyPeriod.num
                )
            );

            this.PluginLoopClanNotify_canceler = new CancellationTokenSource();

            try
            {
                this.PluginLoopClanNotify_task = Task.Factory.StartNew(() =>
                {
                    DataTable dt = null;

                    while (Conf.Opt.IsRun.bval)
                    {
                        try
                        {
                            ///
                            if (Conf.Api == null)
                            {
                                throw new ArgumentNullException();
                            }
                            dt = Conf.Api.NotifyEventGetData();

                            if ((dt != null) && (dt.Rows.Count > 0))
                            {
                                this._SendFromTask(false, channel, null,
                                    string.Format(
                                        Conf.Api.GetResource("fmtNotifyHeaderCount", this._ci),
                                        dt.Rows.Count,
                                        Conf.Api.UpdateLastTime,
                                        Conf.Api.UpdateNextTime
                                    )
                                    .ColorText(IrcFormatText.Color.White, IrcFormatText.Color.LightGreen)
                                );
                                foreach (DataRow row in dt.Rows)
                                {
                                    string msg = Conf.Api.NotifyEventGetRowToString(row, this._ci);
                                    if (!string.IsNullOrWhiteSpace(msg))
                                    {
                                        this._SendFromTask(false, channel, null,
                                            msg
                                            .ColorText(IrcFormatText.Color.White, IrcFormatText.Color.Yellow)
                                        );
                                    }
                                    try
                                    {
                                        this.PluginLoopClanNotify_canceler
                                            .Token
                                            .ThrowIfCancellationRequested();

                                        this.PluginLoopClanNotify_canceler
                                            .Token.WaitHandle
                                            .WaitOne(
                                                TimeSpan.FromSeconds(Conf.Opt.IRCPluginLoopClanNotifyPeriod.num)
                                            );
                                    }
                                    catch (ObjectDisposedException e)
                                    {
                                        throw e;
                                    }
                                }
                            }
                            ///
                            try
                            {
                                this.PluginLoopClanNotify_canceler
                                    .Token
                                    .ThrowIfCancellationRequested();

                                this.PluginLoopClanNotify_canceler
                                    .Token.WaitHandle
                                    .WaitOne(
                                        Conf.Api.UpdateNextTimeSpan
                                    );
                            }
                            catch (ObjectDisposedException e)
                            {
                                throw e;
                            }
                            ///
                        }
#if DEBUG_ClanNotify
                        catch (OperationCanceledException e)
                        {
                            Conf.ILog.LogError(
                                string.Format(
                                    (string)Properties.Resources.ResourceManager.GetString("cmdClanNotifyError", this._ci),
                                    e.Message
                                )
                            );
#else
                        catch (OperationCanceledException)
                        {
#endif
                            break;
                        }
                        catch (Exception e)
                        {
                            Conf.ILog.LogError(
                                string.Format(
                                    (string)Properties.Resources.ResourceManager.GetString("cmdClanNotifyError", this._ci),
                                    e.Message
                                )
                            );
                            break;
                        }
                        finally
                        {
                            if (dt != null)
                            {
                                dt.Dispose();
                                dt = null;
                            }
                        }
                    }

                    this.IRCPluginLoopClanNotify_TaskClear();

                }, this.PluginLoopClanNotify_canceler.Token);
            }
#if DEBUG_ClanNotify
            catch (AggregateException e)
            {
                foreach (Exception ex in e.InnerExceptions)
                {
                    Conf.ILog.LogError(ex.Message);
                }
#else
            catch (AggregateException)
            {
#endif
                return;
            }
            catch (Exception e)
            {
#if DEBUG_ClanNotify
                Conf.ILog.LogError(
                    string.Format(
                        (string)Properties.Resources.ResourceManager.GetString("cmdClanNotifyError", this._ci),
                        e.Message
                    )
                );
#endif
                throw e;
            }
        }
        private void IRCPluginLoopClanNotify_TaskClear()
        {
            if (this.PluginLoopClanNotify_task != null)
            {
                try
                {
                    while (
                        (!this.PluginLoopClanNotify_task.IsCanceled) &&
                        (!this.PluginLoopClanNotify_task.IsCompleted) &&
                        (!this.PluginLoopClanNotify_task.IsFaulted)
                       )
                    {
                        if (this.PluginLoopClanNotify_canceler != null)
                        {
                            this.PluginLoopClanNotify_canceler.Cancel();
                        }
                        this.PluginLoopClanNotify_task.Wait();
                    }
                    this.PluginLoopClanNotify_task.Dispose();
                    this.PluginLoopClanNotify_task = null;
                }
#if DEBUG_ClanNotify
                catch (Exception e)
                {
                    Conf.ILog.LogError(
                        string.Format(
                            (string)Properties.Resources.ResourceManager.GetString("cmdClanNotifyError", this._ci),
                            e.Message
                        )
                    );
#else
                catch (Exception)
                {
#endif
                }
            }
            if (this.PluginLoopClanNotify_canceler != null)
            {
                try
                {
                    this.PluginLoopClanNotify_canceler.Dispose();
                    this.PluginLoopClanNotify_canceler = null;
                }
#if DEBUG_ClanNotify
                catch (Exception e)
                {
                    Conf.ILog.LogError(
                        string.Format(
                            (string)Properties.Resources.ResourceManager.GetString("cmdClanNotifyError", this._ci),
                            e.Message
                        )
                    );
#else
                catch (Exception)
                {
#endif
                }
            }
        }

        #endregion
    }
}
