using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WJOP.Foundation.Common
{
    public class FoundationResponse : WJOPResponseBase
    {
        public FoundationResponse()
        {

        }
        public FoundationResponse(bool isSuccess)
            : base(isSuccess)
        {

        }
        public FoundationResponse(WJOPResponseBase src)
        {
            base.Code = src.Code;
            base.Msg = src.Msg;
            base.ErrMsg = src.ErrMsg;
            base.SubCode = src.SubCode;
            base.SubMsg = src.SubMsg;
            base.IsSuccess = src.IsSuccess;
        }
    }

    public class FoundationResponse<T> : FoundationResponse
    {
        public T Data { get; set; }

        public FoundationResponse()
        {
        }

        public FoundationResponse(bool isSuccess)
            : base(isSuccess)
        {
        }

        public FoundationResponse(bool isSuccess, T data)
            : base(isSuccess)
        {
            this.Data = data;
        }

        public FoundationResponse(FoundationResponse src)
        {
            base.Code = src.Code;
            base.Msg = src.Msg;
            base.ErrMsg = src.ErrMsg;
            base.SubCode = src.SubCode;
            base.SubMsg = src.SubMsg;
            base.IsSuccess = src.IsSuccess;
        }
    }
}
