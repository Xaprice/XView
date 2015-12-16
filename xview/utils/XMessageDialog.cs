using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace xview
{
    public class XMessageDialog
    {
        public static DialogResult Warning(string msg)
        {
            return MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult Question(string msg)
        {
            return MessageBox.Show(msg, "请确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }

        public static DialogResult Info(string msg)
        {
            return MessageBox.Show(msg, "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult Error(string msg)
        {
            return MessageBox.Show(msg, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static DialogResult Debug(string msg)
        {
            return MessageBox.Show(msg, "调试", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
