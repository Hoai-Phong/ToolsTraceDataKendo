using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vietlott.Constants;

namespace vietlott.Settings
{
    public class ToolConfiguration : IToolConfiguration
    {
        IConfigurationRoot _configurationRoot;
        public ToolConfiguration(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }
        public string PhysicalInput => _configurationRoot[CommonConst.Key_Input];
        public string PhysicalOutPut => _configurationRoot[CommonConst.Key_Output];
        public string FileNameNotCopy => _configurationRoot[CommonConst.Key_FileNameNotCopy];
        public string FileNameNotDelete => _configurationRoot[CommonConst.Key_FileNameNotDelete];
    }
    public interface IToolConfiguration
    {
        string PhysicalInput { get; }
        string PhysicalOutPut { get; }
        string FileNameNotCopy { get; }
        string FileNameNotDelete { get; }

    }
}
