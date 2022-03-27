﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Models.ResponseModels
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
        }
        public BaseResponse(T data,string message = null)
        {
            Message = message;
            Data = data;
        }
        public BaseResponse(T data, bool succeeded = false, string message = null)
        {
            Message = message;
            Data = data;
            Succeeded = succeeded;
        }
        public BaseResponse(string message)
        {
            Message = message;
            Succeeded = false;
        }
        public bool Succeeded;
        public string Message { get; set; }
        public List<string> Errors;
        public T Data { get; set; }
    }
}
