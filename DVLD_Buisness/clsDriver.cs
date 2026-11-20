using System; // استيراد مكتبة System الأساسية
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Diagnostics.Eventing.Reader; // استيراد مكتبة Eventing.Reader لقراءة سجلات الأحداث
using DVLD_DataAccess; // استيراد طبقة الوصول للبيانات

namespace DVLD_Buisness // مساحة الاسم الخاصة بطبقة الأعمال (Business Layer)
{
    public class clsDriver // كلاس لإدارة السائقين في طبقة الأعمال
    {

        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew; // متغير لتحديد الوضع الحالي (افتراضياً إضافة جديدة)

        public clsPerson PersonInfo; // معلومات الشخص المرتبط بالسائق

        // خصائص السائق
        public int DriverID { set; get; } // رقم السائق
        public int PersonID { set; get; } // رقم الشخص المرتبط بالسائق
        public int CreatedByUserID { set; get; } // رقم المستخدم الذي أنشأ السجل
        public DateTime CreatedDate {  get; } // تاريخ الإنشاء (للقراءة فقط)

        // Constructor الافتراضي - يُستخدم عند إنشاء سائق جديد
        public clsDriver()

        {
            this.DriverID = -1; // -1 يعني سائق جديد لم يُحفظ بعد
            this.PersonID = -1; // -1 يعني لم يتم ربط شخص بعد
            this.CreatedByUserID = -1; // -1 يعني لم يتم تعيين مستخدم بعد
            this.CreatedDate=DateTime.Now; // تاريخ الإنشاء (التاريخ الحالي)
            Mode = enMode.AddNew; // تعيين الوضع إلى إضافة جديدة

        }

        public clsDriver(int DriverID, int PersonID,int CreatedByUserID, DateTime CreatedDate)

        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
            this.PersonInfo = clsPerson.Find(PersonID);

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            //call DataAccess Layer 

            this.DriverID = clsDriverData.AddNewDriver( PersonID,  CreatedByUserID);
              

            return (this.DriverID != -1);
        }

        private bool _UpdateDriver()
        {
            //call DataAccess Layer 

            return clsDriverData.UpdateDriver(this.DriverID,this.PersonID,this.CreatedByUserID);
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            
            int PersonID = -1; int CreatedByUserID = -1;DateTime CreatedDate= DateTime.Now; 

            if (clsDriverData.GetDriverInfoByDriverID(DriverID, ref PersonID,ref CreatedByUserID,ref CreatedDate))

                return new clsDriver(DriverID,  PersonID,  CreatedByUserID,  CreatedDate);
            else
                return null;

        }

        public static clsDriver FindByPersonID(int PersonID)
        {

            int DriverID = -1; int CreatedByUserID = -1; DateTime CreatedDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByPersonID( PersonID, ref DriverID,  ref CreatedByUserID, ref CreatedDate))

                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;

        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }

            return false;
        }

        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }

        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }

    }
}
