using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActivityCalculator.ViewModels
{
    public class ResponseVm<T> : BaseResponseVm
    {
        public T Data { get; set; }

        public ResponseVm(T data) : base()
        {
            Data = data;
        }
    }
}
