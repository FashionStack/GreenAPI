using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GreenAPI.Models
{
    public class Error
    {
        public string Type { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}