using System; // استيراد مكتبة System الأساسية
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Runtime.InteropServices; // استيراد مكتبة InteropServices للتعامل مع الكود غير المُدار
using DVLD_DataAccess; // استيراد طبقة الوصول للبيانات

namespace DVLD_Buisness // مساحة الاسم الخاصة بطبقة الأعمال (Business Layer)
{
    public  class clsUser // كلاس لإدارة المستخدمين في طبقة الأعمال
    {
        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew; // متغير لتحديد الوضع الحالي (افتراضياً إضافة جديدة)

        // خصائص المستخدم
        public int UserID { set; get; } // رقم المستخدم
        public int PersonID { set; get; } // رقم الشخص المرتبط بالمستخدم
        public clsPerson PersonInfo; // معلومات الشخص المرتبط بالمستخدم
        public string UserName { set; get; } // اسم المستخدم
        public string Password { set; get; } // كلمة المرور
        public bool IsActive { set; get; } // حالة المستخدم (نشط/غير نشط)
     
        // Constructor الافتراضي - يُستخدم عند إنشاء مستخدم جديد
        public clsUser()

        {     
            this.UserID = -1; // -1 يعني مستخدم جديد لم يُحفظ بعد
            this.UserName = ""; // تهيئة اسم المستخدم كسلسلة فارغة
            this.Password = ""; // تهيئة كلمة المرور كسلسلة فارغة
            this.IsActive = true; // المستخدم الجديد يكون نشطاً افتراضياً
            Mode = enMode.AddNew; // تعيين الوضع إلى إضافة جديدة
        }

        // Constructor خاص - يُستخدم عند تحميل مستخدم موجود من قاعدة البيانات
        private clsUser(int UserID, int PersonID, string Username,string Password,
            bool IsActive)

        {
            this.UserID = UserID; // رقم المستخدم
            this.PersonID = PersonID; // رقم الشخص
            this.PersonInfo = clsPerson.Find(PersonID); // تحميل معلومات الشخص المرتبط
            this.UserName = Username; // اسم المستخدم
            this.Password = Password; // كلمة المرور
            this.IsActive = IsActive; // حالة المستخدم

            Mode = enMode.Update; // تعيين الوضع إلى تحديث (لأن المستخدم موجود)
        }

        // دالة خاصة لإضافة مستخدم جديد إلى قاعدة البيانات
        // تستدعي طبقة الوصول للبيانات (DataAccess Layer)
        private bool _AddNewUser()
        {
            // استدعاء طبقة الوصول للبيانات

            // إضافة مستخدم جديد والحصول على رقم المستخدم الجديد
            this.UserID = clsUserData.AddNewUser(this.PersonID,this.UserName,
                this.Password,this.IsActive);

            // إرجاع true إذا تم الحصول على رقم مستخدم صحيح (غير -1)
            return (this.UserID != -1);
        }
        
        // دالة خاصة لتحديث بيانات مستخدم موجود في قاعدة البيانات
        // تستدعي طبقة الوصول للبيانات (DataAccess Layer)
        private bool _UpdateUser()
        {
            // استدعاء طبقة الوصول للبيانات

            // تحديث بيانات المستخدم وإرجاع true إذا نجحت العملية
            return clsUserData.UpdateUser(this.UserID,this.PersonID,this.UserName,
                this.Password,this.IsActive);
        }
        // دالة ثابتة للبحث عن مستخدم بناءً على رقم المستخدم (UserID)
        // ترجع كائن clsUser إذا تم العثور عليه، و null إذا لم يتم العثور عليه
        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            // استدعاء طبقة الوصول للبيانات للحصول على معلومات المستخدم
            bool IsFound = clsUserData.GetUserInfoByUserID
                                ( UserID,ref PersonID, ref UserName,ref Password,ref IsActive);

            if (IsFound)
                // نرجع كائن جديد من المستخدم بالبيانات الصحيحة
                return new clsUser(UserID,PersonID,UserName,Password,IsActive);
            else
                return null; // لم يتم العثور على المستخدم
        }
        
        // دالة ثابتة للبحث عن مستخدم بناءً على رقم الشخص (PersonID)
        // ترجع كائن clsUser إذا تم العثور عليه، و null إذا لم يتم العثور عليه
        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            // استدعاء طبقة الوصول للبيانات
            bool IsFound = clsUserData.GetUserInfoByPersonID
                                (PersonID, ref UserID, ref UserName, ref Password, ref IsActive);

            if (IsFound)
                // نرجع كائن جديد من المستخدم بالبيانات الصحيحة
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null; // لم يتم العثور على المستخدم
        }
        
        // دالة ثابتة للبحث عن مستخدم بناءً على اسم المستخدم وكلمة المرور
        // تُستخدم في تسجيل الدخول
        // ترجع كائن clsUser إذا تم العثور عليه، و null إذا لم يتم العثور عليه
        public static clsUser FindByUsernameAndPassword(string UserName,string Password)
        {
            int UserID = -1;
            int PersonID=-1;

            bool IsActive = false;

            // استدعاء طبقة الوصول للبيانات للتحقق من صحة بيانات الدخول
            bool IsFound = clsUserData.GetUserInfoByUsernameAndPassword
                                (UserName , Password,ref UserID,ref PersonID, ref IsActive);

            if (IsFound)
                // نرجع كائن جديد من المستخدم بالبيانات الصحيحة
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null; // بيانات الدخول غير صحيحة
        }

        // دالة لحفظ المستخدم (إضافة جديد أو تحديث موجود)
        // ترجع true إذا نجحت العملية، و false إذا فشلت
        public bool Save()
        {
            switch (Mode) // التحقق من الوضع الحالي
            {
                case enMode.AddNew: // إذا كان الوضع إضافة جديدة
                    if (_AddNewUser()) // محاولة إضافة المستخدم
                    {

                        Mode = enMode.Update; // تغيير الوضع إلى تحديث بعد الإضافة الناجحة
                        return true; // إرجاع true للنجاح
                    }
                    else
                    {
                        return false; // إرجاع false للفشل
                    }

                case enMode.Update: // إذا كان الوضع تحديث

                    return _UpdateUser(); // تحديث المستخدم وإرجاع النتيجة

            }

            return false; // إرجاع false في حالة أخرى
        }

        // دالة ثابتة للحصول على جميع المستخدمين من قاعدة البيانات
        // ترجع DataTable يحتوي على جميع المستخدمين
        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers(); // استدعاء طبقة الوصول للبيانات
        }

        // دالة ثابتة لحذف مستخدم من قاعدة البيانات
        // ترجع true إذا تم الحذف بنجاح، و false إذا فشلت العملية
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID); // استدعاء طبقة الوصول للبيانات
        }

        // دالة ثابتة للتحقق من وجود مستخدم بناءً على رقم المستخدم (UserID)
        // ترجع true إذا كان المستخدم موجوداً، و false إذا لم يكن موجوداً
        public static bool isUserExist(int UserID)
        {
           return clsUserData.IsUserExist(UserID); // استدعاء طبقة الوصول للبيانات
        }

        // دالة ثابتة للتحقق من وجود مستخدم بناءً على اسم المستخدم (UserName)
        // ترجع true إذا كان اسم المستخدم موجوداً، و false إذا لم يكن موجوداً
        public static bool isUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName); // استدعاء طبقة الوصول للبيانات
        }

        // دالة ثابتة للتحقق من وجود مستخدم بناءً على رقم الشخص (PersonID)
        // ترجع true إذا كان الشخص لديه حساب مستخدم، و false إذا لم يكن لديه حساب
        public static bool isUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID); // استدعاء طبقة الوصول للبيانات
        }


    }
}
