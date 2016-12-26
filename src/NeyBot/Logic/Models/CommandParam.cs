using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeyBot.Logic.Models
{
    class CommandParam
    {
        private string _paramName;
        private bool _isRequired;

        public CommandParam(string paramName, bool isRequired)
        {
            _paramName = paramName;
            _isRequired = isRequired;
        }

        public string GetParamName()
        {
            return _paramName;
        }

        public bool IsRequired()
        {
            return _isRequired;
        }
    }
}
