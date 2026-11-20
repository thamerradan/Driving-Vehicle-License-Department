using System; // استيراد مكتبة System الأساسية
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Xml.Linq; // استيراد مكتبة Xml.Linq للتعامل مع XML
using DVLD_DataAccess; // استيراد طبقة الوصول للبيانات


namespace DVLD_Buisness // مساحة الاسم الخاصة بطبقة الأعمال (Business Layer)
{
    public  class clsPerson // كلاس لإدارة الأشخاص في طبقة الأعمال
    {
        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew; // متغير لتحديد الوضع الحالي (افتراضياً إضافة جديدة)

        // خصائص الشخص
        public int PersonID { set; get; } // رقم الشخص
        public string FirstName { set; get; } // الاسم الأول
        public string SecondName { set; get; } // الاسم الثاني
        public string ThirdName { set; get; } // الاسم الثالث
        public string LastName { set; get; } // اسم العائلة
        // خاصية للاسم الكامل (يتم حسابها تلقائياً من الأسماء)
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }
        public string NationalNo { set; get; } // الرقم الوطني
        public DateTime DateOfBirth { set; get; } // تاريخ الميلاد
        public short Gendor { set; get; } // الجنس (0 = ذكر، 1 = أنثى)
        public string Address { set; get; } // العنوان
        public string Phone { set; get; } // رقم الهاتف
        public string Email { set; get; } // البريد الإلكتروني
        public int NationalityCountryID { set; get; } // رقم الدولة (الجنسية)

        public clsCountry CountryInfo; // معلومات الدولة المرتبطة بالشخص

        private string _ImagePath; // متغير خاص لتخزين مسار الصورة
      
        // خاصية لمسار الصورة
        public string ImagePath   
        {
            get { return _ImagePath; }   // الحصول على مسار الصورة
            set { _ImagePath = value; }  // تعيين مسار الصورة
        }

        // Constructor الافتراضي - يُستخدم عند إنشاء شخص جديد
        public clsPerson()

        {
            this.PersonID = -1; // -1 يعني شخص جديد لم يُحفظ بعد
            this.FirstName = ""; // تهيئة الاسم الأول كسلسلة فارغة
            this.SecondName = ""; // تهيئة الاسم الثاني كسلسلة فارغة
            this.ThirdName = ""; // تهيئة الاسم الثالث كسلسلة فارغة
            this.LastName = ""; // تهيئة اسم العائلة كسلسلة فارغة
            this.DateOfBirth = DateTime.Now; // تهيئة تاريخ الميلاد بالتاريخ الحالي
            this.Address = ""; // تهيئة العنوان كسلسلة فارغة
            this.Phone = ""; // تهيئة رقم الهاتف كسلسلة فارغة
            this.Email = ""; // تهيئة البريد الإلكتروني كسلسلة فارغة
            this.NationalityCountryID = -1; // تهيئة رقم الدولة إلى -1
            this.ImagePath = ""; // تهيئة مسار الصورة كسلسلة فارغة

            Mode = enMode.AddNew; // تعيين الوضع إلى إضافة جديدة
        }

        private clsPerson(int PersonID, string FirstName,string SecondName, string ThirdName,
            string LastName,string NationalNo, DateTime DateOfBirth,short Gendor,
             string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)

        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName= SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;   
            this.DateOfBirth = DateOfBirth;
            this.Gendor= Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountry.Find(NationalityCountryID);
            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            //call DataAccess Layer 

            this.PersonID = clsPersonData.AddNewPerson(
                this.FirstName,this.SecondName ,this.ThirdName,
                this.LastName,this.NationalNo,
                this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email,
                this.NationalityCountryID, this.ImagePath);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            //call DataAccess Layer 

            return clsPersonData.UpdatePerson(
                this.PersonID, this.FirstName,this.SecondName,this.ThirdName,
                this.LastName, this.NationalNo, this.DateOfBirth, this.Gendor,
                this.Address, this.Phone, this.Email, 
                  this.NationalityCountryID, this.ImagePath);
        }

        public static clsPerson Find(int PersonID)
        {

            string FirstName = "", SecondName = "", ThirdName = "", LastName = "",NationalNo="", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            short Gendor = 0;

            bool IsFound = clsPersonData.GetPersonInfoByID 
                                (
                                    PersonID, ref FirstName, ref SecondName,
                                    ref ThirdName, ref LastName, ref NationalNo, ref DateOfBirth,
                                    ref Gendor, ref Address, ref Phone, ref Email,
                                    ref NationalityCountryID, ref ImagePath
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsPerson(PersonID, FirstName,SecondName ,ThirdName, LastName,
                          NationalNo, DateOfBirth,Gendor, Address, Phone, Email,NationalityCountryID, ImagePath);
            else
                return null;
        }

        public static clsPerson Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "",  Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int PersonID=-1,NationalityCountryID = -1;
            short Gendor = 0;

            bool IsFound = clsPersonData.GetPersonInfoByNationalNo
                                (
                                    NationalNo, ref PersonID, ref FirstName, ref SecondName,
                                    ref ThirdName, ref LastName, ref DateOfBirth,
                                    ref Gendor,ref Address, ref Phone, ref Email,
                                    ref NationalityCountryID, ref ImagePath
                                );

            if (IsFound)

                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                          NationalNo, DateOfBirth,Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            else
                return null;
        }

        // دالة لحفظ الشخص (إضافة جديد أو تحديث موجود)
        // ترجع true إذا نجحت العملية، و false إذا فشلت
        public bool Save()
        {
            switch (Mode) // التحقق من الوضع الحالي
            {
                case enMode.AddNew: // إذا كان الوضع إضافة جديدة
                    if (_AddNewPerson()) // محاولة إضافة الشخص
                    {

                        Mode = enMode.Update; // تغيير الوضع إلى تحديث بعد الإضافة الناجحة
                        return true; // إرجاع true للنجاح
                    }
                    else
                    {
                        return false; // إرجاع false للفشل
                    }

                case enMode.Update: // إذا كان الوضع تحديث

                    return _UpdatePerson(); // تحديث الشخص وإرجاع النتيجة

            }

            return false; // إرجاع false في حالة أخرى
        }

        // دالة ثابتة للحصول على جميع الأشخاص من قاعدة البيانات
        // ترجع DataTable يحتوي على جميع الأشخاص
        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople(); // استدعاء طبقة الوصول للبيانات
        }

        // دالة ثابتة لحذف شخص من قاعدة البيانات
        // ترجع true إذا تم الحذف بنجاح، و false إذا فشلت العملية
        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID); // استدعاء طبقة الوصول للبيانات
        }

        // دالة ثابتة للتحقق من وجود شخص بناءً على رقم الشخص (PersonID)
        // ترجع true إذا كان الشخص موجوداً، و false إذا لم يكن موجوداً
        public static bool isPersonExist(int ID)
        {
           return clsPersonData.IsPersonExist(ID); // استدعاء طبقة الوصول للبيانات
        }

        // دالة ثابتة للتحقق من وجود شخص بناءً على الرقم الوطني (NationalNo)
        // ترجع true إذا كان الرقم الوطني موجوداً، و false إذا لم يكن موجوداً
        public static bool isPersonExist(string NationlNo)
        {
            return clsPersonData.IsPersonExist(NationlNo); // استدعاء طبقة الوصول للبيانات
        }

    }
}
