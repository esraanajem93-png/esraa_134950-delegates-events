using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace esraa_134950_delegates___events
{
    // تعريف التفويض (Delegate) الخاص بحالة الجهاز
    public delegate void DeviceStatusDelegate(string deviceName, string newStatus);

    // الكلاس الأساسي المجرد (Abstract Class) للأجهزة
    public abstract class Appliance
    {
        // خاصية مجردة يجب تطبيقها في الكلاسات الوراثية
        public abstract string DeviceName { get; set; }

        private string status;

        // الحدث الذي يتم استدعاؤه عند تغير الحالة
        public event DeviceStatusDelegate DeviceStatusChange;

        // الخاصية المسؤولة عن مراقبة وتحديث الحالة
        public string Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    // استدعاء الحدث بشكل آمن
                    DeviceStatusChange?.Invoke(DeviceName, status);
                }
            }
        }
    }

    // مثال على كلاس فرعي يرث من Appliance
    public class AirConditioner : Appliance
    {
        public override string DeviceName { get; set; } = "المكيف";
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // إنشاء كائن من الجهاز
            AirConditioner ac = new AirConditioner();

            // الاشتراك في الحدث
            ac.DeviceStatusChange += (name, newStatus) =>
            {
                Console.WriteLine($"تنبيه: تغيرت حالة الجهاز [{name}] وأصبحت: [{newStatus}]");
            };

            // تجربة تغيير الحالة
            ac.Status = "يعمل";
            ac.Status = "يعمل";
            ac.Status = "متوقف";

            Console.ReadKey();
        }
    }
}
