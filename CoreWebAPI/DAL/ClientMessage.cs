using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.DAL
{
    public class ClientMessage
    {

        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string MessageText { get; set; }
    }
}

