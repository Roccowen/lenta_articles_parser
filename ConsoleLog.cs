using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace s2._3
{
    class ConsoleLog : TextWriter
    {
        StreamWriter SW;
        TextWriter TW;
        public ConsoleLog(string logPath, TextWriter writer)
        {
            SW = new StreamWriter(logPath);
            TW = writer;
        }
        public override Encoding Encoding => throw new NotImplementedException();
        public override void WriteLine()
        {
            TW.WriteLine();
            SW.WriteLine();
        }
        public override void WriteLine(string format, object arg0)
        {
            TW.WriteLine(format, arg0);
            SW.WriteLine(String.Format(format, arg0));
        }
        public override void WriteLine(string format, params object[] arg)
        {
            TW.WriteLine(format, arg);
            SW.WriteLine(String.Format(format, arg));
        }
        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            TW.WriteLine(format, arg0, arg1, arg2);
            SW.WriteLine(String.Format(format, arg0, arg1, arg2));
        }
        public override void WriteLine(string format, object arg0, object arg1)
        {
            TW.WriteLine(format, arg0, arg1);
            SW.WriteLine(String.Format(format, arg0, arg1));
        }
        public override void WriteLine(object value)
        {
            TW.WriteLine(value);
            SW.WriteLine(value);
        }
        public override void WriteLine(int value)
        {
            TW.WriteLine(value);
            SW.WriteLine(value);
        }
        public override void WriteLine(decimal value)
        {
            TW.WriteLine(value);
            SW.WriteLine(value);
        }
        public override void WriteLine(char value)
        {
            TW.WriteLine(value);
            SW.WriteLine(value);
        }
        public override void WriteLine(string value)
        {
            SW.WriteLine(value);
        }
        public override void WriteLine(char[] buffer, int index, int count)
        {
            TW.WriteLine(buffer, index, count);
            SW.WriteLine(buffer, index, count);
        }
        public override void WriteLine(bool value)
        {
            TW.WriteLine(value);
            SW.WriteLine(value);

        }
        public override void WriteLine(char[] buffer)
        {
            TW.WriteLine(buffer);
            SW.WriteLine(buffer);
        }
        public override void WriteLine(long value)
        {
            TW.WriteLine(value);
            SW.WriteLine(value);
        }
        public override void WriteLine(ulong value)
        {
            TW.WriteLine(value);
            SW.WriteLine(value);
        }
        public override void WriteLine(float value)
        {
            TW.WriteLine(value);
            SW.WriteLine(value);
        }
        public override void WriteLine(uint value)
        {
            TW.WriteLine(value);
            SW.WriteLine(value);
        }
    }
}
