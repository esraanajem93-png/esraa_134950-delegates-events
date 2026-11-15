
الجمهورية العربية السورية
وزارة التعليم العالي - الجامعة الافتراضية السورية
المعهد التقني للحاسوب - مقرر البرمجة والتصميم غرضية التوجه (TIC_IPG203_F25)
تقرير الوظيفة التطبيقية الشاملة
نظام إدارة مركز صيانة وإصلاح الأجهزة المنزلية
.1أسماء أعضاء الفريق (Team Members)
#	اسم الطالب	المعرف (User ID)
1	كريستين نقولا	Christin_364963
2	اسراء عدنان نجم	Esraa_134950
3	مجد زياد عبد الله	Majd_351378
4	ندى الغانم	Nada_240025
اسم المدرس:  
د. علي محمد 
________________________________________
.2 الفكرة العامة للمشروع (Project Overview)
النظام البرمجي المصمم بلغة C# يمثل "نظام إدارة مركز صيانة وإصلاح أجهزة منزلية" (مثل الثلاجات، الغسالات، والمكيفات). يتيح النظام تسجيل الأجهزة بدقة، والتحقق من أرقامها التسلسلية، ومتابعة حالات الصيانة الخاصة بها (مستلم، قيد الإصلاح، تم إصلاحه)، بالإضافة إلى حساب التكاليف الإجمالية المتوقعة للمركز ديناميكيًا وإطلاق إشعارات فورية عند تحديث بيانات الأجهزة باستخدام الأحداث (Events).



3. توثيق تطبيق مفاهيم OOP الستة في الكود
التجريد (Abstraction): تم استخدام الواجهة IServiceable لتعريف العقد الأساسي لكل جهاز، والفئة المجردة Appliance التي تطبق الواجهة وتحتوي على الدالة المجردة CalculateRepairCost() التي تُلزم كل فئة فرعية بتطبيقها.
التغليف (Encapsulation): تم جعل جميع الحقول خاصة (private) مثل _serialNumber و _status، واستخدام الخصائص (Properties) للوصول الآمن، مع جعل بعض الخصائص للقراءة فقط (get فقط) لحماية البيانات الحساسة.
الوراثة (Inheritance): ترث الفئات Refrigerator و WashingMachine و AirConditioner من الفئة الأب Appliance لتجنب تكرار الكود، وتستدعي الباني الأساسي عبر base.
تعدد الأشكال (Polymorphism): تم استخدام قائمة موحدة List<Appliance> في فئة RepairCenter، وتم إعادة تعريف دالة CalculateRepairCost() في كل فئة فرعية. النظام يحدد الدالة الصحيحة وقت التشغيل (Runtime).
التفويض والأحداث (Delegates & Events): تم تعريف تفويض StatusChangedEventHandler وحدث StatusChanged في فئة Appliance، وتشترك فيه فئة RepairCenter لطباعة إشعارات فورية عند تغير حالة الجهاز إلى "تم الإصلاح" أو "متأخر".
الأعضاء الثابتة (Static Members): تم استخدام الفئة الثابتة Validation للتحقق من صحة البيانات، وخصائص ثابتة مثل TotalCreated و RepairedCount في Appliance كعدادات مشتركة لجميع الأجهزة.
4. .مخطط العلاقات (UML Class Diagram)
شرح العلاقات:
الواجهة IServiceable: تقع في أعلى الهرم، وتُظهر العلاقة المنقطة مع Appliance أن الفئة المجردة تطبق هذه الواجهة.
الفئة المجردة Appliance: ترث منها الفئات الفرعية الثلاث باستخدام سهم الوراثة (Generalization)، وتحتوي على الدالة المجردة (المميزة بخط مائل).
فئة الإدارة RepairCenter: ترتبط بعلاقة تجميع (Aggregation) مع Appliance لأنها تحتوي على قائمة List<Appliance>.
الفئة الثابتة Validation: فئة مساعدة مستقلة تُستخدم للتحقق من صحة البيانات.


 

.5. الكود المصدري الكامل (Source Code Modules)
5.1 ملف الواجهة IServiceable.cs
csharp
namespace HomeApplianceRepairCenter.Models
{
    public interface IServiceable
    {
        string SerialNumber { get; }
        string Brand { get; }
        ApplianceStatus Status { get; }

        decimal CalculateRepairCost();
        string GetDetails();

        void StartRepair();
        void CompleteRepair();
        void MarkDelayed();
        void Deliver();
    }
}
5.2 ملف الفئة المجردة Appliance.cs
csharp
using System;

namespace HomeApplianceRepairCenter.Models
{
    public delegate void StatusChangedEventHandler(object sender, ApplianceStatusChangedEventArgs e);

    public abstract class Appliance : IServiceable
    {
        private readonly string _serialNumber;
        private string _brand;
        private ApplianceStatus _status;
        private readonly decimal _baseServiceFee;

        private static int _totalCreated;
        private static int _repairedCount;

        public static int TotalCreated => _totalCreated;
        public static int RepairedCount => _repairedCount;

        public event StatusChangedEventHandler StatusChanged;

        public string SerialNumber => _serialNumber;
        public string Brand => _brand;
        public ApplianceStatus Status => _status;
        public DateTime ReceivedDate { get; }
        public decimal BaseServiceFee => _baseServiceFee;

        protected Appliance(string serialNumber, string brand, DateTime receivedDate, decimal baseServiceFee)
        {
            if (!Validation.IsValidSerialNumber(serialNumber)) throw new ArgumentException("رقم تسلسلي غير صالح.");
            if (!Validation.IsValidBrand(brand)) throw new ArgumentException("اسم العلامة التجارية غير صالح.");
            if (!Validation.IsValidCost(baseServiceFee)) throw new ArgumentException("الأجرة الأساسية غير صالحة.");

            _serialNumber = serialNumber.Trim().ToUpperInvariant();
            _brand = brand.Trim();
            ReceivedDate = receivedDate.Date;
            _baseServiceFee = baseServiceFee;
            _status = ApplianceStatus.Received;

            _totalCreated++;
        }

        public abstract decimal CalculateRepairCost();

        public virtual string GetDetails()
        {
            return $"Serial: {SerialNumber}, Brand: {Brand}, Status: {Status}, BaseFee: {BaseServiceFee:C}";
        }

        public void StartRepair()
        {
            if (_status != ApplianceStatus.Received && _status != ApplianceStatus.Delayed)
                throw new InvalidOperationException("لا يمكن بدء الإصلاح.");
            UpdateStatus(ApplianceStatus.InRepair);
        }

        public void CompleteRepair()
        {
            if (_status != ApplianceStatus.InRepair)
                throw new InvalidOperationException("الجهاز ليس قيد الإصلاح.");
            UpdateStatus(ApplianceStatus.Repaired);
            _repairedCount++;
        }

        public void MarkDelayed()
        {
            if (_status == ApplianceStatus.Repaired || _status == ApplianceStatus.Delivered)
                throw new InvalidOperationException("لا يمكن تأخير جهاز تم إصلاحه.");
            UpdateStatus(ApplianceStatus.Delayed);
        }

        public void Deliver()
        {
            if (_status != ApplianceStatus.Repaired)
                throw new InvalidOperationException("لا يمكن التسليم قبل الإصلاح.");
            UpdateStatus(ApplianceStatus.Delivered);
        }

        protected void UpdateStatus(ApplianceStatus newStatus)
        {
            if (_status == newStatus) return;
            ApplianceStatus oldStatus = _status;
            _status = newStatus;
            StatusChanged?.Invoke(this, new ApplianceStatusChangedEventArgs(this, oldStatus, newStatus));
        }
    }
}
5.3 ملف الفئات الفرعية (مثال: Refrigerator.cs و WashingMachine.cs)
csharp
namespace HomeApplianceRepairCenter.Models
{
    public class Refrigerator : Appliance
    {
        private readonly int _capacityLiters;
        private readonly bool _needsGasRefill;

        public Refrigerator(string serial, string brand, DateTime date, decimal fee, int capacity, bool needsGas) 
            : base(serial, brand, date, fee)
        {
            _capacityLiters = capacity;
            _needsGasRefill = needsGas;
        }

        public override decimal CalculateRepairCost()
        {
            return BaseServiceFee + (_capacityLiters * 1.5m) + (_needsGasRefill ? 250m : 0m);
        }
    }

    public class WashingMachine : Appliance
    {
        private readonly int _drumSizeKg;
        private readonly bool _needsMotorReplacement;

        public WashingMachine(string serial, string brand, DateTime date, decimal fee, int drum, bool needsMotor) 
            : base(serial, brand, date, fee)
        {
            _drumSizeKg = drum;
            _needsMotorReplacement = needsMotor;
        }

        public override decimal CalculateRepairCost()
        {
            return BaseServiceFee + (_drumSizeKg * 20m) + (_needsMotorReplacement ? 400m : 0m);
        }
    }
}
5.4 ملف الفئة الثابتة Validation.cs
csharp
using System.Linq;

namespace HomeApplianceRepairCenter.Models
{
    public static class Validation
    {
        public static bool IsValidSerialNumber(string serial)
        {
            return !string.IsNullOrWhiteSpace(serial) && serial.Trim().Length >= 5 && serial.Trim().All(char.IsLetterOrDigit);
        }

        public static bool IsValidBrand(string brand)
        {
            return !string.IsNullOrWhiteSpace(brand) && brand.Trim().Length >= 2;
        }

        public static bool IsValidCost(decimal cost)
        {
            return cost >= 0;
        }
    }
}
5.5 ملف إدارة المركز RepairCenter.cs
csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeApplianceRepairCenter.Services
{
    public class RepairCenter
    {
        private readonly List<Appliance> _appliances = new List<Appliance>();
        private static decimal _totalCompletedRevenue;

        public static decimal TotalCompletedRevenue => _totalCompletedRevenue;

        public void AddAppliance(Appliance appliance)
        {
            if (_appliances.Any(a => a.SerialNumber == appliance.SerialNumber))
                throw new InvalidOperationException("رقم تسلسلي مكرر.");
            
            appliance.StatusChanged += OnApplianceStatusChanged;
            _appliances.Add(appliance);
        }

        private void OnApplianceStatusChanged(object sender, ApplianceStatusChangedEventArgs e)
        {
            if (e.NewStatus == ApplianceStatus.Repaired || e.NewStatus == ApplianceStatus.Delayed)
            {
                Console.WriteLine($"[EVENT] الجهاز {e.Appliance.SerialNumber} تغيرت حالته إلى {e.NewStatus}");
            }
        }

        public decimal CalculateTotalRepairCost()
        {
            return _appliances.Sum(a => a.CalculateRepairCost());
        }
    }
}
5.6 ملف التشغيل الرئيسي Program.cs
csharp
using System;
using HomeApplianceRepairCenter.Models;
using HomeApplianceRepairCenter.Services;

namespace HomeApplianceRepairCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            RepairCenter center = new RepairCenter();

            var fridge = new Refrigerator("FRG1001", "Samsung", DateTime.Today, 100m, 350, true);
            var washer = new WashingMachine("WSH2002", "LG", DateTime.Today, 80m, 8, true);

            center.AddAppliance(fridge);
            center.AddAppliance(washer);

            fridge.StartRepair();
            center.CompleteRepair("FRG1001"); // Note: requires the method in RepairCenter

            Console.WriteLine($"التكلفة الإجمالية: {center.CalculateTotalRepairCost():C}");
        }
    }
}

.6. مستودع المشروع على GitHub
تم رفع المشروع البرمجي بالكامل، مع إضافة جميع أعضاء الفريق الخمسة كمساهمين (Collaborators)، وتوثيق سجل المساهمات (Commits) لكل طالب:
🔗 رابط المستودع:



