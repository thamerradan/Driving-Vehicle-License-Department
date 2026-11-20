using System; // استيراد مكتبة System الأساسية
using System.ComponentModel; // استيراد مكتبة ComponentModel للخصائص والسمات
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Data.SqlClient; // استيراد مكتبة SqlClient للاتصال بقاعدة بيانات SQL Server

namespace DVLD_DataAccess // مساحة الاسم الخاصة بطبقة الوصول للبيانات
{
    public class clsUserData // كلاس لإدارة بيانات المستخدمين في قاعدة البيانات
    {
       
        // دالة للحصول على معلومات المستخدم بناءً على رقم المستخدم (UserID)
        // تستخدم ref parameters لإرجاع القيم من الدالة
        public static bool GetUserInfoByUserID(int UserID, ref int PersonID, ref string UserName,
            ref string Password, ref bool IsActive)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان السجل موجوداً أم لا

            // إنشاء اتصال جديد بقاعدة البيانات باستخدام سلسلة الاتصال
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للحصول على بيانات المستخدم بناءً على UserID
            // استخدام @UserID لحماية من SQL Injection
            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            // إنشاء أمر SQL لتنفيذ الاستعلام
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل (Parameter) للاستعلام لحماية من SQL Injection
            command.Parameters.AddWithValue("@UserID", UserID);

            try // محاولة تنفيذ الكود
            {
                connection.Open(); // فتح الاتصال بقاعدة البيانات
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام وإرجاع قارئ البيانات

                if (reader.Read()) // قراءة السجل الأول من النتائج
                {
                    // تم العثور على السجل
                    isFound = true;

                    // استخراج البيانات من قاعدة البيانات وتحويلها إلى الأنواع المناسبة
                    PersonID = (int)reader["PersonID"]; // رقم الشخص
                    UserName = (string)reader["UserName"]; // اسم المستخدم
                    Password = (string)reader["Password"]; // كلمة المرور
                    IsActive = (bool)reader["IsActive"]; // حالة المستخدم (نشط/غير نشط)
                    

                }
                else
                {
                    // لم يتم العثور على السجل
                    isFound = false;
                }

                reader.Close(); // إغلاق قارئ البيانات


            }
            catch (Exception ex) // معالجة الأخطاء في حالة حدوث استثناء
            {
                //Console.WriteLine("Error: " + ex.Message); // طباعة رسالة الخطأ (معطلة حالياً)
                
                isFound = false; // تعيين isFound إلى false في حالة الخطأ
            }
            finally // يتم تنفيذ هذا الكود دائماً حتى في حالة الخطأ
            {
                connection.Close(); // إغلاق الاتصال بقاعدة البيانات
            }

            return isFound; // إرجاع true إذا تم العثور على السجل، و false إذا لم يتم العثور عليه
        }


        // دالة للحصول على معلومات المستخدم بناءً على رقم الشخص (PersonID)
        // تستخدم عندما نريد معرفة إذا كان الشخص لديه حساب مستخدم
        public static bool GetUserInfoByPersonID(int PersonID, ref int UserID, ref string UserName,
          ref string Password,ref bool IsActive)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان السجل موجوداً أم لا

            // إنشاء اتصال جديد بقاعدة البيانات
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للحصول على بيانات المستخدم بناءً على PersonID
            string query = "SELECT * FROM Users WHERE PersonID = @PersonID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open(); // فتح الاتصال
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                if (reader.Read()) // قراءة السجل
                {
                    // تم العثور على السجل
                    isFound = true;

                    // استخراج البيانات
                    UserID = (int)reader["UserID"]; // رقم المستخدم
                    UserName = (string)reader["UserName"]; // اسم المستخدم
                    Password = (string)reader["Password"]; // كلمة المرور
                    IsActive = (bool)reader["IsActive"]; // حالة المستخدم


                }
                else
                {
                    // لم يتم العثور على السجل
                    isFound = false;
                }

                reader.Close(); // إغلاق القارئ

            }
            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);

                isFound = false; // في حالة الخطأ، نعيد false
            }
            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            return isFound; // إرجاع النتيجة
        }

        // دالة للتحقق من صحة اسم المستخدم وكلمة المرور (تستخدم في تسجيل الدخول)
        // تقوم بالبحث عن مستخدم يطابق اسم المستخدم وكلمة المرور
        public static bool GetUserInfoByUsernameAndPassword(string UserName,  string Password, 
            ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان المستخدم موجوداً وصحيح

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للتحقق من اسم المستخدم وكلمة المرور
            string query = "SELECT * FROM Users WHERE Username = @Username and Password=@Password;";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@Username", UserName); // اسم المستخدم
            command.Parameters.AddWithValue("@Password", Password); // كلمة المرور


            try
            {
                connection.Open(); // فتح الاتصال
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                if (reader.Read()) // قراءة السجل
                {
                    // تم العثور على المستخدم (اسم المستخدم وكلمة المرور صحيحة)
                    isFound = true;
                    UserID= (int)reader["UserID"]; // رقم المستخدم
                    PersonID = (int)reader["PersonID"]; // رقم الشخص
                    UserName = (string)reader["UserName"]; // اسم المستخدم
                    Password = (string)reader["Password"]; // كلمة المرور
                    IsActive = (bool)reader["IsActive"]; // حالة المستخدم (نشط/غير نشط)


                }
                else
                {
                    // لم يتم العثور على المستخدم (اسم المستخدم أو كلمة المرور خاطئة)
                    isFound = false;
                }

                reader.Close(); // إغلاق القارئ


            }
            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);

                isFound = false; // في حالة الخطأ، نعيد false
            }
            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            return isFound; // إرجاع true إذا كانت بيانات الدخول صحيحة، و false إذا كانت خاطئة
        }

        // دالة لإضافة مستخدم جديد إلى قاعدة البيانات
        // ترجع رقم المستخدم الجديد (UserID) إذا نجحت العملية، و -1 إذا فشلت
        public static int AddNewUser(int PersonID,  string UserName,
             string Password,  bool IsActive)
        {
            // هذه الدالة ترجع رقم المستخدم الجديد إذا نجحت العملية و -1 إذا فشلت
            int UserID = -1; // تهيئة القيمة الافتراضية (-1 يعني فشل العملية)

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لإضافة مستخدم جديد
            // SCOPE_IDENTITY() ترجع آخر رقم تم إدراجه في عمود IDENTITY (UserID)
            string query = @"INSERT INTO Users (PersonID,UserName,Password,IsActive)
                             VALUES (@PersonID, @UserName,@Password,@IsActive);
                             SELECT SCOPE_IDENTITY();";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@PersonID", PersonID); // رقم الشخص
            command.Parameters.AddWithValue("@UserName", UserName); // اسم المستخدم
            command.Parameters.AddWithValue("@Password", Password); // كلمة المرور
            command.Parameters.AddWithValue("@IsActive", IsActive); // حالة المستخدم

            try
            {
                connection.Open(); // فتح الاتصال

                // ExecuteScalar() تنفذ الاستعلام وترجع قيمة واحدة (رقم المستخدم الجديد)
                object result = command.ExecuteScalar();

                // التحقق من أن النتيجة ليست null وتحويلها إلى int
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    UserID = insertedID; // حفظ رقم المستخدم الجديد
                }
            }

            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);
                // في حالة الخطأ، UserID سيبقى -1

            }

            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            return UserID; // إرجاع رقم المستخدم الجديد أو -1 في حالة الفشل
        }


        // دالة لتحديث بيانات مستخدم موجود في قاعدة البيانات
        // ترجع true إذا تم التحديث بنجاح، و false إذا فشلت العملية
        public static bool UpdateUser(int UserID, int PersonID, string UserName,
             string Password, bool IsActive)
        {

            int rowsAffected = 0; // عدد الصفوف المتأثرة بالتحديث
            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لتحديث بيانات المستخدم
            string query = @"Update  Users  
                            set PersonID = @PersonID,
                                UserName = @UserName,
                                Password = @Password,
                                IsActive = @IsActive
                                where UserID = @UserID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@PersonID", PersonID); // رقم الشخص
            command.Parameters.AddWithValue("@UserName", UserName); // اسم المستخدم
            command.Parameters.AddWithValue("@Password", Password); // كلمة المرور
            command.Parameters.AddWithValue("@IsActive", IsActive); // حالة المستخدم
            command.Parameters.AddWithValue("@UserID", UserID); // رقم المستخدم (للتحديث)


            try
            {
                connection.Open(); // فتح الاتصال
                // ExecuteNonQuery() تنفذ الاستعلام وترجع عدد الصفوف المتأثرة
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false; // إرجاع false في حالة الخطأ
            }

            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            // إرجاع true إذا تم تحديث صف واحد على الأقل، و false إذا لم يتم تحديث أي صف
            return (rowsAffected > 0);
        }


        // دالة للحصول على جميع المستخدمين من قاعدة البيانات
        // ترجع DataTable يحتوي على جميع المستخدمين مع أسمائهم الكاملة
        public static DataTable GetAllUsers()
        {

            DataTable dt = new DataTable(); // إنشاء جدول بيانات فارغ
            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للحصول على جميع المستخدمين مع أسمائهم الكاملة
            // INNER JOIN لربط جدول Users بجدول People للحصول على الاسم الكامل
            // ISNULL للتعامل مع ThirdName إذا كان null
            string query = @"SELECT  Users.UserID, Users.PersonID,
                            FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL( People.ThirdName,'') +' ' + People.LastName,
                             Users.UserName, Users.IsActive
                             FROM  Users INNER JOIN
                                    People ON Users.PersonID = People.PersonID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open(); // فتح الاتصال

                // تنفيذ الاستعلام وإرجاع قارئ البيانات
                SqlDataReader reader = command.ExecuteReader();

                // التحقق من وجود صفوف في النتائج
                if (reader.HasRows)

                {
                    dt.Load(reader); // تحميل البيانات من القارئ إلى DataTable
                }

                reader.Close(); // إغلاق القارئ


            }

            catch (Exception ex) // معالجة الأخطاء
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            return dt; // إرجاع DataTable يحتوي على جميع المستخدمين

        }

        // دالة لحذف مستخدم من قاعدة البيانات
        // ترجع true إذا تم الحذف بنجاح، و false إذا فشلت العملية
        public static bool DeleteUser(int UserID)
        {

            int rowsAffected = 0; // عدد الصفوف المتأثرة بالحذف

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لحذف المستخدم
            string query = @"Delete Users 
                                where UserID = @UserID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open(); // فتح الاتصال

                // ExecuteNonQuery() تنفذ الاستعلام وترجع عدد الصفوف المتأثرة
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex) // معالجة الأخطاء
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally // يتم تنفيذه دائماً
            {

                connection.Close(); // إغلاق الاتصال

            }

            // إرجاع true إذا تم حذف صف واحد على الأقل، و false إذا لم يتم حذف أي صف
            return (rowsAffected > 0);

        }

        // دالة للتحقق من وجود مستخدم بناءً على رقم المستخدم (UserID)
        // ترجع true إذا كان المستخدم موجوداً، و false إذا لم يكن موجوداً
        public static bool IsUserExist(int UserID)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان المستخدم موجوداً

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للتحقق من وجود المستخدم
            // نستخدم SELECT Found=1 بدلاً من SELECT * لأننا لا نحتاج البيانات، فقط نريد التحقق من الوجود
            string query = "SELECT Found=1 FROM Users WHERE UserID = @UserID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open(); // فتح الاتصال
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                // HasRows ترجع true إذا كان هناك صفوف في النتائج
                isFound = reader.HasRows;

                reader.Close(); // إغلاق القارئ
            }
            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false; // في حالة الخطأ، نعيد false
            }
            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            return isFound; // إرجاع true إذا كان المستخدم موجوداً، و false إذا لم يكن موجوداً
        }

        // دالة للتحقق من وجود مستخدم بناءً على اسم المستخدم (UserName)
        // ترجع true إذا كان اسم المستخدم موجوداً، و false إذا لم يكن موجوداً
        // تستخدم للتحقق من عدم تكرار أسماء المستخدمين
        public static bool IsUserExist(string UserName)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان اسم المستخدم موجوداً

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للتحقق من وجود اسم المستخدم
            string query = "SELECT Found=1 FROM Users WHERE UserName = @UserName";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open(); // فتح الاتصال
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                // HasRows ترجع true إذا كان هناك صفوف في النتائج
                isFound = reader.HasRows;

                reader.Close(); // إغلاق القارئ
            }
            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false; // في حالة الخطأ، نعيد false
            }
            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            return isFound; // إرجاع true إذا كان اسم المستخدم موجوداً، و false إذا لم يكن موجوداً
        }

        // دالة للتحقق من وجود مستخدم بناءً على رقم الشخص (PersonID)
        // ترجع true إذا كان الشخص لديه حساب مستخدم، و false إذا لم يكن لديه حساب
        // تستخدم للتحقق من أن الشخص لا يملك حساب مستخدم قبل إنشاء حساب جديد له
        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان الشخص لديه حساب مستخدم

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للتحقق من وجود مستخدم لهذا الشخص
            string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open(); // فتح الاتصال
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                // HasRows ترجع true إذا كان هناك صفوف في النتائج
                isFound = reader.HasRows;

                reader.Close(); // إغلاق القارئ
            }
            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false; // في حالة الخطأ، نعيد false
            }
            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            return isFound; // إرجاع true إذا كان الشخص لديه حساب مستخدم، و false إذا لم يكن لديه حساب
        }

        // دالة للتحقق من وجود مستخدم بناءً على رقم الشخص (PersonID)
        // هذه الدالة مشابهة لـ IsUserExistForPersonID (يبدو أنها نسخة مكررة)
        // ترجع true إذا كان الشخص لديه حساب مستخدم، و false إذا لم يكن لديه حساب
        public static bool DoesPersonHaveUser44(int PersonID)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان الشخص لديه حساب مستخدم

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للتحقق من وجود مستخدم لهذا الشخص
            string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open(); // فتح الاتصال
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                // HasRows ترجع true إذا كان هناك صفوف في النتائج
                isFound = reader.HasRows;

                reader.Close(); // إغلاق القارئ
            }
            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false; // في حالة الخطأ، نعيد false
            }
            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            return isFound; // إرجاع true إذا كان الشخص لديه حساب مستخدم، و false إذا لم يكن لديه حساب
        }

        // دالة لتغيير كلمة مرور مستخدم
        // ترجع true إذا تم تغيير كلمة المرور بنجاح، و false إذا فشلت العملية
        public static bool ChangePassword(int UserID, string NewPassword)
        {

            int rowsAffected = 0; // عدد الصفوف المتأثرة بالتحديث
            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لتحديث كلمة المرور
            string query = @"Update  Users  
                            set Password = @Password
                            where UserID = @UserID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@UserID", UserID); // رقم المستخدم
            command.Parameters.AddWithValue("@Password", NewPassword); // كلمة المرور الجديدة

            try
            {
                connection.Open(); // فتح الاتصال
                // ExecuteNonQuery() تنفذ الاستعلام وترجع عدد الصفوف المتأثرة
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false; // إرجاع false في حالة الخطأ
            }

            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            // إرجاع true إذا تم تحديث صف واحد على الأقل، و false إذا لم يتم تحديث أي صف
            return (rowsAffected > 0);
        }

    }
}
