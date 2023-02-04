using Microsoft.Extensions.Configuration;
using Vietlott.Services.Constants;

namespace Vietlott.Services.Settings
{
    public class ToolConfiguration
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
   
}
