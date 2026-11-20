using System; // استيراد مكتبة System الأساسية
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using DVLD_DataAccess; // استيراد طبقة الوصول للبيانات

namespace DVLD_Buisness // مساحة الاسم الخاصة بطبقة الأعمال (Business Layer)
{
    public class clsCountry // كلاس لإدارة الدول في طبقة الأعمال
    {

        // خصائص الدولة
        public int ID { set; get; } // رقم الدولة
        public string CountryName { set; get; } // اسم الدولة
   
        // Constructor الافتراضي - يُستخدم عند إنشاء دولة جديدة
        public clsCountry()

        {
            this.ID = -1; // -1 يعني دولة جديدة لم تُحفظ بعد
            this.CountryName = ""; // تهيئة اسم الدولة كسلسلة فارغة

        }

        // Constructor خاص - يُستخدم عند تحميل دولة موجودة من قاعدة البيانات
        private clsCountry(int ID, string CountryName)

        {
            this.ID = ID; // رقم الدولة
            this.CountryName = CountryName; // اسم الدولة
        }

        // دالة ثابتة للبحث عن دولة بناءً على رقم الدولة (ID)
        // ترجع كائن clsCountry إذا تم العثور عليه، و null إذا لم يتم العثور عليه
        public static clsCountry Find(int ID)
        {
            string CountryName = "";

            // استدعاء طبقة الوصول للبيانات للحصول على معلومات الدولة
            if (clsCountryData.GetCountryInfoByID(ID, ref CountryName))

                return new clsCountry(ID, CountryName); // إرجاع كائن جديد بالبيانات الصحيحة
            else
                return null; // لم يتم العثور على الدولة

        }

        // دالة ثابتة للبحث عن دولة بناءً على اسم الدولة (CountryName)
        // ترجع كائن clsCountry إذا تم العثور عليه، و null إذا لم يتم العثور عليه
        public static clsCountry Find(string CountryName)
        {

            int ID = -1;
           
            // استدعاء طبقة الوصول للبيانات
            if (clsCountryData.GetCountryInfoByName(CountryName, ref ID ))

                return new clsCountry(ID, CountryName); // إرجاع كائن جديد بالبيانات الصحيحة
            else
                return null; // لم يتم العثور على الدولة

        }

        // دالة ثابتة للحصول على جميع الدول من قاعدة البيانات
        // ترجع DataTable يحتوي على جميع الدول
        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries(); // استدعاء طبقة الوصول للبيانات

        }

    }
}
