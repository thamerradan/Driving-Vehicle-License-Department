using System; // استيراد مكتبة System الأساسية
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Diagnostics.Eventing.Reader; // استيراد مكتبة Eventing.Reader لقراءة سجلات الأحداث
using DVLD_DataAccess; // استيراد طبقة الوصول للبيانات

namespace DVLD_Buisness // مساحة الاسم الخاصة بطبقة الأعمال (Business Layer)
{
    public class clsLicenseClass // كلاس لإدارة فئات الرخص في طبقة الأعمال
    {

        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew; // متغير لتحديد الوضع الحالي (افتراضياً إضافة جديدة)

        // خصائص فئة الرخصة
        public int LicenseClassID { set; get; } // رقم فئة الرخصة
        public string ClassName { set; get; } // اسم فئة الرخصة
        public string ClassDescription { set; get; } // وصف فئة الرخصة
        public byte MinimumAllowedAge { set; get; } // الحد الأدنى للعمر المسموح
        public byte DefaultValidityLength { set; get; } // مدة الصلاحية الافتراضية (بالسنوات)
        public float ClassFees { set; get; } // رسوم فئة الرخصة

        // Constructor الافتراضي - يُستخدم عند إنشاء فئة رخصة جديدة
        public clsLicenseClass()

        {
            this.LicenseClassID = -1; // -1 يعني فئة رخصة جديدة لم تُحفظ بعد
            this.ClassName = ""; // تهيئة اسم الفئة كسلسلة فارغة
            this.ClassDescription = ""; // تهيئة الوصف كسلسلة فارغة
            this.MinimumAllowedAge = 18; // الحد الأدنى للعمر (18 سنة)
            this.DefaultValidityLength = 10; // مدة الصلاحية الافتراضية (10 سنوات)
            this.ClassFees = 0; // تهيئة الرسوم إلى 0
          
            Mode = enMode.AddNew;

        }

        public clsLicenseClass(int LicenseClassID, string ClassName,
            string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)

        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
            Mode = enMode.Update;
        }

        private bool _AddNewLicenseClass()
        {
            //call DataAccess Layer 

            this.LicenseClassID = clsLicenseClassData.AddNewLicenseClass(this.ClassName,this.ClassDescription,
                this.MinimumAllowedAge,this.DefaultValidityLength,this.ClassFees);
              

            return (this.LicenseClassID != -1);
        }

        private bool _UpdateLicenseClass()
        {
            //call DataAccess Layer 

            return clsLicenseClassData.UpdateLicenseClass(this.LicenseClassID, this.ClassName, this.ClassDescription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = ""; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; float ClassFees = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref ClassName, ref ClassDescription,
                    ref  MinimumAllowedAge,  ref DefaultValidityLength, ref ClassFees))

                return new clsLicenseClass(LicenseClassID,ClassName ,ClassDescription,
                    MinimumAllowedAge,DefaultValidityLength,ClassFees);
            else
                return null;

        }

        public static clsLicenseClass Find(string ClassName)
        {
            int LicenseClassID = -1; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; float ClassFees = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByClassName( ClassName, ref  LicenseClassID, ref ClassDescription,
                    ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;

        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicenseClass())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicenseClass();

            }

            return false;
        }

    }
}
