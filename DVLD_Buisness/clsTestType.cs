using System; // استيراد مكتبة System الأساسية
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Diagnostics.Eventing.Reader; // استيراد مكتبة Eventing.Reader لقراءة سجلات الأحداث
using DVLD_DataAccess; // استيراد طبقة الوصول للبيانات

namespace DVLD_Buisness // مساحة الاسم الخاصة بطبقة الأعمال (Business Layer)
{
    public class clsTestType // كلاس لإدارة أنواع الاختبارات في طبقة الأعمال
    {

        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew; // متغير لتحديد الوضع الحالي (افتراضياً إضافة جديدة)
        // تعداد (Enum) لتحديد أنواع الاختبارات المختلفة
        public  enum enTestType { VisionTest = 1, WrittenTest = 2,StreetTest=3 };

        public clsTestType.enTestType ID { set; get; } // نوع الاختبار (من الـ Enum)
        public string Title { set; get; } // عنوان نوع الاختبار
        public string Description { set; get; } // وصف نوع الاختبار
        public float Fees { set; get; } // رسوم نوع الاختبار
        
        // Constructor الافتراضي - يُستخدم عند إنشاء نوع اختبار جديد
        public clsTestType()

        {
            this.ID = clsTestType.enTestType.VisionTest; // تعيين نوع الاختبار الافتراضي إلى اختبار النظر
            this.Title = ""; // تهيئة العنوان كسلسلة فارغة
            this.Description = ""; // تهيئة الوصف كسلسلة فارغة
            this.Fees = 0; // تهيئة الرسوم إلى 0
            Mode = enMode.AddNew; // تعيين الوضع إلى إضافة جديدة

        }

        public clsTestType(clsTestType.enTestType ID, string TestTypeTitel,string Description,float TestTypeFees)

        {
            this.ID = ID;
            this.Title = TestTypeTitel;
            this.Description = Description;

            this.Fees = TestTypeFees;
            Mode = enMode.Update;
        }

        private bool _AddNewTestType()
        {
            //call DataAccess Layer 

            this.ID =(clsTestType.enTestType) clsTestTypeData.AddNewTestType(this.Title,this.Description, this.Fees);
              
            return (this.Title !="");
        }

        private bool _UpdateTestType()
        {
            //call DataAccess Layer 

            return clsTestTypeData.UpdateTestType((int) this.ID,this.Title,this.Description,this.Fees);
        }

        public static clsTestType Find(clsTestType.enTestType TestTypeID)
        {
            string Title = "", Description=""; float Fees=0;

            if (clsTestTypeData.GetTestTypeInfoByID((int) TestTypeID, ref Title,ref Description, ref Fees))

                return new clsTestType(TestTypeID, Title, Description,Fees);
            else
                return null;

        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestType();

            }

            return false;
        }

    }
}
