using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.Data.SqlClient; // استيراد مكتبة SqlClient للاتصال بقاعدة بيانات SQL Server
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Text; // استيراد مكتبة Text للتعامل مع النصوص
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة

namespace DVLD_DataAccess // مساحة الاسم الخاصة بطبقة الوصول للبيانات
{
    public class clsCountryData // كلاس لإدارة بيانات الدول في قاعدة البيانات
    {
        // تعداد (Enum) لتحديد الجنس
        // Male = 0 يعني ذكر
        // Female = 1 يعني أنثى
        public enum enGendor { Male = 0, Female = 1 };

        // دالة للحصول على معلومات دولة بناءً على رقم الدولة (CountryID)
        // ترجع true إذا تم العثور على الدولة، و false إذا لم يتم العثور عليها
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
            {
                bool isFound = false; // متغير لتحديد ما إذا كان السجل موجوداً أم لا

                // إنشاء اتصال جديد بقاعدة البيانات
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                // استعلام SQL للحصول على اسم الدولة بناءً على CountryID
                string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

                // إنشاء أمر SQL
                SqlCommand command = new SqlCommand(query, connection);

                // إضافة المعامل للاستعلام
                command.Parameters.AddWithValue("@CountryID", ID);

                try // محاولة تنفيذ الكود
                {
                    connection.Open(); // فتح الاتصال
                    SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                    if (reader.Read()) // قراءة السجل
                    {

                        // تم العثور على السجل
                        isFound = true;

                        CountryName = (string)reader["CountryName"]; // اسم الدولة

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

        // دالة للحصول على معلومات دولة بناءً على اسم الدولة (CountryName)
        // ترجع true إذا تم العثور على الدولة، و false إذا لم يتم العثور عليها
        public static bool GetCountryInfoByName(string CountryName, ref int ID)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان السجل موجوداً أم لا

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للحصول على رقم الدولة بناءً على اسم الدولة
            string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open(); // فتح الاتصال
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                if (reader.Read()) // قراءة السجل
                {

                    // تم العثور على السجل
                    isFound = true;

                    ID = (int)reader["CountryID"]; // رقم الدولة

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

        // دالة للحصول على جميع الدول من قاعدة البيانات
        // ترجع DataTable يحتوي على جميع الدول مرتبة حسب الاسم
        public static DataTable GetAllCountries()
        {

            DataTable dt = new DataTable(); // إنشاء جدول بيانات فارغ
            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للحصول على جميع الدول مرتبة حسب الاسم
            string query = "SELECT * FROM Countries order by CountryName";

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

            return dt; // إرجاع DataTable يحتوي على جميع الدول

        }

    }
}
