using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.Data.SqlClient; // استيراد مكتبة SqlClient للاتصال بقاعدة بيانات SQL Server
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Text; // استيراد مكتبة Text للتعامل مع النصوص
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة
using static DVLD_DataAccess.clsCountryData; // استيراد static من clsCountryData
using System.Net; // استيراد مكتبة Net للشبكات
using System.Security.Policy; // استيراد مكتبة Security.Policy للسياسات الأمنية

namespace DVLD_DataAccess // مساحة الاسم الخاصة بطبقة الوصول للبيانات
{
    public class clsDriverData // كلاس لإدارة السائقين (Drivers) في قاعدة البيانات
    {

        // دالة للحصول على معلومات سائق بناءً على رقم السائق (DriverID)
        // ترجع true إذا تم العثور على السائق، و false إذا لم يتم العثور عليه
        public static bool GetDriverInfoByDriverID(int DriverID, 
            ref int PersonID,ref int CreatedByUserID,ref DateTime CreatedDate )
            {
                bool isFound = false; // متغير لتحديد ما إذا كان السجل موجوداً أم لا

                // إنشاء اتصال جديد بقاعدة البيانات
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                // استعلام SQL للحصول على بيانات السائق بناءً على DriverID
                string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

                // إنشاء أمر SQL
                SqlCommand command = new SqlCommand(query, connection);

                // إضافة المعامل (Parameter) للاستعلام لحماية من SQL Injection
                command.Parameters.AddWithValue("@DriverID", DriverID);

                try // محاولة تنفيذ الكود
                {
                    connection.Open(); // فتح الاتصال بقاعدة البيانات
                    SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام وإرجاع قارئ البيانات

                    if (reader.Read()) // قراءة السجل الأول من النتائج
                    {

                        // تم العثور على السجل
                        isFound = true;

                        // استخراج البيانات من قاعدة البيانات وتحويلها إلى الأنواع المناسبة
                    PersonID = (int)reader["PersonID"]; // رقم الشخص المرتبط بالسائق
                    CreatedByUserID = (int)reader["CreatedByUserID"]; // رقم المستخدم الذي أنشأ السجل
                    CreatedDate = (DateTime)reader["CreatedDate"]; // تاريخ إنشاء السجل


                }
                    else // إذا لم يتم العثور على السجل
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

        // دالة للحصول على معلومات سائق بناءً على رقم الشخص (PersonID)
        // ترجع true إذا تم العثور على السائق، و false إذا لم يتم العثور عليه
        public static bool GetDriverInfoByPersonID(int PersonID,ref int DriverID,
            ref int CreatedByUserID,ref DateTime CreatedDate)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان السجل موجوداً أم لا

            // إنشاء اتصال جديد بقاعدة البيانات
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للحصول على بيانات السائق بناءً على PersonID
            string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل (Parameter) للاستعلام
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try // محاولة تنفيذ الكود
            {
                connection.Open(); // فتح الاتصال
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                if (reader.Read()) // قراءة السجل
                {

                    // تم العثور على السجل
                    isFound = true;

                    // استخراج البيانات
                    DriverID = (int)reader["DriverID"]; // رقم السائق
                    CreatedByUserID = (int)reader["CreatedByUserID"]; // رقم المستخدم الذي أنشأ السجل
                    CreatedDate = (DateTime)reader["CreatedDate"]; // تاريخ الإنشاء

                }
                else // إذا لم يتم العثور على السجل
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

        // دالة للحصول على جميع السائقين من قاعدة البيانات
        // ترجع DataTable يحتوي على جميع السائقين مرتبين حسب الاسم الكامل
        public static DataTable GetAllDrivers()
            {

                DataTable dt = new DataTable(); // إنشاء جدول بيانات فارغ
                // إنشاء اتصال جديد بقاعدة البيانات
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                // استعلام SQL للحصول على جميع السائقين من View مرتبين حسب الاسم الكامل
                string query = "SELECT * FROM Drivers_View order by FullName";
           
                // إنشاء أمر SQL
                SqlCommand command = new SqlCommand(query, connection);

                try // محاولة تنفيذ الكود
                {
                    connection.Open(); // فتح الاتصال

                    SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                    if (reader.HasRows) // إذا كانت هناك صفوف في النتائج

                    {
                        dt.Load(reader); // تحميل البيانات في جدول البيانات
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

                return dt; // إرجاع جدول البيانات

            }

        // دالة لإضافة سائق جديد إلى قاعدة البيانات
        // ترجع رقم السائق الجديد (DriverID) إذا نجحت العملية، و -1 إذا فشلت
        public static int AddNewDriver( int PersonID, int CreatedByUserID)
        {
            int DriverID = -1; // تهيئة رقم السائق إلى -1 (يعني فشل العملية)

            // إنشاء اتصال جديد بقاعدة البيانات
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لإضافة سائق جديد والحصول على رقم السائق الجديد
            // SCOPE_IDENTITY() يعيد آخر قيمة تم إدخالها في عمود Identity
            string query = @"Insert Into Drivers (PersonID,CreatedByUserID,CreatedDate)
                            Values (@PersonID,@CreatedByUserID,@CreatedDate);
                          
                            SELECT SCOPE_IDENTITY();";
         

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);
            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@PersonID", PersonID); // رقم الشخص
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID); // رقم المستخدم الذي أنشأ السجل
            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now); // تاريخ الإنشاء (التاريخ الحالي)
            try // محاولة تنفيذ الكود
            {
                connection.Open(); // فتح الاتصال

                // تنفيذ الاستعلام وإرجاع القيمة الأولى من النتيجة (رقم السائق الجديد)
                object result = command.ExecuteScalar();

                // التحقق من أن النتيجة ليست null وتحويلها إلى رقم صحيح
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    DriverID = insertedID; // حفظ رقم السائق الجديد
                }
            }

            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }


            return DriverID; // إرجاع رقم السائق الجديد أو -1 في حالة الفشل

        }

        // دالة لتحديث بيانات سائق موجود في قاعدة البيانات
        // ترجع true إذا تم التحديث بنجاح، و false إذا فشلت العملية
        // ملاحظة: لا نقوم بتحديث تاريخ الإنشاء (CreatedDate)
        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {

            int rowsAffected = 0; // متغير لتخزين عدد الصفوف المتأثرة
            // إنشاء اتصال جديد بقاعدة البيانات
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            // لا نقوم بتحديث تاريخ الإنشاء للسائق
            // استعلام SQL لتحديث بيانات السائق
            string query = @"Update  Drivers  
                            set PersonID = @PersonID,
                                CreatedByUserID = @CreatedByUserID
                                where DriverID = @DriverID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@DriverID", DriverID); // رقم السائق
            command.Parameters.AddWithValue("@PersonID", PersonID); // رقم الشخص
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID); // رقم المستخدم
           
            try // محاولة تنفيذ الكود
            {
                connection.Open(); // فتح الاتصال
                rowsAffected = command.ExecuteNonQuery(); // تنفيذ التحديث والحصول على عدد الصفوف المتأثرة

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

            return (rowsAffected > 0); // إرجاع true إذا تم تحديث صف واحد على الأقل
        }

    }
}
