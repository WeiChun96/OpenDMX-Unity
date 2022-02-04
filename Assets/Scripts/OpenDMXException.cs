using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class OpenDMXException : Exception
{
    public Status Status { get; private set; }

    public OpenDMXException(string message, Status status) : base(message)
    {
        Status = status;
    }
}

