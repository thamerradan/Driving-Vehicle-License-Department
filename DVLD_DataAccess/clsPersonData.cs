using System; // استيراد مكتبة System الأساسية
using System.ComponentModel; // استيراد مكتبة ComponentModel للخصائص والسمات
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Data.SqlClient; // استيراد مكتبة SqlClient للاتصال بقاعدة بيانات SQL Server

namespace DVLD_DataAccess // مساحة الاسم الخاصة بطبقة الوصول للبيانات
{
    public class clsPersonData // كلاس لإدارة بيانات الأشخاص في قاعدة البيانات
    {
       
        // دالة للحصول على معلومات شخص بناءً على رقم الشخص (PersonID)
        // تستخدم ref parameters لإرجاع القيم من الدالة
        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
          ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
           ref short Gendor,ref string Address,  ref string Phone, ref string Email,
           ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان السجل موجوداً أم لا

            // إنشاء اتصال جديد بقاعدة البيانات
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للحصول على بيانات الشخص بناءً على PersonID
            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            // إنشاء أمر SQL لتنفيذ الاستعلام
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل (Parameter) للاستعلام لحماية من SQL Injection
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try // محاولة تنفيذ الكود
            {
                connection.Open(); // فتح الاتصال بقاعدة البيانات
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام وإرجاع قارئ البيانات

                if (reader.Read()) // قراءة السجل الأول من النتائج
                {
                    // تم العثور على السجل
                    isFound = true;

                    // استخراج البيانات من قاعدة البيانات وتحويلها إلى الأنواع المناسبة
                    FirstName = (string)reader["FirstName"]; // الاسم الأول
                    SecondName = (string)reader["SecondName"]; // الاسم الثاني

                    // ThirdName: يسمح بقيمة null في قاعدة البيانات لذلك يجب التعامل مع null
                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"]; // الاسم الثالث
                    }
                    else
                    {
                        ThirdName = ""; // إذا كان null، نضع قيمة فارغة
                    }

                    LastName = (string)reader["LastName"]; // اسم العائلة
                    NationalNo = (string)reader["NationalNo"]; // الرقم الوطني
                    DateOfBirth = (DateTime)reader["DateOfBirth"]; // تاريخ الميلاد
                    Gendor = (byte) reader["Gendor"]; // الجنس (0 = ذكر، 1 = أنثى)
                    Address = (string)reader["Address"]; // العنوان
                    Phone = (string)reader["Phone"]; // رقم الهاتف


                    // Email: يسمح بقيمة null في قاعدة البيانات لذلك يجب التعامل مع null
                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"]; // البريد الإلكتروني
                    }
                    else
                    {
                        Email = ""; // إذا كان null، نضع قيمة فارغة
                    }

                    NationalityCountryID = (int)reader["NationalityCountryID"]; // رقم الدولة (الجنسية)

                    // ImagePath: يسمح بقيمة null في قاعدة البيانات لذلك يجب التعامل مع null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"]; // مسار الصورة
                    }
                    else
                    {
                        ImagePath = ""; // إذا كان null، نضع قيمة فارغة
                    }

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


        // دالة للحصول على معلومات شخص بناءً على الرقم الوطني (NationalNo)
        // تستخدم للبحث عن شخص باستخدام الرقم الوطني
        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
        ref string ThirdName, ref string LastName,   ref DateTime DateOfBirth,
         ref short Gendor,ref string Address, ref string Phone, ref string Email,
         ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان السجل موجوداً أم لا

            // إنشاء اتصال جديد بقاعدة البيانات
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للحصول على بيانات الشخص بناءً على الرقم الوطني
            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open(); // فتح الاتصال
                SqlDataReader reader = command.ExecuteReader(); // تنفيذ الاستعلام

                if (reader.Read()) // قراءة السجل
                {
                    // تم العثور على السجل
                    isFound = true;

                    // استخراج البيانات
                    PersonID = (int)reader["PersonID"]; // رقم الشخص
                    FirstName = (string)reader["FirstName"]; // الاسم الأول
                    SecondName = (string)reader["SecondName"]; // الاسم الثاني

                    // ThirdName: يسمح بقيمة null في قاعدة البيانات لذلك يجب التعامل مع null
                    if (reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)reader["ThirdName"]; // الاسم الثالث
                    }
                    else
                    {
                        ThirdName = ""; // إذا كان null، نضع قيمة فارغة
                    }

                    LastName = (string)reader["LastName"]; // اسم العائلة
                    DateOfBirth = (DateTime)reader["DateOfBirth"]; // تاريخ الميلاد
                    Gendor = (byte)reader["Gendor"]; // الجنس
                    Address = (string)reader["Address"]; // العنوان
                    Phone = (string)reader["Phone"]; // رقم الهاتف

                    // Email: يسمح بقيمة null في قاعدة البيانات لذلك يجب التعامل مع null
                    if (reader["Email"] != DBNull.Value)
                    {
                        Email = (string)reader["Email"]; // البريد الإلكتروني
                    }
                    else
                    {
                        Email = ""; // إذا كان null، نضع قيمة فارغة
                    }

                    NationalityCountryID = (int)reader["NationalityCountryID"]; // رقم الدولة

                    // ImagePath: يسمح بقيمة null في قاعدة البيانات لذلك يجب التعامل مع null
                    if (reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)reader["ImagePath"]; // مسار الصورة
                    }
                    else
                    {
                        ImagePath = ""; // إذا كان null، نضع قيمة فارغة
                    }

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



        // دالة لإضافة شخص جديد إلى قاعدة البيانات
        // ترجع رقم الشخص الجديد (PersonID) إذا نجحت العملية، و -1 إذا فشلت
        public static int AddNewPerson( string FirstName,  string SecondName,
           string ThirdName,  string LastName,  string NationalNo,  DateTime DateOfBirth,
           short Gendor, string Address,  string Phone,  string Email,
            int NationalityCountryID,  string ImagePath)
        {
            // هذه الدالة ترجع رقم الشخص الجديد إذا نجحت العملية و -1 إذا فشلت
            int PersonID = -1; // تهيئة القيمة الافتراضية (-1 يعني فشل العملية)

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لإضافة شخص جديد
            // SCOPE_IDENTITY() ترجع آخر رقم تم إدراجه في عمود IDENTITY (PersonID)
            string query = @"INSERT INTO People (FirstName, SecondName, ThirdName,LastName,NationalNo,
                                                   DateOfBirth,Gendor,Address,Phone, Email, NationalityCountryID,ImagePath)
                             VALUES (@FirstName, @SecondName,@ThirdName, @LastName, @NationalNo,
                                     @DateOfBirth,@Gendor,@Address,@Phone, @Email,@NationalityCountryID,@ImagePath);
                             SELECT SCOPE_IDENTITY();";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@FirstName", FirstName); // الاسم الأول
            command.Parameters.AddWithValue("@SecondName", SecondName); // الاسم الثاني
           
            // التعامل مع ThirdName إذا كان فارغاً أو null
            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName); // الاسم الثالث
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value); // null إذا كان فارغاً

            command.Parameters.AddWithValue("@LastName", LastName); // اسم العائلة
            command.Parameters.AddWithValue("@NationalNo", NationalNo); // الرقم الوطني
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth); // تاريخ الميلاد
            command.Parameters.AddWithValue("@Gendor", Gendor); // الجنس
            command.Parameters.AddWithValue("@Address", Address); // العنوان
            command.Parameters.AddWithValue("@Phone", Phone); // رقم الهاتف
            
            // التعامل مع Email إذا كان فارغاً أو null
            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email); // البريد الإلكتروني
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value); // null إذا كان فارغاً

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID); // رقم الدولة

            // التعامل مع ImagePath إذا كان فارغاً أو null
            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath); // مسار الصورة
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value); // null إذا كان فارغاً

            try
            {
                connection.Open(); // فتح الاتصال

                // ExecuteScalar() تنفذ الاستعلام وترجع قيمة واحدة (رقم الشخص الجديد)
                object result = command.ExecuteScalar();

                // التحقق من أن النتيجة ليست null وتحويلها إلى int
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonID = insertedID; // حفظ رقم الشخص الجديد
                }
            }

            catch (Exception ex) // معالجة الأخطاء
            {
                //Console.WriteLine("Error: " + ex.Message);
                // في حالة الخطأ، PersonID سيبقى -1

            }

            finally // يتم تنفيذه دائماً
            {
                connection.Close(); // إغلاق الاتصال
            }

            return PersonID; // إرجاع رقم الشخص الجديد أو -1 في حالة الفشل
        }



        // دالة لتحديث بيانات شخص موجود في قاعدة البيانات
        // ترجع true إذا تم التحديث بنجاح، و false إذا فشلت العملية
        public static bool UpdatePerson(int PersonID,  string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {

            int rowsAffected = 0; // عدد الصفوف المتأثرة بالتحديث
            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لتحديث بيانات الشخص
            string query = @"Update  People  
                            set FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName, 
                                NationalNo = @NationalNo,
                                DateOfBirth = @DateOfBirth,
                                Gendor=@Gendor,
                                Address = @Address,  
                                Phone = @Phone,
                                Email = @Email, 
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath =@ImagePath
                                where PersonID = @PersonID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعاملات للاستعلام
            command.Parameters.AddWithValue("@PersonID", PersonID); // رقم الشخص (للتحديث)
            command.Parameters.AddWithValue("@FirstName", FirstName); // الاسم الأول
            command.Parameters.AddWithValue("@SecondName", SecondName); // الاسم الثاني

            // التعامل مع ThirdName إذا كان فارغاً أو null
            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName); // الاسم الثالث
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value); // null إذا كان فارغاً

          
            command.Parameters.AddWithValue("@LastName", LastName); // اسم العائلة
            command.Parameters.AddWithValue("@NationalNo", NationalNo); // الرقم الوطني
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth); // تاريخ الميلاد
            command.Parameters.AddWithValue("@Gendor", Gendor); // الجنس
            command.Parameters.AddWithValue("@Address", Address); // العنوان
            command.Parameters.AddWithValue("@Phone", Phone); // رقم الهاتف

            // التعامل مع Email إذا كان فارغاً أو null
            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email); // البريد الإلكتروني
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value); // null إذا كان فارغاً

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID); // رقم الدولة

            // التعامل مع ImagePath إذا كان فارغاً أو null
            if (ImagePath != "" && ImagePath != null)
                command.Parameters.AddWithValue("@ImagePath", ImagePath); // مسار الصورة
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value); // null إذا كان فارغاً


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


        // دالة للحصول على جميع الأشخاص من قاعدة البيانات
        // ترجع DataTable يحتوي على جميع الأشخاص مع معلوماتهم الكاملة
        public static DataTable GetAllPeople()
        {

            DataTable dt = new DataTable(); // إنشاء جدول بيانات فارغ
            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للحصول على جميع الأشخاص مع معلوماتهم
            // INNER JOIN لربط جدول People بجدول Countries للحصول على اسم الدولة
            // CASE لتحويل رقم الجنس (0/1) إلى نص (Male/Female)
            string query = 
              @"SELECT People.PersonID, People.NationalNo,
              People.FirstName, People.SecondName, People.ThirdName, People.LastName,
			  People.DateOfBirth, People.Gendor,  
				  CASE
                  WHEN People.Gendor = 0 THEN 'Male'  -- إذا كان الجنس = 0، اكتب 'Male'

                  ELSE 'Female'  -- وإلا اكتب 'Female'

                  END as GendorCaption ,  -- اسم عمود جديد يحتوي على نص الجنس
			  People.Address, People.Phone, People.Email, 
              People.NationalityCountryID, Countries.CountryName, People.ImagePath
              FROM            People INNER JOIN
                         Countries ON People.NationalityCountryID = Countries.CountryID
                ORDER BY People.FirstName";  




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

            return dt; // إرجاع DataTable يحتوي على جميع الأشخاص

        }

        // دالة لحذف شخص من قاعدة البيانات
        // ترجع true إذا تم الحذف بنجاح، و false إذا فشلت العملية
        public static bool DeletePerson(int PersonID)
        {

            int rowsAffected = 0; // عدد الصفوف المتأثرة بالحذف

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL لحذف الشخص
            string query = @"Delete People 
                                where PersonID = @PersonID";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        // دالة للتحقق من وجود شخص بناءً على رقم الشخص (PersonID)
        // ترجع true إذا كان الشخص موجوداً، و false إذا لم يكن موجوداً
        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان الشخص موجوداً

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للتحقق من وجود الشخص
            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";

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

            return isFound; // إرجاع true إذا كان الشخص موجوداً، و false إذا لم يكن موجوداً
        }

        // دالة للتحقق من وجود شخص بناءً على الرقم الوطني (NationalNo)
        // ترجع true إذا كان الرقم الوطني موجوداً، و false إذا لم يكن موجوداً
        // تستخدم للتحقق من عدم تكرار الأرقام الوطنية
        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false; // متغير لتحديد ما إذا كان الرقم الوطني موجوداً

            // إنشاء اتصال جديد
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            // استعلام SQL للتحقق من وجود الرقم الوطني
            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

            // إنشاء أمر SQL
            SqlCommand command = new SqlCommand(query, connection);

            // إضافة المعامل للاستعلام
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

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

            return isFound; // إرجاع true إذا كان الرقم الوطني موجوداً، و false إذا لم يكن موجوداً
        }


    }
}
