using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.Data.SqlClient; // استيراد مكتبة SqlClient للاتصال بقاعدة بيانات SQL Server
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Text; // استيراد مكتبة Text للتعامل مع النصوص
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة
using static DVLD_DataAccess.clsCountryData; // استيراد static من كلاس clsCountryData
using System.Net; // استيراد مكتبة Net للشبكات
using System.Security.Policy; // استيراد مكتبة Security.Policy للسياسات الأمنية

namespace DVLD_DataAccess // مساحة الاسم الخاصة بطبقة الوصول للبيانات
{
    public class clsApplicationTypeData // كلاس لإدارة أنواع الطلبات (Application Types) في قاعدة البيانات
    {

        // دالة للحصول على معلومات نوع طلب بناءً على رقم نوع الطلب (ApplicationTypeID)
        // ترجع true إذا تم العثور على نوع الطلب، و false إذا لم يتم العثور عليه
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, 
            ref string ApplicationTypeTitle, ref float ApplicationFees)
            {
                bool isFound = false; // متغير لتحديد ما إذا كان السجل موجوداً أم لا

                // إنشاء اتصال جديد بقاعدة البيانات
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                // استعلام SQL للحصول على معلومات نوع الطلب بناءً على ApplicationTypeID
                string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

                // إنشاء أمر SQL
                SqlCommand command = new SqlCommand(query, connection);

                // إضافة المعامل للاستعلام
                command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {

                        // The record was found
                        isFound = true;

                        // استخراج البيانات
                        ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"]; // عنوان نوع الطلب
                        ApplicationFees = Convert.ToSingle( reader["ApplicationFees"]); // رسوم نوع الطلب

                  



                }
                    else
                    {
                        // The record was not found
                        isFound = false;
                    }

                    reader.Close();


                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Error: " + ex.Message);
                    isFound = false;
                }
                finally
                {
                    connection.Close();
                }

                return isFound;
            }

        // دالة للحصول على جميع أنواع الطلبات من قاعدة البيانات
        // ترجع DataTable يحتوي على جميع أنواع الطلبات مرتبة حسب العنوان
        public static DataTable GetAllApplicationTypes()
            {

                DataTable dt = new DataTable(); // إنشاء جدول بيانات فارغ
                // إنشاء اتصال جديد
                SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                // استعلام SQL للحصول على جميع أنواع الطلبات مرتبة حسب العنوان
                string query = "SELECT * FROM ApplicationTypes order by ApplicationTypeTitle";

                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)

                    {
                        dt.Load(reader);
                    }

                    reader.Close();


                }

                catch (Exception ex)
                {
                    // Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                return dt;

            }

        // دالة لإضافة نوع طلب جديد إلى قاعدة البيانات
        // ترجع رقم نوع الطلب الجديد (ApplicationTypeID) إذا نجحت العملية، و -1 إذا فشلت
        public static int AddNewApplicationType( string Title, float Fees)
        {
            int ApplicationTypeID = -1; // تهيئة القيمة الافتراضية (-1 يعني فشل العملية)

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لإضافة نوع طلب جديد
            // SCOPE_IDENTITY() ترجع آخر رقم تم إدراجه في عمود IDENTITY (ApplicationTypeID)
            string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)
                            
                            SELECT SCOPE_IDENTITY();";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@ApplicationTypeTitle", Title); // عنوان نوع الطلب
            command.Parameters.AddWithValue("@ApplicationFees", Fees); // رسوم نوع الطلب

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationTypeID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return ApplicationTypeID;

        }

        // دالة لتحديث معلومات نوع طلب موجود في قاعدة البيانات
        // ترجع true إذا تم التحديث بنجاح، و false إذا فشلت العملية
        public static bool UpdateApplicationType(int ApplicationTypeID,string Title, float Fees)
        {

            int rowsAffected = 0; // عدد الصفوف المتأثرة بالتحديث
            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لتحديث معلومات نوع الطلب
            string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID); // رقم نوع الطلب (للتحديث)
            command.Parameters.AddWithValue("@Title", Title); // عنوان نوع الطلب
            command.Parameters.AddWithValue("@Fees", Fees); // رسوم نوع الطلب
           
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

    }
}
