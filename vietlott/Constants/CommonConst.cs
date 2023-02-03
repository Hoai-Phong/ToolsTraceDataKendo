using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vietlott.Constants
{
    public class CommonConst
    {
        public const string Message_Name_Application_Start = "**-------------------------START TOOL -------------------------**";
        public const string Message_Name_Application_End = "**-------------------------END TOOL-------------------------**";
        public const string Message_Name_Application_Copyright = "**------Copyright © 2022 || Desgin By HoaiPhong-------**";
        // Message_Program
        public const string Message_Program_Start = "Program has started.";
        public const string Message_Program_Finished = "Successfully:  Program has finished.";
        // Message_Creation 
        public const string Message_Creation_Start = " creation process has started.";
        public const string Message_Creation_Finished = "Successfully: creation process has finished.";
        public const string Message_Creation_Failed = "Error:  creation process has failed..";
        // Message_API
        public const string Message_Start = "Program execution process has started.";
        public const string Message_Finished = "Successfully: Tool execution process has finished.";
        public const string Message_Failed = "Tool: WebAPI execution process has failed.";
        // App_Settings    
        public const string Key_Input = "PhysicalPathSettings:Input";
        public const string Key_Output = "PhysicalPathSettings:Output";
        public const string Key_FileNameNotCopy = "Settings:FileNameNotCopy";
        public const string  Key_FileNameNotDelete = "Settings:FileNameNotDelete";
        //   
        public const string Message_PhysicalOutPut = "Error: PhysicalOutPut appsettings on the server do not exist.";
        public const string Message_PhysicalInput = "Error: PhysicalInput appsettings on the server do not exist.";
        public const string Message_Copy_File = "Successfully: Copy file successfully";
        public const string Message_Copy_Failed = "Error:Program Error";
        public const string Message_Delete_File = "Successfully: Delete file successfully";
        public const string Message_Delete_Failed = "Error: Delete Error";



        public const string Message_Text = "Do you want to delete files in the folder or not?";
        public const string Message_Text_Choose = "Please choose: 0: Agree, 1 : Disagree";

    }
}
