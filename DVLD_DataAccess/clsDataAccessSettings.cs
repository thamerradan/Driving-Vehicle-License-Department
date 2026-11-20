using System; // استيراد مكتبة System الأساسية

namespace DVLD_DataAccess // مساحة الاسم الخاصة بطبقة الوصول للبيانات
{
    static class clsDataAccessSettings // كلاس ثابت (static) لتخزين إعدادات الاتصال بقاعدة البيانات
    {
        // سلسلة الاتصال بقاعدة البيانات SQL Server
        // Server=. يعني السيرفر المحلي (localhost)
        // Database=DVLD اسم قاعدة البيانات
        // User Id=sa اسم المستخدم
        // Password=123321 كلمة المرور
        public static string ConnectionString = "Server=.;Database=DVLD;User Id=sa;Password=123321;";


    }
}
