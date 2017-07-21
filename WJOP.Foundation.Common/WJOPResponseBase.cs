using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common
{
    public class WJOPResponseBase
    {
        public int Code { get; set; }
        public string ErrMsg { get; set; }
        public bool IsSuccess { get; set; }
        public string Msg { get; set; }
        public string SubCode { get; set; }
        public string SubMsg { get; set; }

        public WJOPResponseBase()
        {
            Init();
        }
        public WJOPResponseBase(bool isSuccess)
        {
            this.Init();
            if (!isSuccess)
            {
                this.Error();
            }
            else
            {
                this.Success();
            }
        }

        private void Init()
        {
            this.Code = -1;
            this.Msg = string.Empty;
            this.ErrMsg = string.Empty;
            this.SubCode = string.Empty;
            this.SubMsg = string.Empty;
            this.IsSuccess = false;
        }

        public void Error()
        {
            Error(string.Empty, StatusCodeEnum.Error);
        }
        public void Error(string errMsg, StatusCodeEnum code)
        {
            Code = (int)code;
            ErrMsg = (!string.IsNullOrEmpty(errMsg) ? errMsg : "Error");
            IsSuccess = false;
        }
        public void Success()
        {
            Success(string.Empty, StatusCodeEnum.Success);
        }
        public void Success(string msg, StatusCodeEnum code)
        {
            Code = (int)code;
            Msg = (!string.IsNullOrEmpty(msg) ? msg : "Success");
            IsSuccess = true;
        }
    }
}
