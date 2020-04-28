using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox
{
    public partial class Helper
    {
        public class Core
        {
            public static string GetCommonErrorMessage(Exception ex, bool showDetail = true)
            {
                showDetail = Debugger.IsAttached || showDetail;

                if (ex == null || string.IsNullOrWhiteSpace(ex.Message) || ex.Message.Length < 6)
                    return "";

                if(ex.Message.Substring(0,6) == "ALERT:")
                    return ex.Message;

                if (ex.Message.Substring(0, 6).ToUpper() == "ERROR:")
                    return showDetail ? ex.Message : "ERROR";

                var sb = new StringBuilder("Exception=" + ex.ToString());

                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                    sb.AppendLine("@ InnerException.Message=" + ex.InnerException.Message);

                return "ERROR" + (showDetail ? ":" + sb.ToString() : "");
            }
        }
        public class Record : Dictionary<string,object>
        {
            public Record() { }
            public Record(SerializationInfo info,StreamingContext context) : base(info,context)
            {

            }
        }

        [Serializable]
        public class Table : List<Record>
        {
            public Table()
            {
            }
            public Table Select(string compareField,object compareValue)
            {
                Table result = new Table();
                string value = Data.GetSafeStr(compareValue);
                for(int i=0; i< this.Count; i++)
                {
                    if (Data.GetSafeStr(this[i][compareField]) == value)
                        result.Add(this[i]);
                }
                return result;
            }

            public Table SelectLike(string compareField, object compareValue)
            {
                Table result = new Table();
                string value = Data.GetSafeStr(compareValue);

                for (int i = 0; i < this.Count; i++)
                {
                    if (Data.GetSafeStr(this[i][compareField]).IndexOf(value) > -1)
                        result.Add(this[i]);
                }
                return result;
            }

            public Table Select(string compareField, int compareValue)
            {
                Table result = new Table();

                for (int i = 0; i < this.Count; i++)
                {
                    if (Data.GetSafeInt(this[i][compareField]) == compareValue)
                        result.Add(this[i]);
                }
                return result;
            }


        }
    }
}
