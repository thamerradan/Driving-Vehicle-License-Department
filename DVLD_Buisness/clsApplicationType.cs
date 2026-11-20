using System; // استيراد مكتبة System الأساسية
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Diagnostics.Eventing.Reader; // استيراد مكتبة Eventing.Reader لقراءة سجلات الأحداث
using DVLD_DataAccess; // استيراد طبقة الوصول للبيانات

namespace DVLD_Buisness // مساحة الاسم الخاصة بطبقة الأعمال (Business Layer)
{
    public class clsApplicationType // كلاس لإدارة أنواع الطلبات في طبقة الأعمال
    {

        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew; // متغير لتحديد الوضع الحالي (افتراضياً إضافة جديدة)


        // خصائص نوع الطلب
        public int ID { set; get; } // رقم نوع الطلب
        public string Title { set; get; } // عنوان نوع الطلب
        public float Fees { set; get; } // رسوم نوع الطلب

        // Constructor الافتراضي - يُستخدم عند إنشاء نوع طلب جديد
        public clsApplicationType()

        {
            this.ID = -1; // -1 يعني نوع طلب جديد لم يُحفظ بعد
            this.Title = ""; // تهيئة العنوان كسلسلة فارغة
            this.Fees = 0; // تهيئة الرسوم إلى 0
            Mode = enMode.AddNew; // تعيين الوضع إلى إضافة جديدة

        }

        // Constructor خاص - يُستخدم عند تحميل نوع طلب موجود من قاعدة البيانات
        public clsApplicationType(int ID, string ApplicationTypeTitel,float ApplicationTypeFees)

        {
            this.ID =  ID; // رقم نوع الطلب
            this.Title = ApplicationTypeTitel; // عنوان نوع الطلب
            this.Fees = ApplicationTypeFees; // رسوم نوع الطلب
            Mode = enMode.Update; // تعيين الوضع إلى تحديث (لأن نوع الطلب موجود)
        }

        // دالة خاصة لإضافة نوع طلب جديد إلى قاعدة البيانات
        // تستدعي طبقة الوصول للبيانات (DataAccess Layer)
        private bool _AddNewApplicationType()
        {
            // استدعاء طبقة الوصول للبيانات

            // إضافة نوع طلب جديد والحصول على رقم نوع الطلب الجديد
            this.ID = clsApplicationTypeData.AddNewApplicationType( this.Title, this.Fees);
              

            // إرجاع true إذا تم الحصول على رقم نوع طلب صحيح (غير -1)
            return (this.ID != -1);
        }

        // دالة خاصة لتحديث بيانات نوع طلب موجود في قاعدة البيانات
        // تستدعي طبقة الوصول للبيانات (DataAccess Layer)
        private bool _UpdateApplicationType()
        {
            // استدعاء طبقة الوصول للبيانات

            // تحديث بيانات نوع الطلب وإرجاع true إذا نجحت العملية
            return clsApplicationTypeData.UpdateApplicationType(this.ID,this.Title,this.Fees);
        }

        // دالة ثابتة للبحث عن نوع طلب بناءً على رقم نوع الطلب (ID)
        // ترجع كائن clsApplicationType إذا تم العثور عليه، و null إذا لم يتم العثور عليه
        public static clsApplicationType Find(int ID)
        {
            string Title = "";float Fees=0;

            // استدعاء طبقة الوصول للبيانات للحصول على معلومات نوع الطلب
            if (clsApplicationTypeData.GetApplicationTypeInfoByID((int) ID, ref Title, ref Fees))

                return new clsApplicationType(ID, Title,Fees); // إرجاع كائن جديد بالبيانات الصحيحة
            else
                return null; // لم يتم العثور على نوع الطلب

        }

        // دالة ثابتة للحصول على جميع أنواع الطلبات من قاعدة البيانات
        // ترجع DataTable يحتوي على جميع أنواع الطلبات
        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes(); // استدعاء طبقة الوصول للبيانات

        }

        // دالة لحفظ نوع الطلب (إضافة جديد أو تحديث موجود)
        // ترجع true إذا نجحت العملية، و false إذا فشلت
        public bool Save()
        {
            switch (Mode) // التحقق من الوضع الحالي
            {
                case enMode.AddNew: // إذا كان الوضع إضافة جديدة
                    if (_AddNewApplicationType()) // محاولة إضافة نوع الطلب
                    {

                        Mode = enMode.Update; // تغيير الوضع إلى تحديث بعد الإضافة الناجحة
                        return true; // إرجاع true للنجاح
                    }
                    else
                    {
                        return false; // إرجاع false للفشل
                    }

                case enMode.Update: // إذا كان الوضع تحديث

                    return _UpdateApplicationType(); // تحديث نوع الطلب وإرجاع النتيجة

            }

            return false; // إرجاع false في حالة أخرى
        }

    }
}
