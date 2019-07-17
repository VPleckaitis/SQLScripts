using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            GetHWDetails.GetDetails hw = new GetHWDetails.GetDetails();
            Console.WriteLine("Single property call - BIOS manufacturer : "+ hw.GetSingleDetail("Win32_BIOS", "Manufacturer"));
            Console.WriteLine("-----------------------");

            List<string> dataCall = new List<string>();
            dataCall.Add("Win32_BIOS:Manufacturer");
            dataCall.Add("Win32_BIOS:SMBIOSBIOSVersion");
            dataCall.Add("Win32_BIOS:IdentificationCode");
            dataCall.Add("Win32_BIOS:SerialNumber");
            dataCall.Add("Win32_BIOS:ReleaseDate");
            dataCall.Add("Win32_BIOS:Version");
            dataCall.Add("Win32_BaseBoard:Model");
            dataCall.Add("Win32_BaseBoard:Manufacturer");
            dataCall.Add("Win32_BaseBoard:Name");
            dataCall.Add("Win32_BaseBoard:SerialNumber");
            dataCall.Add("Win32_Processor:UniqueId");
            dataCall.Add("Win32_Processor:ProcessorId");
            dataCall.Add("Win32_Processor:Name");
            dataCall.Add("Win32_Processor:Manufacturer");
            dataCall.Add("Win32_Processor:MaxClockSpeed");
            var result = hw.GetMultiDetails(dataCall);
            foreach (var res in result)
            {
                Console.WriteLine(res.Key + " | " + res.Value);
            }
            Console.ReadKey();
        }
    }
}
