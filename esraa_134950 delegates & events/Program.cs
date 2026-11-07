using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esraa_134950_delegates___events
{
    
        public class Program// إنشاء تتفويض باسم DeviceStatusDelegate
                            // ليستقبل نصين هما: اسم الجهاز وحالته الجديدة
        {
            public delegate void DeviceStatusDelegate(string deviceName, string newStatus);
        }
        public abstract class appliance
        {
            // تعريف خاصية مجردة باسم DeviceName
            public abstract string DeviceName { get; set; }
            // تعريف متغير لتخزين الحالة الحالية للجهاز
            private string status;

            // تعريف الحدث 
            public event DeviceStatusDelegate DeviceStatusChange;
            public string Status// تعريف خاصية باسم Status
            {
                get { return status; }// استرجاع الحالة الحالية للجهاز
                set
                {
                    if (status != value)// التحقق من أن الحالة الجديدة مختلفة عن الحالة الحالية
                    {
                        status = value;// تحديث الحالة الحالية للجهاز

                        DeviceStatusChange?.Invoke(DeviceName, status);// استدعاء الحدث وتمرير اسم الجهاز والحالة الجديدة
                    }
                }
            }

        }










    }

}
