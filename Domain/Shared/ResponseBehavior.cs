using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Shared
{

    public class ErrorType
    {
        private ErrorType(string value) { Value = value; }
        public string Value { get; private set; }
        public static ErrorType Error { get { return new ErrorType("Error"); } }
        public static ErrorType ModelError { get { return new ErrorType("ModelError"); } }
        public static ErrorType ValidationError { get { return new ErrorType("ValidationError"); } }
        public static ErrorType Business { get { return new ErrorType("Business"); } }
       
    }

    public class ResponseBehavior<T>
    {
        public bool Success { get; set; }
        public ErrorType? ErrorType { get; set; } = null;
        public T? Data { get; set; }
        public List<ResponseMessagesBehavior> Messages { get; set; } = new List<ResponseMessagesBehavior>();
    }

    public class ResponseMessagesBehavior
    {
        public string Key { set; get; } = null!;
        public string Value { set; get; } = null!;
    }

}

