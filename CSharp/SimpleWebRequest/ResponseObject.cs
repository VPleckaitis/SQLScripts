using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebRequest
{
    public class ResponseObject
    {
        public System.Net.HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
        public string ErrorMessage { get; set; }
    }
}
