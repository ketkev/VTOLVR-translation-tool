using System;
using System.Collections.Generic;
using System.Text;

namespace VTOLVR_Translation_tool
{
    class InvalidCsvException : Exception
    {
        public InvalidCsvException()
        {
        }

        public InvalidCsvException(string message) : base(message)
        {
        }
    }
}
