using System;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using stCore;
using stCoreUI;
using stClient;
using stClient.SpellChecker;
using System.Collections.Generic;

namespace stCoCClient.Control
{
    [Serializable]
    public partial class TBSpellControl : UserControl
    {
        private string _origin = String.Empty;
        private SpellChecker _spc = null;
        private IMessage _iLog = null;
        private FlatTextBox _ftb = null;
        private Word _bw = null;
        private bool isSCInit = false;

        public override string Text
        {
            get
            {
                return _ftb.Text;
            }
            set
            {
                _ftb.Text = value;
                _origin = ((string.IsNullOrWhiteSpace(_origin)) ? value : _origin);
            }
        }

        public IMessage iLog
        {
            get
            {
                return _iLog;
            }
            set
            {
                _iLog = value;
            }
        }

        public TBSpellControl(FlatTextBox ftb, IMessage iLog)
        {
            _ftb = ftb;
            _iLog = iLog;
            Visible = false;
            InitializeComponent();
            _origin = ((string.IsNullOrWhiteSpace(_ftb.Text)) ? String.Empty : _ftb.Text);
            try
            {
                _spc = new SpellChecker();
                isSCInit = true;
            }
            catch (Exception ex)
            {
                _iLog.LogError(ex.Message);
            }
        }

        private void _Check()
        {
            List<Word> lbw;

            if ((!isSCInit) || (_ftb == null) || (string.IsNullOrWhiteSpace(_ftb.Text)))
            {
                return;
            }
            try
            {
                lbw = _spc.CheckTextSpell(_ftb.Text);
                if (lbw.Count == 0)
                {
                    FCBTWordSelector.DataSource = new string[] {
                        Properties.Resources.txtSpellMwnuNoMiss,
                        Properties.Resources.txtSpellMwnuClose
                    };
                    FLSpellWorCount.Text =
                        string.Format(
                            Properties.Resources.fmtSpellCount,
                            0
                        );
                    _SpellReset();
                }
                else if ((lbw[0] != null) && (!string.IsNullOrWhiteSpace(lbw[0].Text)))
                {
                    _bw = lbw[0];
                    int i = 0;
                    SpellSuggestion[] suggest = _spc.SuggestCorrectedWords(_bw.Text, true);
                    List<string> dtsrc = suggest.Select(s => s.Text).ToList<string>();

                    dtsrc.Insert(i++, Properties.Resources.txtSpellMwnuReplace);
                    dtsrc.Insert(i++, Properties.Resources.txtSpellMwnuRestore);
                    dtsrc.Insert(i++, Properties.Resources.txtSpellMwnuAddDictionary + _bw.Text + "'");
                    dtsrc.Insert(i++, Properties.Resources.txtSpellMwnuCancel);
                    dtsrc.Insert(i++, Properties.Resources.txtSpellMwnuClose);
                    dtsrc.Insert(i++, Properties.Resources.txtSpellMwnuUnderline);
                    FCBTWordSelector.DataSource = dtsrc.ToArray();
                    FLSpellWorCount.Text =
                        string.Format(
                            Properties.Resources.fmtSpellCount,
                            lbw.Count
                        );
                    
                    List<TxtPosition> ltp = new List<TxtPosition>();

                    foreach (Word w in lbw)
                    {
                        TxtPosition tp = new TxtPosition(w.StartIndex, w.EndIndex, w.Length);
                        ltp.Add(tp);
                    }
                    _ftb.SpellMark = ltp;
                    _ftb.Select(_bw.StartIndex, _bw.Length);
                    _ftb.Invalidate();
                    _ftb.Focus();
                    _iLog.Line(
                        Properties.Resources.txtStatusBadWord + 
                        String.Join(", ", lbw.Select(s => s.Text).ToArray())
                    );
                }
            }
            catch (Exception ex)
            {
                _iLog.LogError(ex.Message);
            }
        }

        private void _FBLangOk_Click(object sender, EventArgs e)
        {
            Text = String.Empty;
            Enabled = false;
            Visible = false;
        }

        private void TBSpellControl_VisibleChanged(object sender, EventArgs e)
        {
            UserControl uc = sender as UserControl;

            if (uc.Visible)
            {
                _Check();
            }
            else
            {
                FCBTWordSelector.DataSource = new string[] { };
            }
        }

        private void FCBTWordSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((!isSCInit) || (_ftb == null))
            {
                return;
            }
            FlatComboBox fcb = sender as FlatComboBox;
            string val = fcb.SelectedValue.ToString();

            switch (val)
            {
                case null:
                case "":
                    {
                        break;
                    }
                default:
                    {
                        try
                        {
                            if (
                                (val.Equals(Properties.Resources.txtSpellMwnuUnderline)) ||
                                (val.Equals(Properties.Resources.txtSpellMwnuNoMiss)) ||
                                (val.Equals(Properties.Resources.txtSpellMwnuReplace)) ||
                                (val.Equals(Properties.Resources.txtSpellMwnuCancelMiss))
                               )
                            {
                                break;
                            }
                            else if (val.Equals(Properties.Resources.txtSpellMwnuRestore))
                            {
                                _ftb.Text = _origin;
                                _SpellReset();
                                break;
                            }
                            else if (val.Equals(Properties.Resources.txtSpellMwnuClose))
                            {
                                this.Visible = false;
                                _SpellReset();
                                break;
                            }
                            else if (val.Equals(Properties.Resources.txtSpellMwnuCancel))
                            {
                                FCBTWordSelector.DataSource = new string[] {
                                Properties.Resources.txtSpellMwnuCancelMiss,
                                Properties.Resources.txtSpellMwnuClose
                            };
                                _SpellReset();
                                break;
                            }
                            else if (val.Contains(Properties.Resources.txtSpellMwnuAddDictionary))
                            {
                                if (_bw != null)
                                {
                                    _spc.AddUserWord(_bw.Text);
                                    _ftb.Select(0, 0);
                                    _Check();
                                }
                                break;
                            }
                            else if ((_bw != null) && (!string.IsNullOrWhiteSpace(_ftb.Text)))
                            {
                                _ftb.Select(_bw.StartIndex, _bw.Length);
                                _ftb.SelectionReplace(val);
                                _Check();
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            _iLog.LogError(ex.Message);
                        }
                        break;
                    }
            }
        }
        private void _SpellReset()
        {
            _ftb.SpellMark = new List<TxtPosition>();
            _ftb.Invalidate();
            _bw = null;
            _iLog.Line(Properties.Resources.txtSpellStatusEndMiss);
        }

    }
}
