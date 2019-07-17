using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHWDetails
{
    internal class GetHWinfo
    {
        public string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                if (mo[wmiMustBeTrue].ToString() == "True")
                {
                    //Only get the first one
                    if (result == "")
                    {
                        try
                        {
                            result = mo[wmiProperty].ToString();
                            break;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return result;
        }

        public string identifier(string wmiClass, string wmiProperty)
        {
            string result = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                //Only get the first one
                if (result == "")
                {
                    try
                    {
                        result = mo[wmiProperty].ToString();
                        break;
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }
    }

    public class GetDetails
    {
        /// <summary>
        /// Queries WMI for list of properties, and returns result in dictionary [string,string]
        /// </summary>
        /// <param name="properties">List of parameters</param>
        /// <returns>Results in dictionary, where key = "wmiClass:wmiProperty", value = result from wmi. All exceptions will be suppressed and value set to exception message</returns>
        public Dictionary<string, string> GetMultiDetails(List<string> properties)
        {
            GetHWinfo HW = new GetHWinfo();
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach(var property in properties)
            {
                var values = property.Split(':');
                string hwresult = "";
                try
                {
                    hwresult = HW.identifier(values[0], values[1]);
                }
                catch (Exception ex)
                {
                    hwresult = ex.Message;
                }

            data.Add(property, hwresult);
            }
            return data;
        }
        /// <summary>
        /// Returns result for single wmi property
        /// </summary>
        /// <param name="wmiClass">wmi class name, e.g. Win32_BIOS</param>
        /// <param name="wmiProperty">wmi property name in wmi class, i.e. SMBIOSBIOSVersion</param>
        /// <returns>result as string if successfull, exception if failed for some reason</returns>
        public string GetSingleDetail(string wmiClass, string wmiProperty)
        {
            GetHWinfo HW = new GetHWinfo();
            return HW.identifier(wmiClass, wmiProperty);
        }
    }
}
